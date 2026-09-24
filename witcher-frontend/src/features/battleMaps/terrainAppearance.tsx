import type { ReactNode } from 'react'
import { HEX_TERRAIN_STYLES, HEX_TERRAIN_TYPES, type HexTerrainStyle, type HexTerrainType } from '../../types/api'
import { terrainFill, terrainPatternId } from './terrainFill'

// Внешний вид гекса = сочетание типа террейна (правила движения) и стиля (сеттинг). Каждое сочетание —
// отдельный SVG <pattern> 24×24, нарисованный векторно: без внешних картинок, масштабируется вместе с
// картой. Чтобы заменить сочетание на растровую текстуру, достаточно вернуть из TILES соответствующей
// ячейки <image href=... width={TILE} height={TILE} /> — остальной код ссылается только на terrainFill() (terrainFill.ts).
// Черное пространство (Void) от стиля не зависит — одна общая заливка.

const TILE = 24

interface Palette {
  base: string
  light: string
  dark: string
  deep: string
  /** Камень/материал стен в этом стиле. */
  wall: string
  wallLight: string
}

const PALETTES: Record<HexTerrainStyle, Palette> = {
  Grass: { base: '#5f8f35', light: '#84b44c', dark: '#3d6522', deep: '#24411a', wall: '#8b8f82', wallLight: '#a9ad9e' },
  Sand: { base: '#d8b46a', light: '#ecd392', dark: '#a98545', deep: '#7a5c2c', wall: '#c4955a', wallLight: '#dcb27a' },
  Water: { base: '#3a79b0', light: '#72aad9', dark: '#245580', deep: '#10304f', wall: '#6d8596', wallLight: '#8ea5b4' },
  AncientStreet: { base: '#b3a78f', light: '#d3c9b3', dark: '#7d7260', deep: '#51483b', wall: '#ddd5c4', wallLight: '#f1ebdd' },
  FuturisticMetal: { base: '#7d8a97', light: '#b6c2ce', dark: '#4c5763', deep: '#262d35', wall: '#5a6673', wallLight: '#8793a0' },
}

const VOID_COLOR = '#07070b'
const METAL_ACCENT = '#35c9da'

// ---- Простой террейн: "чистая" поверхность стиля ----
const OPEN: Record<HexTerrainStyle, (p: Palette) => ReactNode> = {
  Grass: (p) => (
    <>
      <rect width={TILE} height={TILE} fill={p.base} />
      <path d="M3 8l1-3M5 8l0-3M13 18l1-3M15 18l0-3M19 7l1-3M9 22l1-2" stroke={p.light} strokeWidth="1" strokeLinecap="round" />
      <path d="M8 12l-1-2M20 16l-1-2M2 19l1-2" stroke={p.dark} strokeWidth="1" strokeLinecap="round" />
    </>
  ),
  Sand: (p) => (
    <>
      <rect width={TILE} height={TILE} fill={p.base} />
      <g fill={p.light}>
        <circle cx="4" cy="5" r="0.8" />
        <circle cx="15" cy="9" r="0.8" />
        <circle cx="9" cy="17" r="0.8" />
        <circle cx="20" cy="20" r="0.8" />
      </g>
      <g fill={p.dark}>
        <circle cx="11" cy="4" r="0.6" />
        <circle cx="20" cy="13" r="0.6" />
        <circle cx="3" cy="14" r="0.6" />
        <circle cx="15" cy="22" r="0.6" />
      </g>
    </>
  ),
  Water: (p) => (
    <>
      <rect width={TILE} height={TILE} fill={p.base} />
      <path d="M0 7q3-2 6 0t6 0t6 0t6 0M0 19q3-2 6 0t6 0t6 0t6 0" fill="none" stroke={p.light} strokeWidth="1" opacity="0.8" />
    </>
  ),
  AncientStreet: (p) => (
    <>
      <rect width={TILE} height={TILE} fill={p.dark} />
      <g fill={p.base}>
        <rect x="0.5" y="0.5" width="11" height="7" rx="1.5" />
        <rect x="12.5" y="0.5" width="11" height="7" rx="1.5" />
        <rect x="-5.5" y="8.5" width="11" height="7" rx="1.5" />
        <rect x="6.5" y="8.5" width="11" height="7" rx="1.5" fill={p.light} />
        <rect x="18.5" y="8.5" width="11" height="7" rx="1.5" />
        <rect x="0.5" y="16.5" width="11" height="7" rx="1.5" fill={p.light} />
        <rect x="12.5" y="16.5" width="11" height="7" rx="1.5" />
      </g>
    </>
  ),
  FuturisticMetal: (p) => (
    <>
      <rect width={TILE} height={TILE} fill={p.base} />
      <path d="M0 0.5H24M0.5 0V24M0 12.5H24M12.5 0V24" stroke={p.dark} strokeWidth="1" />
      <path d="M1 1.5H12M1.5 1V12M13 13.5H24M13.5 13V24" stroke={p.light} strokeWidth="0.6" opacity="0.7" />
      <g fill={p.light}>
        <circle cx="3" cy="3" r="0.7" />
        <circle cx="15" cy="15" r="0.7" />
        <circle cx="10" cy="10" r="0.7" />
        <circle cx="22" cy="22" r="0.7" />
      </g>
    </>
  ),
}

