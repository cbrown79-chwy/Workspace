type BiggerThanZero = private BiggerThanZero of bigint 

module BiggerThanZero = 
    let create uintValue = 
        if (uintValue > 0I ) then BiggerThanZero uintValue
        else failwith "No zeros"
       
    let value (BiggerThanZero u) = 
        u
        
type Fraction = { Numerator: bigint; Denominator: BiggerThanZero }


let getFraction decimalValue = 
    let rec fract d (m : decimal) = 
        let a = (d * m)
        let r = a % 1.0m
        match r with
        | 0.0m -> 
            { Numerator = (bigint a) ; Denominator = BiggerThanZero.create (bigint m) }           
        | _ -> fract d (m * 10.0m)
    fract decimalValue 10.0m

let getDecimal fraction = 
    (decimal fraction.Numerator) / (decimal (BiggerThanZero.value fraction.Denominator))


let getFactors (value : bigint) = 
    let isFactor i pf = 
        i % pf = 0I
    let rec potentialFactors start i = seq {
        match start with
        | Some v when v > 2I -> for a in v..2I..i do yield a 
        | Some _ | None -> 
                    yield 2I
                    yield! potentialFactors (Some 3I) i
                  } 
    let rec loop acc lf i = 
        let findFunc = isFactor i
        let seq = potentialFactors lf i
        match Seq.tryFind findFunc seq with
            | Some (f) -> loop (f::acc) (Some (f)) (i / f)
            | None -> acc

    if (value < 0I) then loop [-1I] None (value * -1I)
    elif (value > 0I) then loop [] None value
    else []

let rec without item list = 
    match list with
    | [] -> []
    | x::xs when x = item -> xs
    | x::xs -> x::(without item xs) 

let rec except list1 list2 =
    match list1 with 
    | [] -> list2
    | x::xs -> except xs (without x list2)

let intersection list1 list2 = 
    let rec loop acc l1 l2 = 
        match l2 with
        | x::xs when List.contains x l1 -> loop (x::acc) (without x l1) xs
        | _::xs -> loop acc l1 xs        
        | [] -> acc

    match List.length list1 - List.length list2 with
        | n when n < 0 -> loop [] list2 list1
        | _ ->  loop [] list1 list2

let reduceFraction fraction = 
    let negative = fraction.Numerator < 0I

    let absNumerator = if negative then fraction.Numerator * -1I else fraction.Numerator;
    let nFactorized = getFactors absNumerator
    let dFactorized = getFactors (BiggerThanZero.value (fraction.Denominator))
    let commonFactors = intersection nFactorized dFactorized
    let bigintMultiply (a : bigint) (b : bigint) =  
        System.Numerics.BigInteger.Multiply(a,  b)

    let newNumerator = 1I::except commonFactors nFactorized
                            |> List.reduce bigintMultiply 

    let newDenominator = 1I::except commonFactors dFactorized
                            |> List.reduce bigintMultiply 

    match negative with
    | false -> { Numerator = newNumerator ; Denominator = (BiggerThanZero.create newDenominator) }
    | _ -> { Numerator = newNumerator * -1I ; Denominator = (BiggerThanZero.create newDenominator) }
 