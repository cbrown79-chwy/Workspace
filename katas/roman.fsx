let convert iVal =  
    let vals = [(1000, "M");(900, "CM");(500, "D");(400, "CD");
                (100, "C");(90, "XC");(50, "L");(40, "XL");
                (10, "X");(9,"IX");(5,"V");(4,"IV");(1, "I")]
    let rec loop acc n list = 
        match list with 
            | [] -> acc + ""
            | (i, s)::xs when n >= i -> loop (acc + s) (n - i) list
            | x::xs -> loop acc n xs
    loop "" iVal vals