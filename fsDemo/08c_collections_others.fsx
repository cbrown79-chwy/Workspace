let array = [|1;2;3|]

// Arrays can change stuff.
array.[2] <- 35



let map = Map.empty
            .Add("AL", "Montgomery")
            .Add("AK", "Juneau")
            .Add("AZ", "Pheonix")
            .Add("AR", "Little Rock")
            // and on and on and on.

let whatsTheCapitalOf stateCode = 
    map.[stateCode]


let set = set ["Apple"; "Banana"; "Banana"]
// no dupes up in here



