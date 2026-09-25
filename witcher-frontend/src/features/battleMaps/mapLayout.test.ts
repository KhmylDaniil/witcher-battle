import { describe, expect, it } from 'vitest'
import { nameAbbreviation } from './mapLayout'

describe('nameAbbreviation', () => {
  it('takes the first three letters of the name', () => {
    expect(nameAbbreviation('Волколак')).toBe('Вол')
  })

  it('keeps short names whole and trims whitespace', () => {
    expect(nameAbbreviation('  Ян ')).toBe('Ян')
  })

  it('falls back to a question mark for an empty name', () => {
    expect(nameAbbreviation('   ')).toBe('?')
  })
})
