namespace SimpleRSS.Persistence.Types

/// specifies types and functionality definitions required for DB connectors
module Connector =

    type UpdateResult<'Data> =
        | Success of 'Data
        | Error of exn

    type GetResult<'Data> =
        | Success of 'Data
        | NotFound
        | Error of exn

    type GetManyResult<'Data> =
        | Success of 'Data list
        | Error of exn

    type DeleteResult =
        | Success
        | Error of exn

    /// defines required functionality for a Store
    type Store<'Id, 'Data> =
        { create: string -> UpdateResult<'Data>
          get: 'Id -> GetResult<'Data>
          getWhere: ('Data -> bool) -> GetManyResult<'Data>
          getAll: unit -> GetManyResult<'Data>
          update: 'Data -> UpdateResult<'Data>
          delete: 'Data -> DeleteResult }

    /// defines required functionality for an Async Store
    type AsyncStore<'Id, 'Data> =
        { create: string -> Async<UpdateResult<'Data>>
          get: 'Id -> Async<GetResult<'Data>>
          getWhere: ('Data -> bool) -> Async<GetManyResult<'Data>>
          getAll: unit -> Async<GetManyResult<'Data>>
          update: 'Data -> Async<UpdateResult<'Data>>
          delete: 'Data -> Async<DeleteResult> }
