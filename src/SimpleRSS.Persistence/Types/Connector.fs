namespace SimpleRSS.Persistence.Types

/// specifies types and functionality definitions required for DB connectors
module Connector =

    type UpdateResult<'T> =
        | Success of 'T
        | NotFound
        | Error of exn

    type CreateResult<'T> =
        | Success of 'T
        | Error of exn

    type GetResult<'T> =
        | Success of 'T
        | NotFound
        | Error of exn

    type GetManyResult<'T> =
        | Success of 'T list
        | Error of exn

    type DeleteResult =
        | Success
        | Error of exn

    type Predicate<'T> = 'T -> bool

    /// defines required functionality for a Store
    type Store<'Id, 'Data> =
        abstract member create : 'Data -> CreateResult<'Id * 'Data>
        abstract member get : 'Id -> GetResult<'Id * 'Data>
        abstract member getWhere : Predicate<'Data> -> GetManyResult<'Id * 'Data>
        abstract member getAll : unit -> GetManyResult<'Id * 'Data>
        abstract member update : 'Id -> 'Data -> UpdateResult<'Id * 'Data>
        abstract member delete : 'Id -> DeleteResult

    /// defines required functionality for an Async Store
    type AsyncStore<'Id, 'Data> =
        abstract member create : 'Data -> Async<CreateResult<'Id * 'Data>>
        abstract member get : 'Id -> Async<GetResult<'Id * 'Data>>
        abstract member getWhere : Predicate<'Data> -> Async<GetManyResult<'Id * 'Data>>
        abstract member getAll : unit -> Async<GetManyResult<'Id * 'Data>>
        abstract member update : 'Id -> 'Data -> Async<UpdateResult<'Id * 'Data>>
        abstract member delete : 'Id -> Async<DeleteResult>
