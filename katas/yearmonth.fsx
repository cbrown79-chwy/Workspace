let m = (System.DateTime.Now.Year * 100) + System.DateTime.Now.Month

[|199001 .. m|] 
    |> Array.filter (fun n -> 
                        let m = n%100
                        match m with 
                        | 0  -> false
                        | i when i > 12 -> false
                        | _ -> true )