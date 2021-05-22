namespace SimpleRSS.Reader

module Paging =
    /// <remarks>
    /// You probably shouldn't instantiate this record directly, use <see cref="createOptions" />.
    /// </remarks>
    type PagingOptions =
        private
            {
              /// <summary>The amount of items to include on each page.</summary>
              pageSize: int

              /// <summary>The zero-indexed page number to retrieve.</summary>
              pageNumber: int }

    /// <summary>
    /// Skips up to <code>amount</code> elements of <code>lst</code>, capping the amount skipped to
    /// the size of the list.
    ///
    /// The reason this function exists is because the stdlib <code>skip</code> function throws an
    /// exception if amount exceeds the amount of items in the list, which we don't want.
    /// </summary>
    /// <param name="amount">Desired amount of elements to skip.</param>
    /// <param name="lst">The whose elements to skip.</param>
    /// <returns>A new list with the remaining elements. Might be empty.</returns>
    let private skipSafely (amount: int) (lst: _ list) = List.skip (min amount lst.Length) lst

    /// <summary>
    /// Takes up to <code>amount</code> elements of <code>lst</code>, capping the amount taken to
    /// the size of the list.
    ///
    /// The reason this function exists is because the stdlib <code>take</code> function throws an
    /// exception if amount exceeds the amount of items in the list, which we don't want.
    /// </summary>
    /// <param name="amount">Desired amount of elements to take.</param>
    /// <param name="lst">The whose elements to take.</param>
    /// <returns>A new list with up to <code>amount</code> items. Might be empty.</returns>
    let private takeSafely (amount: int) (lst: _ list) = List.take (min amount lst.Length) lst

    let createPagingOptions pageSize pageNumber : Result<PagingOptions, string> =
        if pageSize < 1 then
            Error "Page size cannot be less than 1"
        else if pageNumber < 0 then
            Error "Page number cannot be less than 0"
        else
            Ok(
                { pageSize = pageSize
                  pageNumber = pageNumber }
            )

    let applyPaging<'a> (pagingOpts: PagingOptions) (items: 'a list) : 'a list =
        items
        |> skipSafely (pagingOpts.pageSize * pagingOpts.pageNumber)
        |> takeSafely pagingOpts.pageSize
