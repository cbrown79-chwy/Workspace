namespace Classifiers.Data
open System
open System.Net
open System.Windows
open System.Threading
open Microsoft.FSharp.Reflection

// ----------------------------------------------------------------------------
// Domain model
// ----------------------------------------------------------------------------

/// Represents possible values that a classifier may produce
type Value = 
  | Float of float
  | Bool of bool
  | Pair of Value * Value

/// Captures how is a classifier constructed  
type ClassifierShape = 
  | Forall of (float -> float -> bool)
  | Regression of ClassifierShape
  | Reduce of (seq<float> -> Value)
  | Map of ClassifierShape * (Value -> Value)
  | Both of ClassifierShape * ClassifierShape
  | Sequence of ClassifierShape * ClassifierShape

/// Represents a classifier that produces a value 'T
type Classifier<'T> = { Shape : ClassifierShape }

// ----------------------------------------------------------------------------
// Handling of values - we use reflection so that the 'map' function can
// be of type 't1 -> 't2, and the internal 'Value' is not exposed
// (this is admittedly a bit ugly, but it does the trick)
// ----------------------------------------------------------------------------

module ValueHelpers = 
  open Fable.Core

  [<Emit("(typeof($0)=='number')")>] 
  let isNumber (o:obj) : bool = failwith "JS"

  [<Emit("(typeof($0)=='boolean')")>] 
  let isBool (o:obj) : bool = failwith "JS"

  [<Emit("Array.isArray($0)")>]
  let isArray(n:obj) : bool = failwith "!"

  let unpack value : 'T =
    let rec loop value = 
      match value with 
      | Pair(v1, v2) -> box [| loop v1; loop v2 |]
      | Float v -> box v
      | Bool v -> box v
    loop value |> unbox<'T>

  let pack (value:'T) = 
    let rec loop value = 
      if isArray value then 
        let tup = unbox<obj[]> value
        Pair(loop tup.[0], loop tup.[1])
      elif isNumber value then
        Float(unbox value)
      elif isBool value then 
        Bool(unbox value)
      else failwithf "Cannot pack value: %A" value
    loop value      

// ----------------------------------------------------------------------------
// Composing the DSL
// ----------------------------------------------------------------------------

module Price =

  /// Runs a classifier and transforms the result using a specified function
  let map (f:'T1 -> 'T2) (cls:Classifier<'T1>) : Classifier<'T2> =     
    { Shape = Map(cls.Shape, ValueHelpers.unpack >> f >> ValueHelpers.pack) }

  /// Creates a classifier that combines the result of two classifiers using a tuple
  let both (f:Classifier<'T1>) (g:Classifier<'T2>) : Classifier<'T1 * 'T2> = 
    { Shape = Both(f.Shape, g.Shape) }

  /// Checks two properties of subsequent parts of the input
  let sequence (f:Classifier<'T1>) (g:Classifier<'T2>) : Classifier<'T1 * 'T2> = 
    { Shape = Sequence(f.Shape, g.Shape) }

  /// Checks whether the price is rising over the whole checked range
  let rising : Classifier<bool> = { Shape = Forall(<=) }

  /// Checks whether the price is declining over the whole checked range
  let declining : Classifier<bool> = { Shape = Forall(>=) }

  /// Gets the average over the whole range
  let average : Classifier<float> = { Shape = Reduce(Seq.average >> Float) }

  /// Checks that the property holds over an approximation 
  /// obtained using linear regression
  let regression (cls:Classifier<'T>) : Classifier<'T> =
    { Shape = Regression(cls.Shape) }
  
  // --------------------------------------------------------------------------
  // Evaluating the classifier
  // --------------------------------------------------------------------------

  let run (cls:Classifier<'T>) input : 'T = 
    let rec loop shape (input:_[]) = 
      match shape with
      | Forall(f) -> 
          input 
          |> Seq.pairwise 
          |> Seq.forall (fun ((_, a), (_, b)) -> f a b) |> Bool

      | Regression(cls) -> 
          let xavg = float (input.Length - 1) / 2.0
          let yavg = Seq.averageBy snd input
          let sums = input |> Seq.mapi (fun x (_, v) -> 
            (float x - xavg) * (v - yavg), pown (float x - xavg) 2)
          let v1 = Seq.sumBy fst sums
          let v2 = Seq.sumBy snd sums
          let a = v1 / v2
          let b = yavg - a * xavg 
          input 
          |> Array.mapi (fun x (dt, _) -> (dt, a * (float x) + b)) 
          |> loop cls

      | Reduce(f) -> 
          f (Seq.map snd input)

      | Map(cls, f) ->
          f (loop cls input)

      | Both(cls1, cls2) ->
          Pair(loop cls1 input, loop cls2 input)

      | Sequence(cls1, cls2) ->
          let length = input.Length
          let input1 = input.[0 .. length/2 - (if length%2=0 then 1 else 0)]
          let input2 = input.[length/2 .. length-1]
          Pair(loop cls1 input1, loop cls2 input2)

    loop cls.Shape input |> ValueHelpers.unpack