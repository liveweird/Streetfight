module Client

open Elmish
open Elmish.React

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack.Fetch

open Shared

open Fulma
open Fulma.Layouts
open Fulma.Elements
open Fulma.Components
open Fulma.BulmaClasses

open Fulma.BulmaClasses.Bulma
open Fulma.BulmaClasses.Bulma.Properties
open Fulma.Extra.FontAwesome

type Model = Counter option

type Msg =
| Increment
| Decrement
| Init of Result<Counter, exn>


module Server = 

  open Shared
  open Fable.Remoting.Client
  
  /// A proxy you can use to talk to server directly
  let api : ICounterProtocol = 
    Proxy.createWithBuilder<ICounterProtocol> Route.builder
    

let init () = 
  let model = None
  let cmd =
    Cmd.ofAsync 
      Server.api.getInitCounter
      () 
      (Ok >> Init)
      (Error >> Init)
  model, cmd

let update msg (model : Model) =
  let model' =
    match model,  msg with
    | Some x, Increment -> Some (x + 1)
    | Some x, Decrement -> Some (x - 1)
    | None, Init (Ok x) -> Some x
    | _ -> None
  model', Cmd.none

let safeComponents =
  let intersperse sep ls =
    List.foldBack (fun x -> function
      | [] -> [x]
      | xs -> x::sep::xs) ls []

  let components =
    [
      "Suave", "http://suave.io"
      "Fable", "http://fable.io"
      "Elmish", "https://fable-elmish.github.io/"
      "Fulma", "https://mangelmaxime.github.io/Fulma" 
      "Bulma\u00A0Templates", "https://dansup.github.io/bulma-templates/"
      "Fable.Remoting", "https://github.com/Zaid-Ajaj/Fable.Remoting"
    ]
    |> List.map (fun (desc,link) -> a [ Href link ] [ str desc ] )
    |> intersperse (str ", ")
    |> span [ ]

  p [ ]
    [ strong [] [ str "SAFE Template" ]
      str " powered by: "
      components ]

let show = function
| Some x -> string x
| None -> "Loading..."

let navBrand =
  Navbar.Brand.div [ ] 
    [ Navbar.Item.a 
        [ Navbar.Item.Props [ Href "https://safe-stack.github.io/" ] ] 
        [ img [ Src "https://safe-stack.github.io/images/safe_top.png"
                Alt "Logo" ] ] 
      Navbar.burger [ ] 
        [ span [ ] [ ]
          span [ ] [ ]
          span [ ] [ ] ] ]

let navMenu =
  Navbar.menu [ ]
    [ Navbar.End.div [ ] 
        [ Navbar.Item.a [ ] 
            [ str "Home" ] 
          Navbar.Item.a [ ]
            [ str "Examples" ]
          Navbar.Item.a [ ]
            [ str "Documentation" ]
          Navbar.Item.div [ ]
            [ Button.a 
                [ Button.Color IsWhite
                  Button.IsOutlined
                  Button.Size IsSmall
                  Button.Props [ Href "https://github.com/SAFE-Stack/SAFE-template" ] ] 
                [ Icon.faIcon [ ] 
                    [ Fa.icon Fa.I.Github; Fa.fw ]
                  span [ ] [ str "View Source" ] ] ] ] ]

let buttonBox model dispatch =
  Box.box' [ CustomClass "cta" ]
    [ Level.level [ ]
        [ Level.item [ ]
            [ Button.a 
                [ Button.Color IsPrimary
                  Button.OnClick (fun _ -> dispatch Increment) ]
                [ str "+" ] ]

          Level.item [ ]
            [ p [ ] [ str (show model) ] ]

          Level.item [ ]
            [ Button.a 
                [ Button.Color IsPrimary
                  Button.OnClick (fun _ -> dispatch Decrement) ]
                [ str "-" ] ] ] ]

let card icon heading body =
  Column.column [ Column.Width (Column.All, Column.Is4) ]
    [ Card.card [ ]
        [ div
            [ ClassName (Card.Classes.Image + " " + Alignment.HasTextCentered) ]
            [ i [ ClassName ("fa fa-" + icon) ] [ ] ]
          Card.content [ ]
            [ Content.content [ ]
                [ h4 [ ] [ str heading ]
                  p [ ] [ str body ]
                  p [ ]
                    [ a [ Href "#" ]
                        [ str "Learn more" ] ] ] ] ] ]

let features = 
  Columns.columns [ Columns.CustomClass "features" ]
    [ card "paw" "Tristique senectus et netus et." "Purus semper eget duis at tellus at urna condimentum mattis. Non blandit massa enim nec. Integer enim neque volutpat ac tincidunt vitae semper quis. Accumsan tortor posuere ac ut consequat semper viverra nam."
      card "id-card-o" "Tempor orci dapibus ultrices in." "Ut venenatis tellus in metus vulputate. Amet consectetur adipiscing elit pellentesque. Sed arcu non odio euismod lacinia at quis risus. Faucibus turpis in eu mi bibendum neque egestas cmonsu songue. Phasellus vestibulum lorem sed risus."
      card "rocket" "Leo integer malesuada nunc vel risus." "Imperdiet dui accumsan sit amet nulla facilisi morbi. Fusce ut placerat orci nulla pellentesque dignissim enim. Libero id faucibus nisl tincidunt eget nullam. Commodo viverra maecenas accumsan lacus vel facilisis." ]

