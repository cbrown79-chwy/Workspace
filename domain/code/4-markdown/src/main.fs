#if INTERACTIVE
#load "tests.fs"
#load "domain.fs"
#load "parser-combinators.fs"
#load "parser-patterns.fs"
#else
module MarkdownDemo.Main

open Fable.Core
open Fable.Import
open Fable.Import.Browser
open Fable.Import.Elmish
#endif
open Markdown

// ----------------------------------------------------------------------------
// Open one of the following two namespaces to load parsers based on
// F# active patterns or functional parser combinators.
// ----------------------------------------------------------------------------

//open Markdown.Parsers
open Markdown.Patterns

// ----------------------------------------------------------------------------
// PART 1: Tests for various Markdown features. The first few tests run fine,
// but the last few test features that are not implemented yet. You can run
// these in F# Interactive (load everything before this part first), or you
// can run them in the browser - the first failure will be reported in 
// the browser console window.
// ----------------------------------------------------------------------------

open Markdown.Tests
open Fable

// The following should already work

// Recognize line breaks
let res0 = "a  \n" |> List.ofSeq |> parseSpans |> List.ofSeq
shouldEqual res0 [Literal "a"; HardLineBreak]

// Recognize strong text
let res1 = "a**a**" |> List.ofSeq |> parseSpans |> List.ofSeq
shouldEqual res1 [Literal "a"; Strong [Literal "a"]]

// Recognize hyperlinks
let res2 = "a [text](http://url) c" |> List.ofSeq |> parseSpans |> List.ofSeq
shouldEqual res2 [Literal "a "; HyperLink ([Literal "text"],"http://url"); Literal " c"]

// The following need your work!

// Recognize emphasis using underscore
let res3 = "a _b_ c" |> List.ofSeq |> parseSpans |> List.ofSeq
shouldEqual res3 [Literal "a "; Emphasis [Literal "b"]; Literal " c"]

// Recognize emhpasis using sta
let res4 = "a *b* c" |> List.ofSeq |> parseSpans |> List.ofSeq
shouldEqual res4 [Literal "a "; Emphasis [Literal "b"]; Literal " c"]

// Recognize images
let res5 = "a ![title](img.png) c" |> List.ofSeq |> parseSpans |> List.ofSeq
shouldEqual res5 [Literal "a "; Image ("title","img.png"); Literal " c"]

// ----------------------------------------------------------------------------
// PART 2: Rendering sample Markdown documents. This will render the `sample`
// document, but it cannot currently handle the `advanced` document.
// ----------------------------------------------------------------------------


let rec renderSpan span =
  match span with
  | Literal(s) -> h?span [] [text s]
  | InlineCode(c) -> h?code [] [text c]
  | Strong(spans) -> h?strong [] (List.map renderSpan spans)
  | HyperLink(body, url) -> h?a ["href" => url] (List.map renderSpan body)  
  | HardLineBreak -> h?br [] []
  | Image (title, url) -> h?img ["src" => url; "title" => title] []
  | Emphasis s -> h?em [] (List.map renderSpan s)
  | Footnote (index, _) -> h?a ["href" => sprintf "#footnote%d" index] [ h?sup [] [text (sprintf "%d" index)]]


let renderDoc id spans =
  let footnotes = List.map (fun a -> 
                              match a with 
                                | Footnote(idx, content) -> HardLineBreak::HardLineBreak::Literal(sprintf "%d. " idx)::content 
                                | _ -> []                             
                            ) spans 
                            |> List.filter (List.isEmpty >> not)
                            |> List.concat
  
  let toRender = spans@footnotes 

  h?div [] (List.map renderSpan toRender)
    |> renderTo (document.getElementById id)

let sample = 
  "The **most important** F# keyword is `let`. For more " +
  "see [F# web site](http://www.fsharp.net)."

let advanced = 
  "The _most important_ F# keyword is `let`. For more " +
  "see [F# web site](http://www.fsharp.net). You might " +
  "also ^[People _are_ strange] appreciate^[when you're a stranger] the **awesome**^[people look **ugly**] F# logo^[when you're alone]: " +
  "![F# logo](http://fsharp.org/img/logo/fsharp256.png)."

parseSpans (List.ofSeq advanced)
|> renderDoc "doc"

// ----------------------------------------------------------------------------
// TASKS
// ----------------------------------------------------------------------------

// TASK #1: Try running the code using both of the versions of the parsers. To
// do this switch between `open Markdown.Parsers` and `open Markdown.Patterns`
// at the beginning of the file. This should not change the results. Make some
// minor change in both to check that you're using the right one!

// TASK #2: Finish the implementation of emhasis and images, so that the three 
// failing tests above pass. You can do this using both of the versions and 
// compare which one is nicer, or just pick one which you prefer :-). You 
// will also need to modify the `renderSpan` function in Part 2 and then
// you should be able to render the `advanced` sample document.

// TASK #3: Add footnotes to the domain model. A footnote is written as: 
// ^[body _can_ be formatted]. A footnote contains some body and is formatted 
// just as a number with the body in the footnote of the document. To render
// footnotes, you will need to collect all the footnotes (while rendering) 
// the document and append them to the end of the document.