// ---- Сложный террейн: поверхность стиля + то, что мешает идти ----
const DIFFICULT: Record<HexTerrainStyle, (p: Palette) => ReactNode> = {
  // Кустарник
  Grass: (p) => (
    <>
      {OPEN.Grass(p)}
      <g fill={p.dark}>
        <circle cx="6" cy="7" r="3.2" />
        <circle cx="9" cy="6" r="2.6" />
        <circle cx="17" cy="17" r="3.2" />
        <circle cx="20" cy="16" r="2.6" />
      </g>
      <g fill={p.light} opacity="0.7">
        <circle cx="7.5" cy="5.5" r="1" />
        <circle cx="18.5" cy="15.5" r="1" />
      </g>
    </>
  ),
  // Барханы
  Sand: (p) => (
    <>
      {OPEN.Sand(p)}
      <path d="M0 9q6-6 12 0t12 0M0 21q6-6 12 0t12 0" fill="none" stroke={p.dark} strokeWidth="1.6" />
      <path d="M0 7.5q6-6 12 0t12 0M0 19.5q6-6 12 0t12 0" fill="none" stroke={p.light} strokeWidth="1" />
    </>
  ),
  // Мелководье с камышом
  Water: (p) => (
    <>
      <rect width={TILE} height={TILE} fill={p.light} />
      <path d="M0 12q3-2 6 0t6 0t6 0t6 0" fill="none" stroke={p.base} strokeWidth="1" />
      <path d="M4 10V3M6 10V5M16 22V15M18 22V16M20 22V14" stroke="#4d7a2c" strokeWidth="1.2" strokeLinecap="round" />
    </>
  ),
  // Обломки кладки
  AncientStreet: (p) => (
    <>
      {OPEN.AncientStreet(p)}
      <g fill={p.deep}>
        <path d="M3 4l5-1 2 4-4 3-4-2z" />
        <path d="M14 14l6 1 1 4-5 2-3-3z" />
        <path d="M17 4l3 1-1 3-3-1z" />
      </g>
      <g fill={p.light}>
        <path d="M4 5l3-0.6 1 2-2 1.2z" />
        <path d="M15 15l4 0.7-1 2-2-0.3z" />
      </g>
    </>
  ),
  // Ящики и трубы
  FuturisticMetal: (p) => (
    <>
      {OPEN.FuturisticMetal(p)}
      <rect x="3" y="3" width="7" height="7" fill={p.dark} stroke={p.deep} strokeWidth="1" />
      <path d="M3 3l7 7M10 3l-7 7" stroke={p.deep} strokeWidth="0.8" />
      <rect x="14" y="15" width="8" height="3" rx="1.5" fill={p.deep} />
      <rect x="14" y="19" width="8" height="3" rx="1.5" fill={p.dark} />
      <rect x="16" y="15" width="1" height="7" fill={METAL_ACCENT} opacity="0.8" />
    </>
  ),
}

