let convert intValue =  
    let vals = [(1000, "M");(900, "CM");(500, "D");
                (400, "CD");(100, "C");(90, "XC");(50, "L");
                (40, "XL");(10, "X");(9,"IX");(5,"V");(4,"IV");(1, "I")]
    let rec loop acc n list = 
        let f t v l = loop (acc + t) (n - v) l
        match list with 
            | (i, s)::xs when n >= i -> f s i list
            | x::xs -> f "" 0 xs
            | [] -> acc + ""
    loop "" intValue vals