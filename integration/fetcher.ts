/**
 * Default fetcher for swr, simply wraps `fetch` and returns the parsed JSON
 * @param input
 * @param init
 * @returns parsed JSON of type `T`
 */
const fetcher = async <T>(
  input: RequestInfo,
  init?: RequestInit,
): Promise<T> => {
  const response = await fetch(input, init)
  const parsed: T = await response.json()
  return parsed
}

/**
 * Default fetcher for swr, simply wraps `fetch` and returns the parsed JSON
 * @param input
 * @param init
 * @returns parsed XMLDocument
 */
export const xmlFetcher = async (input: RequestInfo, init?: RequestInit) => {
  const response = await fetch(input, init)
  console.log(response)
  const str = await response.text()
  const xml: XMLDocument = await new DOMParser().parseFromString(
    str,
    'text/xml',
  )

  return xml
}

export default fetcher
