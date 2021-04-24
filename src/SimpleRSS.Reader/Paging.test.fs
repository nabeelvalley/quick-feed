module SimpleRSS.Reader.Test.Paging

open Xunit
open SimpleRSS.Reader.Paging

[<Fact>]
let ``page size 1`` () =
    let lst = [ 1; 2; 3; 4; 5 ]

    for i in 0 .. (lst.Length - 1) do
        let expected = [ lst.[i] ]
        let actual = applyPaging { pageSize = 1; pageNumber = i } lst

        Assert.Equal<int list>(expected, actual)

[<Fact>]
let ``page size greater than content`` () =
    let lst = [ 1; 2; 3 ]

    let expected = [ for i in lst do yield i ]
    let actual = applyPaging { pageSize = 5; pageNumber = 0 } lst

    Assert.Equal<int list>(expected, actual)

[<Fact>]
let ``page number greater than content`` () =
    let lst = [ 1; 2; 3 ]

    let expected = []
    let actual = applyPaging { pageSize = 3; pageNumber = 1 } lst

    Assert.Equal<int list>(expected, actual)

[<Fact>]
let ``nth page with page size greater than 1`` () =
    let lst = [ 1; 2; 3; 4; 5; 6; 7; 8; 9; 10 ]

    let expected = [ 5; 6 ]
    let actual = applyPaging { pageSize = 2; pageNumber = 2 } lst

    Assert.Equal<int list>(expected, actual)

[<Fact>]
let ``last page`` () =
    let lst = [ 1; 2; 3; 4; 5; 6 ]

    let expected = [ 5; 6 ]
    let actual = applyPaging { pageSize = 2; pageNumber = 2 } lst

    Assert.Equal<int list>(expected, actual)
