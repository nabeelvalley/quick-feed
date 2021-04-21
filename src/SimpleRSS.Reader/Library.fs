namespace SimpleRSS.Reader

open SimpleRSS.Reader.Types
open SimpleRSS.Reader.Paging

open CodeHollow.FeedReader

module Library =
    /// <summary>Retrieves the RSS feed document at the given URL including items with paging applied.</summary>
    /// <param name="pagingOpts">Paging options for the feed's items. A page size &lt; 1 will include all items.</param>
    /// <param name="feedUrl">The full URL to the feed document, including or excluding <code>http(s)://</code>.</param>
    let getFeedPagedAsync (pagingOpts: PagingOptions) (feedUrl: string) =
        async {
            let! feed =
                FeedReader.GetAbsoluteUrl feedUrl
                |> FeedReader.ReadAsync
                |> Async.AwaitTask

            let feed' = Feed.fromClass feed

            return
                if pagingOpts.pageSize < 1 then
                    feed'
                else
                    { feed' with
                          items = applyPaging pagingOpts feed'.items }
        }

    let getFeedAsync =
        getFeedPagedAsync { pageSize = -1; pageNumber = 0 }
