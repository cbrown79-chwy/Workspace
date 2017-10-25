type ShortCode = ShortCode of string
type ModelShortCode = ModelShortCode of ShortCode
type AccountShortCode = AccountShortCode of ShortCode

module ShortCode =
    let create s = 
        if System.String.IsNullOrEmpty s then None
        elif s.Length > 15 then None
        else Some (ShortCode s)

    let value v  =
        match box v with 
        | :? ShortCode -> let m = box v :?> ShortCode 
                          match m with | ShortCode s -> s
        | _ -> ""

let createAccount s = 
    let m = ShortCode.create s |> Option.get
    AccountShortCode m
let createModel s = 
    let m = ShortCode.create s |> Option.get
    ModelShortCode m



type AsOfDate = AsOfDate of System.DateTime

type ModelType = 
    | SingleShare  // an ETF, or a Mutual Fund
    | EnternallyManaged // A model managed by an advisor, or the all cash model ('astcash').
    | Core // An index (SP500, RU3000, etc)

type CustodianAccountIdentifier = CustodianAccountIdentifier of string

type AllocationPercentage = AllocationPercentage of decimal
type PercentageReturn = PercentageReturn of decimal
type AllocationDate = AllocationDate of AsOfDate 

type ModelReturn = ModelReturn of PercentageReturn
type AccountReturn = AccountReturn of PercentageReturn 
type AccountMarketValue = AccountMarketValue of decimal

type Model = ModelShortCode * ModelReturn
type ModelAllocations = (Model * AllocationPercentage) list
type AccountAllocations = ModelAllocations * AccountMarketValue * AsOfDate

open System.Collections.Generic
let memo f = 
    let cache = Dictionary<_, _>()
    fun x -> 
        if cache.ContainsKey(x) then 
            printfn "Cache hit" 
            cache.[x]
        else
            let res = f x
            printfn "Cache miss" 
            cache.[x] <- res
            res

let memoPlus x = 
    let l = memo (+) x
    l x