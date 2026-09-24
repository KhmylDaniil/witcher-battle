import type { HexTerrainStyle, HexTerrainType } from '../../types/api'

/** id SVG-паттерна для сочетания тип × стиль (определяются в TerrainPatternDefs). Void от стиля не зависит. */
export function terrainPatternId(type: HexTerrainType, style: HexTerrainStyle): string {
  return type === 'Void' ? 'hex-terrain-Void' : `hex-terrain-${type}-${style}`
}

/** Значение fill для гекса данного типа и стиля. Требует, чтобы на странице был отрендерен <TerrainPatternDefs />. */
export function terrainFill(type: HexTerrainType, style: HexTerrainStyle): string {
  return `url(#${terrainPatternId(type, style)})`
}

/** Короткие обозначения типа поверх гекса (включаются в редакторе, чтобы тип читался независимо от стиля). Стене и пустоте не нужны — их текстуры однозначны. */
export const TERRAIN_TYPE_GLYPHS: Record<HexTerrainType, string> = {
  Open: '',
  Difficult: '≈',
  Impassable: '✕',
  Wall: '',
  Void: '',
}
