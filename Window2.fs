namespace MultiWindow

open Avalonia.Threading

module Window2 =
    open Elmish
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI.Components.Hosts
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Elmish

    type State =
        { count: int }

    let init = { count = 0 }

    type Msg =
        | Increment
        | Decrement
        | Reset

    let update (msg: Msg) (state: State): State =
        match msg with
        | Increment -> { state with count = state.count + 1 }
        | Decrement -> { state with count = state.count - 1 }
        | Reset -> init

    let view (state: State) (dispatch) =
        DockPanel.create
            [ DockPanel.children
                [ Button.create
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

    type Window2() as this =
        inherit HostWindow()
        do
            base.Title <- "Window2"
            base.Width <- 400.0
            base.Height <- 400.0

            //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
            //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true
            Elmish.Program.mkSimple (fun () -> init) update view
            |> Program.withHost this
            |> Program.run
