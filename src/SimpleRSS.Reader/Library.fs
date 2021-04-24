namespace SimpleRSS.Reader

open SimpleRSS.Reader.Types.Feed
open SimpleRSS.Reader.Types.Reader
open SimpleRSS.Reader.Paging

module Library =
    /// <summary>Retrieves the RSS feed document at the given URL including items with paging applied.</summary>
    /// <param name="reader">The RSS parser/reader to use. See <code>Reader.fs</code>.</param>
    /// <param name="pagingOpts">Paging options for the feed's items. A page size &lt; 1 will include all items.</param>
    /// <param name="feedUrl">The full URL to the feed document, including or excluding <code>http(s)://</code>.</param>
    /// <remarks>It's unlikely you'd have to use this function directly. See <code>getFeedPaged</code>.</remarks>
    let getFeedPagedWithReader (reader: IReader) (feedUrl: string) (pagingOpts: PagingOptions) =
        async {
            let! feed =
                reader.GetAbsoluteUrl feedUrl
                |> reader.ReadAsync

            let feed' = Feed.fromClass feed

            return
                if pagingOpts.pageSize < 1 then
                    feed'
                else
                    { feed' with
                          items = applyPaging pagingOpts feed'.items }
        }

    /// <summary>Retrieves the RSS feed document at the given URL including items with paging applied.</summary>
    /// <param name="pagingOpts">Paging options for the feed's items. A page size &lt; 1 will include all items.</param>
    /// <param name="feedUrl">The full URL to the feed document, including or excluding <code>http(s)://</code>.</param>
    let getFeedPaged feedUrl pagingOpts =
        getFeedPagedWithReader (Reader()) pagingOpts feedUrl

    /// <summary>
    /// Retrieves the RSS feed document at the given URL including item without any paging applied.
    /// </summary>
    /// <param name="reader">The RSS parser/reader to use. See <code>Reader.fs</code>.</param>
    /// <param name="feedUrl">The full URL to the feed document, including or excluding <code>http(s)://</code>.</param>
    /// <remarks>It's unlikely you'd have to use this function directly. See <code>getFeed</code>.</remarks>
    /// <seealso cref="SimpleRSS.Reader.Library.getFeedPaged" />
    let getFeedWithReader (reader: IReader) (feedUrl: string) =
        getFeedPagedWithReader reader feedUrl { pageSize = -1; pageNumber = 0 }


    /// <summary>
    /// Retrieves the RSS feed document at the given URL including item without any paging applied.
    /// </summary>
    /// <param name="feedUrl">The full URL to the feed document, including or excluding <code>http(s)://</code>.</param>
    /// <seealso cref="SimpleRSS.Reader.Library.getFeedPaged" />
    let getFeed feedUrl =
        getFeedWithReader (Reader()) feedUrl
