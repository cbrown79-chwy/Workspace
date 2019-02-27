let listOfThings = [1;2;3]
let bigOleListOfThings = [1L .. 100000L]

let x = List.head listOfThings
let restOfIt = List.tail bigOleListOfThings

// List functions that are important

// List.iter = do something with each item on a list

List.iter (fun x -> printfn "%i" x) listOfThings

// List.map = take each item in a list and apply a function to it to make a new list.

List.map (fun x -> x * x + 3) listOfThings
List.map (fun x -> sprintf "%i = %i" x x) listOfThings


// List.fold --- fancy function that applies a function and an initial state to 'sum up' items in a list.

let sumOfItemAndAHalf a b = a + (b + b / 2L)

bigOleListOfThings |> List.fold sumOfItemAndAHalf 0L

