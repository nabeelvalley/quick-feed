namespace SimpleRSS.Persistence

open Xunit
open SimpleRSS.Persistence.MongoDB
open SimpleRSS.Persistence.Types.Connector
open SimpleRSS.Persistence.Store.Utils
open System

module MongoDBTest =
    type TestData = { Name: string; Email: string }

    let connectionString = "mongodb://localhost:27017"
    let dbName = "simple-rss-test"
    let collectionName = "mongodb-test"

    let createTestStore<'T> () =
        createStore<'T> connectionString dbName collectionName

    let testItems =
        [ { Name = ""; Email = "test@email.com" }
          { Name = ""; Email = "test2@email.com" }
          { Name = ""; Email = "" } ]

    let getTestItems () =
        [ { Name = Guid.NewGuid().ToString()
            Email = Guid.NewGuid().ToString() }
          { Name = ""
            Email = Guid.NewGuid().ToString() }
          { Name = ""; Email = "" } ]

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    let ``MongoDBStore.create -> adds item to db`` (testDataIndex) =
        async {
            let items = getTestItems ()
            let expectedEl = items.[testDataIndex]

            let sut = createTestStore ()
            let! actual = sut.create (expectedEl)
            let! db = sut.getAll ()

            // failwith (sprintf "%O \n%O" actual db)

            match actual with
            | CreateResult.Success s -> Assert.Equal(expectedEl, valOf s)
            | _ -> failwith "result not CreateResult.Success"

            match db with
            | GetManyResult.Success d ->
                let containsNewEl = valsOf d |> List.contains expectedEl
                Assert.True(containsNewEl)
            | _ -> failwith "result not GetManyResult.Success"
        }


// [<Fact>]
// let ``MongoDBStore.create multiple-> adds all to db`` () =

//     let sut = createStore<string> ()

//     List.map sut.create testItems |> ignore

//     let actual = sut.getAll ()

//     match actual with
//     | GetManyResult.Success d -> Assert.Equal<string list>(testItems, valsOf d)
//     | _ -> failwith "result not GetManyResult.Success"



// [<Fact>]
// let ``MongoDBStore.get id -> correct item`` () =
//     let sut = createStore<string> ()

//     List.map sut.create testItems |> ignore

//     let all = sut.getAll ()

//     match all with
//     | GetManyResult.Success a ->
//         let handleAssert e =
//             let actual = sut.get (idOf e)

//             match actual with
//             | GetResult.Success a -> Assert.Equal(valOf e, valOf a)
//             | _ -> failwith "result not GetResult.Success"

//         let expected0 = a.[0]
//         let expected1 = a.[1]
//         let expected2 = a.[2]

//         [ expected0; expected1; expected2 ]
//         |> List.map handleAssert


//     | _ -> failwith "result not GetManyResult.Success"

// [<Fact>]
// let ``MongoDBStore.get invalid id -> NotFound`` () =
//     let sut = createStore<string> ()

//     List.map sut.create testItems |> ignore

//     let actual = sut.get (Guid.NewGuid())

//     Assert.Equal(GetResult.NotFound, actual)

// [<Fact>]
// let ``MongoDBStore.getWhere -> Success`` () =
//     let expected = [ "one" ]

//     let sut = createStore<string> ()

//     List.map sut.create testItems |> ignore

//     let actual = sut.getWhere (fun d -> d = "one")

//     match actual with
//     | GetManyResult.Success a -> Assert.Equal<string list>(expected, valsOf a)
//     | _ -> failwith "result not GetManyResult.Success"

// [<Fact>]
// let ``MongoDBStore.getWhere erroring -> Error`` () =
//     let sut = createStore<string> ()

//     List.map sut.create testItems |> ignore

//     let actual =
//         sut.getWhere (fun d -> failwith "test error")

//     match actual with
//     | GetManyResult.Error _ -> () // test passes
//     | _ -> failwith "sut.getWhere result not Error"


// [<Fact>]
// let ``MongoDBStore.getAll id -> expected db`` () =
//     let sut = createStore<string> ()

//     List.map sut.create testItems |> ignore
//     let actual = sut.getAll ()

//     match actual with
//     | GetManyResult.Success a -> Assert.Equal<string list>(testItems, valsOf a)
//     | _ -> failwith "result not GetManyResult.Success"

// [<Fact>]
// let ``MongoDBStore.delete -> expected db`` () =
//     let expectedDb = [ "zero"; "two" ]

//     let sut = createStore<string> ()

//     List.map sut.create testItems |> ignore

//     let db = sut.getAll ()

//     match db with
//     | GetManyResult.Success d ->
//         let second = d.[1]

//         let actualDelete = sut.delete (idOf second)

//         match actualDelete with
//         | DeleteResult.Success -> () // pass
//         | _ -> failwith "result not DeleteResult.Success"

//         let dbAfterDelete = sut.getAll ()

//         match dbAfterDelete with
//         | GetManyResult.Success d -> Assert.Equal<string list>(expectedDb, valsOf d)
//         | _ -> failwith "result not GetManyResult.Success"

//     | _ -> failwith "result not GetManyResult.Success"



// [<Fact>]
// let ``MongoDBStore.update -> updates correctly`` () =
//     let item = "updated"

//     let sut = createStore<string> ()

//     List.map sut.create testItems |> ignore

//     let db = sut.getAll ()

//     match db with
//     | GetManyResult.Success d ->
//         let first = d.[0]

//         let actualUpdate = sut.update (idOf first) item

//         match actualUpdate with
//         | UpdateResult.Success u -> Assert.Equal(item, valOf u)
//         | _ -> failwith "result not UpdateResult.Success"

//         let actualGet = sut.get (idOf first)

//         match actualGet with
//         | GetResult.Success g -> Assert.Equal(item, valOf g)
//         | _ -> failwith "result not GetResult.Success"
//     | _ -> failwith "result not GetManyResult.Success"


// [<Fact>]
// let ``MongoDBStore.update invalid id -> NotFound`` () =
//     let sut = createStore<string> ()

//     List.map sut.create testItems |> ignore

//     let actual =
//         sut.update (Guid.NewGuid()) "non-existent entry"

//     Assert.Equal(UpdateResult.NotFound, actual)
