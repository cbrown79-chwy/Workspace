open System


type Sector = | TopLeft = 1 | TopMiddle = 2 | TopRight = 3 | CenterLeft = 4 | CenterMiddle = 5 | CenterRight = 6 | BottomLeft = 7 | BottomMiddle = 8 | BottomRight = 9
type Position = { Rank : Rank; File : File }
    and File = | A = 1 | B = 2 | C = 3 | D = 4 | E = 5 | F = 6 | G = 7 | H = 8 | I = 9
    and Rank = | One = 1 | Two = 2 | Three = 3 | Four = 4 | Five = 5 | Six = 6 | Seven = 7 | Eight = 8 | Nine = 9
type Grid = Map<Position, int>
let validValues = set [1..9]

let sectorMap = Map.ofList [({Rank = Rank.One; File = File.A;}, Sector.TopLeft)
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
                            ({Rank = Rank.Nine;  File = File.I;}, Sector.BottomRight)]  

let lengthOfSecondItemInTuple e = (snd e) |> Array.length

let allPositions = [| for a in sectorMap.Keys do yield a |]
let positionsToCheck =
    let allPositionsBySector = allPositions |> Array.groupBy (fun n -> sectorMap.[n]) |> Map.ofArray                    
    allPositions |> Array.map (fun n -> (n, [for r in 1..9 do 
                                                yield { Rank = enum<Rank>(r); File = n.File } 
                                                yield { Rank = n.Rank; File = enum<File>(r); } ] 
                                                @ List.ofArray allPositionsBySector.[sectorMap.[n]] |> List.filter (fun m -> m <> n) |> Set.ofList )) |> Map.ofArray




let asString (g : Grid option) = 
    match g with 
    | None -> ""
    | Some grid -> let stringValues = allPositions |> Array.map (fun position -> if grid.[position] = 0 then "-" else sprintf "%i" grid.[position]) 
                   String.Join(",", stringValues)

// expect a string like "A1,B1,C1," (etc, till I9)
let parseGrid (puzzle : string) : Grid option = 
    let m = puzzle.Split(",".ToCharArray(), StringSplitOptions.None)
    let tryForValue (c : string) = 
        let success, value = System.Int32.TryParse(c)
        if success then value else 0
    let values = m |> Array.map tryForValue 
    if Array.length values <> 81 then None 
    else
        let gridAsList = Array.zip allPositions values
        Some (Map.ofArray gridAsList)

let renderGrid (grid : Grid option) = 
    match grid with 
    | None -> ""
    | Some g -> Environment.NewLine + String.Join("", [|
                        for r in 1..9 
                            do 
                                if r > 1 && r % 3 = 1 then yield "*********************************" + Environment.NewLine
                                for f in 1..9 
                                    do 
                                        let pos = { Rank = enum<Rank>(r) ;  File = enum<File>(f)}
                                        if f > 1 && f % 3 = 1 then yield " | "
                                        match g.[pos] with 
                                        | 0 -> yield " - "
                                        | m -> yield sprintf " %i " m
                                yield Environment.NewLine
                    |])


// given a grid, and a position, return an integer set of available values
let getAvailableValues (grid : Grid) position = 
    if grid.[position] > 0 then
        position, [|grid.[position]|]
    else 
        let currentlySelectedValues = positionsToCheck.[position] |> Set.map (fun p -> grid.[p])
        position, (Set.difference validValues currentlySelectedValues) |> Set.toArray


let isNotSolvable solutions = 
    if Array.isEmpty solutions then false 
    else 
        let test = solutions |> Array.tryFind (fun (_, b) -> Array.length b = 0)
        Option.isSome test

let isSolved solutions = 
    let verify filter  = solutions 
                            |> Array.filter filter 
                            |> Array.fold (fun f (_, a:int[]) -> a.[0]::f) [] 
                            |> Set.ofList 
                            |> Set.isSubset validValues
    let verifyRank p = verify (fun (a, _) -> a.Rank = p.Rank)
    let verifyFile p = verify (fun (a, _) -> a.File = p.File)
    let verifySector p = verify (fun (a, _) -> sectorMap[p] = sectorMap[a])

    let allAreSingleLength = Array.fold (fun a (_, b) -> a && (Array.length b = 1)) true solutions
    let test = allPositions |> Array.tryFind (fun a -> (not (verifyFile a || verifyRank a || verifySector a)))

    match test with 
    | None -> true && allAreSingleLength
    | _ -> false
    
