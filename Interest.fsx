module Interest = 
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