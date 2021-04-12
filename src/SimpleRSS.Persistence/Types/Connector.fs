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
        abstract member create : 'Data -> CreateResult<'Data>
        abstract member get : 'Id -> GetResult<'Data>
        abstract member getWhere : Predicate<'Data> -> GetManyResult<'Data>
        abstract member getAll : unit -> GetManyResult<'Data>
        abstract member update : 'Id -> 'Data -> UpdateResult<'Data>
        abstract member delete : 'Id -> DeleteResult

    /// defines required functionality for an Async Store
    type AsyncStore<'Id, 'Data> =
        abstract member create : 'Data -> Async<CreateResult<'Data>>
        abstract member get : 'Id -> Async<GetResult<'Data>>
        abstract member getWhere : Predicate<'Data> -> Async<GetManyResult<'Data>>
        abstract member getAll : unit -> Async<GetManyResult<'Data>>
        abstract member update : 'Id -> 'Data -> Async<UpdateResult<'Data>>
        abstract member delete : 'Id -> Async<DeleteResult>
