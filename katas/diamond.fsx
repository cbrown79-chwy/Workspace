let availableChars = ['A' .. 'Z']

open System

let diamond char = 
    let location = (List.findIndex (fun a -> a = char) availableChars) + 1
    let locCount i = location - (i + 1)
    let start = availableChars |> List.takeWhile (fun a -> a <> char) |> List.mapi (fun i c -> c, locCount i)
    let en = (char,0) 
    let all = start @ (en :: List.rev start)
    let m = all |> List.map (fun (c, i) -> 
                       match (location * 2 - 1) - (i * 2 + 1) with 
                       | 0 -> new string(' ', i) + c.ToString() + new string(' ', i)
                       | a -> new string(' ', i) + c.ToString() + new string(' ', a - 1) + c.ToString() + new string(' ', i))
    let di = m |> List.reduce (fun x y -> sprintf "%s%s%s" x Environment.NewLine y)
    sprintf "%s%s%s" Environment.NewLine Environment.NewLine di