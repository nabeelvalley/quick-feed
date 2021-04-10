namespace SimpleRSS.Persistence

open Xunit

module LibraryTests =

    [<Theory>]
    [<InlineData("world", "Hello world")>]
    [<InlineData("Bob", "Hello Bob")>]
    let ``Say.hello -> returns appended string`` (input: string, expected: string) =
        let result = Say.hello input

        Assert.Equal(expected, result)
