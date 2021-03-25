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

export default fetcher
