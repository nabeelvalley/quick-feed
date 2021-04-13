namespace SimpleRSS.Persistence

open Xunit
open SimpleRSS.Persistence.InMemory
open SimpleRSS.Persistence.Types.Connector

module InMemoryTest =

    let testItems = [ "zero"; "one"; "two" ]

    [<Theory>]
    [<InlineData("test0")>]
    [<InlineData("test1")>]
    [<InlineData("test2")>]
    let ``InMemoryStore.create -> adds item to db`` (item) =
        let expectedEl = CreateResult.Success(0, item)
        let expectedDb = GetManyResult.Success [ 0, item ]

        let sut =
            new InMemoryStore<string>() :> Store<int, string>

        let actual = sut.create (item)
        let db = sut.getAll ()

        Assert.Equal(expectedEl, actual)
        Assert.Equal(expectedDb, db)

    [<Theory>]
    [<InlineData(0, "zero")>]
    [<InlineData(1, "one")>]
    [<InlineData(2, "two")>]
    let ``InMemoryStore.create multiple-> adds all to db`` (index, expected) =
        let expectedDb =
            testItems
            |> List.mapi (fun i item -> (i, item))
            |> GetManyResult.Success

        let sut =
            new InMemoryStore<string>() :> Store<int, string>

        List.map sut.create testItems |> ignore

        let actual = sut.getAll ()

        Assert.Equal(expectedDb, actual)


    [<Theory>]
    [<InlineData(0, "zero")>]
    [<InlineData(1, "one")>]
    [<InlineData(2, "two")>]
    let ``InMemoryStore.get id -> correct item`` (index, value) =
        let expected = GetResult.Success(index, value)

        let sut =
            new InMemoryStore<string>() :> Store<int, string>

        List.map sut.create testItems |> ignore

        let actual = sut.get index

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``InMemoryStore.get invalid id -> NotFound`` () =
        let sut =
            new InMemoryStore<string>() :> Store<int, string>

        List.map sut.create testItems |> ignore

        let actual = sut.get 1000

        Assert.Equal(GetResult.NotFound, actual)

    [<Fact>]
    let ``InMemoryStore.getWhere -> Success`` () =
        let expected = GetManyResult.Success [ (1, "one") ]

        let sut =
            new InMemoryStore<string>() :> Store<int, string>

        List.map sut.create testItems |> ignore

        let actual = sut.getWhere (fun d -> d = "one")

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``InMemoryStore.getWhere erroring -> Error`` () =
        let sut =
            new InMemoryStore<string>() :> Store<int, string>

        List.map sut.create testItems |> ignore

        let actual =
            sut.getWhere (fun d -> failwith "test error")

        match actual with
        | GetManyResult.Error _ -> () // test passes
        | _ -> failwith "sut.getWhere result not Error"


    [<Fact>]
    let ``InMemoryStore.getAll id -> expected db`` () =
        let expected =
            testItems
            |> List.mapi (fun i d -> (i, d))
            |> GetManyResult.Success

        let sut =
            new InMemoryStore<string>() :> Store<int, string>

        List.map sut.create testItems |> ignore
        let actual = sut.getAll ()

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``InMemoryStore.delete -> Success`` () =
        let sut =
            new InMemoryStore<string>() :> Store<int, string>

        List.map sut.create testItems |> ignore

        let result = sut.delete 1

        match result with
        | Error _ -> failwith "sut.delete result not Success"
        | Success -> () // test passes


    [<Fact>]
    let ``InMemoryStore.delete -> expected db`` () =
        let expectedDb =
            GetManyResult.Success [ (0, "zero")
                                    (2, "two") ]

        let sut =
            new InMemoryStore<string>() :> Store<int, string>

        List.map sut.create testItems |> ignore

        sut.delete 1 |> ignore

        let actual = sut.getAll ()

        Assert.Equal(expectedDb, actual)


    [<Fact>]
    let ``InMemoryStore.create indexes correctly after delete`` () =
        let expectedCreate = CreateResult.Success(3, "three")
        let expectedGet = GetResult.Success(3, "three")

        let sut =
            new InMemoryStore<string>() :> Store<int, string>

        List.map sut.create testItems |> ignore

        let actualCreate = sut.create "three"
        let actualGet = sut.get 3

        Assert.Equal(expectedCreate, actualCreate)
        Assert.Equal(expectedGet, actualGet)


    [<Fact>]
    let ``InMemoryStore.update -> updates correctly`` () =
        let expectedUpdate = UpdateResult.Success(1, "updated")
        let expectedGet = GetResult.Success(1, "updated")

        let sut =
            new InMemoryStore<string>() :> Store<int, string>

        List.map sut.create testItems |> ignore

        let actualUpdate = sut.update 1 "updated"
        let actualGet = sut.get 1

        Assert.Equal(expectedUpdate, actualUpdate)
        Assert.Equal(expectedGet, actualGet)

    [<Fact>]
    let ``InMemoryStore.update invalid id -> NotFound`` () =
        let sut =
            new InMemoryStore<string>() :> Store<int, string>

        List.map sut.create testItems |> ignore

        let actual = sut.update 1000 "non-existent entry"

        Assert.Equal(UpdateResult.NotFound, actual)
