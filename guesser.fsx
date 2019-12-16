open System

type Results = { Games : int; Guesses : int; BestGame : int; TotalGuesses : int }


let rec again results = 
    Console.Write "\r\nWould you like to play again? (y/n): "
    let t = {results with Guesses = 0; TotalGuesses = results.Guesses + results.TotalGuesses; }
    match Console.ReadKey() with 
    | m when m.Key = ConsoleKey.Y -> {t with Games = results.Games + 1; }
    | m when m.Key = ConsoleKey.N -> t
    | _ -> again results

let rec guess answer results = 
    Console.Write "Your guess? "
    let t = {results with Guesses = results.Guesses + 1 }
    let s, i = Console.ReadLine() |> Int32.TryParse
    match (s, i) with 
    | false, _ -> 
        Console.WriteLine "I couldn't understand you..."
        guess answer results
    | true, v when v < answer -> 
        Console.WriteLine "It's higher."
        guess answer t
    | true, v when v > answer -> 
        Console.WriteLine "It's lower."
        guess answer t
    | true, _ -> 
        let res = {t with BestGame = if (t.Guesses < t.BestGame) then t.Guesses else t.BestGame }
        let youGotItRight = sprintf "\r\nYou got it right in %d guesses." t.Guesses
        Console.WriteLine youGotItRight
        res

let rec play results = 
    Console.WriteLine "\r\nI'm thinking of a number between 1 and 100..."
    let r = guess (Random().Next(1, 101)) results 
    let s = again r
    if s.Games = r.Games then s
    else play s


let game() = 
    let init = { BestGame = Int32.MaxValue; Games = 1; Guesses = 0; TotalGuesses = 0 }
    Console.WriteLine @"This program allows you to play a guessing game.
I will think of a number between 1 and 
100 and will allow you to guess until
you get it. For each guess I will tell you
whether the right answer is higher or lower
than your guess."
    let results = play init
    let guessesPerGame = (float results.TotalGuesses) / (float results.Games)
    Console.WriteLine (sprintf "\r\nOverall results: \r\n\ttotal games   = %d\r\n\ttotal guesses = %d\r\n\tguesses/game  = %f\r\n\tbest game     = %d" results.Games results.TotalGuesses guessesPerGame results.BestGame)
    0

