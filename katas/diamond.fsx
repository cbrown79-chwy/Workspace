open System

let diamond char = 
    let availableChars = [ 'A' .. char ]
    let location = List.length availableChars
    let gridWidth = (location * 2 - 1)
    let pad i = new string (' ', i)
    let makeTuple ch index = (string ch, index)
    let en = makeTuple char 0 
    let locCount i = location - (i + 1)
    let start = availableChars |> List.mapi (fun i c -> makeTuple c (locCount i))
    let all = start @ (List.rev start)
    let m = all |> List.map (fun (c, i) -> 
                       match gridWidth - (i * 2 + 1) with 
                       | 0 -> pad i + c + pad i
                       | a -> pad i + c + pad (a - 1) + c + pad i)
    Environment.NewLine :: m |> List.reduce (fun x y -> sprintf "%s%s%s" x Environment.NewLine y)