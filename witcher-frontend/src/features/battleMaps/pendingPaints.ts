import type { BattleMapHex, HexPaint, HexTerrainStyle, HexTerrainType } from '../../types/api'
import { hexKey, type HexCoord } from './hexGrid'

/** Несохранённые изменения редактора: ключ hexKey(column,row) → новое состояние гекса. */
export type PendingPaints = ReadonlyMap<string, HexPaint>

export interface HexLook {
  terrainType: HexTerrainType
  terrainStyle: HexTerrainStyle
}

export function indexHexes(hexes: readonly BattleMapHex[]): ReadonlyMap<string, BattleMapHex> {
  return new Map(hexes.map((h) => [hexKey(h.column, h.row), h]))
}

/** Как гекс выглядит сейчас в редакторе: несохранённое изменение, иначе — сохранённое состояние. */
export function effectiveLook(key: string, saved: ReadonlyMap<string, BattleMapHex>, pending: PendingPaints): HexLook | undefined {
  return pending.get(key) ?? saved.get(key)
}

/**
 * Красит гексы coords в type/style. Гекс, вернувшийся к сохранённому состоянию, из pending убирается —
 * так счётчик несохранённых изменений не врёт. Если ничего не поменялось, возвращает тот же объект
 * (React не перерисует лишний раз).
 */
export function applyPaint(
  pending: PendingPaints,
  saved: ReadonlyMap<string, BattleMapHex>,
  coords: readonly HexCoord[],
  terrainType: HexTerrainType,
  terrainStyle: HexTerrainStyle,
): PendingPaints {
  let next: Map<string, HexPaint> | null = null
  for (const { column, row } of coords) {
    const key = hexKey(column, row)
    const savedHex = saved.get(key)
    if (!savedHex) continue
    const current = pending.get(key) ?? savedHex
    if (current.terrainType === terrainType && current.terrainStyle === terrainStyle) continue

    next ??= new Map(pending)
    if (savedHex.terrainType === terrainType && savedHex.terrainStyle === terrainStyle) next.delete(key)
    else next.set(key, { column, row, terrainType, terrainStyle })
  }
  return next ?? pending
}
