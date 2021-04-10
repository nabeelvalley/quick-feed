namespace SimpleRSS.Persistence

module InMemoryTest =
    let createStore<'T> (): Store<string, 'T> = 