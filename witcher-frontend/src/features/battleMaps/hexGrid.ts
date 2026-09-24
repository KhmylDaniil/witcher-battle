// Геометрия гексагональной сетки карты боя. Раскладка — "pointy-top odd-r" (гексы вершиной вверх,
// нечётные ряды сдвинуты вправо на полгекса), та же, что описана в Wastelands.Service.Domain/Entities/
// BattleMap.cs. Координаты гекса — offset (column, row), как они хранятся на бэке; соседи и расстояния
// считаются через кубические координаты (https://www.redblobgames.com/grids/hexagons/).

export interface HexCoord {
  column: number
  row: number
}

interface Cube {
  q: number
  r: number
  s: number
}

const SQRT3 = Math.sqrt(3)

export function hexKey(column: number, row: number): string {
  return `${column},${row}`
}

function toCube({ column, row }: HexCoord): Cube {
  const q = column - (row - (row & 1)) / 2
  return { q, r: row, s: -q - row }
}

function fromCube({ q, r }: Cube): HexCoord {
  return { column: q + (r - (r & 1)) / 2, row: r }
}

function roundCube(q: number, r: number): Cube {
  const s = -q - r
  let rq = Math.round(q)
  let rr = Math.round(r)
  const rs = Math.round(s)
  const dq = Math.abs(rq - q)
  const dr = Math.abs(rr - r)
  const ds = Math.abs(rs - s)
  if (dq > dr && dq > ds) rq = -rr - rs
  else if (dr > ds) rr = -rq - rs
  return { q: rq, r: rr, s: -rq - rr }
}

/** Габариты сетки из columns × rows гексов радиуса size (от центра до вершины), в пикселях. */
export function gridPixelSize(columns: number, rows: number, size: number): { width: number; height: number } {
  const hexWidth = SQRT3 * size
  return {
    // +0.5 гекса — сдвинутые нечётные ряды торчат вправо (если рядов больше одного).
    width: hexWidth * (columns + (rows > 1 ? 0.5 : 0)),
    height: size * (1.5 * (rows - 1) + 2),
  }
}

/** Центр гекса в пикселях (левый верхний угол сетки — (0, 0)). */
export function hexCenter(column: number, row: number, size: number): { x: number; y: number } {
  return {
    x: SQRT3 * size * (column + 0.5 * (row & 1)) + (SQRT3 * size) / 2,
    y: size * (1.5 * row + 1),
  }
}

/** Вершины гекса для атрибута points у SVG <polygon>. */
export function hexPolygonPoints(column: number, row: number, size: number): string {
  const { x, y } = hexCenter(column, row, size)
  const points: string[] = []
  for (let i = 0; i < 6; i++) {
    const angle = (Math.PI / 180) * (60 * i - 30)
    points.push(`${(x + size * Math.cos(angle)).toFixed(2)},${(y + size * Math.sin(angle)).toFixed(2)}`)
  }
  return points.join(' ')
}

/** Какой гекс под точкой (x, y) в пикселях сетки. Может вернуть координаты за пределами карты. */
export function pixelToHex(x: number, y: number, size: number): HexCoord {
  const px = x - (SQRT3 * size) / 2
  const py = y - size
  const q = ((SQRT3 / 3) * px - py / 3) / size
  const r = ((2 / 3) * py) / size
  return fromCube(roundCube(q, r))
}

export function isInBounds({ column, row }: HexCoord, columns: number, rows: number): boolean {
  return column >= 0 && column < columns && row >= 0 && row < rows
}

const CUBE_DIRECTIONS: Cube[] = [
  { q: 1, r: 0, s: -1 },
  { q: 1, r: -1, s: 0 },
  { q: 0, r: -1, s: 1 },
  { q: -1, r: 0, s: 1 },
  { q: -1, r: 1, s: 0 },
  { q: 0, r: 1, s: -1 },
]

/** Шесть соседей гекса (без учёта границ карты). */
export function hexNeighbors(hex: HexCoord): HexCoord[] {
  const c = toCube(hex)
  return CUBE_DIRECTIONS.map((d) => fromCube({ q: c.q + d.q, r: c.r + d.r, s: c.s + d.s }))
}

/** Расстояние в шагах между гексами. */
export function hexDistance(a: HexCoord, b: HexCoord): number {
  const ca = toCube(a)
  const cb = toCube(b)
  return Math.max(Math.abs(ca.q - cb.q), Math.abs(ca.r - cb.r), Math.abs(ca.s - cb.s))
}

/** Все гексы на расстоянии ≤ radius от center, попадающие в карту (кисть радиуса radius). */
export function hexesInRadius(center: HexCoord, radius: number, columns: number, rows: number): HexCoord[] {
  const c = toCube(center)
  const result: HexCoord[] = []
  for (let dq = -radius; dq <= radius; dq++) {
    for (let dr = Math.max(-radius, -dq - radius); dr <= Math.min(radius, -dq + radius); dr++) {
      const hex = fromCube({ q: c.q + dq, r: c.r + dr, s: c.s - dq - dr })
      if (isInBounds(hex, columns, rows)) result.push(hex)
    }
  }
  return result
}

/**
 * Заливка: связная область гексов, для которых sameRegion(hex) истинно, начиная со start (обход в
 * ширину по соседям в пределах карты). start всегда входит в результат.
 */
export function floodFill(start: HexCoord, columns: number, rows: number, sameRegion: (hex: HexCoord) => boolean): HexCoord[] {
  const visited = new Set<string>([hexKey(start.column, start.row)])
  const queue: HexCoord[] = [start]
  const result: HexCoord[] = []
  while (queue.length > 0) {
    const hex = queue.shift()!
    result.push(hex)
    for (const n of hexNeighbors(hex)) {
      const key = hexKey(n.column, n.row)
      if (visited.has(key) || !isInBounds(n, columns, rows) || !sameRegion(n)) continue
      visited.add(key)
      queue.push(n)
    }
  }
  return result
}
