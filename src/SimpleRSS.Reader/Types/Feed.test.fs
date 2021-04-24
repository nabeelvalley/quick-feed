module SimpleRSS.Reader.Test.Types.Feed

open System
open Xunit

type CHFeed = CodeHollow.FeedReader.Feed
type SRFeed = SimpleRSS.Reader.Types.Feed.Feed
type CHFeedItem = CodeHollow.FeedReader.FeedItem
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

[<Fact>]
let ``fromClass maps all fields`` () =
    let chFeed = createCHFeed ()

    let expected = createSRFeed ()
    let actual = SRFeed.fromClass chFeed

    Assert.Equal(expected, actual)

[<Fact>]
let ``None publishing date`` () =
    let chFeed = createCHFeed ()
    chFeed.LastUpdatedDateString <- "2020-01-140T12:34:56"
    chFeed.LastUpdatedDate <- Nullable<DateTime>()

    let expected =
        { createSRFeed () with
              lastUpdatedDateString = "2020-01-140T12:34:56"
              lastUpdatedDate = None }

    let actual = SRFeed.fromClass chFeed

    Assert.Equal(expected, actual)

[<Fact>]
let ``no items`` () =
    let chFeed = createCHFeed ()
    chFeed.Items <- ResizeArray<CHFeedItem> []

    let expected = { createSRFeed () with items = [] }
    let actual = SRFeed.fromClass chFeed

    Assert.Equal(expected, actual)

[<Fact>]
let ``multiple items`` () =
    let chFeed = createCHFeed ()

    chFeed.Items <-
        ResizeArray<CHFeedItem>
            [ createCHFeedItem ()
              createCHFeedItem ()
              createCHFeedItem () ]

    let expected =
        { createSRFeed () with
              items =
                  [ createSRFeedItem ()
                    createSRFeedItem ()
                    createSRFeedItem () ] }

    let actual = SRFeed.fromClass chFeed

    Assert.Equal(expected, actual)
