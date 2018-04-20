module Fable.Import.Elmish
open Fable.Core
open Fable.Import
open Fable.Import.Browser
open Fable.Core.JsInterop
open System.Diagnostics

module FsOption = FSharp.Core.Option

module Virtualdom = 
  [<Import("h","virtual-dom")>]
  let h(arg1: string, arg2: obj, arg3: obj[]): obj = failwith "JS only"

  [<Import("diff","virtual-dom")>]
  let diff (tree1:obj) (tree2:obj): obj = failwith "JS only"

  [<Import("patch","virtual-dom")>]
  let patch (node:obj) (patches:obj): Fable.Import.Browser.Node = failwith "JS only"

  [<Import("create","virtual-dom")>]
  let createElement (e:obj): Fable.Import.Browser.Node = failwith "JS only"

[<Emit("$0[$1]")>]
let private getProperty (o:obj) (s:string) = failwith "!"

[<Emit("$0[$1] = $2")>]
let private setProperty (o:obj) (s:string) (v:obj) = failwith "!"

[<Fable.Core.Emit("event")>]
let private event () : Event = failwith "JS"

type DomAttribute = 
  | Event of (HTMLElement -> Event -> unit)
  | Attribute of string
  | Property of obj

type DomNode = 
  | Text of string
  | Element of ns:string * tag:string * key:string * attributes:(string * DomAttribute)[] * children : DomNode[] 
  | Func of DomNode * (unit -> unit) 

let createTree ns tag key args children =
  let attrs = ResizeArray<_>()
  let props = ResizeArray<_>()
  for k, v in args do
    match k, v with 
    | k, Attribute v ->
        attrs.Add (k, box v)
    | k, Property o ->
        props.Add(k, o)
    | k, Event f ->
        props.Add ("on" + k, box (fun o -> f (getProperty o "target") (event()) ))
  let attrs = JsInterop.createObj attrs
  props.Add("attributes", attrs)
  if ns <> null && ns <> "" then props.Add("namespace", box ns)
  if key <> null && key <> "" then props.Add("key", box key)
  let props = JsInterop.createObj props
  Virtualdom.h(tag, props, children)

let mutable counter = 0

let rec renderVirtual node = 
  match node with
  | Text(s) -> 
      box s, []
  | Element(ns, tag, key, attrs, children) ->
      let children, funcs = Array.map renderVirtual children |> Array.unzip
      createTree ns tag key attrs children, List.concat funcs
  | Func(node, func) ->
      let dom, fs = renderVirtual node
      dom, func::fs

let rec render node = 
  match node with
  | Func(node, func) ->
      let dom, f = render node
      dom, fun () -> f(); func()

  | Text(s) -> 
      document.createTextNode(s) :> Node, ignore

  | Element(ns, tag, _, attrs, children) ->
      let el = 
        if ns = null || ns = "" then document.createElement(tag)
        else document.createElementNS(ns, tag) :?> HTMLElement
      let rc = Array.map render children
      for c, _ in rc do el.appendChild(c) |> ignore
      for k, a in attrs do 
        match a with
        | Property(o) -> setProperty el k o
        | Attribute(v) -> el.setAttribute(k, v)
        | Event(f) -> el.addEventListener(k, U2.Case1(EventListener(f el)))
      let onRender () = 
        for _, f in rc do f()
      el :> Node, onRender

let renderTo (node:HTMLElement) dom = 
  while box node.lastChild <> null do ignore(node.removeChild(node.lastChild))
  let el, f = render dom
  node.appendChild(el) |> ignore
  f()

let createApp id initial r u = 
  let mutable container = document.createElement("div") :> Node
  document.getElementById(id).innerHTML <- ""
  document.getElementById(id).appendChild(container) |> ignore

  let worker = MailboxProcessor.Start(fun inbox -> async {
    let mutable tree = Fable.Core.JsInterop.createObj []
    let mutable state = initial
    while true do
      let! evt = inbox.Receive()
      try
        let! newState = 
          match evt with 
          | Choice1Of2 () -> async.Return(state)
          | Choice2Of2 e ->  
              //Common.Log.trace("worker", "Processing message: %O", box evt)
              async.Return(u state e)
        state <- newState
        let newTree, funcs = r (Choice2Of2 >> inbox.Post) state |> renderVirtual
        let patches = Virtualdom.diff tree newTree
        container <- Virtualdom.patch container patches
        for f in funcs do f()
        tree <- newTree
      with e ->
        console.log("Worker failed: %A", box e)  })

  worker.Post(Choice1Of2())

let text s = Text(s)
let (=>) k v = k, Attribute(v)
let (=!>) k f = k, Event(f)


type El(ns) = 
  member x.Namespace = ns
  static member (?) (el:El, n:string) = fun a b ->
    Element(el.Namespace, n, null, Array.ofList a, Array.ofList b)

  member x.el(n:string) = fun a b ->
    Element(x.Namespace, n, null, Array.ofList a, Array.ofList b)

  member x.elk(n:string) = fun k a b ->
    Element(x.Namespace, n, k, Array.ofList a, Array.ofList b)
      
  member x.func f node =
    Func(node, f)

  member x.once id f node = 
    Func(Element(x.Namespace, "div", null, [|"id" => "htmlonce-" + id|], [| node |]), fun () ->
      let el = Browser.document.getElementById("htmlonce-" + id)
      if (el.dataset.["initialized"] <> "true") then
        f el
        el.dataset.["initialized"] <- "true")

let h = El(null)
let s = El("http://www.w3.org/2000/svg")
