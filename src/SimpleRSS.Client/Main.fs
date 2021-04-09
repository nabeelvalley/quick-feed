module SimpleRSS.Client.Main

open System
open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client
open Bolero.Html

/// Routing endpoints definition.
type Page = | [<EndPoint "/">] Home

/// The Elmish application's model.
type Model =
    { page: Page
      error: string option
      username: string
      password: string
      signedInAs: option<string>
      signInFailed: bool }

let initModel =
    { page = Home
      error = None
      username = ""
      password = ""
      signedInAs = None
      signInFailed = false }

type UserService =
    { signIn: string * string -> Async<option<string>>
      getUsername: unit -> Async<string>
      signOut: unit -> Async<unit> }

    interface IRemoteService with
        member this.BasePath = "/user"

/// The Elmish application's update messages.
type Message =
    | SetPage of Page
    | SetUsername of string
    | SetPassword of string
    | GetSignedInAs
    | RecvSignedInAs of option<string>
    | SendSignIn
    | RecvSignIn of option<string>
    | SendSignOut
    | RecvSignOut
    | Error of exn
    | ClearError

let update remote message model =
    match message with
    | SetPage page -> { model with page = page }, Cmd.none

    | SetUsername s -> { model with username = s }, Cmd.none
    | SetPassword s -> { model with password = s }, Cmd.none
    | GetSignedInAs -> model, Cmd.OfAuthorized.either remote.getUsername () RecvSignedInAs Error
    | RecvSignedInAs username -> { model with signedInAs = username }, Cmd.none
    | SendSignIn -> model, Cmd.OfAsync.either remote.signIn (model.username, model.password) RecvSignIn Error
    | RecvSignIn username ->
        { model with
              username = ""
              password = ""
              signedInAs = username
              signInFailed = Option.isNone username },
        Cmd.none
    | SendSignOut -> model, Cmd.OfAsync.either remote.signOut () (fun () -> RecvSignOut) Error
    | RecvSignOut ->
        { model with
              signedInAs = None
              signInFailed = false },
        Cmd.none

    | Error RemoteUnauthorizedException ->
        { model with
              error = Some "You have been logged out."
              signedInAs = None },
        Cmd.none
    | Error exn -> { model with error = Some exn.Message }, Cmd.none
    | ClearError -> { model with error = None }, Cmd.none

/// Connects the routing system to the Elmish application.
let router =
    Router.infer SetPage (fun model -> model.page)

type Main = Template<"wwwroot/main.html">

let homePage model dispatch =
    let displayUsername =
        match model.signedInAs with
        | None -> "anon"
        | Some s -> s

    Main
        .Home()
        .Username(displayUsername)
        .SignOut(fun _ -> dispatch SendSignOut)
        .Elt()

let menuItem (model: Model) (page: Page) (text: string) =
    Main
        .MenuItem()
        .Active(
            if model.page = page then
                "is-active"
            else
                ""
        )
        .Url(router.Link page)
        .Text(text)
        .Elt()

let loginPage (model: Model) dispatch =
    printfn "%s" model.username |> ignore

    div [] [
        form [ on.submit (fun _ -> dispatch SendSignIn) ] [
            div [] [
                input [ attr.placeholder "Username"
                        bind.input.string model.username (SetUsername >> dispatch) ]
                input [ attr.placeholder "Password"
                        bind.input.string model.password (SetPassword >> dispatch) ]
            ]
            div [] [
                button [ attr.``type`` "submit" ] [
                    text "Sign In"
                ]
            ]
        ]

        if model.signInFailed then
            text "Sign in failed. Use any username and the password \"password\"."
        else
            empty
    ]

let view model dispatch =
    printfn "%O" model |> ignore

    let body =
        match model.signedInAs with
        | None -> loginPage model dispatch
        | Some s ->
            match model.page with
            | Home -> homePage model dispatch

    Main()
        .Menu(concat [ menuItem model Home "Home" ])
        .Body(body)
        .Error(
            match model.error with
            | None -> empty
            | Some err ->
                Main
                    .ErrorNotification()
                    .Text(err)
                    .Hide(fun _ -> dispatch ClearError)
                    .Elt()
        )
        .Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        let userService = this.Remote<UserService>()
        let update = update userService

        Program.mkProgram (fun _ -> initModel, Cmd.ofMsg GetSignedInAs) update view
        |> Program.withRouter router
#if DEBUG
        |> Program.withHotReload
#endif
