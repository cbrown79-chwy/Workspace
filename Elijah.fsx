#r "FSharp.Data.SqlClient.2.0.5\\lib\\net40\\FSharp.Data.SqlClient.dll"

open System
open System.Data
open System.Data.SqlClient
open FSharp.Data

type Info = { 
    ShortName : string;
    LongBeginningMarketValue: decimal; 
    ShortBeginningMarketValue : decimal; 
    LongEndingMarketValue: decimal; 
    ShortEndingMarketValue : decimal; 
    LongBeginningCostBasis : decimal; 
    ShortBeginningCostBasis: decimal; 
    LongEndingCostBasis: decimal; 
    ShortEndingCostBasis : decimal; 
    InceptionDate: DateTime; 
    SimBenchStartYearMonth: string; 
    Error: string; 
    AccountId: int  

}

[<Literal>]
let ConnectionString = "Data Source=uniondb.dev.paraport.com;Initial Catalog=APP_SUPPORT_CAM;Integrated Security=True"

let accountIds() = 
    use cmd = new SqlCommandProvider<"SELECT c.accountid FROM dbo.CLIENTACCOUNT AS c WHERE c.closedate IS NULL AND c.inceptiondate <= GETDATE() AND c.taxableind = 'Y'", ConnectionString>(ConnectionString)
    cmd.Execute() |> Seq.toList

let getSimBenchData accountId = 
    try 
        printfn "Running report for %i" accountId
        use sConn = new SqlConnection(ConnectionString)
        use da =  new SqlDataAdapter()
        use cmd = new SqlCommand("sp_TS_GetAccountSimBench", sConn)
        let p = SqlParameter()
        p.ParameterName <- "@account_id"
        p.Value <- accountId
        cmd.Parameters.Add(p) |> ignore
        let mutable ds = new DataSet()
        cmd.CommandType <- CommandType.StoredProcedure
        da.SelectCommand <- cmd
        sConn.Open()
        da.Fill(ds) |> ignore
        sConn.Close()
        if(ds.Tables.[1].Rows.Count > 0) then
            { 
                AccountId = accountId
                ShortName = ds.Tables.[0].Rows.[0].["account_short_name"].ToString();
                InceptionDate = DateTime.Parse(ds.Tables.[0].Rows.[0].["inception_date"].ToString());
                SimBenchStartYearMonth = ds.Tables.[1].Rows.[0].["year_month"].ToString();
                LongBeginningMarketValue = Decimal.Parse(ds.Tables.[1].Rows.[1].["begin_market_value"].ToString());
                ShortBeginningMarketValue = Decimal.Parse(ds.Tables.[1].Rows.[0].["begin_market_value"].ToString());
                
                LongEndingMarketValue = Decimal.Parse(ds.Tables.[1].Rows.[1].["end_market_value"].ToString());
                ShortEndingMarketValue = Decimal.Parse(ds.Tables.[1].Rows.[0].["end_market_value"].ToString());
                
                LongBeginningCostBasis = Decimal.Parse(ds.Tables.[1].Rows.[1].["begin_cost_basis"].ToString());
                ShortBeginningCostBasis = Decimal.Parse(ds.Tables.[1].Rows.[0].["begin_cost_basis"].ToString());
                
                LongEndingCostBasis = Decimal.Parse(ds.Tables.[1].Rows.[1].["end_cost_basis"].ToString());
                ShortEndingCostBasis = Decimal.Parse(ds.Tables.[1].Rows.[0].["end_cost_basis"].ToString());

                Error = ""
            }
        else
            { 
                AccountId = accountId
                ShortName = ds.Tables.[0].Rows.[0].["account_short_name"].ToString();
                InceptionDate = DateTime.Parse(ds.Tables.[0].Rows.[0].["inception_date"].ToString());
                SimBenchStartYearMonth = "-"
                LongBeginningMarketValue = 0m;
                ShortBeginningMarketValue = 0M;
                
                LongEndingMarketValue = 0m
                ShortEndingMarketValue = 0m
                
                LongBeginningCostBasis = 0m
                ShortBeginningCostBasis = 0m
                
                LongEndingCostBasis = 0m
                ShortEndingCostBasis = 0m

                Error = ""
            }

    with 
    | e -> { AccountId = accountId; 
        Error = e.Message; 
        ShortName = ""; 
        InceptionDate = DateTime.MaxValue; SimBenchStartYearMonth = ""; LongBeginningMarketValue = Decimal.Zero; ShortBeginningMarketValue = Decimal.Zero;
                LongEndingMarketValue = 0m;ShortEndingMarketValue = 0m;LongBeginningCostBasis = 0m;ShortBeginningCostBasis = 0m;LongEndingCostBasis = 0m;ShortEndingCostBasis = 0m;}


let format (n : Info) = 
    let d = n.InceptionDate.ToString "yyyy-MM-dd"
    sprintf "%d,%s,%s,%s,%M,%M,%M,%M,%M,%M,%M,%M,%s" n.AccountId n.ShortName d n.SimBenchStartYearMonth n.LongBeginningMarketValue n.ShortBeginningMarketValue n.LongEndingMarketValue n.ShortEndingMarketValue n.LongBeginningCostBasis n.ShortBeginningCostBasis n.LongEndingCostBasis n.ShortEndingCostBasis n.Error

let execProcess() = 
    let aIds = accountIds()
    let header = "Id,ShortName,InceptionDate,SimBenchStartYearMonth,LongBeginningMarketValue,ShortBeginningMarketValue,LongEndingMarketValue,ShortEndingMarketValue,LongBeginningCostBasis,ShortBeginningCostBasis,LongEndingCostBasis,ShortEndingCostBasis,Err"
    let results = aIds 
                    |> List.map getSimBenchData 
                    |> List.sortBy (fun n -> n.LongBeginningMarketValue)
                    |> List.map format
    let final = (header::results) |> List.toArray
    System.IO.File.WriteAllLines("C:\\working\\report.csv", final)
    0

execProcess()