let intro =
  Column.column 
    [ Column.CustomClass "intro"
      Column.Width (Column.All, Column.Is8)
      Column.Offset (Column.All, Column.Is2) ]
    [ h2 [ ClassName "title" ] [ str "Perfect for developers or designers!" ]
      br [ ]
      p [ ClassName "subtitle"] [ str "Vel fringilla est ullamcorper eget nulla facilisi. Nulla facilisi nullam vehicula ipsum a. Neque egestas congue quisque egestas diam in arcu cursus." ] ]

let tile title subtitle content =
  article [ ClassName "tile is-child notification is-white" ]
    [ yield p [ ClassName "title" ] [ str title ]
      yield p [ ClassName "subtitle" ] [ str subtitle ]
      match content with
      | Some c -> yield c
      | None -> () ]

let content txts =
  Content.content [ ]
    [ for txt in txts -> p [ ] [ str txt ] ]

module Tiles =
  let hello = tile "Hello World" "What is up?" None

  let foo = tile "Foo" "Bar" None

  let third = 
    tile 
      "Third column"
      "With some content"
      (Some (content ["Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin ornare magna eros, eu pellentesque tortor vestibulum ut. Maecenas non massa sem. Etiam finibus odio quis feugiat facilisis."]))

  let verticalTop = tile "Vertical tiles" "Top box" None
  
  let verticalBottom = tile "Vertical tiles" "Bottom box" None

  let middle = 
    tile 
      "Middle box" 
      "With an image"
      (Some (figure [ ClassName "image is-4by3" ] [ img [ Src "http://bulma.io/images/placeholders/640x480.png"] ]))

  let wide =
    tile
      "Wide column"
      "Aligned with the right column"
      (Some (content ["Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin ornare magna eros, eu pellentesque tortor vestibulum ut. Maecenas non massa sem. Etiam finibus odio quis feugiat facilisis."]))

  let tall =
    tile
      "Tall column"
      "With even more content"
      (Some (content 
              ["Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam semper diam at erat pulvinar, at pulvinar felis blandit. Vestibulum volutpat tellus diam, consequat gravida libero rhoncus ut. Morbi maximus, leo sit amet vehicula eleifend, nunc dui porta orci, quis semper odio felis ut quam."
               "Suspendisse varius ligula in molestie lacinia. Maecenas varius eget ligula a sagittis. Pellentesque interdum, nisl nec interdum maximus, augue diam porttitor lorem, et sollicitudin felis neque sit amet erat. Maecenas imperdiet felis nisi, fringilla luctus felis hendrerit sit amet. Aenean vitae gravida diam, finibus dignissim turpis. Sed eget varius ligula, at volutpat tortor."
               "Integer sollicitudin, tortor a mattis commodo, velit urna rhoncus erat, vitae congue lectus dolor consequat libero. Donec leo ligula, maximus et pellentesque sed, gravida a metus. Cras ullamcorper a nunc ac porta. Aliquam ut aliquet lacus, quis faucibus libero. Quisque non semper leo."]))

  let side = 
    tile
      "Side column"
      "With some content"
      (Some (content ["Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin ornare magna eros, eu pellentesque tortor vestibulum ut. Maecenas non massa sem. Etiam finibus odio quis feugiat facilisis."]))

  let main =
    tile
      "Main column"
      "With some content"
      (Some (content ["Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin ornare magna eros, eu pellentesque tortor vestibulum ut. Maecenas non massa sem. Etiam finibus odio quis feugiat facilisis."]))


let sandbox =
  div [ ClassName "sandbox" ]
    [ Tile.ancestor [ ]
        [ Tile.parent [ ] 
            [ Tiles.hello ]
          Tile.parent [ ] 
            [ Tiles.foo ]
          Tile.parent [  ] 
            [ Tiles.third ] ]
      Tile.ancestor [ ]
        [ Tile.tile [ Tile.IsVertical; Tile.Size Tile.Is8 ]
            [ Tile.tile [ ] 
                [ Tile.parent [ Tile.IsVertical ] 
                    [ Tiles.verticalTop
                      Tiles.verticalBottom ]
                  Tile.parent [ ] 
                    [ Tiles.middle ] ]
              Tile.parent [ ]
                [ Tiles.wide ] ]
          Tile.parent [ ]
            [ Tiles.tall ] ]
      Tile.ancestor [ ]
        [ Tile.parent [ ]
            [ Tiles.side ]
          Tile.parent [ Tile.Size Tile.Is8 ]
            [ Tiles.main ] ] ]

let footerContainer =
  Container.container [ ]
    [ Content.content [ Content.CustomClass Alignment.HasTextCentered ] 
        [ p [ ] 
            [ safeComponents ]
          p [ ]
            [ a [ Href "https://github.com/SAFE-Stack/SAFE-template" ]
                [ Icon.faIcon [ ] 
                    [ Fa.icon Fa.I.Github; Fa.fw ] ] ] ] ]

let view model dispatch =
  div [ ]
    [ Hero.hero 
        [ Hero.Color IsPrimary
          Hero.IsMedium
          Hero.IsBold ]
        [ Hero.head [ ]
            [ Navbar.navbar [ ]
                [ Container.container [ ] 
                    [ navBrand
                      navMenu ] ] ]
          Hero.body [ ]
            [ Container.container [ Container.CustomClass Alignment.HasTextCentered ]
                [ h1 [ ClassName "title" ] 
                    [ str "SAFE Template" ]
                  div [ ClassName "subtitle" ]
                    [ safeComponents ] ] ] ]
      
      buttonBox model dispatch
      
      Container.container [ ]
        [ features
          intro
          sandbox ]

      footer [ ClassName "footer" ]
        [ footerContainer ] ]

  
#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
