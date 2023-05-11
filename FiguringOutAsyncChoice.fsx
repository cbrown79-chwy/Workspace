open System

let optionA b = 
    async {
        let mutable keepWorking = true

        use! c = Async.OnCancel (fun () -> keepWorking <- false )
        let a = Random()

        if keepWorking then
            do! Async.Sleep(a.Next(5000, 10000))
            
            printfn "OptionA %d" b |> ignore
            if b > 5 then return Some(0)        
            else return None
        else
            return None
    }

let optionb c =
    async {
        let a = Random()
        do! Async.Sleep(a.Next(5000, 10000))
        printfn "OptionA %d" c |> ignore
        if c < 5 then 
            let! completor = optionA (c - 5) |> Async.StartChild 
            let! result = completor
            return result
        else return None
    }

let tasks = [ optionA 1; optionb 6; optionA 8; optionb 4]

let a = Async.Choice tasks |> Async.RunSynchronously

let tkn = new Threading.CancellationTokenSource()

match a with
    | Some n -> printfn "%d returned" n |> ignore
    | None -> printfn "None returned"