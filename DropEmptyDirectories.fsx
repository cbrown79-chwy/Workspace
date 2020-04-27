open System.IO

// the source path to deal with
let sourcePath = @"D:\Music"

// the stuff I want done
let printDirectory path = 
    printfn "Directory: %s" path
    path

let deleteDirectory path = 
    Directory.Delete path
    path

let printDirectoryNameAndDeleteIt = printDirectory >> deleteDirectory


let rec iterateDirectories path = 
    [for d in Directory.EnumerateDirectories(path) do 
        yield! iterateDirectories d
        yield d]

let filterSubdirectories filter rootPath = 
    let directories = rootPath :: iterateDirectories rootPath
    let result = [for d in directories do 
                    yield (d, filter d)]
    result 
        |> List.filter (fun (_, testResult) -> testResult) 
        |> List.map (fun (path, _) -> path)

let pathIsEmpty path = 
    Directory.EnumerateFileSystemEntries(path) |> Seq.isEmpty

let applyToTestedSubdirectories fn test path = 
    [for m in filterSubdirectories test path do 
        yield fn m]

let mechanism path = applyToTestedSubdirectories printDirectoryNameAndDeleteIt pathIsEmpty path

let rec applyMechanismUntilReturnIsEmpty lastRunResult path = 
    let comparison a b = System.String.Compare (a, b, true)
    let r = mechanism path
    if List.isEmpty r then Result.Ok 0
    else if List.compareWith comparison r lastRunResult = 0 then Result.Error r
    else 
        applyMechanismUntilReturnIsEmpty r path
    
let main path = 
    let r = applyMechanismUntilReturnIsEmpty [] path
    match r with 
    | Result.Ok _ -> printfn "Completed successfully."
    | Result.Error x -> printfn "Error occurred. Items [%A] could not be removed." x

    
