#r "ExcelProvider.1.0.1\\lib\\net45\\ExcelProvider.Runtime.dll"

open System
open System.IO
open FSharp.Interop.Excel

[<Literal>]
let Source = @"I:\Sales and Service\Sales Administration\Portfolio Analysis and Reporting\Coric\Quickletters\web\Archive"
[<Literal>]
let Destation = @"C:\working\justineSample\data\dest"

type DataFile = ExcelFile<"C:\\working\\justineSample\\data\\Source.xlsx"> // this is an excel file that should have 3 columns. SN, IMS Code (if available), 

type Account = {
    ShortName : string
    CoricReportId : string
    ImsShortName : string option
}

let processAccountFile sourceDirectory destinationDirectory account = 
    let reportDate = DateTime.Today.Subtract (TimeSpan.FromDays(30.0))
    let d1, d2, d3 = reportDate.ToString("yyyy MM"), reportDate.ToString("yyyyMM"), reportDate.ToString("yyyy-MM")
    let incomingFileName = sprintf "%s\\%s\\QuickLetter_%s_%s_web.PDF" sourceDirectory d2 d2 account.CoricReportId
    let outgoingFileName =  
        let newShortName = if Option.isSome account.ImsShortName then Option.get account.ImsShortName else account.ShortName    
        sprintf "%s\\%s\\%s.Monthly Performance Report.%s.pdf" destinationDirectory d3 newShortName d1
    try
        let directory = sprintf "%s\\%s" destinationDirectory d3
        Directory.CreateDirectory(directory) |> ignore
        File.Copy(incomingFileName, outgoingFileName, true)
        Ok (account.ShortName, incomingFileName, outgoingFileName)
    with 
    | e ->
        Error (e.ToString(), account.ShortName, incomingFileName, outgoingFileName)

let renderResult res = 
    match res with
    | Ok (s, i, f) -> sprintf "SUCCESS - Short name : '%s' copied from '%s' to %s successfully." s i f
    | Error (e, s, i, f) -> sprintf "FAILURE - Short name : '%s' could not be copied from '%s' to '%s'.\r\n\r\n%s" s i f e

let execute = processAccountFile Source Destation 



let file = DataFile()

let data = 
    file.Data 
        |> Seq.filter (fun row -> not (isNull row.``Coric Number``))
        |> Seq.map (fun row -> 
                    { 
                        ShortName = row.SN
                        CoricReportId = row.``Coric Number``.ToString()
                        ImsShortName = if isNull(row.``IMS Code (If Applicable)``) then None else Some (row.``IMS Code (If Applicable)``.ToString()) }
                    )


let results = data 
                |> Seq.map (execute >> renderResult)

for i in results
    do
        Console.WriteLine i
