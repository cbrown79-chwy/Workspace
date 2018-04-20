// ============================================================================
// SETUP: Some magic tricks for the script Walkthroughs!
// ============================================================================

let __<'T> : 'T =
  failwith "A place holder '__' has not been filled!"

let shouldEqual (a:'T) b =
  if typeof<'T> = typeof<float> then
    let a = unbox<float> a
    let b = unbox<float> b
    if a > b - 0.0000001 && a < b + 0.0000001 then ()
    else failwithf "The 'shouldEqual' operation failed!\nFirst: %A\nSecond: %A" a b
  else if not (a = b) then failwithf "The 'shouldEqual' operation failed!\nFirst: %A\nSecond: %A" a b
