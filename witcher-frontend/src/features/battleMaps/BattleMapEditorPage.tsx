import {
  memo,
  useCallback,
  useEffect,
  useMemo,
  useRef,
  useState,
  type ButtonHTMLAttributes,
  type PointerEvent as ReactPointerEvent,
  type ReactNode,
} from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useParams } from 'react-router-dom'
import { Button, ErrorText, Field, Input, Select, Spinner, Textarea } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import {
  BATTLE_MAP_MAX_DIMENSION,
  BATTLE_MAP_MIN_DIMENSION,
  HEX_TERRAIN_STYLE_LABELS,
  HEX_TERRAIN_STYLES,
  HEX_TERRAIN_TYPE_LABELS,
  HEX_TERRAIN_TYPES,
  type BattleMap,
  type HexTerrainStyle,
  type HexTerrainType,
} from '../../types/api'
import { battleMapsApi } from './api'
import { notifyBattleMapsChanged } from './editorWindow'
import { floodFill, gridPixelSize, hexCenter, hexesInRadius, hexKey, hexPolygonPoints, isInBounds, pixelToHex, type HexCoord } from './hexGrid'
import { applyPaint, effectiveLook, indexHexes, type PendingPaints } from './pendingPaints'
import { TerrainPatternDefs, TerrainSwatch } from './terrainAppearance'
import { TERRAIN_TYPE_GLYPHS, terrainFill } from './terrainFill'

/** Радиус гекса (от центра до вершины) в единицах SVG при масштабе 1. */
const HEX_SIZE = 24
const PADDING = 8
const MAX_UNDO = 50

type Tool = 'brush' | 'fill' | 'picker'

const TOOLS: { id: Tool; label: string; hotkey: string }[] = [
  { id: 'brush', label: 'Кисть', hotkey: 'B' },
  { id: 'fill', label: 'Заливка', hotkey: 'F' },
  { id: 'picker', label: 'Пипетка', hotkey: 'I' },
]

const BRUSH_SIZES = [
  { radius: 0, label: '1 гекс' },
  { radius: 1, label: '7 гексов' },
  { radius: 2, label: '19 гексов' },
]

/**
 * Редактор гексагональной карты боя. Открывается в отдельном окне (см. editorWindow.ts), поэтому живёт
 * вне AppLayout — вся площадь окна отдана карте. Изменения копятся локально и уходят на бэк одним
 * запросом по кнопке "Сохранить".
 */
export function BattleMapEditorPage() {
  const { gameId, battleMapId } = useParams<{ gameId: string; battleMapId: string }>()
  const id = Number(battleMapId)
  const gid = Number(gameId)

  const battleMap = useQuery({ queryKey: ['battle-maps', id], queryFn: () => battleMapsApi.get(id) })

  useEffect(() => {
    if (battleMap.data) document.title = `${battleMap.data.name} — карта боя`
  }, [battleMap.data])

  if (battleMap.isLoading) return <Spinner />
  if (battleMap.error)
    return (
      <div className="p-6">
        <ErrorText>{battleMap.error instanceof ApiError ? battleMap.error.message : 'Не удалось загрузить карту'}</ErrorText>
      </div>
    )
  if (!battleMap.data) return null

  // key: после сохранения настроек (ресайз) редактор начинает с чистого листа по новой карте.
  return <Editor key={`${battleMap.data.columns}x${battleMap.data.rows}`} gameId={gid} map={battleMap.data} />
}

