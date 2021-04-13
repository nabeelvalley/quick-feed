namespace SimpleRSS.Persistence

open System
open System.Linq
open System.Collections.Generic
open SimpleRSS.Persistence.Types.Connector


module InMemory =

    type Item<'D> = int * 'D

    /// create a `Store` with a `mutable list` as the db.
    /// implements Connector.Store
    type InMemoryStore<'T>() =

        let mutable db : Item<'T> list = []

        let nextId (d: (int * 'T) list) : int =
            if List.length d > 0 then
                let (id, _) = List.last d
                id + 1
            else
                0

        interface Store<int, 'T> with

            member this.create data : CreateResult<Item<'T>> =
                try
                    let id = nextId db
                    db <- List.append db [ (id, data) ]
                    CreateResult.Success(id, data)
                with ex -> CreateResult.Error ex

            member this.get(id: int) : GetResult<Item<'T>> =
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

            member this.update (id: int) (data: 'T) : UpdateResult<Item<'T>> =
                try
                    db <- List.map (fun (i, d) -> if i = id then (i, data) else (i, d)) db
                    db.[id] |> UpdateResult.Success
                with
                | :? ArgumentException -> UpdateResult.NotFound
                | ex -> UpdateResult.Error ex

            member this.delete id : DeleteResult =
                try
                    db <-
                        db
                        |> List.indexed
                        |> List.filter (fun (i, _) -> i <> id)
                        |> List.map (fun (i, d) -> d)

                    DeleteResult.Success
                with ex -> DeleteResult.Error ex

    /// create an `InMemoryStore` instance and upcast to `Store`
    let createStore<'T> () =
        new InMemoryStore<'T>() :> Store<int, 'T>
