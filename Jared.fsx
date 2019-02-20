open System

// these may not be strictly necessary, but they do help describe
// precisely what data I'm dealing with
module UpperCaseString = 
    type UpperCaseString = UpperCaseString of string
    let private upper (a:string) =
        a.ToUpper()

    let create (a:string) = 
        if String.IsNullOrEmpty(a) then None
        else Some (UpperCaseString (upper a))
    let value (UpperCaseString s) =
        s
open UpperCaseString

module FloatingDecimalBetweenZeroAndOne = 
    type FloatingDecimalBetweenZeroAndOne = FloatingDecimalBetweenZeroAndOne of decimal

    let create (a:decimal) = 
        if a > Decimal.One || a < Decimal.Zero then 
            None
        else Some (FloatingDecimalBetweenZeroAndOne a)
    let value (FloatingDecimalBetweenZeroAndOne f) =
        f
    let make a = (create a |> Option.get)    
   
open FloatingDecimalBetweenZeroAndOne        

type Model = Model of UpperCaseString
    with static member Value (Model (UpperCaseString s)) = s
type Ticker = Ticker of UpperCaseString
    with static member Value (Ticker (UpperCaseString s)) = s
type SleevedHolding = { Ticker : Ticker; Model : Model; Allocation : FloatingDecimalBetweenZeroAndOne }
    with override x.ToString () = sprintf "%s,%s,%5M" (Ticker.Value x.Ticker) (Model.Value x.Model) (FloatingDecimalBetweenZeroAndOne.value x.Allocation)

type Holding = { Ticker : Ticker; Allocation : FloatingDecimalBetweenZeroAndOne }
type ModelAllocations = { Model : Model; Holdings : Holding list }
type ModelWeight = { Model : Model; Weight : FloatingDecimalBetweenZeroAndOne }

let makeModel str =
    UpperCaseString.create str |> Option.get |> Model  

    //  I kinda hate this. All that work to avoid bad data,
    //  but I have to shorthand functions to not be driven crazy in the REPL
    //  In practice, I wouldn't care.

let makeTicker str =
    UpperCaseString.create str |> Option.get |> Ticker


let holding str all =
    { Ticker = makeTicker str; Allocation = make all }

let makeSleeve t m a = { Ticker = t; Model = m; Allocation = a }
let modelWeight str a = { Model = makeModel str; Weight = make a }
let modelAllocation str h = { Model = makeModel str; Holdings = h }

// This is the simplest description of what we're doing.
// taking a list of holdings, a list of models and their holdings
// a list of weights to apply to the model, and returning a set of 'sleeved' holdings.
type SleeveProcess = Holding list -> ModelAllocations list -> ModelWeight list -> SleevedHolding list

let allocation ticker holding  = 
    match holding.Ticker = ticker with 
    | true -> Some holding.Allocation
    | false -> None

let findHolding ticker modelAllocations =
    let fi holdings = holdings |> List.choose (fun modelHolding -> allocation ticker modelHolding)
    let modelsWithTicker = modelAllocations |> List.where (fun x -> not (List.isEmpty (fi x.Holdings)))
    match modelsWithTicker with 
    | [] -> []
    | _ -> modelsWithTicker |> List.map (fun x -> (x.Model, (List.exactlyOne (fi x.Holdings))))
    

let sleeveHoldings holdings models weights =
    holdings 
        |> List.collect (fun h ->
                            let t = h.Ticker
                            let m = findHolding t models  // a list of models that hold the security
                            match m with 
                            | [] -> 
                                weights |> 
                                    List.map (fun w -> 
                                        let weightTimesAllocation = (value w.Weight * value h.Allocation)
                                        makeSleeve t w.Model (make weightTimesAllocation))
                            | [(x,_)] -> [makeSleeve t x h.Allocation]
                            | xs -> 
                                let weightsMap = weights |> List.map (fun n -> (n.Model, value n.Weight)) |> Map.ofList
                                let total = xs |> List.sumBy (fun (m, w) -> weightsMap.[m] * value (w))
                                xs |> List.map (fun (m, w) -> 
                                                    let weightTimesAllocationOverTotal = (value w * weightsMap.[m]) / total
                                                    makeSleeve t m (make (weightTimesAllocationOverTotal * value h.Allocation)))
                    )   
        

let testHoldings = [
        holding  "A" 0.20m;
        holding  "B" 0.20m;
        holding  "C" 0.20m;
        holding  "D" 0.20m; 
        holding  "E" 0.20m; ]
        
let testWeights =  [modelWeight "MODEL1" 0.5m; modelWeight "MODEL2" 0.3m;modelWeight "MODEL3" 0.2m;]

let testModelAllocations = [
    modelAllocation "MODEL1" [
            holding  "A" 1.0m;
    ];
    modelAllocation "MODEL2" [
            holding  "A" 0.5m;
            holding  "B" 0.3m;
            holding  "D" 0.2m;
    ];
    modelAllocation "MODEL3" [
            holding  "B" 0.5m;
            holding  "C" 0.3m;
            holding  "D" 0.2m;
    ]
]

sleeveHoldings testHoldings testModelAllocations testWeights |> List.map (fun m -> m.ToString ())
