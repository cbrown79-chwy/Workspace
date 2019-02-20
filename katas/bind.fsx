open FSharp





type OrderQty = OrderQty of int

let optionBind fn obj =
    match obj with 
    | Some x -> fn x 
    | None -> None

let parseInt n = 
    match n with 
    | "-1" -> Some -1
    | "0" -> Some 0
    | "1" -> Some 1
    | _ -> None

let order qty = 
    if qty < 1 then
        Some (OrderQty qty)
    else
        None

let composed str = 
    parseInt str 
        >>= order


