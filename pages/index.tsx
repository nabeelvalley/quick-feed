import { NextPage } from 'next'
import React, { Children, FormEventHandler, useState } from 'react'
import ActiveFeed from '../components/ActiveFeed'
import useFeedUrlStore from '../hooks/useFeedUrlStore'
import clearWhitespace from '../string/clearWhitespace'

const H2: React.FC = ({ children }) => (
  <h2 className="text-2xl font-semibold text-pink-500">{children}</h2>
)

const Home: NextPage = () => {
  const [url, setUrl] = useState<string>('')
  const [feedUrls, addFeedUrl, removeFeedUrl] = useFeedUrlStore()
  const [activeFeed, setActiveFeed] = useState<string | undefined>()

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()

    // TODO: make use of some actual URL validation here
    if (url.length > 0) {
      addFeedUrl(url)
      setUrl('')
    }
  }

  return (
    <main className="p-10">
      <section className="text-center">
        <h1 className="text-4xl font-extrabold text-pink-500">Quick Feed</h1>
        <p className="mt-8">The Open Source RSS Feed Reader</p>
      </section>

      <section className="mt-8 text-center">
        <form
          className="p-4 rounded-lg border-solid border-pink-400 border-2 text-center"
          onSubmit={handleSubmit}
        >
          <H2>Add a Feed</H2>
          <input
            className="mt-4 border-solid border-pink-200 border-2 rounded-full inline-block"
            value={url}
            onChange={(e) => setUrl(clearWhitespace(e.target.value))}
          />
          <button
            className="ml-4 inline-block bg-pink-500 hover:bg-pink-700 text-white px-4 py-1 rounded-full text-sm font-bold capitalize"
            onSubmit={handleSubmit}
          >
            Add to Feeds
          </button>
        </form>
      </section>

      <section className="mt-8 text-center">
        <H2>Feeds</H2>
        <ul className="mt-2 list-disc">
          {feedUrls.map((f, i) => (
            <li key={i}>
              <button onClick={() => removeFeedUrl(f)}>🔥</button>
              <button onClick={() => setActiveFeed(f)}>✨</button>
              {f}
            </li>
          ))}
        </ul>
      </section>

      <section className="mt-8">
        <H2>Feed Content</H2>
        <ActiveFeed feedUrl={activeFeed} />
      </section>
    </main>
  )
}

export default Home
