import { describe, expect, it } from 'vitest'
import { getAvailableOptions } from './options'

describe('getAvailableOptions', () => {
  it('returns every value when none are used', () => {
    expect(getAvailableOptions(['a', 'b', 'c'], new Set())).toEqual(['a', 'b', 'c'])
  })

  it('filters out values present in the used set', () => {
    expect(getAvailableOptions(['a', 'b', 'c'], new Set(['b']))).toEqual(['a', 'c'])
  })

  it('returns an empty array once everything is used', () => {
    expect(getAvailableOptions(['a', 'b'], new Set(['a', 'b']))).toEqual([])
  })

  it('preserves the original order of all', () => {
    expect(getAvailableOptions(['c', 'a', 'b'], new Set(['a']))).toEqual(['c', 'b'])
  })
})
