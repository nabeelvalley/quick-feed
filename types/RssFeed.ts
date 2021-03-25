export interface Item {
  title: [string]
  description: [string]
  link: [string]
  pubDate: [string]
  'content:encoded': [string]
}

export interface Channel {
  title: [string]
  description: [string]
  link: [string]
  generator: [string]
  lastBuildDate: [string]
  item: Item[]
}

export interface Rss {
  channel: [Channel]
}

export interface RssFeed {
  rss: Rss
}

export default RssFeed
