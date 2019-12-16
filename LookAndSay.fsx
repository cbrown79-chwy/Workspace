(*
    
1 is read off as "one 1" or 11.
11 is read off as "two 1s" or 21.
21 is read off as "one 2, then one 1" or 1211.
1211 is read off as "one 1, one 2, then two 1s" or 111221.
111221 is read off as "three 1s, two 2s, then one 1" or 312211.

*)
type Item = { Char : char; Count : int }


let breakString (str : string) = 
    let l = String.length str
    if l = 1 then seq { yield { Char = str.[0]; Count = 1} }
    else        
        seq {    
            let mutable count =0           
            for m in 0..(l - 1) 
                do
                    if m + 1 = l then 
                        yield { Char = str.[m]; Count = count + 1; }
                    else if (str.[m] = str.[m + 1]) then
                        count <- count + 1
                    else if (str.[m] <> str.[m + 1]) then
                        count <- count + 1
                        yield { Char = str.[m]; Count = count; }
                        count <- 0
            }



let readOff (n: string) = 
    let sb = System.Text.StringBuilder()
    for m in (breakString n)
        do
            sb.Append ( m.Count ) |> ignore
            sb.Append  (m.Char)  |> ignore
    sb.ToString()
   
let generateLookAndSay (startWith:string) nth = 
    let rec iterate (vl:string) counter =
       match counter with 
       | n when n <= 1 -> vl
       | n -> 
            let r = iterate vl (n - 1)
            readOff r
    let first = readOff startWith
    if nth <= 1 then String.length first 
    else 

        let r = iterate first nth
        String.length r

