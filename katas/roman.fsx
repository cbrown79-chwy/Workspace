let convert intValue =  
    let rec loop acc n = 
        let q t v = loop (acc + t) (n - v)
        match n with 
        | a when a >= 1000 -> q "M" 1000
        | a when a >= 900 -> q "CM" 900
        | a when a >= 500 -> q "D" 500
        | a when a >= 400 -> q "CD" 400
        | a when a >= 100 -> q "C" 100
        | a when a >= 90 -> q "XC" 90
        | a when a >= 50 -> q "L" 50
        | a when a >= 40 -> q "XL" 40
        | a when a >= 10 -> q "X" 10
        | a when a >= 9 -> q "IX" 9
        | a when a >= 5 -> q "V" 5
        | a when a >= 4 -> q "IV" 4
        | a when a >= 1 -> q "I" 1
        | _ -> acc + ""
    loop "" intValue
