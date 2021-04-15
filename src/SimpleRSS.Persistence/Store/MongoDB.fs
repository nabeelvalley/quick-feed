namespace SimpleRSS.Persistence

open System
open System.Collections.Generic
open SimpleRSS.Persistence.Types.Connector
open MongoDB.Bson
open MongoDB.FSharp
open MongoDB.Driver

module MongoDB =
    type Item<'D> = ObjectId * 'D

    /// create a `Store` with a `mutable list` as the db.
    /// implements Connector.Store
    type MongoDBStore<'T>(connectionString: string, dbName: string, collectionName: string) =

        let client = MongoClient(connectionString)
        let db = client.GetDatabase(dbName)

        let mutable db : Item<'T> list = []

        let nextId () = new ObjectId()

        interface Store<ObjectId, 'T> with

            member this.create data : CreateResult<Item<'T>> =
                try
                    let id = nextId ()
                    db <- List.append db [ (id, data) ]
                    CreateResult.Success(id, data)
                with ex -> CreateResult.Error ex

            member this.get(id: ObjectId) : GetResult<Item<'T>> =
                try
                    db
                    |> List.find (fun (x, _) -> x = id)
                    |> GetResult.Success
                with
                | :? KeyNotFoundException -> GetResult.NotFound
                | ex -> GetResult.Error ex

            member this.getAll() : GetManyResult<Item<'T>> =
                try
                    GetManyResult.Success db
                with ex -> GetManyResult.Error ex

            member this.getWhere predicate : GetManyResult<Item<'T>> =
                try
                    List.filter (fun (_, d) -> predicate d) db
                    |> GetManyResult.Success
                with ex -> GetManyResult.Error ex

            member this.update (id: ObjectId) (data: 'T) : UpdateResult<Item<'T>> =
                try
                    db <- List.map (fun (i, d) -> if i = id then (i, data) else (i, d)) db

                    db
                    |> List.find (fun (x, _) -> x = id)
                    |> UpdateResult.Success
                with
                | :? KeyNotFoundException -> UpdateResult.NotFound
                | :? ArgumentException -> UpdateResult.NotFound
                | ex -> UpdateResult.Error ex

            member this.delete id : Types.Connector.DeleteResult =
                try
                    db <- db |> List.filter (fun (i, _) -> i <> id)

                    DeleteResult.Success
                with ex -> DeleteResult.Error ex

    /// create an `MongoDBStore` instance and upcast to `Store`
    let createStore<'T> connectionString dbName collectionName =
        new MongoDBStore<'T>(connectionString, dbName, collectionName) :> Store<ObjectId, 'T>
