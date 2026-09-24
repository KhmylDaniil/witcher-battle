import { describe, expect, it } from 'vitest'
import { floodFill, hexCenter, hexDistance, hexesInRadius, hexKey, hexNeighbors, pixelToHex } from './hexGrid'

const sortKeys = (hexes: { column: number; row: number }[]) => hexes.map((h) => hexKey(h.column, h.row)).sort()

describe('hexNeighbors (odd-r)', () => {
  it('returns the six neighbours of a hex in an even row', () => {
    expect(sortKeys(hexNeighbors({ column: 2, row: 2 }))).toEqual(sortKeys([
      { column: 3, row: 2 }, { column: 1, row: 2 },
      { column: 1, row: 1 }, { column: 2, row: 1 },
      { column: 1, row: 3 }, { column: 2, row: 3 },
    ]))
  })

  it('shifts diagonal neighbours right for a hex in an odd row', () => {
    expect(sortKeys(hexNeighbors({ column: 2, row: 1 }))).toEqual(sortKeys([
      { column: 3, row: 1 }, { column: 1, row: 1 },
      { column: 2, row: 0 }, { column: 3, row: 0 },
      { column: 2, row: 2 }, { column: 3, row: 2 },
    ]))
  })
})

describe('hexDistance', () => {
  it('is zero for the same hex and one for every neighbour', () => {
    const hex = { column: 4, row: 3 }
    expect(hexDistance(hex, hex)).toBe(0)
    for (const n of hexNeighbors(hex)) expect(hexDistance(hex, n)).toBe(1)
  })

  it('counts steps across rows', () => {
    expect(hexDistance({ column: 0, row: 0 }, { column: 3, row: 0 })).toBe(3)
    expect(hexDistance({ column: 0, row: 0 }, { column: 0, row: 4 })).toBe(4)
    expect(hexDistance({ column: 0, row: 0 }, { column: 2, row: 4 })).toBe(4)
  })
})

describe('pixelToHex', () => {
  it('maps every hex centre back to that hex', () => {
    for (let row = 0; row < 5; row++) {
      for (let column = 0; column < 5; column++) {
        const { x, y } = hexCenter(column, row, 20)
        expect(pixelToHex(x, y, 20)).toEqual({ column, row })
      }
    }
  })
})

describe('hexesInRadius', () => {
  it('returns 1, 7 and 19 hexes for radius 0, 1 and 2 away from the edges', () => {
    const center = { column: 5, row: 5 }
    expect(hexesInRadius(center, 0, 20, 20)).toHaveLength(1)
    expect(hexesInRadius(center, 1, 20, 20)).toHaveLength(7)
    expect(hexesInRadius(center, 2, 20, 20)).toHaveLength(19)
  })

  it('clips to the map bounds', () => {
    const hexes = hexesInRadius({ column: 0, row: 0 }, 1, 20, 20)
    expect(hexes.every((h) => h.column >= 0 && h.row >= 0)).toBe(true)
    expect(hexes).toHaveLength(3)
  })
})

describe('floodFill', () => {
  it('fills the connected region only, not hexes behind a barrier', () => {
    // Вертикальная "стена" в колонке 2 делит карту 5×3 на левую и правую части.
    const isWall = (h: { column: number }) => h.column === 2
    const region = floodFill({ column: 0, row: 0 }, 5, 3, (h) => !isWall(h))
    expect(region).toHaveLength(6)
    expect(region.every((h) => h.column < 2)).toBe(true)
  })
})
