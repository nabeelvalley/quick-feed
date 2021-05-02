module SimpleRSS.Reader.Test.Paging

open System
open Xunit
open SimpleRSS.Reader.Paging
open SimpleRSS.Reader.Utils
open Xunit.Sdk

[<Fact>]
let ``paging options creates successfully with valid arguments`` () =
    match (createPagingOptions 1 0) with
    | Ok _ -> ignore
    | Error _ -> raise (XunitException "Valid paging options should successfully create an instance.")
    |> ignore

let ``paging options should error with invalid page size`` () =
    match (createPagingOptions 0 0) with
    | Error e when (e.Contains("size", StringComparison.InvariantCultureIgnoreCase)) -> ignore
    | _ -> raise (XunitException "Invalid page size should result in an error with mention of invalid page size.")
    |> ignore

[<Fact>]
let ``paging options should error with invalid page number`` () =
    match (createPagingOptions 1 -1) with
    | Error e when (e.Contains("number", StringComparison.InvariantCultureIgnoreCase)) -> ignore
    | _ -> raise (XunitException "Invalid page number should result in an error with mention of invalid page number.")
    |> ignore

[<Fact>]
let ``page size 1`` () =
    let lst = [ 1; 2; 3; 4; 5 ]

    for i in 0 .. (lst.Length - 1) do
        let expected = [ lst.[i] ]
        let pagingOpts = createPagingOptions 1 i
        let actual = applyPaging (unwrap pagingOpts) lst

        Assert.Equal<int list>(expected, actual)

[<Fact>]
let ``page size greater than content`` () =
    let lst = [ 1; 2; 3 ]

    let expected = [ for i in lst do yield i ]
    let pagingOpts = createPagingOptions 5 0
    let actual = applyPaging (unwrap pagingOpts) lst

    Assert.Equal<int list>(expected, actual)

[<Fact>]
let ``page number greater than content`` () =
    let lst = [ 1; 2; 3 ]

    let expected = []
    let pagingOpts = createPagingOptions 3 1
    let actual = applyPaging (unwrap pagingOpts) lst

    Assert.Equal<int list>(expected, actual)

[<Fact>]
let ``nth page with page size greater than 1`` () =
    let lst = [ 1; 2; 3; 4; 5; 6; 7; 8; 9; 10 ]

    let expected = [ 5; 6 ]
    let pagingOpts = createPagingOptions 2 2
    let actual = applyPaging (unwrap pagingOpts) lst

    Assert.Equal<int list>(expected, actual)

[<Fact>]
let ``last page`` () =
    let lst = [ 1; 2; 3; 4; 5; 6 ]

    let expected = [ 5; 6 ]
    let pagingOpts = createPagingOptions 2 2
    let actual = applyPaging (unwrap pagingOpts) lst

    Assert.Equal<int list>(expected, actual)
