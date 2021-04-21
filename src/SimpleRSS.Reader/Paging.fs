namespace SimpleRSS.Reader

module Paging =
    type PagingOptions =
        {
            /// <summary>The amount of items to include on each page.</summary>
            pageSize: int;

            /// <summary>The zero-indexed page number to retrieve.</summary>
            pageNumber: int
        }

    let applyPaging (pagingOpts: PagingOptions) (items: _ list) =
        items
        |> List.skip (pagingOpts.pageSize * pagingOpts.pageNumber)
        |> List.take pagingOpts.pageSize
