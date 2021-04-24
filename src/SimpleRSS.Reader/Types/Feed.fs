namespace SimpleRSS.Reader.Types

open System
open SimpleRSS.Reader.Utils

module Feed =
    type Feed =
        { title: string
          link: string
          description: string
          language: string
          copyright: string
          lastUpdatedDateString: string
          lastUpdatedDate: DateTime option
          imageUrl: string
          items: FeedItem.FeedItem list }

        static member fromClass(obj: CodeHollow.FeedReader.Feed) =
            { title = obj.Title
              link = obj.Link
              description = obj.Description
              language = obj.Language
              copyright = obj.Copyright
              lastUpdatedDateString = obj.LastUpdatedDateString
              lastUpdatedDate = nullableToOption obj.LastUpdatedDate
              imageUrl = obj.ImageUrl
              items =
                  obj.Items
                  |> Seq.cast
                  |> Seq.map FeedItem.FeedItem.fromClass
                  |> List.ofSeq }
