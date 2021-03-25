import { useEffect, useState } from 'react'

/**
 * Set a specific value to be stored and retreived from local storage.
 * Derived from https://www.joshwcomeau.com/react/persisting-react-state-in-localstorage/
 * @param key key to use to retreive entry's data
 * @returns state data
 */
const useStickyState = <T>(
  key: string,
  defaultValue?: T,
): [T, (value: T) => void] => {
  const [value, setValue] = useState<T>(() => {
    // TODO: Find a more correct method of syncing state, probably use a DB and SWR
    const stickyValue =
      typeof window !== 'undefined'
        ? window.localStorage.getItem(key)
        : undefined

    return stickyValue? JSON.parse(stickyValue) : defaultValue
  })

  useEffect(() => {
    window.localStorage.setItem(key, JSON.stringify(value))
  }, [key, value])

  return [value, setValue]
}

export default useStickyState
