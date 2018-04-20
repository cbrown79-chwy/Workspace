#r @"..\FSharp.Data.2.4.3\lib\net45\FSharp.Data.dll"

open FSharp.Data

type Info = {
    ShortName : string
    Holdings : int
    Beta : decimal
    Yield : decimal
    WtCap : decimal
    Weights : decimal list
}

type InvalidModel = {
    ValidationMessage : string
    Model : Info
}

type QuarterlyIntlCharx = CsvProvider<"data\\qr_imports.csv">
QuarterlyIntlCharx.GetSample()
let quarterlyData = QuarterlyIntlCharx.Load("data\\qr_imports.csv")

let modelInfo (n:QuarterlyIntlCharx.Row) = 
    {
               ShortName = n.Benchmark
               Holdings = n.B_Holdings
               Beta = n.B_Beta
               Yield = n.B_Yield
               WtCap = n.B_WtCap
               Weights = [n.B_Wt1;n.B_Wt2;n.B_Wt3;n.B_Wt4;n.B_Wt5;n.B_Wt6;n.B_Wt7;
                            n.B_Wt8;n.B_Wt9;n.B_Wt10;n.B_Wt11;n.B_Wt12;n.B_Wt13;(decimal n.B_Wt14);(decimal n.B_Wt15);n.B_Wt16;n.B_Wt17;n.B_Wt18;n.B_Wt19;n.B_Wt20;n.B_Wt21;n.B_Wt22;n.B_Wt23;n.B_Wt24;n.B_Wt25;n.B_Wt26;n.B_Wt27;n.B_Wt28]
    }

let accountInfo (n:QuarterlyIntlCharx.Row) = 
    {
               ShortName = n.Client
               Holdings = n.P_Holdings
               Beta = n.P_Beta
               Yield = n.P_Yield
               WtCap = n.P_WtCap
               Weights = [n.P_Wt1;n.P_Wt2;n.P_Wt3;n.P_Wt4;n.P_Wt5;n.P_Wt6;n.P_Wt7;
                            n.P_Wt8;n.P_Wt9;n.P_Wt10;n.P_Wt11;n.P_Wt12;n.P_Wt13;(decimal n.P_Wt14);(decimal n.P_Wt15);n.P_Wt16;n.P_Wt17;n.P_Wt18;n.P_Wt19;n.P_Wt20;n.P_Wt21;n.P_Wt22;n.P_Wt23;n.P_Wt24;n.P_Wt25;n.P_Wt26;n.P_Wt27;n.P_Wt28]
    }


let itemsAreUnique data = 
    data
        |> List.groupBy (fun item -> item.ShortName)
        |> List.filter (fun (key, s) -> List.length s > 1)
        |> List.map (fun (key, s) -> s)
        |> List.reduce ( @ )
        |> List.map (fun i -> { ValidationMessage = "Duplicate Item"; Model = i })
    
let weightsSumTo100 data = 
    data 
        |> List.filter (fun item -> abs (List.sum item.Weights - 100.0M) > 2.0M)
        |> List.map (fun i -> { ValidationMessage = "Weights do not sum to 100%"; Model = i })

let doValidations() =
    let distinctModels = quarterlyData.Rows 
                            |> Seq.map modelInfo 
                            |> Seq.distinct 
                            |> List.ofSeq
    (distinctModels |> itemsAreUnique) @ (distinctModels |> weightsSumTo100)
        |> List.map (fun a -> a.Model.ShortName, a.ValidationMessage)
