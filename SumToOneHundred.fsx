#r "Facades/netstandard"
#r "netstandard"

let vals = [1;2;3;4;5;6;7;8;9]
let a = (+)
let s = (-)
let c = (fun x y -> (x * 10) + y)
let operations = [a;s;c]

let render ops expectedResult =
    let s op = 
        if op 1 1 = 2 then Some " + "
        else if op 1 1 = 0 then Some " - "
        else if op 1 1 = 11 then Some ""
        else None
    let strs = ops |> List.map s
    if List.length strs = ((List.length vals) - 1) && List.forall Option.isSome strs then 
        (vals |> List.mapi (fun i v -> match List.tryItem i strs with | Some a -> (string v) + Option.get a | None -> string v) |> List.reduce (+)) + " = " + (string expectedResult)
    else
        "Error."

let getSum ops = 
    


    
let test ops expectedResult = 
    getSum ops = expectedResult

// there are only 6561 possible results here... this is pretty painless to eval all options of.
let answer expectedResult = 
    let filter m = test m expectedResult
    let r t = render t expectedResult
    let allAnswers = [
                        for a in 0..2 do
                         for b in 0..2 do
                         for c in 0..2 do
                         for d in 0..2 do
                         for e in 0..2 do
                         for f in 0..2 do
                         for g in 0..2 do
                         for h in 0..2 do 
                            yield 
                                [operations.[a];operations.[b];operations.[c];operations.[d];operations.[e];operations.[f];operations.[g];operations.[h]]
                        ] 
    allAnswers |> 
        List.filter filter |> List.fold (fun s t -> sprintf "%s\r\n%s" s (r t)) "" 


