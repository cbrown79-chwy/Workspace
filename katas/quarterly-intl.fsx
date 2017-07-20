#r @"C:\Working\git\Workspace\FSharp.Data.2.3.3\lib\net40\FSharp.Data.dll"

open FSharp.Data

type ModelInfo = {
    ShortName : string
    Holdings : int
    Beta : decimal
    Yield : decimal
    WtCap : decimal
    Weights : decimal list
}
type InvalidModel = {
    ValidationMessage : string
    Model : ModelInfo
}

type QuarterlyIntlCharx = CsvProvider<"C:\\Users\\christob\\Documents\\qr_imports.csv">
QuarterlyIntlCharx.GetSample()
let quarterlyData = QuarterlyIntlCharx.Load("C:\\Users\\christob\\Documents\\qr_imports.csv")

let modelInfo (n:QuarterlyIntlCharx.Row) = {
               ShortName = n.Benchmark
               Holdings = n.B_Holdings
               Beta = n.B_Beta
               Yield = n.B_Yield
               WtCap = n.B_WtCap
               Weights = [n.B_Wt1;n.B_Wt2;n.B_Wt3;n.B_Wt4;n.B_Wt5;n.B_Wt6;n.B_Wt7;
                            n.B_Wt8;n.B_Wt9;n.B_Wt10;n.B_Wt11;n.B_Wt12;n.B_Wt13;(decimal n.B_Wt14);(decimal n.B_Wt15);n.B_Wt16;n.B_Wt17;n.B_Wt18;n.B_Wt19;n.B_Wt20;n.B_Wt21;n.B_Wt22;n.B_Wt23;n.B_Wt24;n.B_Wt25;n.B_Wt26;n.B_Wt27;n.B_Wt28]
            }

let benchmarksAreUnique data = 
    data
        |> List.groupBy (fun item -> item.ShortName)
        |> List.filter (fun (key, s) -> List.length s > 1)
        |> List.map (fun (key, s) -> s)
        |> List.reduce ( @ )
        |> List.map (fun i -> { ValidationMessage = "Duplicate benchmark"; Model = i })
    
let weightsSumTo100 data = 
    data 
        |> List.filter (fun item -> abs (List.sum item.Weights - 100.0M) > 2.0M)
        |> List.map (fun i -> { ValidationMessage = "Weights do not sum to 100%"; Model = i })

let doValidations() =
    let distinctModels = quarterlyData.Rows 
                            |> Seq.map modelInfo 
                            |> Seq.distinct 
                            |> List.ofSeq
    (distinctModels |> benchmarksAreUnique) @ (distinctModels |> weightsSumTo100)
        |> List.map (fun a -> a.Model.ShortName, a.ValidationMessage)
