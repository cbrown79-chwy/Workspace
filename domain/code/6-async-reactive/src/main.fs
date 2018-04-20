module AsyncReactive.Main

open System
open Fable.Core
open Fable.Import
open Fable.Import.Browser
open Fable.Import.Elmish
open System.Collections.Generic

// ------------------------------------------------------------------
// TASKS - This file just calls demos in other files. Go through the 
// tasks one by one and uncomment the calls to demos!
// ------------------------------------------------------------------

// TASK #1: Look at `async.fs` to see how asynchronous workflows
// work. Modify the demo so that it iterates over the three colours
// of the traffic light using a `for` loop rather than by explicitly
// waiting and then setting different colour three times.

// AsyncReactive.Async.demo ()


// TASK #2: Run the first demo and look at how events work. Next, run 
// the second demo - to make this work, you will need to implement
// the `merge` combinator for events (see comments in `events.fs`)

// AsyncReactive.Events.demo1 ()
// AsyncReactive.Events.demo2 ()


// TASK #3: Run the first demo and look at how observables work. Next, run 
// the second demo - to make this work, you will need to implement
// the `map` combinator for observables (see comments in `observable.fs`)

// AsyncReactive.Observables.demo1 ()
// AsyncReactive.Observables.demo2 ()


// TASK #4: Run the first demo to see what async sequences can do.
// Then, fix the second demo, which waits before advancing to the next
// item of the asynchronous sequence. To do this, you will need to implement
// the `delay` combinator (see `asyncseq.fs`) and you will need to uncomment
// one of the two `delay` calls in `demo2`.

// AsyncReactive.AsyncSeq.demo1 ()
// AsyncReactive.AsyncSeq.demo2 ()

// TASK #5: As a bonus, try to implement `AsyncSeq.take` function that
// accepts a number and returns an asynchronous sequence with at most
// that number of elements (like `List.take`) and use this function to 
// only iterate over the first 10 elements in `demo2`.