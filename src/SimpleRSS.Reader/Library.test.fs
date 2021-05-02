module SimpleRSS.Reader.Test.Library

open System
open Xunit
open SimpleRSS.Reader.Types.Reader
open SimpleRSS.Reader.Library
open SimpleRSS.Reader.Utils
open Xunit.Sdk

type CHFeed = CodeHollow.FeedReader.Feed
type CHFeedItem = CodeHollow.FeedReader.FeedItem

type SRFeed = SimpleRSS.Reader.Types.Feed.Feed
type SRFeedItem = SimpleRSS.Reader.Types.FeedItem.FeedItem

let createCHFeedItem () =
    let item = CHFeedItem()
    item.Title <- "Title"
    item.Link <- "Link"
    item.Description <- "Description"
    item.PublishingDateString <- "2020-01-14T12:34:56"
    item.PublishingDate <- DateTime.Parse(item.PublishingDateString)
    item.Author <- "Author"
    item.Id <- "12345678"
    item.Categories <- ResizeArray<string> [ "none" ]
    item.Content <- "Content"

    item

let createSRFeedItem () : SRFeedItem =
    let pds = "2020-01-14T12:34:56"

    { title = "Title"
      link = "Link"
      description = "Description"
      publishingDateString = pds
      publishingDate = Some(DateTime.Parse pds)
      author = "Author"
      id = "12345678"
      categories = [ "none" ]
      content = "Content" }

let createCHFeed () =
    let item = CHFeed()
    item.Title <- "Title"
    item.Link <- "Link"
    item.Description <- "Description"
    item.Language <- "Language"
    item.Copyright <- "Copyright"
    item.LastUpdatedDateString <- "2020-01-14T12:34:56"
    item.LastUpdatedDate <- DateTime.Parse(item.LastUpdatedDateString)
    item.ImageUrl <- "ImageUrl"
    item.Items <- ResizeArray<CHFeedItem> [ createCHFeedItem () ]

    item

let createSRFeed () : SRFeed =
    let lud = "2020-01-14T12:34:56"

    { title = "Title"
      link = "Link"
      description = "Description"
      language = "Language"
      copyright = "Copyright"
      lastUpdatedDateString = lud
      lastUpdatedDate = Some(DateTime.Parse(lud))
      imageUrl = "ImageUrl"
      items = [ createSRFeedItem () ] }

let createMockReader (entries: seq<_ * _>) : IReader =
    { new IReader with
        member this.ReadAsync url = async { return (Map entries).[url] }

        member this.GetAbsoluteUrl url = $"http://{url}" }

let okOrFail =
    function
    | Ok x -> x
    | Error _ -> raise (XunitException())

let createPagingOptions = SimpleRSS.Reader.Paging.createPagingOptions

[<Fact>]
let ``no paging - general test`` =
    let testUrl = "test"

    let mockReader =
        createMockReader (seq { ($"http://{testUrl}", createCHFeed ()) })

    let expected = createSRFeed ()

    let actual =
        getFeedWithReader mockReader testUrl
        |> Async.RunSynchronously
        |> okOrFail

    Assert.Equal(expected, actual)

[<Fact>]
let ``no paging - no items`` =
    let testUrl = "test"
    let feed = createCHFeed ()
    feed.Items <- ResizeArray<CHFeedItem> []

    let mockReader =
        createMockReader (seq { ($"http://{testUrl}", feed) })

    let expected = { createSRFeed () with items = [] }

    let actual =
        getFeedWithReader mockReader testUrl
        |> Async.RunSynchronously
        |> okOrFail

    Assert.Equal(expected, actual)

[<Fact>]
let ``paging - 4 items 4 page size`` () =
    let testUrl = "test"

    let mockReader =
        createMockReader (seq { ($"http://{testUrl}", createCHFeed ()) })

    let expected = createSRFeed ()

    let pagingOpts = unwrap (createPagingOptions 4 0)

    let actual =
        getFeedPagedWithReader mockReader testUrl (Some pagingOpts)
        |> Async.RunSynchronously
        |> okOrFail

    Assert.Equal(expected, actual)

[<Fact>]
let ``paging - page size smaller than item count`` () =
    let testUrl = "test"

    let feed = createCHFeed ()

    feed.Items <-
        ResizeArray<CHFeedItem>
            [ createCHFeedItem ()
              createCHFeedItem ()
              createCHFeedItem () ]

    let mockReader =
        createMockReader (seq { ($"http://{testUrl}", feed) })

    let expected =
        { createSRFeed () with
              items =
                  [ createSRFeedItem ()
                    createSRFeedItem () ] }

    let pagingOpts = unwrap (createPagingOptions 2 0)

    let actual =
        getFeedPagedWithReader mockReader testUrl (Some pagingOpts)
        |> Async.RunSynchronously
        |> okOrFail

    Assert.Equal(expected, actual)

