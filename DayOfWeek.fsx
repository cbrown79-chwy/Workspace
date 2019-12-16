type Year = int
type Month = | January = 1 | February = 2 | March = 3 | April = 4 | May = 5 | June = 6 | July = 7 | August = 8 | September = 9 | October = 10 | November = 11 | December = 12
type Day = int
type Date = { Year : Year; Month : Month; Day : Day }
type DayOfWeek = | Sunday = 1 | Monday = 2 | Tuesday = 3 | Wednesday = 4 | Thursday = 5 | Friday = 6 | Saturday = 7

let isLeapYear y = 
    y % 4 = 0 && y % 100 <> 0

let render d =
    sprintf "%A %i %i" d.Month (int d.Day) (int d.Year)

let getMonthDays date = 
    match date.Month with 
    | Month.September | Month.April | Month.June | Month.November -> 30
    | Month.February -> if isLeapYear date.Year then 29 else 28
    | _ -> 31

let compare d1 d2 = 
    if d1.Year <> d2.Year then sign (d1.Year - d2.Year)
    else if d1.Month <> d2.Month then sign ((int d1.Month) - (int d2.Month))
    else if d1.Day <> d2.Day then sign (d1.Day - d2.Day)
    else 0

let nextMonth date =
    {date with Month = (if date.Month = Month.December then Month.January else date.Month + Month.January); Year = (if date.Month = Month.December then date.Year + 1 else date.Year) }

let distanceInDays d1 d2 = 
    let m = compare d1 d2
    let (u1, u2) = if d2 < d1 then (d2, d1) else (d1, d2)
    let d = 
        if (u1.Year = u2.Year) && (u1.Month = u2.Month) then u2.Day - u1.Day 
        else 
            seq { 
                yield ((getMonthDays u1) - u1.Day)
                let mutable m = nextMonth u1
                while not ((m.Year = u2.Year) && (m.Month = u2.Month))
                    do 
                        yield getMonthDays m
                        m <- nextMonth m
                yield u2.Day
            } |> Seq.sum
    d * m

let KnownDate = { Year = 2019; Month = Month.December; Day =  1 } // This is always Sunday = 1

let getDayOfWeek date = 
    let distance = distanceInDays date KnownDate
    let value = (abs (distance % 7)) + 1
    enum<DayOfWeek>(value)