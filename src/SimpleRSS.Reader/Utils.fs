namespace SimpleRSS.Reader

open System

module Utils =
    let nullableToOption (n: Nullable<_>) =
        if n.HasValue then
            Some n.Value
        else
            None
