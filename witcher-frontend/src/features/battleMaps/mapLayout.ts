import { gridPixelSize, isInBounds, pixelToHex, type HexCoord } from './hexGrid'

// Общая раскладка SVG-карты для редактора и окна карты боя: радиус гекса в единицах SVG при масштабе 1
// и поля вокруг сетки.
export const HEX_SIZE = 24
export const MAP_PADDING = 8

/** Размер viewBox карты columns × rows (с полями). */
export function mapViewSize(columns: number, rows: number): { width: number; height: number } {
  const grid = gridPixelSize(columns, rows, HEX_SIZE)
  return { width: grid.width + MAP_PADDING * 2, height: grid.height + MAP_PADDING * 2 }
}

/** Гекс под точкой экрана (clientX/clientY события) на карте в <svg> с масштабом zoom; null — мимо карты. */
export function hexAtClientPoint(
  svg: SVGSVGElement,
  clientX: number,
  clientY: number,
  zoom: number,
  columns: number,
  rows: number,
): HexCoord | null {
  const rect = svg.getBoundingClientRect()
  const hex = pixelToHex((clientX - rect.left) / zoom - MAP_PADDING, (clientY - rect.top) / zoom - MAP_PADDING, HEX_SIZE)
  return isInBounds(hex, columns, rows) ? hex : null
}

/** "Начало названия" для карточки участника без аватарки: первые буквы имени. */
export function nameAbbreviation(name: string, length = 3): string {
  const trimmed = name.trim()
  return trimmed.length === 0 ? '?' : [...trimmed].slice(0, length).join('')
}