// ---- Непроходимый террейн: зайти нельзя, но это не стена ----
const IMPASSABLE: Record<HexTerrainStyle, (p: Palette) => ReactNode> = {
  // Непролазная чаща
  Grass: (p) => (
    <>
      <rect width={TILE} height={TILE} fill={p.deep} />
      <g fill={p.dark}>
        <circle cx="6" cy="6" r="6" />
        <circle cx="18" cy="18" r="6" />
        <circle cx="19" cy="4" r="4" />
        <circle cx="4" cy="19" r="4" />
      </g>
      <g fill={p.base}>
        <circle cx="5" cy="5" r="2.5" />
        <circle cx="17" cy="17" r="2.5" />
      </g>
    </>
  ),
  // Скалы
  Sand: (p) => (
    <>
      <rect width={TILE} height={TILE} fill={p.dark} />
      <path d="M1 22L7 6l6 16zM12 14l6-12 6 12z" fill={p.deep} />
      <path d="M7 6l2 16h4zM18 2l1 12h5z" fill={p.base} opacity="0.6" />
    </>
  ),
  // Глубокая вода
  Water: (p) => (
    <>
      <rect width={TILE} height={TILE} fill={p.deep} />
      <path d="M0 6q3-2 6 0t6 0t6 0t6 0M0 14q3-2 6 0t6 0t6 0t6 0M0 22q3-2 6 0t6 0t6 0t6 0" fill="none" stroke={p.dark} strokeWidth="1.4" />
    </>
  ),
  // Провал в мостовой
  AncientStreet: (p) => (
    <>
      <rect width={TILE} height={TILE} fill={p.deep} />
      <path d="M0 0h24v5l-4 2-5-2-4 3-6-2-5 2zM0 24h24v-5l-5 2-4-3-5 2-5-2-5 2z" fill={p.base} />
      <path d="M0 11h24v2H0z" fill="#1e1a15" />
    </>
  ),
  // Опасная зона (силовое поле / машинерия)
  FuturisticMetal: (p) => (
    <>
      <rect width={TILE} height={TILE} fill={p.deep} />
      <path d="M-6 6L6 -6M-6 18L18 -6M-6 30L30 -6M6 30L30 6M18 30L30 18" stroke="#e0b21f" strokeWidth="4" />
    </>
  ),
}

// ---- Стена: кладка из материала стиля ----
function bricks(p: Palette, extra?: ReactNode) {
  return (
    <>
      <rect width={TILE} height={TILE} fill={p.deep} />
      <g fill={p.wall}>
        <rect x="0.75" y="0.75" width="10.5" height="4.5" />
        <rect x="12.75" y="0.75" width="10.5" height="4.5" />
        <rect x="-5.25" y="6.75" width="10.5" height="4.5" />
        <rect x="6.75" y="6.75" width="10.5" height="4.5" />
        <rect x="18.75" y="6.75" width="10.5" height="4.5" />
        <rect x="0.75" y="12.75" width="10.5" height="4.5" />
        <rect x="12.75" y="12.75" width="10.5" height="4.5" />
        <rect x="-5.25" y="18.75" width="10.5" height="4.5" />
        <rect x="6.75" y="18.75" width="10.5" height="4.5" />
        <rect x="18.75" y="18.75" width="10.5" height="4.5" />
      </g>
      <g fill={p.wallLight}>
        <rect x="0.75" y="0.75" width="10.5" height="1.2" />
        <rect x="6.75" y="6.75" width="10.5" height="1.2" />
        <rect x="12.75" y="12.75" width="10.5" height="1.2" />
        <rect x="-5.25" y="18.75" width="10.5" height="1.2" />
        <rect x="18.75" y="18.75" width="10.5" height="1.2" />
      </g>
      {extra}
    </>
  )
}

