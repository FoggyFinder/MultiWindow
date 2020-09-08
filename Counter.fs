namespace MultiWindow

module Counter =
    open Elmish
    open System
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI.Components.Hosts
    open Avalonia.FuncUI.DSL

    type State =
        { count: int }

    let init() = { count = 0 }, Cmd.none

    type Msg =
        | Increment
        | Decrement
        | Reset
        | OpenWindow
        | WindowSuccess
        | WindowOpenError of string

    let update (msg: Msg) (state: State) (mainWindow: HostWindow) (dialog: HostWindow): State * Cmd<Msg> =
        match msg with
        | Increment -> { state with count = state.count + 1 }, Cmd.none
        | Decrement -> { state with count = state.count - 1 }, Cmd.none
        | Reset -> init()
        | OpenWindow ->
            let cmd = 
                async {
                    try
                        do! mainWindow.ShowDialog(dialog) |> Async.AwaitTask
                        return WindowSuccess
                    with 
                    | :? Exception as ex ->
                        return WindowOpenError (ex.Message)
                } |> Cmd.OfAsync.result
            state, cmd
        | WindowSuccess ->
            printfn "Success!"
            state, Cmd.none
        | WindowOpenError err ->
            printfn "Error: %s" err
            state, Cmd.none

    let view (state: State) (dispatch) =
        DockPanel.create
            [ DockPanel.children
                [ Button.create
                    [ Button.dock Dock.Bottom
                      Button.onClick (fun _ -> dispatch OpenWindow)
                      Button.content "Open Second Window" ]
                  Button.create
                      [ Button.dock Dock.Bottom
                        Button.onClick (fun _ -> dispatch Reset)
                        Button.content "reset" ]
                  Button.create
                      [ Button.dock Dock.Bottom
                        Button.onClick (fun _ -> dispatch Decrement)
                        Button.content "-" ]
                  Button.create
                      [ Button.dock Dock.Bottom
                        Button.onClick (fun _ -> dispatch Increment)
                        Button.content "+" ]
                  TextBlock.create
                      [ TextBlock.dock Dock.Top
                        TextBlock.fontSize 48.0
                        TextBlock.verticalAlignment VerticalAlignment.Center
                        TextBlock.horizontalAlignment HorizontalAlignment.Center
                        TextBlock.text (string state.count) ] ] ]
