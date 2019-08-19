open System

type Sector = | TopLeft = 1 | TopMiddle = 2 | TopRight = 3 | CenterLeft = 4 | CenterMiddle = 5 | CenterRight = 6 | BottomLeft = 7 | BottomMiddle = 8 | BottomRight = 9
type Position = { Rank : Rank; File : File }
    and File = | A = 1 | B = 2 | C = 3 | D = 4 | E = 5 | F = 6 | G = 7 | H = 8 | I = 9
    and Rank = | One = 1 | Two = 2 | Three = 3 | Four = 4 | Five = 5 | Six = 6 | Seven = 7 | Eight = 8 | Nine = 9
type Grid = Map<Position, int>


let lengthOfSecondItemInTuple e = (snd e) |> Array.length

let allPositions = [for r in 1..9 do for f in 1..9 do yield { Rank = enum<Rank>(r) ;  File = enum<File>(f)}]
let positionsToCheck =
    let sectorMap =[({Rank = Rank.One; File = File.A;}, Sector.TopLeft)
                    ({Rank = Rank.One; File = File.B;}, Sector.TopLeft)
                    ({Rank = Rank.One;  File = File.C;}, Sector.TopLeft)
                    ({Rank = Rank.One;  File = File.D;}, Sector.TopMiddle)
                    ({Rank = Rank.One;  File = File.E;}, Sector.TopMiddle)
                    ({Rank = Rank.One;  File = File.F;}, Sector.TopMiddle)
                    ({Rank = Rank.One;  File = File.G;}, Sector.TopRight)
                    ({Rank = Rank.One;  File = File.H;}, Sector.TopRight)
                    ({Rank = Rank.One;  File = File.I;}, Sector.TopRight)
                    ({Rank = Rank.Two;  File = File.A;}, Sector.TopLeft)
                    ({Rank = Rank.Two;  File = File.B;}, Sector.TopLeft)
                    ({Rank = Rank.Two;  File = File.C;}, Sector.TopLeft)
                    ({Rank = Rank.Two;  File = File.D;}, Sector.TopMiddle)
                    ({Rank = Rank.Two;  File = File.E;}, Sector.TopMiddle)
                    ({Rank = Rank.Two;  File = File.F;}, Sector.TopMiddle)
                    ({Rank = Rank.Two;  File = File.G;}, Sector.TopRight)
                    ({Rank = Rank.Two;  File = File.H;}, Sector.TopRight)
                    ({Rank = Rank.Two;  File = File.I;}, Sector.TopRight)
                    ({Rank = Rank.Three;  File = File.A;}, Sector.TopLeft)
                    ({Rank = Rank.Three;  File = File.B;}, Sector.TopLeft)
                    ({Rank = Rank.Three;  File = File.C;}, Sector.TopLeft)
                    ({Rank = Rank.Three;  File = File.D;}, Sector.TopMiddle)
                    ({Rank = Rank.Three;  File = File.E;}, Sector.TopMiddle)
                    ({Rank = Rank.Three;  File = File.F;}, Sector.TopMiddle)
                    ({Rank = Rank.Three;  File = File.G;}, Sector.TopRight)
                    ({Rank = Rank.Three;  File = File.H;}, Sector.TopRight)
                    ({Rank = Rank.Three;  File = File.I;}, Sector.TopRight)
                    ({Rank = Rank.Four;  File = File.A;}, Sector.CenterLeft)
                    ({Rank = Rank.Four;  File = File.B;}, Sector.CenterLeft)
                    ({Rank = Rank.Four;  File = File.C;}, Sector.CenterLeft)
                    ({Rank = Rank.Four;  File = File.D;}, Sector.CenterMiddle)
                    ({Rank = Rank.Four;  File = File.E;}, Sector.CenterMiddle)
                    ({Rank = Rank.Four;  File = File.F;}, Sector.CenterMiddle)
                    ({Rank = Rank.Four;  File = File.G;}, Sector.CenterRight)
                    ({Rank = Rank.Four;  File = File.H;}, Sector.CenterRight)
                    ({Rank = Rank.Four;  File = File.I;}, Sector.CenterRight)
                    ({Rank = Rank.Five;  File = File.A;}, Sector.CenterLeft)
                    ({Rank = Rank.Five;  File = File.B;}, Sector.CenterLeft)
                    ({Rank = Rank.Five;  File = File.C;}, Sector.CenterLeft)
                    ({Rank = Rank.Five;  File = File.D;}, Sector.CenterMiddle)
                    ({Rank = Rank.Five;  File = File.E;}, Sector.CenterMiddle)
                    ({Rank = Rank.Five;  File = File.F;}, Sector.CenterMiddle)
                    ({Rank = Rank.Five;  File = File.G;}, Sector.CenterRight)
                    ({Rank = Rank.Five;  File = File.H;}, Sector.CenterRight)
                    ({Rank = Rank.Five;  File = File.I;}, Sector.CenterRight)
                    ({Rank = Rank.Six;  File = File.A;}, Sector.CenterLeft)
                    ({Rank = Rank.Six;  File = File.B;}, Sector.CenterLeft)
                    ({Rank = Rank.Six;  File = File.C;}, Sector.CenterLeft)
                    ({Rank = Rank.Six;  File = File.D;}, Sector.CenterMiddle)
                    ({Rank = Rank.Six;  File = File.E;}, Sector.CenterMiddle)
                    ({Rank = Rank.Six;  File = File.F;}, Sector.CenterMiddle)
                    ({Rank = Rank.Six;  File = File.G;}, Sector.CenterRight)
                    ({Rank = Rank.Six;  File = File.H;}, Sector.CenterRight)
                    ({Rank = Rank.Six;  File = File.I;}, Sector.CenterRight)
                    ({Rank = Rank.Seven;  File = File.A;}, Sector.BottomLeft)
                    ({Rank = Rank.Seven;  File = File.B;}, Sector.BottomLeft)
                    ({Rank = Rank.Seven;  File = File.C;}, Sector.BottomLeft)
                    ({Rank = Rank.Seven;  File = File.D;}, Sector.BottomMiddle)
                    ({Rank = Rank.Seven;  File = File.E;}, Sector.BottomMiddle)
                    ({Rank = Rank.Seven;  File = File.F;}, Sector.BottomMiddle)
                    ({Rank = Rank.Seven;  File = File.G;}, Sector.BottomRight)
                    ({Rank = Rank.Seven;  File = File.H;}, Sector.BottomRight)
                    ({Rank = Rank.Seven;  File = File.I;}, Sector.BottomRight)
                    ({Rank = Rank.Eight;  File = File.A;}, Sector.BottomLeft)
                    ({Rank = Rank.Eight;  File = File.B;}, Sector.BottomLeft)
                    ({Rank = Rank.Eight;  File = File.C;}, Sector.BottomLeft)
                    ({Rank = Rank.Eight;  File = File.D;}, Sector.BottomMiddle)
                    ({Rank = Rank.Eight;  File = File.E;}, Sector.BottomMiddle)
                    ({Rank = Rank.Eight;  File = File.F;}, Sector.BottomMiddle)
                    ({Rank = Rank.Eight;  File = File.G;}, Sector.BottomRight)
                    ({Rank = Rank.Eight;  File = File.H;}, Sector.BottomRight)
                    ({Rank = Rank.Eight;  File = File.I;}, Sector.BottomRight)
                    ({Rank = Rank.Nine;  File = File.A;}, Sector.BottomLeft)
                    ({Rank = Rank.Nine;  File = File.B;}, Sector.BottomLeft)
                    ({Rank = Rank.Nine;  File = File.C;}, Sector.BottomLeft)
                    ({Rank = Rank.Nine;  File = File.D;}, Sector.BottomMiddle)
                    ({Rank = Rank.Nine;  File = File.E;}, Sector.BottomMiddle)
                    ({Rank = Rank.Nine;  File = File.F;}, Sector.BottomMiddle)
                    ({Rank = Rank.Nine;  File = File.G;}, Sector.BottomRight)
                    ({Rank = Rank.Nine;  File = File.H;}, Sector.BottomRight)
                    ({Rank = Rank.Nine;  File = File.I;}, Sector.BottomRight)] |> Map.ofList
    let allPositionsBySector = allPositions |> List.groupBy (fun n -> sectorMap.[n]) |> Map.ofList                    
    allPositions |> List.map (fun n -> (n, [for r in 1..9 do 
                                                yield { Rank = enum<Rank>(r); File = n.File } 
                                                yield { Rank = n.Rank; File = enum<File>(r); } ] 
                                                @ allPositionsBySector.[sectorMap.[n]] |> List.filter (fun m -> m <> n) |> Set.ofList )) |> Map.ofList


