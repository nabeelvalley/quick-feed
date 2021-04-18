namespace SimpleRSS.Persistence

open System
open System.Collections.Generic
open SimpleRSS.Persistence.Types.Connector
open MongoDB.Bson
open MongoDB.Driver
open SimpleRSS.Persistence.Store.Utils
open System.Threading.Tasks
open System.Collections.Generic

module MongoDB =
    type Item<'D> = ObjectId * 'D

    type DBItem<'D> = { _id: ObjectId; data: 'D }

    let dbFromItem i = { _id = idOf i; data = valOf i }

    let itemFromDb (d: DBItem<'T>) : Item<'T> = (d._id, d.data) //DBItem<'T> -> Item<'T>

    let cursorToList (cursor: Task<IAsyncCursor<DBItem<'D>>>) =
        cursor
        |> Async.AwaitTask
        |> mapAsync (fun c -> c.ToListAsync())
        |> mapFromAsync Async.AwaitTask


    /// create a `Store` with a `mutable list` as the db.
    /// implements Connector.Store
    type MongoDBStore<'T>(connectionString: string, dbName: string, collectionName: string) =

        let client = MongoClient connectionString
        let db = client.GetDatabase dbName

        let collection = db.GetCollection collectionName

        let nextId () = ObjectId.GenerateNewId()

        let get (id: ObjectId) : Async<GetResult<Item<'T>>> =
            async {
                try
                    let! item =
                        collection.FindAsync(fun dbi -> dbi._id = id)
                        |> cursorToList
                        |> mapAsync Seq.head
                        |> mapAsync (fun item -> item.data)

                    return GetResult.Success(id, item)
                with
                | :? KeyNotFoundException -> return GetResult.NotFound
                | :? ArgumentException -> return GetResult.NotFound
                | ex -> return GetResult.Error ex
            }

        interface AsyncStore<ObjectId, 'T> with
            member this.get(id: ObjectId) : Async<GetResult<Item<'T>>> = async { return! get (id) }

            member this.create data : Async<CreateResult<Item<'T>>> =
                async {
                    try
                        let id = nextId ()
                        let newItem = dbFromItem (id, data)

                        collection.InsertOneAsync(newItem)
                        |> Async.AwaitTask
                        |> ignore

                        let! getResult = get (newItem._id)

                        match getResult with
                        | GetResult.Success g -> return CreateResult.Success g
                        | _ -> return CreateResult.Error(exn "Failed to verify element exists")

                    with ex -> return CreateResult.Error ex
                }

            member this.getAll() : Async<GetManyResult<Item<'T>>> =
                async {
                    try
                        let! (items) =
                            collection.FindAsync<DBItem<'T>>(Builders.Filter.Empty)
                            |> cursorToList

                        let data =
                            items |> Seq.map (itemFromDb) |> Seq.toList

                        return GetManyResult.Success data
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
