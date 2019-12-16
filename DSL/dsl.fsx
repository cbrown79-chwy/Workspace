#r "netstandard"
#r "Facades/netstandard"

open System

type Side = Buy | Sell
type LotLevelTrade = {
    AccountShortName : string
    CustodianAccountId : string
    Side : Side
    ShareCount : decimal
    Fins : string
    FinsClearing : string
    SettleDate : DateTime
    CostBasisDate : DateTime
    TradeDate : DateTime
    ExecutionPrice : decimal
    CommissionAmount : decimal
    SECFee : decimal
    LotShareCount : decimal
}
type RenderExpression = (LotLevelTrade -> string)
type Field = { Header : string; RenderExpression : RenderExpression }
type Delimited = { RowDelimiter : string; ColumnDelimiter : string; HasHeader : bool; Fields : Field list }

let SideText = (fun n -> match n.Side with | Sell -> "Sell" | Buy -> "Buy")
let AccountShortName = (fun n -> n.AccountShortName)
let Fins = (fun n -> n.Fins)
let FinsClearing = (fun n -> n.FinsClearing)
let CustodianAccountId = (fun n -> n.CustodianAccountId)
let TradeDate = (fun n -> n.TradeDate)
let CostBasisDate = (fun n -> n.CostBasisDate)
let SettleDate = (fun n -> n.SettleDate)
let ShareCount = (fun n -> n.ShareCount)
let CommissionAmount = (fun n -> n.CommissionAmount)
let ExecutionPrice = (fun n-> n.ExecutionPrice)

let GroupBy (expressionList:RenderExpression list) trades = 
    let getKey trade = expressionList |> List.map (fun n -> n trade) |> List.reduce (fun x y -> sprintf "%s%s" x y)
    trades |> List.groupBy getKey


let CharAt (a:RenderExpression) idx trade = 
    let m = a trade
    m.[idx].ToString()

let Substring (a:RenderExpression) count trade = 
    let m = a trade
    m.Substring count

let FormatDate (a:LotLevelTrade -> DateTime) (formatString:string) trade = 
    let m = a trade
    m.ToString(formatString)

let Round (a:LotLevelTrade -> Decimal) decimals trade = 
    let value = a trade
    let standardRound value = 
        let m = Decimal.Round(value) 
        let n = Decimal.Round(value + 0.5m) 
        min m n
    match decimals with 
    | 0 -> sprintf "%0M" (standardRound value)
    | n -> let mutliplier = (decimal(10.0 ** (float)n))
           sprintf "%0M" (standardRound (value * mutliplier) / mutliplier)




let flatten (lotLevelTrades : LotLevelTrade list) filterExpression groupExpression aggregations = 
    let applyAggregations group = 
        let rec apply aggs first second = 
            match aggs with 
            | [] -> first
            | f :: xs -> apply xs (f first second) second
        let fn f s = apply aggregations f s 
        match group with 
        | x :: xs -> xs |> List.fold fn x 
        | _ -> failwith "Impossible state"
    
    lotLevelTrades 
        |> List.filter filterExpression 
        |> groupExpression
        |> List.map (fun (_, gp) -> applyAggregations gp) 

let CsvFile fields = 
    { RowDelimiter = Environment.NewLine; ColumnDelimiter = ","; HasHeader = true; Fields = fields }

let myFile = 
    CsvFile [ 
        {
            Header = "Side"; 
            RenderExpression = CharAt SideText 0
        };
        {
            Header = "AccountId"; 
            RenderExpression = Substring CustodianAccountId 2
        } ; 
        {
            Header = "Account"; 
            RenderExpression = AccountShortName
        } ; 
        {
            Header = "Shares"; 
            RenderExpression = Round ShareCount 0
        } ; 
        {
            Header = "Commission"; 
            RenderExpression = Round CommissionAmount 4
        }  ; 
        {
            Header = "Price"; 
            RenderExpression = Round ExecutionPrice 6
        }  ; 
        {
            Header = "Trade Date"; 
            RenderExpression = FormatDate TradeDate "MM/dd/yyyy"
        }  ; 
        {
            Header = "Settle Date"; 
            RenderExpression = FormatDate SettleDate "MM/dd/yyyy"
        }  ; 
        {
            Header = "DTC ID"; 
            RenderExpression = Fins
        }  ; 
        {
            Header = "Account NM"; 
            RenderExpression = CustodianAccountId
        }  
    ] 

let render file trades =
    let join delim x y = sprintf "%s%s%s" x delim y
    let joinColumn x y = join file.ColumnDelimiter x y
    let joinRow x y = join file.RowDelimiter x y
    let runTrades trade = 
        file.Fields
            |> List.map (fun n -> n.RenderExpression trade)
            |> List.reduce joinColumn
    let renderedTrades = trades |> List.map runTrades
    if file.HasHeader then             
        let header = file.Fields 
                        |> List.map (fun n -> n.Header) 
                        |> List.reduce joinColumn
        header :: renderedTrades |> List.reduce joinRow
    else
        renderedTrades |> List.reduce joinRow

let noFilter _ = true
let trade = { AccountShortName = "Christopher"; CustodianAccountId = "Brown"; CommissionAmount = 0.04m; SettleDate=DateTime.Today; CostBasisDate=DateTime.Today; TradeDate =DateTime.Today; ExecutionPrice=5.0m; SECFee= 0.01m;Fins = "0153"; FinsClearing ="0153"; Side=Side.Buy;ShareCount=150.0m;LotShareCount=225.0m; }
let trades = [trade;trade;{trade with CustodianAccountId="Winkle"; AccountShortName="John";ShareCount=253.0m;CostBasisDate= (DateTime.Today.Subtract (new TimeSpan(25, 0, 0, 0)))} ]

let flat = flatten trades noFilter (GroupBy [CustodianAccountId;SideText]) [(fun s1 s2 -> {s1 with CommissionAmount = s1.CommissionAmount + s2.CommissionAmount; ShareCount = s1.ShareCount + s1.ShareCount})]

render myFile flat


