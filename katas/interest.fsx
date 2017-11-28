let cp p r e = 
    p * (r ** e)

let constantCompounding principal rateAsDecimal termInYears =
    let exponent = rateAsDecimal * termInYears
    cp principal System.Math.E exponent

let compoundInterest principal rateAsDecimal numberOfTimesInterestCompoundedAnnually termInYears =
    let exponent = numberOfTimesInterestCompoundedAnnually * termInYears
    let rate = rateAsDecimal / numberOfTimesInterestCompoundedAnnually
    cp principal (1.0 + rate) exponent


let kMax s p m = 
    let percentage = min p 0.15
    let amount = min (s * percentage) 18500.0
    amount + m

let nestEgg salary salaryGrowth years kMaxMatch =
    let sal = salary
    let approxSS = 1991.0
    let nest = [1.0 .. years] 
                |> List.sumBy (fun n -> 
                                let x, y = ((constantCompounding sal salaryGrowth n) * 0.15, (years - n + 1.0))
                                let ps = constantCompounding x 0.04 y  
                                let k = kMax sal (n * 0.01) kMaxMatch
                                let tot = ps + k
                                System.Math.Round (tot, 2)
                              )
    ((nest * 0.04) / 12.0) + approxSS

