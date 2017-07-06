type Fraction = { Numerator: int; Denominator: uint32 }

let getFraction decimalValue = 
    let rec fract d m = 
        let acvl = (d * m)
        let r = acvl % 1.0m
        match r with
        | 0.0m -> { Numerator = (int acvl) ; Denominator = (uint32 m)}           
        | _ -> fract d (m * 10.0m)
    fract decimalValue 10.0m


let getFactors value = 
    let isFactor i pf = 
        i % pf = 0
    let potentialFactors i = seq { 
        yield 2
        for a in 3..2..i do yield a 
    }
    let rec loop acc i = 
        let findFunc = isFactor i
        let seq = potentialFactors i
        match Seq.tryFind findFunc seq with
            | Some (f) -> loop (f::acc) (i / f)
            | None -> acc
    loop [1] value 
        |> List.rev // ordering is implied by the fucntion

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
        | x::xs -> loop acc l1 xs        
        | [] -> acc

    let ret = match List.length list1 - List.length list2 with
                | n when n < 0 -> loop [] list2 list1
                | _ ->  loop [] list1 list2
    List.sort ret

let reduceFraction fraction = 
    let negative = fraction.Numerator < 0

    let absNumerator = System.Math.Abs fraction.Numerator
    let nFactorized = getFactors absNumerator
    let dFactorized = getFactors (int fraction.Denominator)
    let commonFactors = intersection nFactorized dFactorized

    let newNumerator = 1::except commonFactors nFactorized
                            |> List.reduce ( * ) 
    let newDenominator = 1::except commonFactors dFactorized
                            |> List.reduce ( * ) 

    match negative with
    | false -> { Numerator = newNumerator ; Denominator = (uint32 newDenominator) }
    | _ -> { Numerator = newNumerator * -1 ; Denominator = (uint32 newDenominator) }
 