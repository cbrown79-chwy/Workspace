module Markdown.Patterns
open System
open Markdown

// ----------------------------------------------------------------------------
// Helper active patterns
// ----------------------------------------------------------------------------

// A parameterized active pattern that checks if 'list' 
// starts with a given 'prefix' (and returns
// the rest of the input)
let (|StartsWith|_|) prefix list =
  let rec loop = function
    | [], rest -> Some(rest)
    | p::prefix, r::rest when p = r -> loop (prefix, rest)
    | _ -> None
  loop (prefix, list)


/// Parse input until we reach 'closing' sub-list
let rec parseBracketedBody closing acc = function
  | StartsWith closing (rest) -> Some(List.rev acc, rest)
  | c::chars -> parseBracketedBody closing (c::acc) chars
  | _ -> None

/// Parse input that matches the scheme <opening> .* <closing> .*
let (|Bracketed|_|) opening closing = function
  | StartsWith opening chars -> parseBracketedBody closing [] chars
  | _ -> None

/// Implement an active pattern (using 'Bracketed')
/// that tests whether input contains a sequence: <delim> .* <delim> .*
let (|Delimited|_|) delim = 
  (|Bracketed|_|) delim delim 

// ----------------------------------------------------------------------------
// Markdown parser
// ----------------------------------------------------------------------------
    
let toString chars =
  System.String(chars |> Array.ofList)

let rec parse acc chars = seq {
  let counter, lst = acc
  // emit literal if we skipped some characters
  let emitLiteral() = seq {
    if lst <> [] then 
      yield lst |> List.rev |> toString |> Literal }

  // now we can use nice pattern matching to detect spans!
  match chars with
  | StartsWith [' '; ' '; '\n'; '\r'] chars
  | StartsWith [' '; ' '; '\n' ] chars
  | StartsWith [' '; ' '; '\r' ] chars -> 
      yield! emitLiteral ()
      yield HardLineBreak
      yield! parse (counter, []) chars
  | Delimited ['`'] (body, chars) ->
      yield! emitLiteral ()
      yield InlineCode(toString body)
      yield! parse [] chars
  | Delimited ['*'; '*' ] (body, chars)
  | Delimited ['_'; '_' ] (body, chars) ->
      yield! emitLiteral ()
      yield Strong(parse [] body |> List.ofSeq)
      yield! parse [] chars
  | Delimited ['*' ] (body, chars)
  | Delimited ['_' ] (body, chars) ->
      yield! emitLiteral ()
      yield Emphasis(parse [] body |> List.ofSeq)
      yield! parse [] chars
  | Bracketed ['['] [']'] (body, Bracketed ['('] [')'] (url, chars)) ->
      yield! emitLiteral ()
      yield HyperLink(parse [] body |> List.ofSeq, toString url)
      yield! parse [] chars
  | StartsWith ['!'] ( Bracketed ['['] [']'] (body, Bracketed ['('] [')'] (url, chars))) ->
      yield! emitLiteral ()
      yield Image(toString body, toString url)
      yield! parse [] chars
  | StartsWith ['^'] ( Bracketed ['['] [']'] (body, chars)) ->
      yield! emitLiteral ()
      yield Footnote(counter + 1, parse [] body |> List.ofSeq)
      yield! parse [] chars
  | c::chars ->
      yield! parse (c::acc) chars
  | [] ->
      yield! emitLiteral () }

let parseSpans input = parse (0, []) input 
                        |> List.ofSeq
