- title : Capturing domain logic in F#
- description : From types to domain specific languages
- author : Tomas Petricek
- theme : white
- transition : none

****************************************************************************************************

# Capturing domain logic in F#

## The F# language foundations

<img src="images/fsharpworks.png" style="width:300px;position:absolute;right:0px;margin-top:250px" />
<div style="position:absolute;text-align:left;left:0px;margin-top:220px">

Tomas Petricek<br />
[www.fsharpworks.com](http://www.fsharpworks.com)

</div>

****************************************************************************************************

# INTRO I

## Expressions, let, type inference and functions

----------------------------------------------------------------------------------------------------

# Everything is an expression

Expressions define _values_ and return them

    123 + ("hello " + "world").Length

Even C# statements are expressions

    "The world is" + (if 1 > 0 then " OK" else " wrong")

Side-effectful operations return `unit` value

    let n = printfn "Hello world"

You can omit `else` when returning `unit`:

    if 1 > 0 then printfn "Everything is OK!"

----------------------------------------------------------------------------------------------------

# Whitespace is significant in F#

Body of definitions is indented further

    let hello =
      "Hello" + " world"

Unit-returning operations can be sequenced

    let greetAndReturn () =
      printfn "Hello"
      printfn "world"
      42

----------------------------------------------------------------------------------------------------

# Let, our first keyword

F# uses _type inference_ to deduce the type

    let intValue = 40 + 2
    let floatValue = 39.9 + 2.1
    let pairList = [ ("F#", 3); ("C#", 4000) ]

The let keyword is used _for functions_ too

    let add a b =
      a + b

    let hello () =
      printfn "Hello world!"

Arguments can be names or the `unit` value!

----------------------------------------------------------------------------------------------------

# Let is also a part of an expression

Let must be followed by a body!

    // Usually separated by newline
    let num = 40 + 2
    printfn "%d" num

    // Implicitly inserts 'in' keyword
    let num = 40 + 2 in printf "%d" num

Missing body is an invalid expression!

    let hello () =
      let nonsense = 40 + 3

****************************************************************************************************

# INTRO II

## Tuples, functions and pipelines

----------------------------------------------------------------------------------------------------

# Creating and decomposing tuples

Write `val1, val2` with optional parentheses

    let person = ("Phil", 27)

Use _pattern matching_ to extract the components

    let (name, age) = person

Note the _symmetry_ between the two!

The type `string * int` becomes `Tuple<string, int>`

----------------------------------------------------------------------------------------------------

# Mixing tuples and functions

Functions can take tuples as arguments:

    let add pair =
      let a, b = pair
      a + b

Using pattern matching on the argument:

    let add (a, b) = a + b

----------------------------------------------------------------------------------------------------

# Mixing tuples and functions

Use tuples when _returning_ multiple values

Use tuples when using _logical composed value_

    let rotate (x, y) = (y, x)
    let moveX by (x, y) = (x + by, y)
    let area (x, y) = x * y

Tuples make your code more composable

    area (rotate (moveX 10 (15, 15)))

----------------------------------------------------------------------------------------------------

# The pipeline operator

Function calls are written _backwards_

    area (rotate (moveX 10 (15, 15)))

Pipeline can be used for _readability_

    (15, 15)
    |> moveX 10
    |> rotate
    |> area

----------------------------------------------------------------------------------------------------

# Looking under the cover

Using _partial function application_

    let moveBy10 = moveX 10
    (15, 15) |> moveBy10

This is exactly what `|>` does

    let (|>) f x = f x

The operator is just a _library definition_

----------------------------------------------------------------------------------------------------

# Using pipeline for list processing

Using `fun x -> ...` for anonymous (lambda) functions:

    [ 1 .. 10 ]
    |> Seq.filter (fun x -> x > 5)
    |> Seq.map (fun x -> x / 2)

F# encourages _transformational_ style

****************************************************************************************************

# INTRO III

## Records, unions and options

----------------------------------------------------------------------------------------------------

# The record type

Like _named tuples_ or named C# _anonymous types_

Records have to be _declared_ in advance

    type Lang = { Name : string; Lines : int }

To create values and access properties use

    let l = { Name = "F#"; Lines = 10 }
    printfn "%s has %d lines" l.Name l.Lines

Use the `with` construct to create cloned values

    let l2 = { l with Name = l.Name + " Enterprise"}

----------------------------------------------------------------------------------------------------

# The discriminated union type

Has to be _declared_ in advance

    type ValidationResult =
      | Invalid of string
      | Valid

Understanding the definition:

 - `ValidationResult` is a type name
 - `Invalid` and `Valid` are constructors

Similar to class hierarchies in OOP

----------------------------------------------------------------------------------------------------

# The discriminated union type

Create values of `ValidationResult` using constructors:

    let v1 = Invalid "Name was missing!"
    let v2 = Valid

Process values using pattern matching:

    match v1 with
    | Invalid error -> printfn "Failed: %s" error
    | Valid -> printfn "All good!"

----------------------------------------------------------------------------------------------------

# Billion dollar mistake

I call it my _billion-dollar mistake_. It was the invention of the  
........................... in 1965.
[...] I couldn't resist the temptation to put in ..........................., simply because it was so
easy to implement. This has led to innumerable errors, vulnerabilities, and system crashes,
which have probably caused a billion dollars of pain and damage in the last forty years.

<p style="text-align:right;width:100%">Tony Hoare</p>

----------------------------------------------------------------------------------------------------

# Billion dollar mistake

I call it my _billion-dollar mistake_. It was the invention of the  
`null` reference in 1965.
[...] I couldn't resist the temptation to put in a `null` reference, simply because it was so
easy to implement. This has led to innumerable errors, vulnerabilities, and system crashes,
which have probably caused a billion dollars of pain and damage in the last forty years.

<p style="text-align:right;width:100%">Tony Hoare</p>

----------------------------------------------------------------------------------------------------

# Introducing the option type

Defined in the standard F# library

    type Option<'T> =
      | Some of 'T
      | None

Understanding the option type:

 - It is a generic type (for any `'T`)
 - Has two cases - `Some` and `None`
 - Makes missing values explicit

----------------------------------------------------------------------------------------------------

# How F# prevents null errors?

Using `null` for missing values in C#

    [lang=csharp]
    var person = GetPersonById(123)
    Console.WriteLine(person.Name);

Using the `option` type for missing values in F#

    let personOpt = getPersonById(123)
    match personOpt with
    | None -> printfn "Missing!"
    | Some p -> printfn "%s" p.name

Type `Option<Person>` differs from `Person`!

****************************************************************************************************

## Agenda

 - [Introduction and why F#](index.html)
 - **The F# language foundations**
 - [Domain modelling & decision trees](2-domain.html)
 - [Domain specific languages](3-dsls.html)
 - [Closing notes](4-closing.html)
 