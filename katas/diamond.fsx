open System

let diamond char = 
    let space i = new string (' ', i)
    let availableChars = [ 'A' .. char ]
    let location = List.length availableChars
    let locCount i = location - (i + 1)
    let start = availableChars |> List.take (location - 1) |> List.mapi (fun i c -> c, locCount i)
    let en = (char,0) 
    let all = start @ (en :: List.rev start)
    let m = all |> List.map (fun (c, i) -> 
                       match (location * 2 - 1) - (i * 2 + 1) with 
                       | 0 -> space i + c.ToString() + space i
                       | a -> space i + c.ToString() + space (a - 1) + c.ToString() + space i)
    Environment.NewLine :: m |> List.reduce (fun x y -> sprintf "%s%s%s" x Environment.NewLine y)