#r "FSharp.Data.3.0.0\\lib\\net45\\Fsharp.Data.dll"

open System.IO;
open System.Collections.Generic
open FSharp.Data
type FactorsFile = CsvProvider<"C:\\working\\data\\martha_sample.txt", "\t">

let writeData filePath stringLines =
    try
        File.WriteAllLines (filePath , Array.ofList stringLines)
        Ok (List.length stringLines)
    with 
    | e -> Error e.Message

let loadFile filePath =         
    let folderPath = Path.GetDirectoryName filePath
    let fileName = Path.GetFileName filePath

    let getNewFileName v = 
        sprintf "%s%c%s_%s" folderPath Path.DirectorySeparatorChar v fileName
    let toUpper c =
        (char ((string c).ToUpper () ))
    let inRange c1 c2 v = 
        v >= c1 && v <= c2

    let groupByFirstCharacterOfIssueColumnName (row:FactorsFile.Row) =  
        match toUpper row.IssueColumnName.[0] with  
        | x when inRange 'A' 'B' x  -> "A-B"
        | x when inRange 'C' 'D' x -> "C-D"
        | x when inRange 'E' 'F' x -> "E-F"
        | 'G' -> "G"
        | x when inRange 'H' 'K' x -> "H-K"
        | 'L' -> "L"
        | _ -> "M-Z"

    let stringifyLine (line:FactorsFile.Row) = 
        sprintf "%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s" line.Company_ID line.Entity_name line.Country line.Ticker line.Cusip line.Sedol line.Isin line.IssueColumnName line.IssueValue

    let fileHeader = "Company_ID\tEntity_name\tcountry\tticker\tcusip\tsedol\tisin\tIssueColumnName\tIssueValue"

    FactorsFile.Load(filePath).Rows 
        |> Seq.groupBy groupByFirstCharacterOfIssueColumnName
        |> Seq.iter (fun (groupByKey, groupedRows) ->    
                            let newFileName = getNewFileName groupByKey
                            let res = writeData newFileName (fileHeader :: ((groupedRows |> Seq.map stringifyLine) |> List.ofSeq))
                            match res with
                            | Ok m -> printfn "Wrote file '%s' with '%d' rows" newFileName m
                            | Error x -> printfn "Error writing file '%s'. Error text: %s" newFileName x
                    ) 


loadFile "V:\\ChristoB\\temp\\4592_Parametric_Compliance_DM_20190201.txt"

