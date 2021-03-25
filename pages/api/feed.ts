import { NextApiRequest, NextApiResponse } from 'next'
import RssFeed, { Channel } from '../../types/RssFeed'
import fetch from "isomorphic-unfetch"
import { parseStringPromise } from 'xml2js'

export default async (req: NextApiRequest, res: NextApiResponse<Channel>) => {
  if (typeof req.query.url !== 'string') {
    res.status(400)
  } else {
    const rssResponse = await fetch(req.query.url)
    const xml = await rssResponse.text()
    const feed: RssFeed = await parseStringPromise(xml)
    res.status(200).json(feed.rss.channel[0])
  }
}
