namespace SimpleRSS.Persistence

open SimpleRSS.Persistence.Types.Connector
open System.Linq
open System

module InMemory =

    /// create a store with a mutable list as the db
    let createStore<'T> () : Store<int, 'T> =
        let mutable db : 'T list = []

        let create data: UpdateResult<'T> =
            try
                db <- List.append db [ data ]
                Success data
            with 
            | ex -> Error ex  

        let get id: GetResult<'T> =
            try 
              match db[id] with
              | None -> NotFound
              | Some d -> Success d
            with
            | ex -> Error ex

        let getAll id: GetManyResult<'T> =
            try 
              Success db
            with
            | ex -> Error ex

        


                /// defines required functionality for a Store
    // type Store<'Id, 'Data> =
    //   { create: string -> UpdateResult<'Data>
    //     get: 'Id -> GetResult<'Data>
    //     getWhere: ('Data -> bool) -> GetManyResult<'Data>
    //     getAll: 'Data -> GetManyResult<'Data>
    //     update: 'Data -> UpdateResult<'Data>
    //     delete: 'Data -> DeleteResult }

       
