let reverse list = 
    let rec collect a b = 
        match a with 
        | [] -> b::a
        | x::xs -> 