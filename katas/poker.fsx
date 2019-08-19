open System

type Suit = | Clubs = 0 | Diamonds = 1 | Hearts = 2 | Spades = 3

type Rank = | Two = 2 | Three = 3 | Four = 4 | Five = 5 
            | Six = 6 | Seven = 7 | Eight = 8 | Nine = 9 | Ten = 10
            | Jack = 11 | Queen = 12 | King = 13 | Ace = 14

type Card = { Rank : Rank; Suit: Suit }

type PokerHand = 
    | StraightFlush of HighestCard : Rank
    | Quads of QuadsRank : Rank * Kicker : Rank
    | FullHouse of TripsRank : Rank * PairRank : Rank
    | Flush of Card1 : Rank * Card2 : Rank * Card3 : Rank * Card4 : Rank * Card5 : Rank
    | Straight of HighestCard : Rank
    | Trips of TripsRank : Rank * Kickers : (Rank * Rank)
    | TwoPair of HighPairRank : Rank * SecondPairRank : Rank * Kicker: Rank
    | Pair of PairRank : Rank * Kickers : (Rank * Rank * Rank)
    | HighCard of Kickers : (Rank * Rank * Rank * Rank * Rank)

let ranks = 
    [('2', Rank.Two)
     ('3', Rank.Three)
     ('4', Rank.Four)
     ('5', Rank.Five)
     ('6', Rank.Six)
     ('7', Rank.Seven)
     ('8', Rank.Eight)
     ('9', Rank.Nine)
     ('T', Rank.Ten)
     ('J', Rank.Jack)
     ('Q', Rank.Queen)
     ('K', Rank.King)
     ('A', Rank.Ace)]

let suits = 
    [('c', Suit.Clubs)
     ('d', Suit.Diamonds)
     ('h', Suit.Hearts)
     ('s', Suit.Spades)]

let handRank = function 
        | StraightFlush _ -> 8 | Quads _ -> 7 | FullHouse _ -> 6 
        | Flush _ -> 5 | Straight _ -> 4 | Trips _ -> 3 
        | TwoPair _ -> 2 | Pair _ -> 1  | HighCard _ -> 0

let swap (a, b) =
    (b, a)

let charRanks = Map.ofList ranks
let rankChars = ranks |> List.map swap |> Map.ofList 
let charSuits = Map.ofList suits
let suitChars = suits |> List.map swap |> Map.ofList

