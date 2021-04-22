namespace SimpleRSS.Reader.Types

open CodeHollow.FeedReader

module Reader =
    /// <summary>
    /// Interface for testing purposes since the CodeHollow FeedReader class is
    /// a static class and can't be mocked.
    /// </summary>
    /// <remarks>
    /// This interface will only contain function actually used by our library.
    /// </remarks>
    type IReader =
        // Signatures and documentation comments taken from the CodeHollow
        // FeedReader class, with slight modifications.

        /// <summary>
        /// gets a url (with or without http) and returns the full url
        /// </summary>
        /// <param name="url">url with or without http</param>
        /// <returns>full url</returns>
        /// <example>
        /// GetAbsoluteUrl("codehollow.com"); => returns https://codehollow.com
        /// </example>
        abstract member GetAbsoluteUrl : url: string -> string


        /// <summary>Reads a feed from an url. The url must be a feed.</summary>
        /// <param name="url">the url to a feed</param>
        /// <returns>parsed feed</returns>
        abstract member ReadAsync : url: string -> Async<Feed>

    type Reader() =
        interface IReader with
            member __.GetAbsoluteUrl url = FeedReader.GetAbsoluteUrl url

            member __.ReadAsync url =
                FeedReader.ReadAsync url |> Async.AwaitTask
