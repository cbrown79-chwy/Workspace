open System



let add x y = x + y




let inceptionEquityValue totalCash equityValue fixedIncomeValue uploadCashAmount =
    let mv = totalCash + equityValue + fixedIncomeValue
    if mv > 1.01m * uploadCashAmount || mv < 0.99m * uploadCashAmount then mv - uploadCashAmount else 0.0m


type PostedAccountOrderTrade = {  AccountOrderTradeTokenId : int; AccountId : int; CreateDate: DateTime }
type OpenAccount = { AccountId : int; AccountStatus : int; }
type OpenAccountWithOnlyArchivableTrades = { AccountId : int; AccountStatus : int; }
type ActivityStepToAutoProgress = { ActivityStepId : int; }
type NewAccount = { AccountId : int; InceptionDate: DateTime; InceptionCash : decimal ; InceptionSecurityValue : decimal }
type TradeOrder = { TradeOrderId : int }
type TradeOrderWithOnlyArchiveableTrades = { TradeOrderId : int }
type AccountsWithNoLastTradedDate = {AccountId : int }


type GetPostedAOTs = unit -> PostedAccountOrderTrade list
type GetOpenAccounts = PostedAccountOrderTrade list -> OpenAccount list

type GetActivityStepsToAutoProgress = PostedAccountOrderTrade list -> ActivityStepToAutoProgress list
type AutoProgressActivitySteps = ActivityStepToAutoProgress list -> unit

type RemoveAccountsWithAnyOpenAOTRemaining = OpenAccount list -> OpenAccountWithOnlyArchivableTrades list

type MarkAccountsAsReadyForArchive = OpenAccountWithOnlyArchivableTrades list -> unit

type GetNewAccounts = unit -> NewAccount list
type UpdateInceptionValues = NewAccount list -> unit

// Why is the Activity Steps to Auto Progress set to repeat after?
// This also includes the Cash Withdrawal / Contribution work


// This seems just randomly included in the process.
type CompleteCashPostStepsForAccountsThatHaveNoCashPosts = unit -> unit

type GetTradeOrdersFromPostedAOTs = PostedAccountOrderTrade list -> TradeOrder list
type RemoveTradeOrdersWithAnyOpenAOTRemaining = TradeOrder list -> TradeOrderWithOnlyArchiveableTrades list
type MarkOrdersAsComplete = TradeOrderWithOnlyArchiveableTrades list -> unit

type GetAccountsWithNoLastTradedDate = unit -> AccountsWithNoLastTradedDate list

type SecondUpdateInceptionValues = AccountsWithNoLastTradedDate list -> unit