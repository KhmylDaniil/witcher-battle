import { describe, expect, it } from 'vitest'
import type { BattleMapHex } from '../../types/api'
import { applyPaint, indexHexes } from './pendingPaints'

const hex = (column: number, row: number): BattleMapHex => ({
  column,
  row,
  terrainType: 'Open',
  terrainStyle: 'Grass',
  isPassable: true,
  movementCost: 1,
  markerText: null,
  markerVisibleToPlayers: false,
})

const saved = indexHexes([hex(0, 0), hex(1, 0), hex(0, 1)])

describe('applyPaint', () => {
  it('records changed hexes as pending', () => {
    const pending = applyPaint(new Map(), saved, [{ column: 0, row: 0 }, { column: 1, row: 0 }], 'Wall', 'Sand')
    expect([...pending.keys()].sort()).toEqual(['0,0', '1,0'])
    expect(pending.get('0,0')).toEqual({ column: 0, row: 0, terrainType: 'Wall', terrainStyle: 'Sand' })
  })

  it('drops a pending change once the hex is painted back to its saved look', () => {
    const painted = applyPaint(new Map(), saved, [{ column: 0, row: 0 }], 'Wall', 'Sand')
    const reverted = applyPaint(painted, saved, [{ column: 0, row: 0 }], 'Open', 'Grass')
    expect(reverted.size).toBe(0)
  })

  it('returns the same object when nothing changes', () => {
    const pending = new Map()
    expect(applyPaint(pending, saved, [{ column: 0, row: 0 }], 'Open', 'Grass')).toBe(pending)
  })

  it('ignores coordinates outside the map', () => {
    const pending = applyPaint(new Map(), saved, [{ column: 5, row: 5 }], 'Wall', 'Sand')
    expect(pending.size).toBe(0)
  })
})
