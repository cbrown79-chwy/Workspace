module AsyncReactive.Observables

open Fable.Import
open Fable.Import.Browser
open AsyncReactive.Async
open AsyncReactive.Gui

// ------------------------------------------------------------------------------------------------
// Observables - implementation
// ------------------------------------------------------------------------------------------------

/// An observable is very similar to events, but it returns a function
/// that can be used to cancel the subscription when we are no longer
/// interested in listening to the event.
type IObservable<'T> = 
  abstract AddHandler : ('T -> unit) -> (unit -> unit)

module Observable = 
  /// Creates an observable that calls handlers when a button is pressed
  let onClick (btn:HTMLButtonElement) = 
    let handlers = ResizeArray<_>()
    btn.onclick <- fun e -> (for h in handlers do h e); null
    { new IObservable<_> with
        member x.AddHandler(h) = 
          handlers.Add(h) 
          (fun () -> handlers.Remove(h) |> ignore) }

  /// Add handler to an observable
  let add f (e:IObservable<_>) = 
    e.AddHandler(fun e -> f e)

  /// Creates an event that is triggered whenever either
  /// of the two observables is triggered.
  let merge (e1:IObservable<_>) (e2:IObservable<_>) = 
    { new IObservable<_> with
        member x.AddHandler(h) = 
          let d1 = e1.AddHandler(h)
          let d2 = e2.AddHandler(h) 
          fun () -> d1 (); d2 () }
  
  /// Creates a new observable that keeps a state. When the source
  /// observable happens, the state is updated using the specified 
  /// function and the new state is reported.
  let scan v f (e:IObservable<_>) = 
    { new IObservable<_> with
        member x.AddHandler(h) = 
          let mutable value = v
          e.AddHandler(fun x -> value <- f value x; h value) }

  /// Create a derived observable that triggers a handler whenever
  /// a source observable happens, but it transforms the value using
  /// a specified function.
  let map f (e:IObservable<_>) = 
    { new IObservable<_> with
        member x.AddHandler(h) = e.AddHandler(fun x -> h (f x))  }


module Async =
  /// Creates an asynchronous workflow that waits for the
  /// first value emitted by a given observable.
  let awaitObservable (e:IObservable<_>) = 
    { new Async<_> with 
        member x.Start(f) = 
          let mutable d = ignore
          d <- e.AddHandler(fun v -> d (); f v) }

// ------------------------------------------------------------------------------------------------
// Observables - demos
// ------------------------------------------------------------------------------------------------

// Traffic lights, but this time, we wait for a click before advancing!
let demo1 () =
 show "section4" 
 async {
   while true do
     for clr in ["green";"orange";"red"] do
       let! e = Async.awaitObservable (Observable.onClick Section4.next)
       Section4.light.style.backgroundColor <- clr } |> Async.start

// Up/down counter, but this time, the `start` and `stop` buttons are
// used to reset the counter and restart it. The `stop` button deletes
// the subscription and the `start` button creates a new one.
let demo2 () = 
  show "section3"
  let e1 = Observable.onClick Section3.up |> Observable.map (fun _ -> 1)
  let e2 = Observable.onClick Section3.down |> Observable.map (fun _ -> -1)

  let evt = 
    Observable.merge e1 e2
    |> Observable.scan 0 (+)
    |> Observable.map (sprintf "Count: %d")
    
  let mutable d = ignore
  Observable.onClick Section3.start |> Observable.add (fun _ -> 
    d <- evt.AddHandler(fun s -> Section3.lbl.innerText <- s)) |> ignore    
  Observable.onClick Section3.stop |> Observable.add (fun _ -> 
    Section3.lbl.innerText <- "Count: 0"
    d () ) |> ignore

