namespace Classifiers.Functions
open System
open System.Net
open System.Windows
open System.Threading

/// Represents a classifier that produces a value 'T
type Classifier<'T> = PT of ((DateTime * float)[] -> 'T)

module Price =

  /// Runs a classifier and transforms the result using a specified function
  let map g (PT f) = PT (f >> g)

  /// Creates a classifier that combines the result of two classifiers using a tuple
  let both (PT f) (PT g) = PT (fun values -> f values, g values)

  /// Checks two properties of subsequent parts of the input
  let sequence (PT f1) (PT f2) = PT (fun input ->
    let length = input.Length
    let input1 = input.[0 .. length/2 - (if length%2=0 then 1 else 0)]
    let input2 = input.[length/2 .. length-1]
    (f1 input1, f2 input2))

  /// Checks whether the price is rising over the whole checked range
  let rising = PT (fun input ->
    input |> Seq.pairwise |> Seq.forall (fun ((_, a), (_, b)) -> b >= a))

  /// Checks whether the price is declining over the whole checked range
  let declining = PT (fun input ->
    input |> Seq.pairwise |> Seq.forall (fun ((_, a), (_, b)) -> b <= a))

  /// Gets the maximum over the whole range
  let average = PT (fun input ->
    Math.Round(input |> Seq.map snd |> Seq.average, 2) )

  /// Checks that the property holds over an approximation 
  /// obtained using linear regression
  let regression (PT f) = PT (fun values ->
    let xavg = float (values.Length - 1) / 2.0
    let yavg = Seq.averageBy snd values
    let sums = values |> Seq.mapi (fun x (_, v) -> 
      (float x - xavg) * (v - yavg), pown (float x - xavg) 2)
    let v1 = Seq.sumBy fst sums
    let v2 = Seq.sumBy snd sums
    let a = v1 / v2
    let b = yavg - a * xavg 
    values |> Array.mapi (fun x (dt, _) -> (dt, a * (float x) + b)) |> f)

  /// Does the property hold over the entire data set?
  let run (PT f) (data:(DateTime * float)[]) = 
    f data