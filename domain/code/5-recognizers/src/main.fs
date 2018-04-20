module Classifiers.Main

open System
open Fable.Core
open Fable.Import
open Fable.Import.Browser
open System.Collections.Generic

// ------------------------------------------------------------------
// PART #1: Switching between implementations
// ------------------------------------------------------------------

// Choose implementation that you want to use
// Uncomment one of the following lines to do this - note
// that you need to change this here and also in `gui.fs`!

// open Classifiers.Data
open Classifiers.Functions

// ----------------------------------------------------------------------------
// DEMO: Creating simple classifiers
// ----------------------------------------------------------------------------

let win = ClassifierWindow()
win.Run("MSFT")

// Add some simple pattern classifiers (always rising never occurs!)
win.Add("Always up", Price.rising)
win.Add("Mostly up", Price.regression Price.rising)
win.Add("Mostly down", Price.regression Price.declining)
win.Add("Average", Price.average)

// Compose pattern for detecting the "V" pattern
let mostlyUp = Price.regression Price.rising
let mostlyDown = Price.regression Price.declining

// Model the V pattern
let v = 
  Price.sequence mostlyDown mostlyUp 
  |> Price.map (fun (a, b) -> a && b)

win.Add("V pattern", v)


// Pattern that detects when price is going up and is above given limit
let highAndRising limit = 
  Price.both
    mostlyUp
    ( Price.average |> Price.map (fun avg -> avg > limit) )
  |> Price.map (fun (a, b) -> a && b)

win.Add("High & rising", highAndRising 30.0)


// TASK #1: Make the above more readable by defining helper functions
// such as `bothAnd` and `sequenceAnd`. (Do we need to extend the core
// DSL to add those?)

// TASK #2: Create a classifier that detects the double bottom pattern
// (that is, price goes DOWN, UP, DOWN, UP - this needs to happen over
// linear regression - make sure that the pattern is ever matched!)

// TASK #3: Using the "data type" implementation, we can write a pretty
// printer that formats the patterns that classifier is looking for!
// Make it so that the classifier in #2 is formatted as "\/\/" etc.

// TASK #5: Extend the DSL so that it can recognize `plateau`. That is
// when a price within a given region stays in a provided range (say,
// $0.1 or something like that). 

// TASK #6: Create a "declining fast" classifier. That is, the price
// is declining (over a linear regression) and at the same time, the 
// difference between min and max is greater than $2.
