
type InterestCalculationOptions = {
    Compounding : Compounding
    Rate : float
    Principal : float
    TermInYears : float
} 
and Compounding = 
    | TimesPerYear of float 
    | Constant 


let compound options = 
    let core r e = options.Principal * (r ** e)
    match options.Compounding with 
    | Constant -> core System.Math.E (options.Rate * options.TermInYears)
    | TimesPerYear f -> let rate = (options.Rate / f) + 1.0
                        core rate (f * options.TermInYears)





let frysSituation = { Compounding = TimesPerYear 1.0; Rate = 0.0225; Principal = 0.93; TermInYears = 1000.0 } 

let frysBankBalance = compound frysSituation


let fryOverTime = [ 1.0 .. 1000.0 ] 
                    |> List.map (fun t -> compound { Compounding = TimesPerYear 1.0
                                                     Rate = 0.0225
                                                     Principal = 0.93
                                                     TermInYears = t }  )



//fryOverTime.[500 .. 650]