// Solve a grid, if possible.
let solve grid =  
    let rec createSolution g = 
        async {
            let possibleValuesByPosition = allPositions 
                                            |> Array.map (fun n -> getAvailableValues g n) 

            if isNotSolvable possibleValuesByPosition then 
                return None
            elif isSolved possibleValuesByPosition then
                let mapToSingleValue (ps, m : int[]) = (ps, m.[0])
                let result = possibleValuesByPosition 
                                |> Array.map mapToSingleValue 
                                |> Map.ofArray
                return Some result
            else
                let moreThanOnePossibleAnswer e = (snd e) |> Array.length > 1
                let tests = possibleValuesByPosition |> Array.filter moreThanOnePossibleAnswer
                // we've run out of possible options if this is Len == 0
                if Array.length tests = 0 then return None
                else
                    let (ps, l) = tests |> Array.minBy lengthOfSecondItemInTuple
                    let notTheCurrentPosition key _ = key <> ps

                    let potentialAnswersCollectors = [for i in l do
                                                        let newGrid = (g |> Map.filter notTheCurrentPosition).Add(ps, i)
                                                        createSolution newGrid]
                    
                    let! result = Async.Choice potentialAnswersCollectors
                    return result
        }
    let solution = createSolution grid |> Async.RunSynchronously 
    solution

let parseAndSolve puzzleAsText =
    let parsed = parseGrid puzzleAsText
    if Option.isNone parsed then "Parse error."
    else
        let a = Option.get parsed |> solve
        
        if Option.isNone a then "No solution found."
        else sprintf "%s\n\nIn unknwon iterations." (renderGrid a)

// Easy puzzle.

let easyPuzzle = "0, 0, 0, 7, 0, 1, 0, 0, 0,
7, 3, 0, 0, 9, 0, 0, 0, 2,
5, 6, 1, 0, 0, 0, 4, 9, 0,
4, 7, 2, 3, 1, 8, 0, 0, 0,
0, 0, 0, 5, 0, 9, 0, 0, 0,
0, 0, 0, 4, 6, 7, 2, 8, 3,
0, 4, 6, 0, 0, 0, 1, 5, 9,
2, 0, 0, 0, 3, 0, 0, 7, 4,
0, 0, 0, 1, 0, 5, 0, 0, 0"

let hardPuzzle = "2,0,0,5,0,7,4,0,6,
0,0,0,0,3,1,0,0,0,
0,0,0,0,0,0,2,3,0,
0,0,0,0,2,0,0,0,0,
8,6,0,3,1,0,0,0,0,
0,4,5,0,0,0,0,0,0,
0,0,9,0,0,0,7,0,0,
0,0,6,9,5,0,0,0,2,
0,0,1,0,0,6,0,0,8"

let extremePuzzle = "0, 0, 0, 8, 0, 1, 0, 0, 0, 
0, 0, 0, 0, 0, 0, 4, 3, 0, 
5, 0, 0, 0, 0, 0, 0, 0, 0, 
0, 0, 0, 0, 7, 0, 8, 0, 0, 
0, 0, 0, 0, 0, 0, 1, 0, 0, 
0, 2, 0, 0, 3, 0, 0, 0, 0, 
6, 0, 0, 0, 0, 0, 0, 7, 5, 
0, 0, 3, 4, 0, 0, 0, 0, 0,
0, 0, 0, 2, 0, 0, 6, 0, 0"


let diabolical = "7,0,5,0,4,0,6,0,0,
3,8,0,0,0,0,0,0,0,
0,0,1,5,0,0,0,0,2,
0,1,0,9,5,2,3,0,6,
0,0,0,0,6,0,0,0,0,
9,0,2,7,3,8,0,1,0,
8,0,0,0,0,5,2,0,0,
0,0,0,0,0,0,0,5,3,
0,0,3,0,2,0,4,0,9";

let evilPuzzle = "0,0,7,4,0,5,0,0,1,
0,4,0,1,0,0,3,0,7,
6,0,0,0,0,9,0,0,0,
0,2,0,0,0,0,0,0,8,
0,9,0,0,0,0,0,3,0,
1,0,0,0,0,0,0,2,0,
0,0,0,6,0,0,0,0,4,
9,0,6,0,0,8,0,7,0,
5,0,0,9,0,7,2,0,0"

let hard2 = "0,3,7,5,0,0,0,0,0,
4,0,0,0,3,0,5,0,9,
0,0,5,0,0,0,0,8,0,
0,0,0,6,7,0,0,0,0,
6,7,1,0,0,0,2,5,4,
0,0,0,0,5,4,0,0,0,
0,4,0,0,0,0,9,0,0,
5,0,2,0,6,0,0,0,3,
0,0,0,0,0,5,4,7,0"