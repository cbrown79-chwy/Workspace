#r "Facades/netstandard"
#r "netstandard"

let vals = [1;2;3;4;5;6;7;8;9]
let add = (+)
let subtract = (-)
let concat = (fun x y -> System.Int32.Parse(sprintf "%d%d" x y))

// this is our order of operations
let operations = [concat;subtract;add]

// a simple renderer of operations to vals
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

// Given an array of ints, and an array of operations, and an operation to match,
// apply [1;2;3] a 1 = [1;5]
// apply [1;2;3] c 0 = [12;3]
let apply (intList : int list) op index = 
    if (index >= ((List.length intList) - 1)) then 
        intList 
    else
        let mappedOut = intList |> List.mapi (fun i n -> if i = index then Some (op n intList.[index + 1]) else if i = index + 1 then None else Some n) 
        mappedOut |> List.filter Option.isSome |> List.map Option.get

// list helper
let removeFromIndex i list = 
    let m = (List.length list) - 1
    list.[0..(i- 1)] @ list.[(i+1)..m]

let applyOperations (iList : int list) (ops : (int -> int -> int) list) =
    let rec keepApplying ints ops op = 
        let doApply il ol =
            let equalFunc (o : int -> int -> int) = o 1 1 = op 1 1
            let o = ops |> List.tryFindIndex equalFunc
            match o with 
            | Some i -> 
                let newInts = apply il op i
                let newOps = ol |> removeFromIndex i
                (newInts, newOps, true)
            | None -> (il, ol, false)    
        let (i, o, a) = doApply ints ops
        if a then keepApplying i o op
        else i, o 

    let mutable i = iList
    let mutable o = ops 

    for op in operations do
        let (i2, o2) = keepApplying i o op
        i <- i2
        o <- o2

    List.head i

// there are only 6561 possible results here... this is pretty painless to eval all options of.
let answer expectedResult = 
    let filter m = 
        let r = applyOperations vals m
        r = expectedResult
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

let rec split l =
    match l with 
    |[] -> []
    |(x1, x2)::xs -> [x1;x2]::(split xs)
// pow <=0 any = [[]]
// pow 1 [1;2] = [[1;2]]
// pow 2 [1;2] = [[1;1];[1;2];[2;1][2;2]]
// pow 3 [1;2] = [[1;1;1];[1;1;2];[1;2;1];[1;2;2];[2;1;1];[2;1;2];[2;2;1];[2;2;2]]
// pow 4 [1;2] = [[1;1;1;1];[1;1;1;2]....]
// this raises the list to a power
let pow iCount list = 
    // should be 3 or above
    let rec raze original p =
        
            

    match iCount with 
        | n when n <= 0 -> [[]]
        | 1 -> [list]
        | 2 -> List.allPairs list list |> split
        | n -> raze list n


