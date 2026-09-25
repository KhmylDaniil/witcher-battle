import { useEffect, useMemo, useRef, useState, type PointerEvent as ReactPointerEvent } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useParams } from 'react-router-dom'
import { Button, ErrorText, Spinner } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { useBattleUpdates } from '../../lib/battleHub'
import { HEX_TERRAIN_TYPE_LABELS, type BattleMapParticipant, type BattleMapView, type ParticipantKind } from '../../types/api'
import { battleMapPlacementApi } from './api'
import { subscribeBattleMapsChanged } from './editorWindow'
import { HexCell, MapTooltip, MarkerPin, ParticipantAvatar, ParticipantToken } from './HexMapLayers'
import { hexKey, hexPolygonPoints, type HexCoord } from './hexGrid'
import { HEX_SIZE, MAP_PADDING, hexAtClientPoint, mapViewSize } from './mapLayout'
import { TerrainPatternDefs } from './terrainAppearance'

type ParticipantRef = { kind: ParticipantKind; id: number }

const sameParticipant = (a: ParticipantRef | null, b: ParticipantRef | null) => !!a && !!b && a.kind === b.kind && a.id === b.id

/**
 * Окно "Карта боя" (открывается со страницы боя в отдельном окне): мастер расставляет участников боя
 * по гексам подключённой карты. Это расстановка, а не движение по правилам — работает и до, и после
 * начала боя. Игроки, чьи персонажи в идущем бою, видят то же окно только для просмотра (view.canEdit
 * = false, маркеры мастера им не приходят). Обновляется по SignalR вместе со страницей боя.
 */
export function BattleMapWindowPage() {
  const { gameId, battleId } = useParams<{ gameId: string; battleId: string }>()
  const gid = Number(gameId)
  const bid = Number(battleId)
  const queryClient = useQueryClient()
  const queryKey = useMemo(() => ['battles', gid, bid, 'map'], [gid, bid])

  const view = useQuery({ queryKey, queryFn: () => battleMapPlacementApi.get(gid, bid), retry: false })
  const invalidate = () => queryClient.invalidateQueries({ queryKey })
  useBattleUpdates(bid, invalidate)
  // Карту перерисовали в окне редактора — подтягиваем свежую.
  useEffect(() => subscribeBattleMapsChanged(gid, () => queryClient.invalidateQueries({ queryKey })), [gid, queryClient, queryKey])

  useEffect(() => {
    if (view.data) document.title = `${view.data.battleName} — карта боя`
  }, [view.data])

  if (view.isLoading) return <Spinner />
  if (view.error)
    return (
      <div className="p-6">
        <ErrorText>{view.error instanceof ApiError ? view.error.message : 'Не удалось загрузить карту боя'}</ErrorText>
      </div>
    )
  if (!view.data) return null

  return <BattleMapBoard gameId={gid} view={view.data} />
}

