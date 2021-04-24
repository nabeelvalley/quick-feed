module SimpleRSS.Reader.Test.Types.FeedItem

open System
open Xunit

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

[<Fact>]
let ``fromClass maps all fields`` () =
    let chFeedItem = createCHFeedItem()

    let expected = createSRFeedItem()
    let actual = SRFeedItem.fromClass chFeedItem

    Assert.Equal(expected, actual)

[<Fact>]
let ``fromClass maps all fields, None publishing date`` () =
    let chFeedItem = createCHFeedItem()
    chFeedItem.PublishingDateString <- "2020-01-140T12:34:56"
    chFeedItem.PublishingDate <- Nullable<DateTime>()

    let expected =
        { createSRFeedItem() with
              publishingDateString = "2020-01-140T12:34:56"
              publishingDate = None }

    let actual = SRFeedItem.fromClass chFeedItem

    Assert.Equal(expected, actual)
