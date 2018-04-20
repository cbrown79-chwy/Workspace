- title : Capturing domain logic in F#
- description : From types to domain specific languages
- author : Tomas Petricek
- theme : white
- transition : none

****************************************************************************************************

# Capturing domain logic in F#

## Domain modelling & decision trees

<img src="images/fsharpworks.png" style="width:300px;position:absolute;right:0px;margin-top:250px" />
<div style="position:absolute;text-align:left;left:0px;margin-top:220px">

Tomas Petricek  
[www.fsharpworks.com](http://www.fsharpworks.com)

</div>

****************************************************************************************************

# DOMAIN I

## Modelling with F# types

----------------------------------------------------------------------------------------------------

# _TDD_: Type-driven-development

Start by thinking about the types in your domain

## Concrete **data types**

    type Price = decimal
    type Quantity = decimal
    type Product =
      { Name : string; UnitPrice : Price }

## Abstract data types

    type IWarehouse =
      abstract HasInventory : Product * Quantity -> bool

----------------------------------------------------------------------------------------------------

# Composing functional types

## Compose domain using three building blocks

** <i class="fa fa-times-circle"></i> Product** using Tuples or Records

 - Combine values of different types in one value

** <i class="fa fa-plus-circle"></i> Choice** using Discriminated Unions

 - Choose one of several alternative values

** <i class="fa fa-chevron-circle-right"></i> Repetition** using Sequences and Lists

 - Zero or more values of the same type


----------------------------------------------------------------------------------------------------

# Make invalid states unrepresentable

**First representation** of a contact:

    type Contact =
      { Name : string
        Email : EmailAddress option
        Phone : PhoneNumber option }

**Second representation** of a contact:

    type ContactInfo =
      | EmailOnly of EmailAddress
      | PhoneOnly of PhoneNumber
      | EmailAndPhone of EmailAddress * PhoneNumber
    type Contact =
      { Name : string; Contact : ContactInfo }

What is the differnece? Which `Contact` is the _correct type_?


----------------------------------------------------------------------------------------------------

<table><tr><td style="padding-right:40px">

## Discriminated union

<div style="padding:65px 0px 65px 0px">

    type Result =
      | Valid of int
      | Invalid of string

</div>

 - Easy to add functions
 - Hard to add new cases

</td><td style="padding-left:40px">

## Class hierarchy

    [lang=csharp]
    abstract class Result { }

    class Valid : Result {
      int Number { get; } }
    }
    class Invalid : Result {
      string Error { get; }
    }

 - Easy to add new clases
 - Hard to add new methods

</td></tr></table>

F# is _functional-first_ but supports _both options_!

****************************************************************************************************

# DOMAIN II
## Decision trees

----------------------------------------------------------------------------------------------------

# Decision trees

<br />
<img src="images/ml-tree.png" style="width:700px;margin-left:50px" />

----------------------------------------------------------------------------------------------------

## Decision tree

 - Learning infers the _structure of the tree_
 - Results are _easy to interpret_
 - But _learning one tree_ is hard

## Random forests

 - Generate large number of _small trees_
 - Use _subset of data and indicators_
 - Can be nicely _parallelized_

----------------------------------------------------------------------------------------------------

# Modeling decision trees with F#

## Decision tree is

 - _Leaf_ containing the outcome (class)
 - _Node_ condition with two or more outcomes

<div class="fragment">

## Decision tree is

    type DecisionChoice =
      { Feature   : Feature
        Threshold : float
        LessThan  : DecisionTree
        MoreThan  : DecisionTree }

    and DecisionTree =
      | DecisionChoice of DecisionChoice
      | DecisionResult of string

</div>

****************************************************************************************************

## Agenda

 - [Introduction and why F#](index.html)
 - [The F# language foundations](1-intro.html)
 - **Domain modelling & decision trees**
 - [Domain specific languages](3-dsls.html)
 - [Closing notes](4-closing.html)
 