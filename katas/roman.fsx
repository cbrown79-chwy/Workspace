let vals = [(1000, "M");(900, "CM");(500, "D");(400, "CD");(100, "C");
            (90, "XC");(50, "L");(40, "XL");(10, "X");(9,"IX");(5,"V");(4,"IV");(1, "I")]
     
let convert intValue =  
    let rec loop acc n = 
        let f t v = loop (acc + t) (n - v)
        match n with 
        | a when a >= 1000 -> f "M" 1000
        | a when a >= 900 -> f "CM" 900
        | a when a >= 500 -> f "D" 500
        | a when a >= 400 -> f "CD" 400
        | a when a >= 100 -> f "C" 100
        | a when a >= 90 -> f "XC" 90
        | a when a >= 50 -> f "L" 50
        | a when a >= 40 -> f "XL" 40
        | a when a >= 10 -> f "X" 10
        | a when a >= 9 -> f "IX" 9
        | a when a >= 5 -> f "V" 5
        | a when a >= 4 -> f "IV" 4
        | a when a >= 1 -> f "I" 1
        | _ -> acc + ""
    loop "" intValue