[<Fact>]
let ``paging - page size greater than item count`` () =
    let testUrl = "test"

    let feed = createCHFeed ()

    feed.Items <-
        ResizeArray<CHFeedItem>
            [ createCHFeedItem ()
              createCHFeedItem ()
              createCHFeedItem () ]

    let mockReader =
        createMockReader (seq { ($"http://{testUrl}", feed) })

    let expected =
        { createSRFeed () with
              items =
                  [ createSRFeedItem ()
                    createSRFeedItem ()
                    createSRFeedItem () ] }

    let pagingOpts = unwrap (createPagingOptions 4 0)

    let actual =
        getFeedPagedWithReader mockReader testUrl (Some pagingOpts)
        |> Async.RunSynchronously
        |> okOrFail

    Assert.Equal(expected, actual)

[<Fact>]
let ``paging - not first page`` () =
    let testUrl = "test"

    let feed = createCHFeed ()

    feed.Items <-
        ResizeArray<CHFeedItem>
            [ createCHFeedItem ()
              createCHFeedItem ()
              createCHFeedItem () ]

    let mockReader =
        createMockReader (seq { ($"http://{testUrl}", feed) })

    let expected =
        { createSRFeed () with
              items =
                  [ createSRFeedItem ()
                    createSRFeedItem ()
                    createSRFeedItem () ] }

    let pagingOpts = unwrap (createPagingOptions 4 0)

    let actual =
        getFeedPagedWithReader mockReader testUrl (Some pagingOpts)
        |> Async.RunSynchronously
        |> okOrFail

    Assert.Equal(expected, actual)

[<Fact>]
let ``paging - last page`` () =
    let testUrl = "test"

    let feed = createCHFeed ()

    feed.Items <-
        ResizeArray<CHFeedItem>
            [ createCHFeedItem ()
              createCHFeedItem ()
              createCHFeedItem ()
              createCHFeedItem () ]

    let mockReader =
        createMockReader (seq { ($"http://{testUrl}", feed) })

    let expected =
        { createSRFeed () with
              items =
                  [ createSRFeedItem ()
                    createSRFeedItem () ] }

    let pagingOpts = unwrap (createPagingOptions 2 1)

    let actual =
        getFeedPagedWithReader mockReader testUrl (Some pagingOpts)
        |> Async.RunSynchronously
        |> okOrFail

    Assert.Equal(expected, actual)

[<Fact>]
let ``paging - last page partial`` () =
    let testUrl = "test"

    let feed = createCHFeed ()

    feed.Items <-
        ResizeArray<CHFeedItem>
            [ createCHFeedItem ()
              createCHFeedItem ()
              createCHFeedItem () ]

    let mockReader =
        createMockReader (seq { ($"http://{testUrl}", feed) })

    let expected =
        { createSRFeed () with
              items = [ createSRFeedItem () ] }

    let pagingOpts = unwrap (createPagingOptions 2 1)

    let actual =
        getFeedPagedWithReader mockReader testUrl (Some pagingOpts)
        |> Async.RunSynchronously
        |> okOrFail

    Assert.Equal(expected, actual)

[<Fact>]
let ``paging - past last page`` () =
    let testUrl = "test"

    let feed = createCHFeed ()

    feed.Items <-
        ResizeArray<CHFeedItem>
            [ createCHFeedItem ()
              createCHFeedItem ()
              createCHFeedItem () ]

    let mockReader =
        createMockReader (seq { ($"http://{testUrl}", feed) })

    let expected = { createSRFeed () with items = [] }

    let pagingOpts = unwrap (createPagingOptions 2 5)

    let actual =
        getFeedPagedWithReader mockReader testUrl (Some pagingOpts)
        |> Async.RunSynchronously
        |> okOrFail

    Assert.Equal(expected, actual)

[<Fact>]
let ``catches exn`` () =
    let testUrl = "test"

    let feed = createCHFeed ()

    feed.Items <-
        ResizeArray<CHFeedItem>
            [ createCHFeedItem ()
              createCHFeedItem ()
              createCHFeedItem () ]

    let mockReader =
        { new IReader with
            member this.ReadAsync url = raise (Exception())
            member this.GetAbsoluteUrl url = url }

    let pagingOpts = unwrap (createPagingOptions 2 5)

    try
        let actual =
            getFeedPagedWithReader mockReader testUrl (Some pagingOpts)
            |> Async.RunSynchronously

        match actual with
        | Ok _ -> raise (XunitException())
        | _ -> ignore
    with _ -> raise (XunitException())
    |> ignore
