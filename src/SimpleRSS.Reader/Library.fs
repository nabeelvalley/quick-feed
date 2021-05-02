namespace SimpleRSS.Reader

open SimpleRSS.Reader.Types.Feed
open SimpleRSS.Reader.Types.Reader
open SimpleRSS.Reader.Paging

module Library =
    // To avoid ambiguity with CodeHollow.FeedReader.Feed
    type SRFeed = SimpleRSS.Reader.Types.Feed.Feed

    let readFeed
        (reader: IReader)
        (feedUrl: string)
        (pagingOpts: PagingOptions option)
        : Async<SRFeed> =
        async {
            let! feed = reader.GetAbsoluteUrl feedUrl |> reader.ReadAsync

            let feed' = fromClass feed

            return
                match pagingOpts with
                | Some o ->
                    { feed' with
                          items = applyPaging o feed'.items }
                | None -> feed'
        }


    /// <summary>Retrieves the RSS feed document at the given URL including items with paging applied.</summary>
    /// <param name="reader">The RSS parser/reader to use. See <code>Reader.fs</code>.</param>
    /// <param name="pagingOpts">Paging options for the feed's items. None will not apply paging.</param>
    /// <param name="feedUrl">The full URL to the feed document, including or excluding <code>http(s)://</code>.</param>
    /// <remarks>It's unlikely you'd have to use this function directly. See <code>getFeedPaged</code>.</remarks>
    let getFeedPagedWithReader
        (reader: IReader)
        (feedUrl: string)
        (pagingOpts: PagingOptions option)
        : Async<Result<SRFeed, string>> =
        async {
            try
                let! feed = readFeed reader feedUrl pagingOpts
                return Ok(feed)
            with e ->
                printfn $"{e}"
                return Error($"Could not retrieve feed at {feedUrl}")
        }

    /// <summary>Retrieves the RSS feed document at the given URL including items with paging applied.</summary>
    /// <param name="pagingOpts">Paging options for the feed's items. None will not apply paging.</param>
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
        getFeedPagedWithReader reader feedUrl None


    /// <summary>
    /// Retrieves the RSS feed document at the given URL including item without any paging applied.
    /// </summary>
    /// <param name="feedUrl">The full URL to the feed document, including or excluding <code>http(s)://</code>.</param>
    /// <seealso cref="SimpleRSS.Reader.Library.getFeedPaged" />
    let getFeed feedUrl = getFeedWithReader (Reader()) feedUrl
