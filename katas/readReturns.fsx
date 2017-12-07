#r "..\\FSharp.Data.2.4.3\\lib\\net45\\FSharp.Data.dll"

open FSharp.Data

type dailyReturns = CsvProvider<"C:\\working\\git\\daily.csv">

let m = dailyReturns.Load("C:\\working\\git\\daily.csv")
m.Rows 
    |> Seq.filter (fun n -> n.Benchmark.Length > 10)
    |> Seq.map (fun n -> n.Benchmark)
    |> Seq.distinct
    |> Array.ofSeq