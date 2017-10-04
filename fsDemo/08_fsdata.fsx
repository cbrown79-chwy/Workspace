#r @"C:\working\git\Sleeve.Reporting\packages\FSharp.Data.2.3.3\lib\net40\\FSharp.Data.dll"


open FSharp.Data

type FirstClear = CsvProvider<"C:\\working\\git\\Workspace\\fsDemo\\data\\SampleDataFile.csv", IgnoreErrors = true>
let file = FirstClear.Load("C:\\working\\git\\Workspace\\fsDemo\\data\\SomeDataFile.csv")
let data = file.Rows |> Seq.toArray |> Array.length

type FilePath = FilePath of string

let create (s:string) =
    if File.Exists s 
        then Some (FilePath s)
        else None