let asString (g : Grid option) = 
    match g with 
    | None -> ""
    | Some grid -> let stringValues = allPositions |> List.map (fun position -> if grid.[position] = 0 then "-" else sprintf "%i" grid.[position]) |> Array.ofList
                   String.Join(",", stringValues)

// expect a string like "A1,B1,C1," (etc, till I9)
let parseGrid (puzzle : string) : Grid option = 
    let m = puzzle.Split(",".ToCharArray(), System.StringSplitOptions.None)
    let tryForValue c = 
        let success, value = System.Int32.TryParse(c)
        if success then value else 0
    let values = Array.toList m |> List.map tryForValue 
    if List.length values <> 81 then None 
    else
        let gridAsList = List.zip allPositions values 
        Some (Map.ofList gridAsList)

let renderGrid (grid : Grid option) = 
    match grid with 
    | None -> ""
    | Some g -> System.Environment.NewLine + String.Join("", [|
                        for r in 1..9 
                            do 
                                if r > 1 && r % 3 = 1 then yield "*********************************" + System.Environment.NewLine
                                for f in 1..9 
                                    do 
                                        let pos = { Rank = enum<Rank>(r) ;  File = enum<File>(f)}
                                        if f > 1 && f % 3 = 1 then yield " | "
                                        match g.[pos] with 
                                        | 0 -> yield " - "
                                        | m -> yield sprintf " %i " m
                                yield System.Environment.NewLine
                    |])


