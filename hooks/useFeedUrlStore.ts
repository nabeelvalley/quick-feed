import useStickyState from './useStickyState'

type AddFeedUrlFn = (url: string) => void
type RemoveFeedUrlFn = (url: string) => void

/**
 * Access the Feed URL Store
 * @return the feed url store, a function to add a feed url, and a function to remove a feed url
 */
const useFeedUrlStore = (): [string[], AddFeedUrlFn, RemoveFeedUrlFn] => {
  const [feedUrls, setFeedUrls] = useStickyState<string[]>('rss_feed_urls', [])

  const addFeedUrl = (url: string) => {
    const newUrls = feedUrls.includes(url) ? feedUrls : [...feedUrls, url]
    setFeedUrls(newUrls)
  }

  const removeFeedUrl = (url: string) => {
    const newUrls = feedUrls.filter((u) => u !== url)
    setFeedUrls(newUrls)
  }

  return [feedUrls, addFeedUrl, removeFeedUrl]
}

export default useFeedUrlStore
