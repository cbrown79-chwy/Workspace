open System.Collections.Generic

let listToGroup = [(1, "Apple");(2, "Banana");(1, "Peach")]
let desiredResult = [(1, ["Apple";"Peach"]);(2, ["Banana"])]

type GroupItem<'a, 'b> = { Key : 'a; Values : 'b list }


// rule... iterate over listToGroup once to get to desiredResult.

let createGroup (i1, s1) (i2, s2) = 
    match (i1 = i2) with 
    | true -> [(i1, [s1;s2])]
    | false -> [(i1, [s1]);(i2, [s2])]


let gb l =
    let m = new Dictionary<int, string list> ()
    List.iter (fun (i, s) -> 
                        if m.ContainsKey i then m.[i] <- s::m.[i]
                        else m.Add ( i, [s] )
                   ) l
    m.Keys |> Seq.map (fun n -> (n, m.[n])) |> List.ofSeq

gb listToGroup = desiredResult    