function BattleMapBoard({ gameId, view }: { gameId: number; view: BattleMapView }) {
  const queryClient = useQueryClient()
  const map = view.map
  const [selected, setSelected] = useState<ParticipantRef | null>(null)
  const [zoom, setZoom] = useState(1)
  const [hovered, setHovered] = useState<HexCoord | null>(null)
  const [pointer, setPointer] = useState<{ x: number; y: number } | null>(null)
  const svgRef = useRef<SVGSVGElement>(null)

  const onSuccess = (updated: BattleMapView) => queryClient.setQueryData(['battles', gameId, view.battleId, 'map'], updated)
  const place = useMutation({
    mutationFn: (p: ParticipantRef & HexCoord) => battleMapPlacementApi.place(gameId, view.battleId, p.kind, p.id, p.column, p.row),
    onSuccess,
  })
  const remove = useMutation({
    mutationFn: (p: ParticipantRef) => battleMapPlacementApi.remove(gameId, view.battleId, p.kind, p.id),
    onSuccess,
  })

  useEffect(() => {
    const handler = (e: KeyboardEvent) => {
      if (e.key === 'Escape') setSelected(null)
    }
    window.addEventListener('keydown', handler)
    return () => window.removeEventListener('keydown', handler)
  }, [])

  const hexesByKey = useMemo(() => new Map((map?.hexes ?? []).map((h) => [hexKey(h.column, h.row), h])), [map])
  const participantByHex = useMemo(() => {
    const result = new Map<string, BattleMapParticipant>()
    for (const p of view.participants) if (p.column !== null && p.row !== null) result.set(hexKey(p.column, p.row), p)
    return result
  }, [view.participants])

  const readOnly = !view.canEdit
  const selectedParticipant = view.participants.find((p) => sameParticipant(p, selected)) ?? null
  const isActive = (p: BattleMapParticipant) => view.currentInitiative !== null && p.initiative === view.currentInitiative
  const error = place.error ?? remove.error

  const header = (
    <header className="flex flex-wrap items-center gap-3 border-b border-neutral-200 bg-white px-4 py-2 dark:border-neutral-800 dark:bg-neutral-900">
      <h1 className="font-semibold">{view.battleName}</h1>
      <span className="text-sm text-neutral-500">
        {map ? `Карта: ${map.name} (${map.columns}×${map.rows})` : 'Карта не подключена'}
        {view.status === 'InProgress' ? ' · бой идёт' : ' · подготовка'}
      </span>
      <div className="ml-auto flex gap-2">
        {window.opener ? (
          <Button variant="secondary" className="px-2 py-1" onClick={() => window.close()}>
            Закрыть окно
          </Button>
        ) : (
          <Link to={`/games/${gameId}/battles/${view.battleId}`}>
            <Button variant="secondary" className="px-2 py-1">
              К бою
            </Button>
          </Link>
        )}
      </div>
    </header>
  )

  if (!map) {
    return (
      <div className="flex h-screen flex-col">
        {header}
        <p className="p-6 text-sm text-neutral-500">
          {view.canEdit ? 'К этому бою карта не подключена — подключите её на странице боя.' : 'Мастер пока не подключил карту к этому бою.'}
        </p>
      </div>
    )
  }

  const hexAt = (e: ReactPointerEvent<SVGSVGElement>) => hexAtClientPoint(svgRef.current!, e.clientX, e.clientY, zoom, map.columns, map.rows)

  const onPointerDown = (e: ReactPointerEvent<SVGSVGElement>) => {
    if (readOnly || e.button !== 0) return
    const hex = hexAt(e)
    if (!hex) return
    const occupant = participantByHex.get(hexKey(hex.column, hex.row))
    // Клик по чужой фишке — выбрать её (а не пытаться поставить выбранного поверх).
    if (occupant && !sameParticipant(occupant, selected)) {
      setSelected({ kind: occupant.kind, id: occupant.id })
      return
    }
    if (selected && !occupant) place.mutate({ ...selected, ...hex })
  }

  const onPointerMove = (e: ReactPointerEvent<SVGSVGElement>) => {
    const hex = hexAt(e)
    setHovered((prev) => (prev?.column === hex?.column && prev?.row === hex?.row ? prev : hex))
    setPointer({ x: e.clientX, y: e.clientY })
  }

  const { width: viewWidth, height: viewHeight } = mapViewSize(map.columns, map.rows)
  const hoveredHex = hovered ? hexesByKey.get(hexKey(hovered.column, hovered.row)) : undefined
  const hoveredParticipant = hovered ? participantByHex.get(hexKey(hovered.column, hovered.row)) : undefined
  const canPlaceOnHovered =
    !!selected && !!hoveredHex && hoveredHex.isPassable && (!hoveredParticipant || sameParticipant(hoveredParticipant, selected))

  const placed = view.participants.filter((p) => p.column !== null)
  const unplaced = view.participants.filter((p) => p.column === null)

  const participantRow = (p: BattleMapParticipant) => {
    const isSelected = sameParticipant(p, selected)
    const content = (
      <>
        <ParticipantAvatar kind={p.kind} name={p.name} imageUrl={p.imageUrl} />
        <span className="min-w-0 flex-1">
          <span className="block truncate">
            {p.name}
            {readOnly && p.controlledByCurrentUser && <span className="ml-1 text-xs text-violet-600">(вы)</span>}
            {isActive(p) && <span className="ml-1 text-xs text-amber-600">● ход</span>}
          </span>
          <span className="block text-xs text-neutral-500">
            {p.kind === 'Creature' ? 'Существо' : 'Персонаж'} · ПЗ {p.currentHP}/{p.maxHP}
            {p.column !== null && ` · (${p.column}, ${p.row})`}
          </span>
        </span>
      </>
    )
    if (readOnly) {
      return (
        <li key={`${p.kind}-${p.id}`} className="flex items-center gap-2 px-2 py-1.5">
          {content}
        </li>
      )
    }
    return (
      <li key={`${p.kind}-${p.id}`}>
        <button
          type="button"
          aria-pressed={isSelected}
          onClick={() => setSelected(isSelected ? null : { kind: p.kind, id: p.id })}
          className={`flex w-full items-center gap-2 rounded-md border px-2 py-1.5 text-left transition ${
            isSelected ? 'border-violet-500 bg-violet-50 dark:bg-violet-950' : 'border-transparent hover:bg-neutral-100 dark:hover:bg-neutral-800'
          }`}
        >
          {content}
        </button>
      </li>
    )
  }

  return (
    <div className="flex h-screen flex-col bg-neutral-100 dark:bg-neutral-950">
      <TerrainPatternDefs />
      {header}
      {error && (
        <div className="px-4 pt-2">
          <ErrorText>{error instanceof ApiError ? error.message : 'Не удалось изменить расстановку'}</ErrorText>
        </div>
      )}

      <div className="flex min-h-0 flex-1">
        <aside className="flex w-72 shrink-0 flex-col gap-4 overflow-y-auto border-r border-neutral-200 bg-white p-3 text-sm dark:border-neutral-800 dark:bg-neutral-900">
          <p className="text-xs text-neutral-500">
            {readOnly
              ? 'Просмотр карты. Расставляет участников мастер; наведите на фишку, чтобы увидеть подробности.'
              : 'Выберите участника и кликните по свободному проходимому гексу, чтобы выставить или переставить его. На гексе может стоять только один участник. Esc — снять выбор.'}
          </p>

          {selectedParticipant && (
            <section className="flex flex-col gap-2 rounded-md border border-violet-400 p-2">
              <div className="flex items-center gap-2">
                <ParticipantAvatar kind={selectedParticipant.kind} name={selectedParticipant.name} imageUrl={selectedParticipant.imageUrl} />
                <span className="font-medium">{selectedParticipant.name}</span>
              </div>
              <p className="text-xs text-neutral-500">
                {selectedParticipant.column !== null
                  ? `Стоит на гексе (${selectedParticipant.column}, ${selectedParticipant.row}). Кликните по другому гексу, чтобы переставить.`
                  : 'Не на карте. Кликните по гексу, чтобы выставить.'}
              </p>
              {selectedParticipant.column !== null && (
                <Button variant="secondary" className="px-2 py-1" disabled={remove.isPending} onClick={() => remove.mutate(selected!)}>
                  Убрать с карты
                </Button>
              )}
            </section>
          )}

          <section>
            <h2 className="mb-1.5 font-medium">{readOnly ? 'Не на карте' : 'Не выставлены'} ({unplaced.length})</h2>
            {unplaced.length === 0 ? (
              <p className="text-xs text-neutral-500">Все участники на карте.</p>
            ) : (
              <ul className="flex flex-col gap-1">{unplaced.map(participantRow)}</ul>
            )}
          </section>

          <section>
            <h2 className="mb-1.5 font-medium">На карте ({placed.length})</h2>
            {placed.length === 0 ? (
              <p className="text-xs text-neutral-500">Пока никого.</p>
            ) : (
              <ul className="flex flex-col gap-1">{placed.map(participantRow)}</ul>
            )}
          </section>

          <label className="flex items-center gap-2">
            <span className="w-16 text-neutral-500">Масштаб</span>
            <input type="range" min={0.4} max={2} step={0.1} value={zoom} onChange={(e) => setZoom(Number(e.target.value))} className="flex-1" />
            <span className="w-10 text-right tabular-nums">{Math.round(zoom * 100)}%</span>
          </label>
        </aside>

        <main className="relative min-w-0 flex-1 overflow-auto bg-neutral-800">
          <svg
            ref={svgRef}
            width={viewWidth * zoom}
            height={viewHeight * zoom}
            viewBox={`0 0 ${viewWidth} ${viewHeight}`}
            className="m-4 touch-none select-none"
            style={{ cursor: readOnly ? 'default' : selected ? (canPlaceOnHovered ? 'copy' : hoveredParticipant ? 'pointer' : 'not-allowed') : hoveredParticipant ? 'pointer' : 'default' }}
            onPointerDown={onPointerDown}
            onPointerMove={onPointerMove}
            onPointerLeave={() => {
              setHovered(null)
              setPointer(null)
            }}
          >
            <g transform={`translate(${MAP_PADDING} ${MAP_PADDING})`}>
              {map.hexes.map((h) => (
                <HexCell
                  key={hexKey(h.column, h.row)}
                  column={h.column}
                  row={h.row}
                  terrainType={h.terrainType}
                  terrainStyle={h.terrainStyle}
                  showGlyph={false}
                />
              ))}
              {map.hexes
                .filter((h) => h.markerText)
                .map((h) => (
                  <MarkerPin key={hexKey(h.column, h.row)} column={h.column} row={h.row} />
                ))}
              {placed.map((p) => (
                <ParticipantToken
                  key={`${p.kind}-${p.id}`}
                  column={p.column!}
                  row={p.row!}
                  kind={p.kind}
                  id={p.id}
                  name={p.name}
                  imageUrl={p.imageUrl}
                  active={isActive(p)}
                  selected={sameParticipant(p, selected)}
                />
              ))}
              {selected && hovered && (
                <polygon
                  points={hexPolygonPoints(hovered.column, hovered.row, HEX_SIZE)}
                  fill={canPlaceOnHovered ? 'rgba(34,197,94,0.25)' : 'rgba(239,68,68,0.2)'}
                  stroke={canPlaceOnHovered ? '#22c55e' : '#ef4444'}
                  strokeWidth={2}
                  pointerEvents="none"
                />
              )}
            </g>
          </svg>

          {pointer && (hoveredParticipant || hoveredHex?.markerText) && (
            <MapTooltip x={pointer.x} y={pointer.y}>
              {hoveredParticipant && (
                <div className={hoveredHex?.markerText ? 'mb-1.5' : undefined}>
                  <div className="font-semibold">{hoveredParticipant.name}</div>
                  <div>
                    ПЗ {hoveredParticipant.currentHP}/{hoveredParticipant.maxHP}
                    {hoveredParticipant.initiative !== null && ` · инициатива ${hoveredParticipant.initiative}`}
                  </div>
                </div>
              )}
              {hoveredHex?.markerText && <div>📍 {hoveredHex.markerText}</div>}
            </MapTooltip>
          )}

          {hovered && hoveredHex && (
            <div className="pointer-events-none fixed bottom-3 right-3 rounded-md bg-black/75 px-3 py-1.5 text-xs text-white">
              Гекс ({hovered.column}, {hovered.row}) · {HEX_TERRAIN_TYPE_LABELS[hoveredHex.terrainType]}
            </div>
          )}
        </main>
      </div>
    </div>
  )
}
