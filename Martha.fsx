#r "FSharp.Data.3.0.0\\lib\\net45\\Fsharp.Data.dll"

open System.IO;
open FSharp.Data
type FactorsFile = CsvProvider<"C:\\working\\data\\martha_sample.csv">

let loadFile filePath = 
    let folderPath = Path.GetDirectoryName filePath
    
    let fileName = Path.GetFileName filePath
    
    let rows = FactorsFile.Load(filePath).Rows

    let toUpper c = (char ((string c).ToUpper () ))
    let inRange c1 c2 v = v >= c1 && v <= c2

    let rowGrouping (row:FactorsFile.Row) = match toUpper row.IssueColumnName.[0] with  
                                            | x when inRange 'A' 'B' x  -> "A-B"
                                            | x when inRange 'C' 'D' x -> "C-D"
                                            | x when inRange 'E' 'F' x -> "E-F"
                                            | 'G' -> "G"
                                            | x when inRange 'H' 'K' x -> "H-K"
                                            | 'L' -> "L"
                                            | _ -> "M-Z"

    let stringifyLine (line:FactorsFile.Row) = sprintf "%s,%s,%s,%s,%s,%s,%s,%s,%s" line.Company_ID line.Entity_name line.Country line.Ticker line.Cusip line.Sedol line.Isin line.IssueColumnName line.IssueValue
    let fileHeader = "Company_ID,Entity_name,country,ticker,cusip,sedol,isin,IssueColumnName,IssueValue"

    rows
        |> Seq.groupBy rowGrouping
        |> Seq.iter (fun (issueColumnValue, rows) ->    
                            let newFileName = folderPath + issueColumnValue + fileName
                            let dataLines =  rows |> Seq.map stringifyLine |> List.ofSeq
                            File.WriteAllLines (newFileName , fileHeader :: dataLines)
                    ) 

