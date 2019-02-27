let printNumbers min max = 
    for m in min..max do
        printfn "%i" m

let printNumber floatVal = 
    printfn "Printing %f!" floatVal

let printMoreNumbers (minimum) (maximum) = 
    List.iter printNumber [minimum..0.45..maximum]


let printEvenMoreNumbers (minimum:decimal) (maximum:decimal) =
    [minimum..0.45m..maximum] |>
        List.filter (fun x -> System.Math.Floor x = x) |>
        List.iter (float >> printNumber)