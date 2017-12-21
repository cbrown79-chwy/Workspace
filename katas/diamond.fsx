open System

let diamond char = 
    let chars = [ 'A' .. char ]
    let numberOfChars = List.length chars
    let padCount i = numberOfChars - (i + 1)
    let gridWidth = numberOfChars * 2 - 1
    let pad i = new string (' ', i)
    let init = chars |> List.mapi (fun i c -> (string c), (padCount i))
    let all = init @ (List.tail (List.rev init))
    let makeLine (str,padCount) = 
        match gridWidth - (padCount * 2 + 1) with
        | 0 -> pad padCount + str + pad padCount
        | a -> pad padCount + str + pad (a - 1) + str + pad padCount
    all |> List.map makeLine
        |> List.reduce (fun x y -> sprintf "%s%s%s" x Environment.NewLine y)

