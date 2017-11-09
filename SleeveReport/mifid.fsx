type Year = int
type Month = int
type Day = int
type Hour = int
type Minute = int

type ClientName = string
type Quantity = decimal
type UnitPrice = decimal
type TotalMarketValueGrossOfFee = decimal
type FXRate = decimal
type NatureOfTheOrder = string
type ReportingFirmIdentification = string
// What is a venue? It's a broker. Maybe a MIC code?
type Venue = Venue of string

type OrderType = 
    | MOC
    | VWAP
    | LIMIT
    | POV
    | CROSS

type Side = 
    | Buy
    | Sell

type Instrument = 
    | Ticker of string
    | CUSIP of string
    | Sedol of string
    | ISIN of string
    

type TradingDay = Year * Month * Day
type TradingTime = Hour * Minute
type DateRange = TradingDay * TradingDay

type ExternalSystemElement = TradingDay * TradingTime * OrderType * Venue * 
                        Instrument * Side * NatureOfTheOrder * Quantity * 
                        UnitPrice * TotalMarketValueGrossOfFee

type PrimarySystemElement = ClientName * TradingDay * Venue * Instrument * 
                            Side * Quantity * UnitPrice * TotalMarketValueGrossOfFee

type AnotherSystemElement = TradingDay * Venue * Instrument * Side * 
                            Quantity * UnitPrice * TotalMarketValueGrossOfFee * FXRate

type GetLzDataFunction = DateRange -> ExternalSystemElement list
type GetLodestoneDataFunction = DateRange -> PrimarySystemElement list
type GetMagnetData = DateRange -> AnotherSystemElement list

type LinkedElements = 
    | Complete of PrimarySystemElement * AnotherSystemElement * ExternalSystemElement
    | Partial of PrimarySystemElement * AnotherSystemElement option * ExternalSystemElement option


type MapDataFunction = ExternalSystemElement list -> PrimarySystemElement list -> AnotherSystemElement list -> LinkedElements list

type OutputDataElement = ReportingFirmIdentification * ClientName * TradingDay * TradingTime * 
                            OrderType * Venue * Instrument * Side * NatureOfTheOrder * Quantity * 
                            UnitPrice * TotalMarketValueGrossOfFee * FXRate

type CreateOutputFunction = LinkedElements -> OutputDataElement

let correctLinkedElements linkedElementRow = 
    match linkedElementRow with
    | Partial (lz, Some l, Some m) -> Complete (lz, l, m)
    | _ -> linkedElementRow


