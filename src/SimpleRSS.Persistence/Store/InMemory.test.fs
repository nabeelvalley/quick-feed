namespace SimpleRSS.Persistence

open Xunit
open SimpleRSS.Persistence.InMemory
open SimpleRSS.Persistence.Types.Connector

module InMemoryTest =

    [<Theory>]
    [<InlineData("test0")>]
    [<InlineData("test1")>]
    [<InlineData("test2")>]
    let ``InMemoryStore.create -> adds item to db`` (item) =
        let expectedEl = CreateResult.Success item
        let expectedDb = GetManyResult.Success [ item ]

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
        let items = [ "zero"; "one"; "two" ]
        let expectedDb = GetManyResult.Success items

        let sut =
            new InMemoryStore<string>() :> Store<int, string>

        let _ = List.map sut.create items

        let actual = sut.getAll ()

        Assert.Equal(expectedDb, actual)

    [<Theory>]
    [<InlineData(0, "zero")>]
    [<InlineData(1, "one")>]
    [<InlineData(2, "two")>]
    let ``InMemoryStore.get id -> returns correct item`` (index, expected) =
        let items = [ "zero"; "one"; "two" ]
        let expectedDb = GetManyResult.Success items

        let sut =
            new InMemoryStore<string>() :> Store<int, string>

        let _ = List.map sut.create items

        let actual = sut.get index

        Assert.Equal(GetResult.Success expected, actual)
