type Name = string
type Percentage = decimal



type Model = Name

type Allocation = ( Model * Percentage ) list
type MarketValue = decimal
type AccountReturn = decimal
type Account = { 
        TAllocations : Allocation * MarketValue; 
        TMinusOneAllocations: Allocation * MarketValue;
         Return: AccountReturn
         } 

type ModelReturn = decimal * Model
type RealizedTrackingError = AccountReturn -> TargetReturn
type SleeveReturn = ModelReturn -> RealizedTrackingError
type SleevedMarketValue = ModelAllocation -> MarketValue
type TargetReturn = decimal
 // no tracking error return

type SleeveLevelPerformance = Account -> ModelReturn list -> TargetReturn


type BloombergModel = Name * Return
type AdventModel = Name * Return
type FactsetModel = Name * Return

type Input = BloombergModel -> Model


let a (b : Input) c = b >> c