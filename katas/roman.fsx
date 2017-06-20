let convert intValue = 
    let rec loop acc n = 
        match n with 
        | a when a >= 1000 -> loop (acc + "M") (n - 1000)
        | a when a >= 500 -> loop (acc + "D") (n - 500)
        | a when a >= 100 -> loop (acc + "C") (n - 100)
        | a when a >= 50 -> loop (acc + "L") (n - 50)
        | a when a >= 10 -> loop (acc + "X") (n - 10)
        | a when a >= 5 -> loop (acc + "V") (n - 5)
        | a when a >= 1 -> loop (acc + "I") (n - 1)
        | _ -> acc + ""
    loop "" intValue
    

let swap (inString : string) (stringToFind : string) swapString = 
    inString.Replace(stringToFind, swapString)