function Editor({ gameId, map }: { gameId: number; map: BattleMap }) {
  const queryClient = useQueryClient()

  const [tool, setTool] = useState<Tool>('brush')
  const [brushRadius, setBrushRadius] = useState(0)
  const [terrainType, setTerrainType] = useState<HexTerrainType>('Wall')
  const [terrainStyle, setTerrainStyle] = useState<HexTerrainStyle>(map.hexes[0]?.terrainStyle ?? 'Grass')
  const [zoom, setZoom] = useState(1)
  const [showGlyphs, setShowGlyphs] = useState(true)
  const [showSettings, setShowSettings] = useState(false)

  const saved = useMemo(() => indexHexes(map.hexes), [map.hexes])
  const [pending, setPending] = useState<PendingPaints>(() => new Map())
  const [undoStack, setUndoStack] = useState<PendingPaints[]>([])
  const [hovered, setHovered] = useState<HexCoord | null>(null)

  const save = useMutation({
    mutationFn: (snapshot: PendingPaints) => battleMapsApi.paintHexes(map.id, [...snapshot.values()]),
    onSuccess: (updated, snapshot) => {
      queryClient.setQueryData(['battle-maps', map.id], updated)
      // Пока шёл запрос, мастер мог продолжить рисовать — сохранённое убираем, более свежее оставляем.
      setPending((prev) => {
        const rest = new Map(prev)
        for (const [key, paint] of snapshot) {
          const current = rest.get(key)
          if (current?.terrainType === paint.terrainType && current.terrainStyle === paint.terrainStyle) rest.delete(key)
        }
        return rest
      })
      setUndoStack([])
    },
  })

  const discard = () => {
    if (!confirm('Отменить все несохранённые изменения?')) return
    setPending(new Map())
    setUndoStack([])
  }

  const undo = useCallback(() => {
    if (undoStack.length === 0) return
    setPending(undoStack[undoStack.length - 1])
    setUndoStack(undoStack.slice(0, -1))
  }, [undoStack])

  // Не даём случайно закрыть окно с несохранённой картой.
  useEffect(() => {
    if (pending.size === 0) return
    const handler = (e: BeforeUnloadEvent) => e.preventDefault()
    window.addEventListener('beforeunload', handler)
    return () => window.removeEventListener('beforeunload', handler)
  }, [pending.size])

  useEffect(() => {
    const handler = (e: KeyboardEvent) => {
      if (e.target instanceof HTMLInputElement || e.target instanceof HTMLTextAreaElement || e.target instanceof HTMLSelectElement) return
      if ((e.ctrlKey || e.metaKey) && e.key.toLowerCase() === 'z') {
        e.preventDefault()
        undo()
        return
      }
      const t = TOOLS.find((x) => x.hotkey.toLowerCase() === e.key.toLowerCase())
      if (t) setTool(t.id)
    }
    window.addEventListener('keydown', handler)
    return () => window.removeEventListener('keydown', handler)
  }, [undo])

  // ---- Рисование мышью/пальцем: гекс под курсором вычисляется из координат, а не из событий <polygon>,
  // чтобы мазок работал и на тач-экранах (pointer capture), и при быстром движении мыши.
  const svgRef = useRef<SVGSVGElement>(null)
  const stroking = useRef(false)
  const lastStrokeHex = useRef<string | null>(null)

  const hexAt = (e: ReactPointerEvent<SVGSVGElement>): HexCoord | null => {
    const rect = svgRef.current!.getBoundingClientRect()
    const hex = pixelToHex((e.clientX - rect.left) / zoom - PADDING, (e.clientY - rect.top) / zoom - PADDING, HEX_SIZE)
    return isInBounds(hex, map.columns, map.rows) ? hex : null
  }

  const applyTool = (hex: HexCoord) => {
    if (tool === 'picker') {
      const look = effectiveLook(hexKey(hex.column, hex.row), saved, pending)
      if (look) {
        setTerrainType(look.terrainType)
        setTerrainStyle(look.terrainStyle)
      }
      setTool('brush')
      return
    }

    let targets: HexCoord[]
    if (tool === 'fill') {
      const start = effectiveLook(hexKey(hex.column, hex.row), saved, pending)
      targets = floodFill(hex, map.columns, map.rows, (h) => {
        const look = effectiveLook(hexKey(h.column, h.row), saved, pending)
        return look?.terrainType === start?.terrainType && look?.terrainStyle === start?.terrainStyle
      })
    } else {
      targets = hexesInRadius(hex, brushRadius, map.columns, map.rows)
    }
    setPending((prev) => applyPaint(prev, saved, targets, terrainType, terrainStyle))
  }

  const onPointerDown = (e: ReactPointerEvent<SVGSVGElement>) => {
    if (e.button !== 0) return
    const hex = hexAt(e)
    if (!hex) return
    e.currentTarget.setPointerCapture(e.pointerId)
    setUndoStack((prev) => [...prev.slice(-(MAX_UNDO - 1)), pending])
    stroking.current = tool === 'brush'
    lastStrokeHex.current = hexKey(hex.column, hex.row)
    applyTool(hex)
  }

  const onPointerMove = (e: ReactPointerEvent<SVGSVGElement>) => {
    const hex = hexAt(e)
    setHovered((prev) => (prev?.column === hex?.column && prev?.row === hex?.row ? prev : hex))
    if (!stroking.current || !hex) return
    const key = hexKey(hex.column, hex.row)
    if (key === lastStrokeHex.current) return
    lastStrokeHex.current = key
    applyTool(hex)
  }

  const endStroke = () => {
    stroking.current = false
    lastStrokeHex.current = null
  }

  const grid = gridPixelSize(map.columns, map.rows, HEX_SIZE)
  const viewWidth = grid.width + PADDING * 2
  const viewHeight = grid.height + PADDING * 2

  const hoveredLook = hovered ? effectiveLook(hexKey(hovered.column, hovered.row), saved, pending) : undefined
  const brushPreview =
    hovered && tool === 'brush' ? hexesInRadius(hovered, brushRadius, map.columns, map.rows) : hovered ? [hovered] : []

  return (
    <div className="flex h-screen flex-col bg-neutral-100 dark:bg-neutral-950">
      <TerrainPatternDefs />

      <header className="flex flex-wrap items-center gap-3 border-b border-neutral-200 bg-white px-4 py-2 dark:border-neutral-800 dark:bg-neutral-900">
        <h1 className="font-semibold">{map.name}</h1>
        <span className="text-sm text-neutral-500">
          {map.columns}×{map.rows} гексов
        </span>
        <div className="ml-auto flex flex-wrap items-center gap-2">
          <span className="text-sm text-neutral-500">
            {pending.size > 0 ? `Несохранённых гексов: ${pending.size}` : 'Все изменения сохранены'}
          </span>
          <Button variant="secondary" className="px-2 py-1" onClick={undo} disabled={undoStack.length === 0} title="Ctrl+Z">
            Отменить шаг
          </Button>
          <Button variant="secondary" className="px-2 py-1" onClick={discard} disabled={pending.size === 0 || save.isPending}>
            Сбросить
          </Button>
          <Button className="px-2 py-1" onClick={() => save.mutate(pending)} disabled={pending.size === 0 || save.isPending}>
            Сохранить
          </Button>
          <Button variant="secondary" className="px-2 py-1" onClick={() => setShowSettings((v) => !v)}>
            Настройки карты
          </Button>
          {window.opener ? (
            <Button variant="secondary" className="px-2 py-1" onClick={() => window.close()}>
              Закрыть окно
            </Button>
          ) : (
            <Link to={`/games/${gameId}`}>
              <Button variant="secondary" className="px-2 py-1">
                К игре
              </Button>
            </Link>
          )}
        </div>
      </header>

      {save.error && (
        <div className="px-4 pt-2">
          <ErrorText>{save.error instanceof ApiError ? save.error.message : 'Не удалось сохранить карту'}</ErrorText>
        </div>
      )}

      <div className="flex min-h-0 flex-1">
        <aside className="flex w-72 shrink-0 flex-col gap-4 overflow-y-auto border-r border-neutral-200 bg-white p-3 text-sm dark:border-neutral-800 dark:bg-neutral-900">
          {showSettings && <MapSettingsForm gameId={gameId} map={map} hasPendingChanges={pending.size > 0} />}

          <section>
            <h2 className="mb-1.5 font-medium">Инструмент</h2>
            <div className="flex gap-1">
              {TOOLS.map((t) => (
                <ToggleButton key={t.id} active={tool === t.id} onClick={() => setTool(t.id)} title={`Горячая клавиша: ${t.hotkey}`}>
                  {t.label}
                </ToggleButton>
              ))}
            </div>
            {tool === 'brush' && (
              <div className="mt-1.5 flex gap-1">
                {BRUSH_SIZES.map((b) => (
                  <ToggleButton key={b.radius} active={brushRadius === b.radius} onClick={() => setBrushRadius(b.radius)}>
                    {b.label}
                  </ToggleButton>
                ))}
              </div>
            )}
            <p className="mt-1.5 text-xs text-neutral-500">
              {tool === 'brush' && 'Зажмите кнопку мыши и ведите по карте.'}
              {tool === 'fill' && 'Перекрашивает связную область гексов того же типа и стиля.'}
              {tool === 'picker' && 'Кликните по гексу, чтобы взять его тип и стиль.'}
            </p>
          </section>

          <section>
            <h2 className="mb-1.5 font-medium">Тип террейна</h2>
            <div className="flex flex-col gap-1">
              {HEX_TERRAIN_TYPES.map((t) => (
                <PaletteButton key={t} active={terrainType === t} onClick={() => setTerrainType(t)}>
                  <TerrainSwatch type={t} style={terrainStyle} />
                  <span>{HEX_TERRAIN_TYPE_LABELS[t]}</span>
                  <span className="ml-auto text-xs text-neutral-400">{movementHint(t)}</span>
                </PaletteButton>
              ))}
            </div>
          </section>

          <section>
            <h2 className="mb-1.5 font-medium">Стиль</h2>
            <div className="flex flex-col gap-1">
              {HEX_TERRAIN_STYLES.map((s) => (
                <PaletteButton key={s} active={terrainStyle === s} onClick={() => setTerrainStyle(s)}>
                  <TerrainSwatch type={terrainType === 'Void' ? 'Open' : terrainType} style={s} />
                  <span>{HEX_TERRAIN_STYLE_LABELS[s]}</span>
                </PaletteButton>
              ))}
            </div>
            {terrainType === 'Void' && (
              <p className="mt-1.5 text-xs text-neutral-500">Черное пространство от стиля не зависит.</p>
            )}
          </section>

          <section className="flex flex-col gap-2">
            <h2 className="font-medium">Вид</h2>
            <label className="flex items-center gap-2">
              <span className="w-16 text-neutral-500">Масштаб</span>
              <input
                type="range"
                min={0.4}
                max={2}
                step={0.1}
                value={zoom}
                onChange={(e) => setZoom(Number(e.target.value))}
                className="flex-1"
              />
              <span className="w-10 text-right tabular-nums">{Math.round(zoom * 100)}%</span>
            </label>
            <label className="flex items-center gap-2">
              <input type="checkbox" checked={showGlyphs} onChange={(e) => setShowGlyphs(e.target.checked)} />
              Обозначения типов на гексах
            </label>
          </section>

          <TerrainLegend />
        </aside>

        <main className="relative min-w-0 flex-1 overflow-auto bg-neutral-800">
          <svg
            ref={svgRef}
            width={viewWidth * zoom}
            height={viewHeight * zoom}
            viewBox={`0 0 ${viewWidth} ${viewHeight}`}
            className="m-4 touch-none select-none"
            style={{ cursor: tool === 'picker' ? 'copy' : 'crosshair' }}
            onPointerDown={onPointerDown}
            onPointerMove={onPointerMove}
            onPointerUp={endStroke}
            onPointerCancel={endStroke}
            onPointerLeave={() => setHovered(null)}
          >
            <g transform={`translate(${PADDING} ${PADDING})`}>
              {map.hexes.map((h) => {
                const key = hexKey(h.column, h.row)
                const look = pending.get(key) ?? h
                return (
                  <HexCell
                    key={key}
                    column={h.column}
                    row={h.row}
                    terrainType={look.terrainType}
                    terrainStyle={look.terrainStyle}
                    showGlyph={showGlyphs}
                  />
                )
              })}
              {brushPreview.map((h) => (
                <polygon
                  key={hexKey(h.column, h.row)}
                  points={hexPolygonPoints(h.column, h.row, HEX_SIZE)}
                  fill="rgba(255,255,255,0.15)"
                  stroke="#facc15"
                  strokeWidth={2}
                  pointerEvents="none"
                />
              ))}
            </g>
          </svg>

          {hovered && hoveredLook && (
            <div className="pointer-events-none fixed bottom-3 right-3 rounded-md bg-black/75 px-3 py-1.5 text-xs text-white">
              Гекс ({hovered.column}, {hovered.row}) · {HEX_TERRAIN_TYPE_LABELS[hoveredLook.terrainType]}
              {hoveredLook.terrainType !== 'Void' && ` · ${HEX_TERRAIN_STYLE_LABELS[hoveredLook.terrainStyle]}`}
            </div>
          )}
        </main>
      </div>
    </div>
  )
}

