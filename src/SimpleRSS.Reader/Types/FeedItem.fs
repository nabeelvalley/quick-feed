namespace SimpleRSS.Reader.Types

open System
open SimpleRSS.Reader.Utils

module FeedItem =
    type FeedItem =
        { title: string
          link: string
          description: string
          publishingDateString: string
          publishingDate: DateTime option
          author: string
          id: string
          categories: string list
          content: string }

    let fromClass (obj: CodeHollow.FeedReader.FeedItem) =
        { title = obj.Title
          link = obj.Link
          description = obj.Description
          publishingDateString = obj.PublishingDateString
          publishingDate = nullableToOption obj.PublishingDate
          author = obj.Author
          id = obj.Id
          categories = obj.Categories |> Seq.cast |> List.ofSeq
          content = obj.Content }