// given a grid, and a position, return an integer set of available values
// if the list is empty, the grid cannot be solved with the numbers as they are 
let getAvailableValues (grid : Grid) position = 
    if grid.[position] > 0 then
        position, [|grid.[position]|]
    else 
        let currentlySelectedValues = positionsToCheck.[position] |> Set.map (fun p -> grid.[p])
        let availableValues = set [1..9]
        position, (Set.difference availableValues currentlySelectedValues) |> Set.toArray


let isNotSolvable solutions = 
    if List.isEmpty solutions then false 
    else 
        let test = solutions |> List.minBy lengthOfSecondItemInTuple
        snd test |> Array.isEmpty

let isSolved solutions = 
    if List.isEmpty solutions then false 
    else 
        let test = solutions |> List.maxBy lengthOfSecondItemInTuple
        snd test |> Array.length = 1

// Solve a grid, if possible.
let solve grid =  
    let mutable iterationCounter = 0L
    let rec createSolution g = 
        iterationCounter <- iterationCounter + 1L
        let possibleValuesByPosition = allPositions 
                                        |> List.map (fun n -> getAvailableValues g n) 

        if isNotSolvable possibleValuesByPosition then 
            None
        elif isSolved possibleValuesByPosition then
            let mapToSingleValue (ps, m : int[]) = (ps, m.[0])
            let result = possibleValuesByPosition |> List.map mapToSingleValue |> Map.ofList
            Some result
        else
            let moreThanOnePossibleAnswer e = (snd e) |> Array.length > 1
            let (ps, l) = possibleValuesByPosition |> List.filter moreThanOnePossibleAnswer |> List.minBy lengthOfSecondItemInTuple
            let notTheCurrentPosition key _ = key <> ps
            let possibleAnswers = seq [for i in l do 
                                            let newGrid = (g |> Map.filter notTheCurrentPosition).Add(ps, i)
                                            let possibleSolution = createSolution newGrid
                                            if Option.isSome (possibleSolution) then yield (Option.get possibleSolution)
                                    ]
            if Seq.isEmpty possibleAnswers then 
                None 
            else 
                Some (Seq.head possibleAnswers)
    createSolution grid, iterationCounter


            

    

