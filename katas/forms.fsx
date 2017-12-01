open System
open System.Windows.Forms

[<STAThread>]
let main args =
    let form1 = new Form()
    form1.Text <- "XYZ"
    form1.ShowDialog() |> ignore