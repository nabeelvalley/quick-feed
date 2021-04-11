namespace SimpleRSS.Persistence.Types

/// specifies types and functionality definitions required for DB connectors
module Connector =

    type UpdateResult<'Data> =
        | Success of 'Data
        | NotFound
        | Error of exn

    type CreateResult<'Data> =
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

    type Predicate<'T> = 'T -> bool

    /// defines required functionality for a Store
    type Store<'Id, 'Data> =
        { create: 'Data -> CreateResult<'Data>
          get: 'Id -> GetResult<'Data>
          getWhere: Predicate<'Data> -> GetManyResult<'Data>
          getAll: unit -> GetManyResult<'Data>
          update: 'Id -> 'Data -> UpdateResult<'Data>
          delete: 'Id -> DeleteResult }

    /// defines required functionality for an Async Store
    type AsyncStore<'Id, 'Data> =
        { create: 'Data -> Async<CreateResult<'Data>>
          get: 'Id -> Async<GetResult<'Data>>
          getWhere: Predicate<'Data> -> Async<GetManyResult<'Data>>
          getAll: unit -> Async<GetManyResult<'Data>>
          update: 'Id -> 'Data -> Async<UpdateResult<'Data>>
          delete: 'Id -> Async<DeleteResult> }