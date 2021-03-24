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
    const stickyValue = window.localStorage.getItem(key)

    return stickyValue !== null ? JSON.parse(stickyValue) : defaultValue
  })

  useEffect(() => {
    window.localStorage.setItem(key, JSON.stringify(value))
  }, [key, value])

  return [value, setValue]
}

export default useStickyState
