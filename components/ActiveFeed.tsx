import React, { FC } from 'react'
import useSWR from 'swr'
import fetcher from '../integration/fetcher'
import { Channel } from '../types/RssFeed'

interface ActiveFeedProps {
  feedUrl: string
}

const feedFetcher = async (url: string) =>
  await fetcher<Channel>(`/api/feed?url=${encodeURI(url)}`, { mode: 'cors' })

const ActiveFeed: FC<ActiveFeedProps> = ({ feedUrl }) => {
  const { data, error } = useSWR(feedUrl, feedFetcher)

  console.log({ data })

  return data ? (
    <>
      {data.item.map((d) => (
        <article className="mt-4 p-8 border-pink-500 shadow-2xl rounded-3xl">
          <h3 className="text-lg text-pink-500">
            <a  className="underline" href={d.link[0]}>{d.title[0]}</a>
          </h3>
          <p>{d.pubDate[0]}</p>
          <p>{d.description[0]}</p>
        </article>
      ))}
    </>
  ) : <div className="text-pink-300 text-center text-2xl">Loading ...</div>
}

export default ActiveFeed
