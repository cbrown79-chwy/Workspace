// ============================================================================
// DEMO: Refactoring the domain model of a simple application
// ============================================================================

module RandomContacts.Main

open Fable.Core
open Fable.Import
open Fable.Import.Browser
open Fable.Import.Elmish

// ----------------------------------------------------------------------------
// PART 1: Domain model
// ----------------------------------------------------------------------------

type Phone = string
type Email = string

type ContactInfo =
  | EmailOnly of Email
  | PhoneOnly of Phone
  | Both of Email * Phone

type Contact = 
  { Name : string
    ContactInfo : ContactInfo }

type Event = 
  | GenerateNew
  | RemoveOne of Contact
  | RemoveAll

type State = 
  { Contacts : Contact list }

// ----------------------------------------------------------------------------
// PART 2: Generating random contacts
// ----------------------------------------------------------------------------

let rnd = System.Random()
let last = "Smith Johnson Williams Brown Jones Miller".Split(' ')
let first = "James John Robert Mary Patricia Linda".Split(' ')

let genEmail (n:string) = 
  n.Replace(' ', '.').ToLower() + "@gmail.com"
  
let genPhone () = 
  String.concat "" [ for i in 1 .. 9 -> string(rnd.Next 10) ]

let randomContact () = 
  let name = first.[rnd.Next first.Length] + " " + last.[rnd.Next last.Length]
  let contactInfo = 
    match (rnd.Next 3) with
    | 0 -> EmailOnly (genEmail name)  
    | 1 -> PhoneOnly (genPhone())
    | _ -> Both (genEmail name, genPhone())
  { Name = name; ContactInfo = contactInfo }

// ----------------------------------------------------------------------------
// PART 3: Render functions
// ----------------------------------------------------------------------------

let renderDeleteContactLink (removeMethod : Event -> unit) = 
  h?a [ "style" => "font-size: 8pt" 
        "href" => "javascript:;" 
        "click" =!> fun _ _ -> removeMethod ] [ text "Remove" ]

let renderContact contact = 
  h?li [] [
    yield h?h2 [] [ text contact.Name ]
    match contact.ContactInfo with
    | Both (email, phone) -> 
        yield h?p [] [ 
          text (sprintf "Call %s or email %s - " phone email)
        ]
    | PhoneOnly (phone) -> 
        yield h?p [] [ 
          text (sprintf "Call %s - " phone) 
        ]
    | EmailOnly email -> 
        yield h?p [] [ 
          text (sprintf "Email %s - " email)
        ]
  ]

let render trigger state = 
  h?div [] [
    h?hr [] []
    h?p [] [
      h?a [ "href" => "javascript:;"; "click" =!> fun _ _ -> trigger GenerateNew ] [ text "Add new contact" ]
      text " | "
      h?a [ "href" => "javascript:;"; "click" =!> fun _ _ -> trigger RemoveAll ] [ text "Remove all contacts" ]
    ]
    h?hr [] []
    h?ul [] [
      for contact in state.Contacts -> renderContact (trigger (RemoveOne contact)) contact
    ]
  ]

// ----------------------------------------------------------------------------
// PART 4: Update function and main
// ----------------------------------------------------------------------------

let update state event = 
  match event with
  | GenerateNew -> 
      let cnt = randomContact ()
      { state with Contacts = cnt::state.Contacts }
  | RemoveAll -> { Contacts = [] }
  | RemoveOne c -> { Contacts = List.except (List.toSeq [c]) state.Contacts }

let initial = { Contacts = [] }
createApp "main" initial render update

// ----------------------------------------------------------------------------
// TASKS
// ----------------------------------------------------------------------------
//
// TASK #1: To get familiar with the Elm-style architecture, implement the
// "Remove all contacts" button. To do this, you need to make the following
// three changes: First, you will need to define a new case of Event in 
// Part 1. Currently, there is `GenerateNew` so you can add e.g. `RemoveAll`.
// Second, add "click" handler to the <a> element generated in Part 3 in the
// `render` function (just copy the code from the existing <a> element and 
// change the event that is triggered). Third, implement the case to remove
// all contacts in the `update` function.
// 
// TASK #2: First, change the `randomContact` function in Part 2 so that it
// generates only valid contacts and test the application to see that it never
// generates contact with no contact information.
//
// TASK #3: Now, we want to improve the domain model so that a contact without
// contact information cannot be represented. Modify the application to use the
// representation discussed earlier in the talk.
