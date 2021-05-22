namespace SimpleRSS.Reader

open System

module Utils =
    let nullableToOption (n: Nullable<_>) =
        if n.HasValue then
            Some n.Value
        else
            None

    /// <summary>
    /// Returns the value of the Ok case, or throws an exception if it's an Error case.
    ///
    /// This function is useful for making code terser when you know the result is an Ok case.
    /// Without this function you'd have to have a full match expression to handle the Ok and Error
    /// case, but with this function you can just call <code>(unwrap result)</code>.
    /// </summary>
    let unwrap = function
                 | Ok o -> o
                 | Error e -> raise (Exception e)

