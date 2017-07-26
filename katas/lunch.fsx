type Message = 
    | Init of string list
    | Upvote of string
    | Downvote of string
    | DownAndDestroy of string

type Lunch = { Name : string; Votes : int }

let merge lunch1 lunch2 = { lunch1 with Votes = lunch1.Votes + lunch2.Votes}
let print listOfLunches = listOfLunches |> List.map (fun i -> sprintf "%s : %i" i.Name i.Votes)

let voteMachine = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop (state: Lunch list) = async {
            let! msg = inbox.Receive()

            match msg with
            | Init l -> 
                let restaurants = List.map (fun item -> { Name= item; Votes= 0 }) l
                print restaurants 
                    |> List.iter System.Console.WriteLine
                    |> ignore
                return! loop restaurants
            | Downvote s | Upvote s -> 
                let newLunch = match msg with 
                                    | Downvote _ -> { Name = s; Votes = -2};
                                    | _ -> { Name = s; Votes = 1}                                    
                let newState = newLunch::state 
                                |> List.groupBy (fun n -> n.Name)
                                |> List.map (fun (s, l) -> List.reduce merge l)
                                |> List.sortByDescending (fun n -> n.Votes)
                print newState 
                    |> List.iter System.Console.WriteLine
                    |> ignore
                return! loop newState
            | DownAndDestroy s ->
                let direction name = if name = s then -2 
                                     else 1
                let newState = state |> List.map (fun n -> { n with Votes = n.Votes + direction n.Name }) |> List.sortByDescending (fun n -> n.Votes)
                print newState 
                    |> List.iter System.Console.WriteLine
                    |> ignore
                return! loop newState
        }
        loop []
    )