const WALL: Record<HexTerrainStyle, (p: Palette) => ReactNode> = {
  // Замшелый камень
  Grass: (p) =>
    bricks(
      p,
      <g fill={p.base}>
        <circle cx="3" cy="5" r="1.4" />
        <circle cx="15" cy="11" r="1.2" />
        <circle cx="21" cy="17" r="1.4" />
        <circle cx="9" cy="23" r="1.2" />
      </g>,
    ),
  // Песчаник
  Sand: (p) => bricks(p),
  // Мокрый камень с потёками
  Water: (p) => bricks(p, <path d="M5 1v6M17 13v6M11 19v4" stroke={p.light} strokeWidth="1" opacity="0.7" />),
  // Мраморные блоки — крупнее, светлее
  AncientStreet: (p) => (
    <>
      <rect width={TILE} height={TILE} fill={p.dark} />
      <rect x="0.75" y="0.75" width="22.5" height="10.5" fill={p.wall} />
      <rect x="-11.25" y="12.75" width="22.5" height="10.5" fill={p.wall} />
      <rect x="12.75" y="12.75" width="22.5" height="10.5" fill={p.wall} />
      <path d="M0.75 3H23.25M12.75 15h10.5M0 15h11.25" stroke={p.wallLight} strokeWidth="1.2" />
    </>
  ),
  // Стальные плиты с заклёпками и неоновой полосой
  FuturisticMetal: (p) => (
    <>
      <rect width={TILE} height={TILE} fill={p.deep} />
      <rect x="1" y="1" width="22" height="10" fill={p.wall} />
      <rect x="1" y="13" width="22" height="10" fill={p.wall} />
      <rect x="1" y="11.25" width="22" height="1.5" fill={METAL_ACCENT} opacity="0.85" />
      <g fill={p.wallLight}>
        <circle cx="3" cy="3" r="0.8" />
        <circle cx="21" cy="3" r="0.8" />
        <circle cx="3" cy="21" r="0.8" />
        <circle cx="21" cy="21" r="0.8" />
      </g>
    </>
  ),
}

const TILES: Record<Exclude<HexTerrainType, 'Void'>, Record<HexTerrainStyle, (p: Palette) => ReactNode>> = {
  Open: OPEN,
  Difficult: DIFFICULT,
  Impassable: IMPASSABLE,
  Wall: WALL,
}

/**
 * Определения всех паттернов тип × стиль. Рендерится один раз на страницу — на паттерны ссылаются все
 * SVG страницы (карта, образцы в палитре). Не display:none: в Chrome паттерны из скрытого так SVG не рисуются.
 */
export function TerrainPatternDefs() {
  return (
    <svg width="0" height="0" style={{ position: 'absolute' }} aria-hidden="true">
      <defs>
        {HEX_TERRAIN_TYPES.filter((t) => t !== 'Void').map((type) =>
          HEX_TERRAIN_STYLES.map((style) => (
            <pattern key={terrainPatternId(type, style)} id={terrainPatternId(type, style)} width={TILE} height={TILE} patternUnits="userSpaceOnUse">
              {TILES[type as Exclude<HexTerrainType, 'Void'>][style](PALETTES[style])}
            </pattern>
          )),
        )}
        <pattern id={terrainPatternId('Void', 'Grass')} width={TILE} height={TILE} patternUnits="userSpaceOnUse">
          <rect width={TILE} height={TILE} fill={VOID_COLOR} />
        </pattern>
      </defs>
    </svg>
  )
}

/** Маленький гекс-образец сочетания тип × стиль (для палитры и легенды). */
export function TerrainSwatch({ type, style, size = 28 }: { type: HexTerrainType; style: HexTerrainStyle; size?: number }) {
  const r = size / 2
  const w = Math.sqrt(3) * r
  const points = Array.from({ length: 6 }, (_, i) => {
    const a = (Math.PI / 180) * (60 * i - 30)
    return `${(w / 2 + r * Math.cos(a)).toFixed(2)},${(r + r * Math.sin(a)).toFixed(2)}`
  }).join(' ')
  return (
    <svg width={w} height={size} viewBox={`0 0 ${w} ${size}`} aria-hidden="true" className="shrink-0">
      <polygon points={points} fill={terrainFill(type, style)} stroke="rgba(0,0,0,0.45)" strokeWidth="1" />
    </svg>
  )
}
