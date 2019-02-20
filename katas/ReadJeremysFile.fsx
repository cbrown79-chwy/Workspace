open System;

let sp (a: string) = 
    let b = a.Split(",".ToCharArray(), StringSplitOptions.None)
    b.[0], b.[1]

let test m = 
    match Map.containsKey "PORTFOLIO VALUE ON 12-31-18" m with
        | true -> Some ((Map.find "PORTFOLIO CODE" m), (Map.find "PORTFOLIO VALUE ON 12-31-18" m))
        | false -> None

let execute () = 
    let fileContents = System.IO.File.ReadAllLines("C:\Users\christob\Downloads\JeremysFile.csv")
    let all = fileContents |> Array.filter (fun m -> (m <> ",") && (m <> ",--------------")) |> Array.map sp
    printfn "%i" (Array.length all)

    let mutable results = Map.empty
    let mutable p = Map.empty
    printf "Iterating over contents"
    for l in all do
        let a, b = l
        match Map.isEmpty p with
            | true -> 
                p <- p.Add(a, b)
            | false ->
                match p.ContainsKey a with
                | true -> 
                    match test p with 
                        | Some (m, n) -> 
                            printfn "Value for %s found" m
                            results <- results.Add(m,n) 
                        | None ->  
                              p <- Map.empty
                    p <- p.Add(a, b)
                | false -> 
                    p <- p.Add(a, b)
    printf "Writing output"
    let str = Map.fold (fun state key value -> state + key + "," + value + System.Environment.NewLine ) ("Short Name,Value As Of 12/31/2018" + System.Environment.NewLine) results
    System.IO.File.WriteAllText("C:\users\Christob\Downloads\JeremysBetterResults.csv", str)
    
execute () 