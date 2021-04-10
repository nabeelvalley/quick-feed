namespace SimpleRSS.Persistence

/// specifies types and functionality definitions required for DB connectors
module Connector =

    type UpdateResult<'Data> =
        | Success of 'Data
        | Error of string

    type GetResult<'Data> =
        | Success of 'Data
        | NotFound
        | Error of string

    type GetManyResult<'Data> =
        | Success of 'Data list
        | Error of string

    type DeleteResult =
        | Success
        | Error of string

    /// defines required functionality for a Store
    type Store<'Id, 'Data> =
        { create: string -> UpdateResult<'Data>
          get: 'Id -> GetResult<'Data>
          getWhere: ('Data -> bool) -> GetManyResult<'Data>
          getAll: 'Data -> GetManyResult<'Data>
          update: 'Data -> UpdateResult<'Data>
          delete: 'Data -> DeleteResult }

    /// defines required functionality for an Async Store
    type AsyncStore<'Id, 'Data> =
        { create: string -> Async<UpdateResult<'Data>>
          get: 'Id -> Async<GetResult<'Data>>
          getWhere: ('Data -> bool) -> Async<GetManyResult<'Data>>
          getAll: 'Data -> Async<GetManyResult<'Data>>
          update: 'Data -> Async<UpdateResult<'Data>>
          delete: 'Data -> Async<DeleteResult> }
