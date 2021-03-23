import React, { FormEventHandler, useState } from 'react'
import './App.css'
import useFeedUrlStore from './hooks/useFeedUrlStore'
import clearWhitespace from './string/clearWhitespace'

const App = () => {
  const [url, setUrl] = useState<string>('')
  const [feedUrls, addFeedUrl] = useFeedUrlStore()

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()

    // TODO: make use of some actual URL validation here
    if (url.length > 0) {
      addFeedUrl(url)
      setUrl('')
    }
  }

  return (
    <main>
      <section>
        <h1>Quick Feed</h1>
        <p>The Open Source RSS Feed Reader</p>
      </section>
      
      <section>
        <h2>Add a Feed</h2>
        <form onSubmit={handleSubmit}>
          <input
            value={url}
            onChange={(e) => setUrl(clearWhitespace(e.target.value))}
          />
          <button onSubmit={handleSubmit}> Add to Feeds </button>
        </form>
      </section>

      <section>
        <h2>Feeds</h2>
        <ul>
          {feedUrls.map((f, i) => (
            <li key={i}>{f}</li>
          ))}
        </ul>
      </section>
    </main>
  )
}

export default App
