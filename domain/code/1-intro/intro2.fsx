// ============================================================================
// INTRO 2: Tuples and functions
// ============================================================================

#load "setup.fsx"
open System
open Setup
open System

// ----------------------------------------------------------------------------
// WALKTHROUGH: Tuples and functions taking tuples
// ----------------------------------------------------------------------------

// Tuple is a simple type that groups two or more values of possibly
// different types. The following sample uses tuples to represent people -
// note that the parentheses are optional.
//
// When decomposing tuple, you can write *pattern* that consists of new
// variables, to be used for individual components of the tuple.
let person1 = ("Ludwig", 56)
let (name1, age1) = person1

let person2 = "Ludwig", "Wittgenstein", 56
let name2, surname2, age2 = person2

shouldEqual name1 "Ludwig"
shouldEqual surname2 "Wittgenstein"

// F# also provides two simple functions for working with two-element tuples
let person = "Ludwig", 56
shouldEqual (fst person) "Ludwig"
shouldEqual (snd person) 56


// Tuples can be useful when you want to return mutliple values as the result
// of a function - for example name and age. The following snippet also
// shows how to use the 'if .. then .. else' construct in F#
let getPerson job =
  if job = "philosopher" then ("Ludwig", 56)
  elif job = "scientist" then ("Albert", 66)
  else ("Someone", 10)

shouldEqual (fst (getPerson "scientist")) "Albert"
shouldEqual (snd (getPerson "scientist")) 66

// F# uses "structural equality" which means that tuples containing the
// same values are treated as equal. For example, try the following:
shouldEqual ("Joe", 13) ("Joe", 13)
shouldEqual (getPerson "philosopher") ("Ludwig", 56)


// When writing functions that take tuples as arguments, the tuple is just
// a single parameter, so you can take e.g. 'person' and then decompose it
// into two values. However, you can write the same thing more compactly
// by using the pattern directly in the argument of the function. The following
// two functions are the same:
let addYear1 person =
  let name, age = person
  name, age + 1

let addYear2 (name, age) =
  name, age + 1

shouldEqual (addYear1 (getPerson "scientist")) ("Albert", 67)
shouldEqual (addYear2 (getPerson "philosopher")) ("Ludwig", 57)

// In some cases, you may need to provide type annotation to specify the
// type explicitly. For example, when you want to call a .NET member,
// the F# compiler needs to know the type (so that it can check whether the
// member exists). You can annotate single variables or composed patterns.
let getLength (name:string) =
  name.Length

let getNameLength ((name, age):string * int) =
  name.Length

shouldEqual (getLength "Ludwig") 6
shouldEqual (getNameLength (getPerson "philosopher")) 6

// ============================================================================
// TASK: Validating inputs
// ============================================================================

// In this task, we want to write a simple validator which tests whether a
// name and age represents a valid person. A valid person details:
//
//  - Have age between 0 and 150 (inclusive)
//  - Start with an upper-case letter
//  - Contains a space & letter after space is upper case
//
// You'll need "str.[index]" to access character at a given index,
// "Char.IsUpper" to check whether character is upper case and string
// operations including "str.IndexOf" and "str.Contains".

let validAge person = 
  snd person < 151 && snd person > -1

let split (str:string) = 
  str.Split([| ' ' |], StringSplitOptions.None)

let firstCharIsUpper (str:string) = 
  str.Length > 0 && Char.IsUpper(str.[0])

let validName (name, _) =
  let names = 
    name 
    |> split
    |> Array.toList
  (List.forall firstCharIsUpper names) && (List.length names > 1) 

let validPerson person =
  validAge person && validName person

shouldEqual (validPerson ("Tomas Petricek", 42)) true
shouldEqual (validPerson ("Tomas Petricek", 242)) false
shouldEqual (validPerson ("Tomas", 42)) false
shouldEqual (validPerson ("Tomas petricek", 42)) false
shouldEqual (validPerson ("tomas Petricek", 42)) false
shouldEqual (validPerson ("Tomas Petricek  A", 42)) false
