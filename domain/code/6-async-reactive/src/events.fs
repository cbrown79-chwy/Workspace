module AsyncReactive.Events

open Fable.Import
open AsyncReactive.Gui
module B = Fable.Import.Browser

// ------------------------------------------------------------------------------------------------
// Events - implementation
// ------------------------------------------------------------------------------------------------

/// An event is an object that triggers all handlers that are registerd with it.
type IEvent<'T> = 
  abstract AddHandler : ('T -> unit) -> unit

module Event = 
  /// Creates an event that calls handlers when a button is pressed
  let onClick (btn:B.HTMLButtonElement) = 
    let handlers = ResizeArray<_>()
    btn.onclick <- fun e -> (for h in handlers do h e); null
    { new IEvent<_> with
        member x.AddHandler(h) = handlers.Add(h) }

  /// Add handler to an event
  let add f (e:IEvent<_>) = 
    e.AddHandler(fun e -> f e)

  /// Create a derived event that triggers a handler whenever
  /// a source event happens, but it transforms the value using
  /// a specified function.
  let map f (e:IEvent<_>) = 
    { new IEvent<_> with
        member x.AddHandler(h) = e.AddHandler(fun x -> h (f x))  }

  /// Creates a new event that keeps a state. When the source
  /// event happens, the state is updated using the specified 
  /// function and the new state is reported.
  let scan v f (e:IEvent<_>) = 
    { new IEvent<_> with
        member x.AddHandler(h) = 
          let mutable value = v
          e.AddHandler(fun x -> value <- f value x; h value) }

  /// TODO: Implement an event that is triggered whenever either
  /// of the two events are triggered. (This is somewhat similar to
  /// how the `map` and `scan` events work!)
  let merge (e1:IEvent<_>) (e2:IEvent<_>) = 
    failwith "Not implemented!"
  
let setLabel text = 
  Section2.lbl.innerText <- text

// Show UP or DOWN message when one of the buttons is clicked
let demo1 () = 
  show "section2"
  Event.onClick Section2.up |> Event.map (fun _ -> "UP") |> Event.add setLabel
  Event.onClick Section2.down |> Event.map (fun _ -> "DOWN") |> Event.add setLabel

// Simple up/down counter that accumulates +1 or -1 values using scan
// formats the result nicely and shows it on the label.
let demo2 () = 
  show "section2"
  let e1 = Event.onClick Section2.up |> Event.map (fun _ -> 1)
  let e2 = Event.onClick Section2.down |> Event.map (fun _ -> -1)

  Event.merge e1 e2
  |> Event.scan 0 (+)
  |> Event.map (sprintf "Count: %d")
  |> Event.add setLabel
