namespace Classifiers

// ------------------------------------------------------------------
// PART #1: Switching between implementations
// ------------------------------------------------------------------

// Choose implementation that you want to use
// Uncomment one of the following lines to do this - note
// that you need to change this here and also in `main.fs`!

// open Classifiers.Data
open Classifiers.Functions

// ------------------------------------------------------------------
// Implementation of a simple GUI for showing charts
// ------------------------------------------------------------------

open System
open Fable.Import.Browser
open System.Threading

module DataSource =
  // Return sequence that reads the prices
  let data lines = seq { 
    let win = ResizeArray<_>()
    let lines = lines |> Seq.skip 1 |> Array.ofSeq |> Array.rev
    while true do
      for line in lines do
        let infos = (line:string).Split(',')
        let dt = DateTime.Parse(infos.[0])
        let op = float infos.[1] 
        win.Add((dt, op))
        if win.Count = 30 then 
          yield win.ToArray()
          win.RemoveAt(0) } 

  /// Asynchronously downloads stock prices from local file
  let downloadPrices from stock = 
    Async.FromContinuations(fun (cont, _, _) ->
      let xh = XMLHttpRequest.Create()
      xh.addEventListener_readystatechange(fun p -> 
        if xh.readyState > 3. && xh.status = 200. then
          let lines = xh.responseText.Split([|'\n'; '\r'|], StringSplitOptions.RemoveEmptyEntries)
          cont (data lines)      
        null)
      xh.``open``("GET", "/data/" + stock + ".csv", true)
      xh.send("") )
  
module Utils = 
  open Fable.Core

  [<Emit("addDataPoint($0)")>]
  let addDataPoint (value:float) = failwith "JS!"

open DataSource

/// Window that displays the GUI
type ClassifierWindow() = 
  
  /// List of update functions to be called from GUI
  let updates = ResizeArray<((DateTime * float)[] -> unit) * (unit -> unit)>()
  let cleanup = ref ignore

  // Current cancellation token
  let tok = ref <| new CancellationTokenSource()
  let evt = new Event<(DateTime * float)[]>()
  let mutable counter = 0
  let nextId () = 
    counter <- counter + 1
    sprintf "el_%d" counter

  /// Add classifier to list & create GUI
  let addBoolClassifier name (cls:Classifier<bool>) = 
    let cl = document.getElementById("classifiers")
    let el = document.createElement("div")
    let id = nextId ()
    el.id <- id
    el.innerHTML <- sprintf "<span class='lbl'>%s</span><span class='sign' id='%s_sign'></span>" name id
    cl.appendChild(el) |> ignore
    let update data =
      document.getElementById(id + "_sign").style.backgroundColor <- if Price.run cls data then "#8EB247" else "#808080"
    let clear () = 
      cl.removeChild(el) |> ignore
    updates.Add( (update, clear) ) 
    
  /// Add classifier to list & create GUI
  let addFloatClassifier name (cls:Classifier<float>) = 
    let cl = document.getElementById("classifiers")
    let el = document.createElement("div")
    let id = nextId ()
    el.id <- id
    el.innerHTML <- sprintf "<span class='lbl'>%s</span><span class='val' id='%s_val'></span>" name id
    cl.appendChild(el) |> ignore
    let update data =
      document.getElementById(id + "_val").innerText <- string (Price.run cls data)
    let clear () = 
      cl.removeChild(el) |> ignore
    updates.Add( (update, clear) ) 

  /// Main loop
  let mainLoop stock = async {
    let! blocks = downloadPrices (DateTime(2009, 1, 1)) stock
    let en = blocks.GetEnumerator()

    while en.MoveNext() do
      do! Async.Sleep(200)
      for fn, _ in updates do fn en.Current

      let lo = Seq.min (Seq.map snd en.Current)
      let hi = Seq.max (Seq.map snd en.Current)
      let diff = (hi - lo) / 6.0

      let lastDate, lastValue = en.Current.[en.Current.Length-1]
      Utils.addDataPoint lastValue
      evt.Trigger(en.Current)  } 

  member x.Run(stock) =
    let cts = new CancellationTokenSource()
    tok := cts
    Async.StartImmediate(mainLoop stock, cts.Token)

  member x.Add(name, cls) =
    addBoolClassifier name cls

  member x.Add(name, cls) =
    addFloatClassifier name cls

  member x.Clear() = 
    for _, clean in updates do clean ()
    updates.Clear() 

  member x.Stop() = 
    (!tok).Cancel()
