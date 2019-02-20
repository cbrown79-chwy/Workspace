type Name = string
type Percentage = decimal
type Return = decimal
type TargetReturn = decimal


type Model = Name

type Allocation = ( Model * Percentage ) list
type MarketValue = decimal
type AccountReturn = Return
type Account = { 
        TAllocations : Allocation * MarketValue; 
        TMinusOneAllocations: Allocation * MarketValue;
         Return: AccountReturn
         } 

type ModelReturn = decimal * Model
type RealizedTrackingError = AccountReturn -> TargetReturn
type SleeveReturn = ModelReturn -> RealizedTrackingError
type SleevedMarketValue = Allocation -> MarketValue
 // no tracking error return

type SleeveLevelPerformance = Account -> ModelReturn list -> TargetReturn

type BloombergModel = Name * Return
type AdventModel = Name * Return
type FactsetModel = Name * Return

type Input = BloombergModel -> Model

