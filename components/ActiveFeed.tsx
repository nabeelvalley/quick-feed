import React, { FC } from 'react'
import useSWR from 'swr'
import fetcher, { xmlFetcher } from '../integration/fetcher'

interface ActiveFeedProps {
  feedUrl: string
}

const feedFetcher = async (url: string) =>
  await xmlFetcher(url, { mode: 'cors' })

const ActiveFeed: FC<ActiveFeedProps> = ({ feedUrl }) => {
  const { data, error } = useSWR<object>(feedUrl, feedFetcher)

  return (
    <section>
      <h2>{feedUrl}</h2>
      <div
        dangerouslySetInnerHTML={{
          __html: `<pre>${JSON.stringify(data, null, 2)}</pre>`,
        }}
      ></div>
    </section>
  )
}

export default ActiveFeed
