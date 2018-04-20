module AsyncReactive.Async

open Fable.Import.Browser
open AsyncReactive.Gui

// ------------------------------------------------------------------------------------------------
// Async - implementation
// ------------------------------------------------------------------------------------------------

/// An async computation is an object that can be started.
/// It eventually calls the given continuation with a result 'T.
type Async<'T> = 
  abstract Start : ('T -> unit) -> unit

module Async = 
  /// Creates an async workflow that calls the 
  /// continuation after the specified number of milliseconds
  let sleep n =
    { new Async<unit> with
        member x.Start(f) = 
          window.setTimeout((fun _ -> f ()), n) |> ignore }

  /// Creates a computation that composes 'a' with the result of 'f'
  let bind (f:'a -> Async<'b>) (a:Async<'a>) : Async<'b> = 
    { new Async<'b> with
        member x.Start(g) =
          a.Start(fun a ->
            let ab = f a
            ab.Start(g) ) }

  /// A computation that immediately returns the given value
  let unit v = 
    { new Async<_> with
        member x.Start(f) = f v }

  /// Start the computation and do nothing when it finishes.
  let start (a:Async<_>) =
    a.Start(fun () -> ())

  /// Defines a computation builed for asyncs.
  /// For and While are defined in terms of bind and unit.
  type AsyncBuilder() = 
    member x.Return(v) = unit v
    member x.Bind(a, f) = bind f a
    member x.Zero() = unit ()
    member x.For(vals, f) =
      match vals with
      | [] -> unit ()
      | v::vs -> f v |> bind (fun () -> x.For(vs, f))
    member x.Delay(f:unit -> Async<_>) =
      { new Async<_> with
          member x.Start(h) = f().Start(h) }
    member x.While(c, f) = 
      if not (c ()) then unit ()
      else f |> bind (fun () -> x.While(c, f))
    member x.ReturnFrom(a) = a
          
let async = Async.AsyncBuilder()

// ------------------------------------------------------------------------------------------------
// Async - demo
// ------------------------------------------------------------------------------------------------

// Simple traffic light that iterates through green, orange, red indifinitely
let demo () =
  show "section1"
  async {
    while true do
      do! Async.sleep 1000
      Section1.light.style.backgroundColor <- "green"
      do! Async.sleep 1000
      Section1.light.style.backgroundColor <- "orange"
      do! Async.sleep 1000
      Section1.light.style.backgroundColor <- "red" } 
  |> Async.start
