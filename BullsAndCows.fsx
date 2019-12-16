type Response = | Bull | Cow | Empty

let bullAndCow (attempt : int[]) (answer : int[])  = 
    let bullCowOrNothing (idx:int) (chr:int) = 
        if (answer.[idx] = chr) then Bull
        else if (Array.contains chr answer) then Cow
        else Empty
    Array.mapi bullCowOrNothing attempt |> Array.filter (fun n -> n <> Empty) |> Array.sort

let rec getGuess() =
    printf "Enter your guess. "
    let guess = System.Console.ReadLine()
    match guess.ToCharArray() with
    | s when Array.forall System.Char.IsDigit s && Array.length s = 4 -> 
        Array.map (fun c -> System.Int32.Parse(c.ToString())) s
    | _ -> getGuess()

let pluralize (item, count) = 
    if count > 1 then sprintf "%d %As" count item
    else sprintf "1 %A" item

let rec generateAnAnswer set = 
    let r = System.Random(); 
    let res = Set.add (r.Next(0, 9)) set
    if Set.count res = 4 then res
    else generateAnAnswer res

let playPersonGuessesComputer() = 
    let computersAnswer = generateAnAnswer Set.empty |> Set.toArray
    let rec play guesses =
        let guess = getGuess()
        let guessCount = guesses + 1
        let result = bullAndCow guess computersAnswer
        if Array.forall (fun r -> r = Bull) result && Array.length result = 4 then
            printfn "You win, in %d guesses" guessCount
            guessCount
        else
            let counts = Array.countBy id result
            if Array.length counts > 1 then printfn "Good try! You got %s and %s." (pluralize counts.[0]) (pluralize counts.[1])
            else if Array.length counts = 1 then printfn "Good try! You got %s." (pluralize counts.[0])
            else printfn "Good try! None of these are in the answer!"
            play guessCount
    play 0

(*
    Solving in micro... assume a 2 digit puzzle with values from 1, 2, 3, 4, 5. Initial guess (1, 2)
     a Result: 2 bulls, win
     b Result: 2 cows, I have only one option remaining (2, 1)
     c Result 0 cows or bulls
          - So answer must be a combo of [3, 4 or 5].  
            Because I have no other information, simply guessing the first two numbers in the list (3,4)
            This will result in option a, b, or the user will get one cow, in which case there are two distinct answers remaining. (5, 3 or 4, 5)
            Picking one of those answers will result in a or b.
    d I get 1 cow, I know 
            1 or 2 will be in the answer, but not in the position they are in
            therefore, the answer must be [3, 4, 5], 1 or 2, [3, 4, 5]
            because both sets are the same length, we need more information. next guess should attempt more info.
            (3, 4) or (4, 5) will get the same info, fs of example, assume 3,4 chosen, and must result in (any of):
                 - 0 bulls 0 cows, the remaining number (5) is the number remaining. Answers must be 5, 1 or 2, 5. See a or b
                 - 1 bull. Answer is either (3, 1) or (2, 4). See a or b
                 - 1 cow. Answer is either (4, 1) or (2, 3). See a or b.

    // The answer always eventually gets to a or b.

    In the case of 4 digits, with 10 values. Initial guess (1, 2, 3, 4)
    
    a Result: 4 bulls = Win
    b Result: 0 cows or bulls = 4 digits, 6 possible values.

    b Result: 3 bulls = There are 24 combos potentially avail ([5,6,7,8,9,0], 2, 3, 4), (1, [5,6,7,8,9,0], 3, 4), (1, 2, [5,6,7,8,9,0], 4) , (1, 2, 3, [5,6,7,8,9,0] ) 
    c Result: 3 cows = 48 combos = ([5,6,7,8,9,0], 3, 4, 2), ([5,6,7,8,9,0], 4, 2, 3)
                                    (1, [5,6,7,8,9,0], 3, 4 ), (1, [5,6,7,8,9,0], 4, 2, 3)

*)