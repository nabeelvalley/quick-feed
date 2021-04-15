namespace SimpleRSS.Persistence.Store

module Utils =

    /// Get `'Id` id from `'Id * T` tuple
    let idOf<'Id, 'T> (data: 'Id * 'T) = fst data

    /// Get `List<'Id>` ids from `List<'Id * T>` tuples
    let idsOf<'Id, 'T> (data: List<'Id * 'T>) = List.map fst data

    /// Get `'T` data from `'Id * T` tuple
    let valOf<'Id, 'T> (data: 'Id * 'T) = snd data

    /// Get `List<'T>` data from `List<'Id * T>` tuples
    let valsOf<'Id, 'T> (data: List<'Id * 'T>) = List.map snd data
