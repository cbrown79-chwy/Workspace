type Suit = | Clubs = 0 | Diamonds = 16 | Hearts = 32 | Spades = 48
type Rank = | Two = 2 | Three = 3 | Four = 4 | Five = 5 | Six = 6 | Seven = 7 | Eight = 8 | Nine = 9 | Ten = 10 | Jack = 11 | Queen = 12 | King = 13 | Ace = 14
type HandClassification = | HighCard = 0 | Pair = 1 | TwoPair = 2 | Trips = 3 | Straight = 4 | Flush = 5 | FullHouse = 6 | Quads = 7 | StraightFlush = 8

type Card = Suit * Rank
type Hand = Card list
type PokerHand = { Hand : Hand; Classification: HandClassification }

let compare<'a> (eq : 'a -> 'a -> bool) (gt : 'a -> 'a -> bool) item1 item2 =  
    match eq item1 item2 with
    | true -> 0
    | _ -> if gt item1 item2 then 1 else -1 

let cmp a b = compare (=) (>) a b

let compareCards (a:Card) (b:Card) = 
    let _, r1 = a
    let _, r2 = b
    cmp r1 r2

let deck = List.allPairs [Suit.Clubs;Suit.Diamonds;Suit.Hearts;Suit.Spades] [for m in System.Enum.GetValues(typeof<Rank>) do yield m :?> Rank]
            |> Set.ofList

let isTrips (cards : Card list) = 
    match cards |> List.groupBy (fun (_, m) -> m) |> List.tryFind (fun (i, k) -> List.length k = 3) with
    | Some n -> true
    | _ -> false

let isPair (cards : Card list) = 
    match cards |> List.groupBy (fun (_, m) -> m) |> List.tryFind (fun (i, k) -> List.length k = 2) with
    | Some n -> true
    | _ -> false

let classificationRules = [
    (HandClassification.Trips, isTrips)
    (HandClassification.Pair, isPair)
    (HandClassification.HighCard, fun _ -> true)] 


let straights = [
    (Rank.Ace, Rank.Two, Rank.Three, Rank.Four, Rank.Five)
    (Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six)
    (Rank.Three, Rank.Four, Rank.Five, Rank.Six, Rank.Seven)
    (Rank.Four, Rank.Five, Rank.Six, Rank.Seven, Rank.Eight)
    (Rank.Five, Rank.Six, Rank.Seven, Rank.Eight, Rank.Nine)
    (Rank.Six, Rank.Seven, Rank.Eight, Rank.Nine, Rank.Ten)
    (Rank.Seven, Rank.Eight, Rank.Nine, Rank.Ten, Rank.Jack)
    (Rank.Eight, Rank.Nine, Rank.Ten, Rank.Jack, Rank.Queen)
    (Rank.Nine, Rank.Ten, Rank.Jack, Rank.Queen, Rank.King)
    (Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Ace)]




    


