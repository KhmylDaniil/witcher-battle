import { memo, type ReactNode } from 'react'
import type { HexTerrainStyle, HexTerrainType, ParticipantKind } from '../../types/api'
import { hexCenter, hexPolygonPoints } from './hexGrid'
import { HEX_SIZE, nameAbbreviation } from './mapLayout'
import { TERRAIN_TYPE_GLYPHS, terrainFill } from './terrainFill'

// Слои SVG-карты, общие для редактора карты и окна карты боя. Все координаты — в системе <g>,
// сдвинутой на MAP_PADDING (см. mapLayout.ts).

/** Один гекс карты. memo — при мазке кистью перерисовываются только изменённые гексы, а не вся карта. */
export const HexCell = memo(function HexCell({
  column,
  row,
  terrainType,
  terrainStyle,
  showGlyph,
}: {
  column: number
  row: number
  terrainType: HexTerrainType
  terrainStyle: HexTerrainStyle
  showGlyph: boolean
}) {
  const glyph = showGlyph ? TERRAIN_TYPE_GLYPHS[terrainType] : ''
  const center = glyph ? hexCenter(column, row, HEX_SIZE) : null
  return (
    <g>
      <polygon
        points={hexPolygonPoints(column, row, HEX_SIZE)}
        fill={terrainFill(terrainType, terrainStyle)}
        stroke={terrainType === 'Void' ? '#1c1c22' : 'rgba(0,0,0,0.35)'}
        strokeWidth={1}
      />
      {center && (
        <text
          x={center.x}
          y={center.y}
          textAnchor="middle"
          dominantBaseline="central"
          fontSize={HEX_SIZE * 0.7}
          fill="#fff"
          stroke="rgba(0,0,0,0.7)"
          strokeWidth={2.5}
          paintOrder="stroke"
          pointerEvents="none"
        >
          {glyph}
        </text>
      )}
    </g>
  )
})

/**
 * Маркер мастера на гексе — булавка в правом верхнем углу гекса, чтобы не перекрываться с фишкой
 * участника в центре (на гексе может быть и участник, и объект). Янтарная — заметка только для мастера,
 * бирюзовая — маркер, открытый игрокам.
 */
export const MarkerPin = memo(function MarkerPin({ column, row, visibleToPlayers }: { column: number; row: number; visibleToPlayers: boolean }) {
  const [fill, dark] = visibleToPlayers ? ['#2dd4bf', '#134e4a'] : ['#f59e0b', '#451a03']
  const { x, y } = hexCenter(column, row, HEX_SIZE)
  const px = x + HEX_SIZE * 0.42
  const py = y - HEX_SIZE * 0.5
  const r = HEX_SIZE * 0.24
  return (
    <g pointerEvents="none">
      <path
        d={`M${px} ${py + r * 2.1} L${px - r * 0.8} ${py + r * 0.6} A${r} ${r} 0 1 1 ${px + r * 0.8} ${py + r * 0.6} Z`}
        fill={fill}
        stroke={dark}
        strokeWidth={1}
      />
      <circle cx={px} cy={py} r={r * 0.4} fill={dark} />
    </g>
  )
})

const KIND_COLORS: Record<ParticipantKind, { ring: string; card: string; text: string }> = {
  Creature: { ring: '#dc2626', card: '#7f1d1d', text: '#fee2e2' },
  Character: { ring: '#2563eb', card: '#1e3a8a', text: '#dbeafe' },
}

/**
 * Фишка участника боя в центре гекса: аватарка в круге, а если картинки нет — карточка с началом имени.
 * Цвет рамки — сторона (существо/персонаж); active — сейчас его ход, selected — выбран мастером.
 */
export function ParticipantToken({
  column,
  row,
  kind,
  id,
  name,
  imageUrl,
  active = false,
  selected = false,
}: {
  column: number
  row: number
  kind: ParticipantKind
  id: number
  name: string
  imageUrl: string | null
  active?: boolean
  selected?: boolean
}) {
  const { x, y } = hexCenter(column, row, HEX_SIZE)
  const colors = KIND_COLORS[kind]
  const r = HEX_SIZE * 0.66
  const clipId = `token-clip-${kind}-${id}`

  return (
    <g pointerEvents="none">
      {(active || selected) && (
        <circle
          cx={x}
          cy={y}
          r={r + 4}
          fill="none"
          stroke={selected ? '#a78bfa' : '#facc15'}
          strokeWidth={3}
          strokeDasharray={selected ? '4 3' : undefined}
        />
      )}
      {imageUrl ? (
        <>
          <clipPath id={clipId}>
            <circle cx={x} cy={y} r={r} />
          </clipPath>
          <circle cx={x} cy={y} r={r} fill={colors.card} />
          <image href={imageUrl} x={x - r} y={y - r} width={r * 2} height={r * 2} preserveAspectRatio="xMidYMid slice" clipPath={`url(#${clipId})`} />
          <circle cx={x} cy={y} r={r} fill="none" stroke={colors.ring} strokeWidth={2.5} />
        </>
      ) : (
        <>
          <rect
            x={x - r * 1.05}
            y={y - r * 0.72}
            width={r * 2.1}
            height={r * 1.44}
            rx={4}
            fill={colors.card}
            stroke={colors.ring}
            strokeWidth={2}
          />
          <text
            x={x}
            y={y}
            textAnchor="middle"
            dominantBaseline="central"
            fontSize={HEX_SIZE * 0.5}
            fontWeight={600}
            fill={colors.text}
          >
            {nameAbbreviation(name)}
          </text>
        </>
      )}
    </g>
  )
}

/** Та же фишка в HTML — для списков участников рядом с картой. */
export function ParticipantAvatar({ kind, name, imageUrl, size = 28 }: { kind: ParticipantKind; name: string; imageUrl: string | null; size?: number }) {
  const colors = KIND_COLORS[kind]
  if (imageUrl) {
    return (
      <img
        src={imageUrl}
        alt=""
        width={size}
        height={size}
        className="shrink-0 rounded-full object-cover"
        style={{ width: size, height: size, boxShadow: `0 0 0 2px ${colors.ring}` }}
      />
    )
  }
  return (
    <span
      className="inline-flex shrink-0 items-center justify-center rounded text-[11px] font-semibold"
      style={{ width: size * 1.3, height: size, background: colors.card, color: colors.text, boxShadow: `0 0 0 2px ${colors.ring}` }}
    >
      {nameAbbreviation(name)}
    </span>
  )
}

/** Всплывающая подсказка у курсора (координаты — clientX/clientY). */
export function MapTooltip({ x, y, children }: { x: number; y: number; children: ReactNode }) {
  return (
    <div
      role="tooltip"
      className="pointer-events-none fixed z-50 max-w-xs whitespace-pre-wrap rounded-md bg-black/85 px-3 py-2 text-xs text-white shadow-lg"
      style={{ left: x + 14, top: y + 14 }}
    >
      {children}
    </div>
  )
}