function movementHint(type: HexTerrainType): string {
  switch (type) {
    case 'Open':
      return 'проходим'
    case 'Difficult':
      return '×2 движение'
    default:
      return 'непроходим'
  }
}

/** Один гекс карты. memo — при мазке кистью перерисовываются только изменённые гексы, а не вся карта. */
const HexCell = memo(function HexCell({
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

function ToggleButton({ active, className = '', ...props }: ButtonHTMLAttributes<HTMLButtonElement> & { active: boolean }) {
  return (
    <button
      type="button"
      className={`flex-1 rounded-md border px-2 py-1 text-xs transition ${
        active
          ? 'border-violet-500 bg-violet-50 text-violet-700 dark:bg-violet-950 dark:text-violet-300'
          : 'border-neutral-200 hover:border-neutral-400 dark:border-neutral-700'
      } ${className}`}
      {...props}
    />
  )
}

function PaletteButton({ active, children, onClick }: { active: boolean; children: ReactNode; onClick: () => void }) {
  return (
    <button
      type="button"
      onClick={onClick}
      aria-pressed={active}
      className={`flex items-center gap-2 rounded-md border px-2 py-1 text-left transition ${
        active ? 'border-violet-500 bg-violet-50 dark:bg-violet-950' : 'border-transparent hover:bg-neutral-100 dark:hover:bg-neutral-800'
      }`}
    >
      {children}
    </button>
  )
}

/** Все сочетания тип × стиль — чтобы мастер видел, как будет выглядеть любая комбинация. */
function TerrainLegend() {
  return (
    <section>
      <h2 className="mb-1.5 font-medium">Сочетания</h2>
      <table className="text-xs">
        <tbody>
          {HEX_TERRAIN_TYPES.filter((t) => t !== 'Void').map((t) => (
            <tr key={t}>
              <td className="pr-2 text-neutral-500">{HEX_TERRAIN_TYPE_LABELS[t]}</td>
              {HEX_TERRAIN_STYLES.map((s) => (
                <td key={s} title={`${HEX_TERRAIN_TYPE_LABELS[t]} · ${HEX_TERRAIN_STYLE_LABELS[s]}`}>
                  <TerrainSwatch type={t} style={s} size={22} />
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </section>
  )
}

function MapSettingsForm({ gameId, map, hasPendingChanges }: { gameId: number; map: BattleMap; hasPendingChanges: boolean }) {
  const queryClient = useQueryClient()
  const [name, setName] = useState(map.name)
  const [description, setDescription] = useState(map.description ?? '')
  const [columns, setColumns] = useState(map.columns)
  const [rows, setRows] = useState(map.rows)
  const [fillTerrainStyle, setFillTerrainStyle] = useState<HexTerrainStyle>(map.hexes[0]?.terrainStyle ?? 'Grass')

  const update = useMutation({
    mutationFn: () => battleMapsApi.update(map.id, { name, description, columns, rows, fillTerrainStyle }),
    onSuccess: (updated) => {
      queryClient.setQueryData(['battle-maps', map.id], updated)
      notifyBattleMapsChanged(gameId)
    },
  })

  const shrinking = columns < map.columns || rows < map.rows
  const resizing = columns !== map.columns || rows !== map.rows

  return (
    <form
      className="flex flex-col gap-2 rounded-md border border-neutral-200 p-2 dark:border-neutral-700"
      onSubmit={(e) => {
        e.preventDefault()
        if (shrinking && !confirm('Карта уменьшится — гексы за новыми границами будут удалены. Продолжить?')) return
        update.mutate()
      }}
    >
      <Field label="Название">
        <Input value={name} onChange={(e) => setName(e.target.value)} maxLength={50} required />
      </Field>
      <Field label="Описание">
        <Textarea rows={2} value={description} onChange={(e) => setDescription(e.target.value)} maxLength={500} />
      </Field>
      <div className="grid grid-cols-2 gap-2">
        <Field label="Ширина">
          <Input
            type="number"
            min={BATTLE_MAP_MIN_DIMENSION}
            max={BATTLE_MAP_MAX_DIMENSION}
            value={columns}
            onChange={(e) => setColumns(Number(e.target.value))}
            required
          />
        </Field>
        <Field label="Высота">
          <Input
            type="number"
            min={BATTLE_MAP_MIN_DIMENSION}
            max={BATTLE_MAP_MAX_DIMENSION}
            value={rows}
            onChange={(e) => setRows(Number(e.target.value))}
            required
          />
        </Field>
      </div>
      {resizing && !shrinking && (
        <Field label="Стиль новых гексов">
          <Select value={fillTerrainStyle} onChange={(e) => setFillTerrainStyle(e.target.value as HexTerrainStyle)}>
            {HEX_TERRAIN_STYLES.map((s) => (
              <option key={s} value={s}>
                {HEX_TERRAIN_STYLE_LABELS[s]}
              </option>
            ))}
          </Select>
        </Field>
      )}
      {hasPendingChanges && resizing && (
        <p className="text-xs text-amber-600">Сначала сохраните или сбросьте изменения гексов — потом меняйте размер.</p>
      )}
      {update.error && <ErrorText>{update.error instanceof ApiError ? update.error.message : 'Не удалось сохранить настройки'}</ErrorText>}
      <Button type="submit" className="px-2 py-1" disabled={update.isPending || (hasPendingChanges && resizing)}>
        Применить
      </Button>
    </form>
  )
}
