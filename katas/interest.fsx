open FSharp.Charting._ChartStyleExtensions
open FSharp.Charting._ChartStyleExtensions
#load "..\\FSharp.Charting.0.91.1\\lib\\net45\\FSharp.Charting.fsx"

open FSharp.Charting

type Compounding = 
    | TimesPerYear of float 
    | Constant 

type InterestCalculationOptions = {
    Compounding : Compounding
    Rate : float
    Principal : float
    TermInYears : float
}

let compound options = 
    let core r e = 
        options.Principal * (r ** e)
    match options.Compounding with 
    | Constant -> core System.Math.E (options.Rate * options.TermInYears)
    | TimesPerYear f -> let rate = options.Rate / f
                        core (1.0 + rate) (f * options.TermInYears)

let fry = compound { Compounding = TimesPerYear 1.0
                     Rate = 0.0225
                     Principal = 0.93
                     TermInYears = 1000.0 } 


let fryOverTime = [ 1.0 .. 1000.0 ] 
                    |> List.map (fun t -> compound { Compounding = TimesPerYear 1.0
                                                     Rate = 0.0225
                                                     Principal = 0.93
                                                     TermInYears = t }  )

Chart.Line(fryOverTime
            , Name="Balance over Years"
            , XTitle = "Years"
            , YTitle = "Balance in Dollars")
       .WithXAxis(Min=0.0, Max=1000.0)
       .WithYAxis(Log = true)


//fryOverTime.[500 .. 650]

