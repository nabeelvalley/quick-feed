namespace SimpleRSS.Reader

open System
open CodeHollow.FeedReader
open SimpleRSS.Reader.Utils

module Types =
    type Feed =
        { title: string
          link: string
          description: string
          language: string
          copyright: string
          lastUpdatedDate: DateTime option
          imageUrl: string
          items: FeedItem list }

        static member fromClass(obj: CodeHollow.FeedReader.Feed) =
            { title = obj.Title
              link = obj.Link
              description = obj.Description
              language = obj.Language
              copyright = obj.Copyright
              lastUpdatedDate = nullableToOption obj.LastUpdatedDate
              imageUrl = obj.ImageUrl
              items =
                  obj.Items
                  |> Seq.cast
                  |> Seq.map SimpleRSS.Reader.Types.FeedItem.fromClass
                  |> List.ofSeq }
