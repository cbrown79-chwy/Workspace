// ============================================================================
// SETUP: Some magic tricks for the script Walkthroughs!
// ============================================================================
module Markdown.Tests

let __<'T> : 'T =
  failwith "A place holder '__' has not been filled!"

let inline shouldEqual (a:'T) b =
  #if INTERACTIVE 
  let err msg = failwith msg
  #else
  let err msg = Fable.Import.Browser.console.error(msg)
  #endif
  if typeof<'T> = typeof<float> then
    let a = unbox<float> a
    let b = unbox<float> b
    if a > b - 0.0000001 && a < b + 0.0000001 then ()
    else err(sprintf "The 'shouldEqual' operation failed!\nFirst: %A\nSecond: %A" a b)
  else if not (a = b) then err(sprintf "The 'shouldEqual' operation failed!\nFirst: %A\nSecond: %A" a b)
