namespace SimpleRSS.Persistence

open System
open System.Collections.Generic
open SimpleRSS.Persistence.Types.Connector
open MongoDB.Bson
open MongoDB.FSharp
open MongoDB.Driver
open SimpleRSS.Persistence.Store.Utils

module MongoDB =
    type Item<'D> = ObjectId * 'D

    type DBItem<'D> = { _id: ObjectId; data: 'D }

    let dbFromItem i = { _id = idOf i; data = valOf i }

    /// create a `Store` with a `mutable list` as the db.
    /// implements Connector.Store
    type MongoDBStore<'T>(connectionString: string, dbName: string, collectionName: string) =

        let client = MongoClient(connectionString)
        let db = client.GetDatabase(dbName)

        let collection =
            db.GetCollection<DBItem<'T>>(collectionName)

        let mutable db : Item<'T> list = []

        let nextId () = new ObjectId()

        interface AsyncStore<ObjectId, 'T> with

            member this.create data : Async<CreateResult<Item<'T>>> =
                async {
                    try
                        let dbItem = dbFromItem (nextId (), data)

                        collection.InsertOneAsync(dbItem)
                        |> Async.AwaitTask
                        |> ignore

                        return CreateResult.Error(exn "Not implemented yet")
                    with ex -> return CreateResult.Error ex
                }

            member this.get(id: ObjectId) : Async<GetResult<Item<'T>>> =
                async {
                    try
                        return GetResult.Error(exn "Not Implemented")
                    with ex -> return GetResult.Error ex
                }

            member this.getAll() : Async<GetManyResult<Item<'T>>> =
                async {
                    try
                        return GetManyResult.Error(exn "Not Implemented")
                    with ex -> return GetManyResult.Error ex
                }

            member this.getWhere predicate : Async<GetManyResult<Item<'T>>> =
                async {
                    try
                        return GetManyResult.Error(exn "Not Implemented")
                    with ex -> return GetManyResult.Error ex
                }


            member this.update (id: ObjectId) (data: 'T) : Async<UpdateResult<Item<'T>>> =
                async {
                    try
                        return UpdateResult.Error(exn "Not Implemented")
                    with ex -> return UpdateResult.Error ex
                }

            member this.delete id : Async<Types.Connector.DeleteResult> =
                async {
                    try
                        return DeleteResult.Error(exn "Not Implemented")
                    with ex -> return DeleteResult.Error ex
                }

    /// create an `MongoDBStore` instance and upcast to `Store`
    let createStore<'T> connectionString dbName collectionName =
        new MongoDBStore<'T>(connectionString, dbName, collectionName) :> AsyncStore<ObjectId, 'T>
