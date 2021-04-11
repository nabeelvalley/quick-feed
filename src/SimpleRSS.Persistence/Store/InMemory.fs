namespace SimpleRSS.Persistence

open System
open System.Linq
open System.Collections.Generic
open SimpleRSS.Persistence.Types.Connector


module InMemory =

    /// create a store with a mutable list as the db
    let createStore<'T> () : Store<int, 'T> =
        let mutable db : 'T list = []

        let create data : CreateResult<'T> =
            try
                db <- List.append db [ data ]
                CreateResult.Success data
            with ex -> CreateResult.Error ex

        let get (id: int) : GetResult<'T> =
            try
                db.[id] |> GetResult.Success
            with
            | :? ArgumentException -> GetResult.NotFound
            | ex -> GetResult.Error ex

        let getAll () : GetManyResult<'T> =
            try
                GetManyResult.Success db
            with ex -> GetManyResult.Error ex


        let getWhere predicate : GetManyResult<'T> =
            try
                List.filter predicate db |> GetManyResult.Success
            with ex -> GetManyResult.Error ex

        let update (id: int) (data: 'T) : UpdateResult<'T> =
            try
                db <- List.mapi (fun i d -> if i = id then data else data) db
                db.[id] |> UpdateResult.Success
            with
            | :? ArgumentException -> UpdateResult.NotFound
            | ex -> UpdateResult.Error ex

        let delete id: DeleteResult =
            try
              db <-
                  db
                  |> List.indexed
                  |> List.filter (fun (i, _) -> i <> id)
                  |> List.map (fun (i, d) -> d)

              DeleteResult.Success
            with
            | ex -> DeleteResult.Error ex

        { create = create
          get = get
          getWhere = getWhere
          getAll = getAll
          update = update
          delete = delete }
