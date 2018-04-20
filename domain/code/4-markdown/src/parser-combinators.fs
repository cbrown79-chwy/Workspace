module Markdown.Parsers
open Markdown

// ----------------------------------------------------------------------------
// Common parser combinators
// ----------------------------------------------------------------------------

/// A parser takes a character list and either fails
/// or returns a parsed value together with remaining characters
type Parser<'T> = Parser of (char list -> option<'T * char list>)

// Parse the specified character
let char c = Parser(function x::xs when x = c -> Some(x, xs) | _ -> None)

/// Apply the first parser, followed by the second parser
let (<*>) (Parser p1) (Parser p2) = Parser(fun input ->
  p1 input |> Option.bind (fun (r1, rest) ->
    p2 rest |> Option.map (fun (r2, rest) -> (r1, r2), rest)))

/// Apply one or the other parser
let (<|>) (Parser p1) (Parser p2) = Parser(fun input ->
  match p1 input with
  | Some res -> Some res
  | _ -> p2 input)

/// Transform the result of parser using a given function
let map f (Parser p) = Parser(fun input -> 
  p input |> Option.map (fun (r, rest) -> f r, rest))


// ----------------------------------------------------------------------------
// Not common Markdown-parsing specific parsers
// ----------------------------------------------------------------------------

/// Apply the first parser that succeeds. Pass all unprocessed input to
/// 'otherwise', which can then stoer it as unprocessed literal
let collect parsers otherwise = Parser(fun input ->
  // Parsed values so far, with skipped characters
  // processed using the 'otherwise' function
  let accWithSkipped acc skipped =
    if List.isEmpty skipped then acc
    else (otherwise (List.rev skipped))::acc

  let rec loop skipped acc input =    
    // Try running some parser - if no parser succeeds, then
    // we just add the current character as 'skipped'
    match parsers |> Seq.tryPick (fun (Parser p) -> p input) with
    | Some(r2, []) -> Some(List.rev (r2::accWithSkipped acc skipped), [])
    | Some(r2, rest) -> loop [] (r2::accWithSkipped acc skipped) rest
    | None -> 
        match input with
        | [] -> Some((List.rev (accWithSkipped acc skipped)), [])
        | x::xs -> loop (x::skipped) acc xs
  loop [] [] input)
  
/// Apply the parser returned by 'f' to the result recognized by 'p'
let nest (Parser p) f = Parser(fun input ->
  match p input with 
  | Some(r1, rest) -> 
      let (Parser nested) = f ()
      nested r1 |> Option.map (fun (r2, _) -> r2, rest)
  | _ -> None)

// ----------------------------------------------------------------------------
// More specialized code for Markdown, similar to active patterns
// ----------------------------------------------------------------------------

// A parser that checks if the input starts with a given 
// 'prefix' (and returns the rest of the input)
let startsWith prefix = Parser(fun list ->
  let rec loop = function
    | [], rest -> Some((), rest)
    | p::prefix, r::rest when p = r -> loop (prefix, rest)
    | _ -> None
  loop (prefix, list))

/// Parse input until the specified parser succeeds
let parseUntil (Parser p) = Parser(fun input ->
  let rec loop acc input =
    match p input, input with 
    | Some((), rest), _ -> Some(List.rev acc, rest)
    | None, x::xs -> loop (x::acc) xs
    | None, [] -> None
  loop [] input)

/// Parse input until we reach 'closing' sub-list
let rec bracketedBody closing =
  parseUntil (startsWith closing)

/// Parse input that matches the scheme <opening> .* <closing>
let bracketed opening closing = 
  (startsWith opening <*> bracketedBody closing)
  |> map snd

// Implement an active pattern (using 'Bracketed')
// that tests whether input contains a sequence <delim> .* <delim>
let delimited delim = bracketed delim delim

// ----------------------------------------------------------------------------
// Markdown parser
// ----------------------------------------------------------------------------

let toString chars =
  System.String(chars |> Array.ofList)

let rec parse () =
  collect 
    [ // Hard line breaks ends with two spaces
      ( startsWith [' '; ' '; '\n'; '\r'] <|>
        startsWith [' '; ' '; '\n' ] <|>
        startsWith [' '; ' '; '\r' ] )
        |> map (fun _ -> HardLineBreak)
      // Inline code is delimited with `code`
      delimited ['`'] 
        |> map (fun body -> InlineCode(toString body))
      // Emphasis and strong are delimited and we
      // need to apply parser to the body using 'nest'
      (nest (delimited ['*';'*'] <|> delimited ['_';'_']) parse)
        |> map (fun body -> Strong(body))
      
      (nest (delimited ['_'] <|> delimited ['*']) parse)
        |> map (fun body -> Emphasis(body))

      // Recognize hyperlinks [some _text_](http://link)
      ((nest (bracketed ['['] [']']) parse) <*> bracketed ['('] [')'])
        |> map (fun (body, url) -> HyperLink(body, toString url)) 
      
      // Recognize Images ![some _text_](http://link)
      (bracketed ['!'; '['] [']']) <*> bracketed ['('] [')']
        |> map (fun (title, url) -> Image(toString title, toString url)) 
      
        
       ]
  
    (toString >> Literal)


let parseSpans input = 
  let (Parser p) = parse()
  p input |> Option.get |> fst
