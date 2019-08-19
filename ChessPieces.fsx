type Square = Rank * File
    and Rank = | One = 1 | Two =2 | Three = 3| Four =  4 | Five = 5 | Six = 6 | Seven = 7 | Eight = 8
    and File = | A = 1 | B = 2 | C = 3 | D = 4 | E = 5 | F = 6 | G = 7 | H = 8

type Piece = | Pawn | King | Queen | Knight | Bishop | Rook 

type Color = | Black | White

type ChessMove = { Piece : Piece; Color : Color; StartingSquare : Square; DestinationSquare : Square; }

type Move = 
    | Standard of ChessMove
    | Promotion of ChessMove * Piece



module Parse =
    let swapToMap l = l |> List.map (fun (a, b) -> (b, a)) |> Map.ofList 

    let pieces =  [(King, 'K');(Queen, 'Q');(Rook, 'R');(Bishop, 'B');(Knight, 'N')]
    let ranks = [(Rank.One, '1');(Rank.Two, '2');(Rank.Three, '3');(Rank.Four, '4');(Rank.Five, '5');(Rank.Six, '6');(Rank.Seven, '7');(Rank.Eight, '8')]
    let files = [(File.A, 'a');(File.B, 'b');(File.C, 'c');(File.D, 'd');(File.E, 'e');(File.F, 'f');(File.G, 'g');(File.H, 'h')] 

    let fChars = files |> Map.ofList
    let charF = files |> swapToMap

    let rChars = ranks |> Map.ofList
    let charR = ranks |> swapToMap

    let pChars = pieces |> Map.ofList
    let charP = pieces |> swapToMap

    let printPiece piece = 
        match piece with 
        | Pawn -> ""
        | x -> pChars.[x].ToString()

    let render move = 
        let cstr color = if color = White then "" else "... "
        let sq s = sprintf "%c%c" fChars.[snd s] rChars.[fst s]
        match move with 
        | Standard cm -> 
            sprintf "%s%s%s" (cstr cm.Color) (printPiece cm.Piece) (sq cm.DestinationSquare)
        | Promotion (cm, p) -> 
            sprintf "%s%s=%s" (cstr cm.Color) (sq cm.DestinationSquare) (printPiece p)
           
    let pchar char (map:Map<char, 'a>) =
        if map.ContainsKey char then 
            Some map.[char]
        else None

    let move (str : string) =
        if str.Length = 3 then
            let p = pchar str.[0] charP
            let f = pchar str.[1] charF
            let r = pchar str.[2] charR
            (p, r, f)
        else if str.Length = 2 then
            let f = pchar str.[0] charF
            let r = pchar str.[1] charR
            (Some Pawn, r, f)
        else
            None, None, None

let asInts sq =
    let sr, sf = sq
    (int) sr, (int) sf

let ranksAndFiles startingSquare intTupleList  =     
    let squareIsValid sq (r, f) = r >= 1 && r <= 8 && f <= 8 && f >= 1 && ((r, f) <> asInts sq)
    let validSquare = squareIsValid startingSquare
    intTupleList 
        |> List.filter validSquare 
        |> List.map (fun (r, f) -> Square (enum<Rank>(r), enum<File>(f)))

let pawnMove color startingSquare = 
    let rank, file = startingSquare
    let newRank v = enum<Rank>((int rank) + v)
    let direction = if color = White then 1 else -1
    let templateMove = { Piece = Pawn; StartingSquare = startingSquare; Color = color; DestinationSquare = Square ( newRank direction, file) }

    match rank, color with
    | Rank.Seven, White | Rank.Two, Black -> 
        [Knight;Bishop;Rook;Queen] |> List.map (fun p -> (Promotion ( templateMove, p)))
    | Rank.Seven, Black | Rank.Two, White -> 
        [Standard (templateMove);Standard ({ templateMove with DestinationSquare = Square ( newRank (direction * 2), file) })]
    | Rank.One, _ | Rank.Eight, _ -> []
    | _ -> [Standard (templateMove)]


let pieceMove piece startingSquare color = 
    let r, f = asInts startingSquare
    let king r f = [ for v in -1 .. 1 do 
                     for v2 in -1 .. 1 do 
                        yield r + v, f + v2 ] 

    let knight r f = [(-1, -2);(-1, 2);(1, -2);(1, 2);(2, -1);(2, 1);(-2, -1);(-2, 1)] 
                        |> List.map (fun (a, b) -> r + a, f + b)

    let rook r f =  [for v in 1 .. 8 do
                        yield v, f
                        yield r, v]
    let bishop r f =  [for v in 1 .. 7 do
                        yield (r + v, f + v) 
                        yield (r + v, f - v) 
                        yield (r - v, f + v) 
                        yield (r - v, f - v)]

    let queen r f =  rook r f @ 
                     bishop r f

    let movesMap = [(King, king);(Knight, knight);(Rook, rook);(Bishop, bishop);(Queen, queen)] |> Map.ofList
    movesMap.[piece] r f
            |> ranksAndFiles startingSquare 
            |> List.map (fun d -> (Standard { Piece = piece; DestinationSquare = d; StartingSquare = startingSquare; Color = color }))

let getAvailableMoves pieceAndSquare color =
    match (Parse.move pieceAndSquare) with 
    | Some p, Some r, Some f -> 
        let moves = if p = Pawn then pawnMove color (r, f) else pieceMove p (r, f) color
        Ok (moves |> List.map Parse.render |> List.sort )
    | None, _, _  | _, None, _ | _, _, None -> Error "Invalid Starting Piece"

let kd1 = getAvailableMoves "Kd1" White // should be OK [ "Kc1"; "Kc2"; "Kd2"; "Ke1"; "Ke2";]
let e4 = getAvailableMoves "e4" Black // should be OK [ "... e3" ]
let e2W = getAvailableMoves "e2" White // should be OK [ "e3"; "e4" ]
let e2B = getAvailableMoves "e2" Black // should be OK [ "... e1=B"; "... e1=N";"... e1=Q";"... e1=R" ]

let nh2 = getAvailableMoves "Nh2" Black // should be OK [ "... Nf1"; "... Nf3"; "... Ng4" ]


let e = getAvailableMoves "Ah2" Black // should be Error "Invalid Starting Piece"