let toList<'a> = [for i in System.Enum.GetValues(typedefof<'a>) 
                   do yield i] |> 
                      List.map (fun n -> downcast n : 'a )

let deck = List.allPairs toList toList |> 
           List.map (fun (s,r) -> { Suit = s; Rank = r; })

let parse (s:string) = 
    let chars = s.ToCharArray()
    if Array.length chars <> 2 then None
    else
        let rc = Char.ToUpper chars.[0]
        let sc = Char.ToLower chars.[1]
        let r, s = charRanks.TryFind rc, charSuits.TryFind sc
        match r, s with 
        | Some a, Some b -> Some { Rank = a; Suit = b }
        | _ -> None

let parseHand (s:string) =
    s.Split([|' '|], StringSplitOptions.RemoveEmptyEntries) 
        |> Array.map parse
        |> Array.toList

let render c = sprintf "%c%c" rankChars.[c.Rank] suitChars.[c.Suit] 


let compare hand1 hand2 = 
    let handRank = function 
        | StraightFlush _ -> 8 | Quads _ -> 7 | FullHouse _ -> 6 | Flush _ -> 5 | Straight _ -> 4  
        | Trips _ -> 3 | TwoPair _ -> 2 | Pair _ -> 1  | HighCard _ -> 0  
    let compareRanks x y = 
        if x > y then 1
        else if y > x then -1
        else 0
    match hand1, hand2 with 
    | (x, y) 
        when (handRank x) - (handRank y) <> 0 -> sign ((handRank x) - (handRank y))
    | (Quads (c, _), Quads (c2, _))
    | (FullHouse (c, _), FullHouse (c2, _))
    | (Flush (c,_, _ ,_ ,_), Flush (c2,_, _ ,_ ,_)) | (Flush (_,c, _ ,_ ,_), Flush (_,c2, _ ,_ ,_)) 
    | (Flush (_,_,c,_,_), Flush (_,_,c2,_,_)) | (Flush (_,_, _ ,c ,_), Flush (_,_, _ ,c2,_)) 
    | (Trips (c,_), Trips (c2, _)) | (Trips (_,(c,_)), Trips(_,(c2,_)))
    | (TwoPair (c, _, _), TwoPair (c2, _ , _)) | (TwoPair (_, c, _), TwoPair (_, c2 , _)) 
    | (Pair (c, _), Pair (c2, _)) | (Pair (_, (c, _, _)), Pair (_, (c2, _, _)))  
    | (Pair (_, (_, c, _)), Pair (_, (_, c2,  _)))
    | (HighCard (c, _, _, _, _), HighCard (c2, _, _, _, _)) | (HighCard (_, c, _, _, _), HighCard (_, c2, _, _, _))
    | (HighCard (_, _, c, _, _), HighCard (_, _, c2, _, _)) | (HighCard (_, _, _, c, _), HighCard (_, _, _, c2, _))
        when compareRanks c c2 <> 0 
            -> compareRanks c c2
    | (StraightFlush c, StraightFlush c2) 
    | (Straight c, Straight c2)
    | (Quads (_, c), Quads (_, c2))
    | (FullHouse (_, c), FullHouse (_, c2))
    | (Flush (_,_, _ ,_ ,c), Flush (_,_, _ ,_ ,c2))
    | (Trips (_,(_,c)), Trips(_,(_,c2))) 
    | (TwoPair (_, _, c), TwoPair (_, _, c2)) 
    | (Pair (_, (_, _, c)), Pair (_, (_, _, c2)))
    | (HighCard (_, _, _, _, c), HighCard (_, _, _, _, c2))
        -> compareRanks c c2
    | _ -> 0        

let getHand card1 card2 card3 card4 card5 = 
    let ranks = [card1.Rank 
                 card2.Rank
                 card3.Rank
                 card4.Rank
                 card5.Rank] |>
                 List.groupBy id |> 
                 List.sortByDescending (fun (m,n) -> (List.length n) * 100 + (int) m);
    match List.length ranks with
    | 4 -> // pair
        Pair (fst ranks.Head, (fst ranks.[1], fst ranks.[2], fst ranks.[3]))
    | 2 -> // quads or fullhouse
        if(List.length (snd ranks.Head) = 4) then
            Quads (fst ranks.Head, fst ranks.[1])
        else
            FullHouse (fst ranks.Head, fst ranks.[1])
    | 3 -> // trips or twopair
        if(List.length (snd ranks.Head) = 3) then
            Trips (fst ranks.Head, (fst ranks.[1], fst ranks.[2]))
        else
            TwoPair (fst ranks.Head, fst ranks.[1], fst ranks.[2])
    | _ -> // straight flush, flush, straight or high
        let r = [card1.Rank; card2.Rank; card3.Rank; card4.Rank; card5.Rank] 
                |> List.sortByDescending id
        let suits = [card1.Suit
                     card2.Suit
                     card3.Suit
                     card4.Suit
                     card5.Suit] |> 
                     List.distinct
        match (List.length suits, r) with
        | (1, Rank.Ace::Rank.Five::_) -> StraightFlush Rank.Five
        | (1, x::xs) when x - (List.last xs) = Rank.Four -> StraightFlush x
        | (1, _) -> Flush (r.[0], r.[1], r.[2], r.[3], r.[4])
        | (_, Rank.Ace::Rank.Five::_) -> Straight Rank.Five
        | (_, x::xs) when x - (List.last xs) = Rank.Four -> Straight x
        | _ -> HighCard (r.[0], r.[1], r.[2], r.[3], r.[4])

let gh (l:Card list) =
    getHand l.[0] l.[1] l.[2] l.[3] l.[4]

let renderHand hand =   
    let ph = gh hand
    let orderCards cards = 
        match ph with 
        | (Quads (q, _)) -> (cards |> List.find (fun n -> n.Rank <> q)) :: (cards |> List.where (fun n -> n.Rank = q)) |> List.rev
        | (FullHouse (c, c2)) -> (cards |> List.where (fun n -> n.Rank = c)) @ (cards |> List.where (fun n -> n.Rank = c2))
        | (Trips (c, (k1, k2))) -> (cards |> List.find (fun n -> n.Rank = k2)) :: (cards |> List.find (fun n -> n.Rank = k1)) :: (cards |> List.where (fun n -> n.Rank = c)) |> List.rev
        | (TwoPair (c1, c2, k2)) -> (cards |> List.find (fun n -> n.Rank = k2)) :: (cards |> List.where (fun n -> n.Rank = c2)) @ (cards |> List.where (fun n -> n.Rank = c1)) |> List.rev
        | (Pair (c, (k1, k2, k3))) -> (cards |> List.find (fun n -> n.Rank = k3)) :: (cards |> List.find (fun n -> n.Rank = k2)) :: (cards |> List.find (fun n -> n.Rank = k1)) :: (cards |> List.where (fun n -> n.Rank = c)) |> List.rev
        | StraightFlush _ | Flush _ | Straight _ | HighCard _ | _ -> cards |> List.sortByDescending (fun a -> a.Rank)
    let s = hand |> orderCards |> List.map render |> List.fold (fun a b -> a + b + " ") "" 
    s.TrimEnd()

let getRandomHand (r:System.Random) = 
    let rec makeHand length list = 
        let m = r.Next(0, 52)
        if (List.contains m list) then
            makeHand length list
        else
            let newList = m::list
            if (List.length newList = length) then
                newList
            else
                makeHand length newList
    makeHand 5 [] |> List.map (fun n -> deck.[n])
    
let pokerHandToString = function 
        | StraightFlush x -> sprintf "%O high Straight Flush" x 
        | (Quads (q, k)) -> sprintf "Quad %Os with a %O kicker" q k 
        | (FullHouse (t, p)) -> sprintf "%Os Full of %Os" t p 
        | (Flush (x, _, _, _, _)) -> sprintf "%O high Flush" x 
        | Straight x -> sprintf "%O high Straight" x 
        | (Trips (x, _)) -> sprintf "Trip %Os" x
        | TwoPair (c, c2, _) -> sprintf "%Os and %Os" c c2 
        | (Pair (c, _)) -> sprintf "Pair of %Os" c 
        | HighCard (x, _, _, _, _) -> sprintf "%O high" x 

let eval h1 h2 =
    let s1, s2 = renderHand h1, renderHand h2
    let ph1, ph2 = gh h1, gh h2
    let result = compare ph1 ph2
    
    if result = 1 then sprintf "Hand 1 is the winner with \r\n %s: %s \r\n %s %s" (ph1 |> pokerHandToString) s1 (ph2 |> pokerHandToString) s2 
    else if result = -1 then sprintf "Hand 2 is the winner with \r\n %s: %s \r\n %s %s" (ph2 |> pokerHandToString) s2 (ph1 |> pokerHandToString) s1 
    else sprintf "Hands are tied! \r\n %s: %s \r\n %s %s" (ph1 |> pokerHandToString) s1 (ph2 |> pokerHandToString) s2 

let evalMultiHands i =
    let r = System.Random()
    let genHands = List.init i (fun a -> getRandomHand r)
    let folder highestHandsEvaluated handToBeEvaluated =
        match highestHandsEvaluated with
        | [] -> [handToBeEvaluated]
        | x::xs -> 
            let hand1 = gh handToBeEvaluated
            let hand2 = gh x
            let result = compare hand1 hand2 
            if result = 0 then x::handToBeEvaluated::xs
            else if result = 1 then [handToBeEvaluated]
            else [x]
    let winningHands = genHands |> List.fold folder []
    let init = genHands |> List.map renderHand
    let winning = winningHands |> List.map renderHand
    let winningText = List.head winningHands |> gh |> pokerHandToString
    let finalList = "Given hands \r\n" :: init @ "\r\nThe following hands were the winner" :: winning @ [winningText]
    let finalArray = finalList |> Array.ofList
    String.Join (Environment.NewLine, finalArray)