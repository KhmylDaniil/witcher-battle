import { useEffect, useMemo, useRef, useState, type PointerEvent as ReactPointerEvent } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useParams } from 'react-router-dom'
import { Button, ErrorText, Spinner } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { useBattleUpdates } from '../../lib/battleHub'
import { HEX_TERRAIN_TYPE_LABELS, type BattleMapParticipant, type BattleMapView, type ParticipantKind } from '../../types/api'
import { battlesApi } from '../battles/api'
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
  // Более широкий префикс battles/gid/bid матчит и 'map', и 'movement-range' — область подсветки
  // остатка движения должна обновляться на каждый SignalR-пуш (например, если движение обновили
  // действием на странице боя, а не в этом окне), не только когда меняется сама карта.
  const battleQueryKey = useMemo(() => ['battles', gid, bid], [gid, bid])

  const view = useQuery({ queryKey, queryFn: () => battleMapPlacementApi.get(gid, bid), retry: false })
  const invalidate = () => queryClient.invalidateQueries({ queryKey: battleQueryKey })
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

  // Префикс, а не полный ключ конкретного запроса — invalidateQueries матчит по нему и 'map', и
  // 'movement-range' (см. movementRange ниже): область подсветки остатка движения обязана обновляться
  // вместе с картой после любого изменения расстановки/позиции, не только после самого move.
  const battleQueryKey = ['battles', gameId, view.battleId]
  const onSuccess = (updated: BattleMapView) => {
    queryClient.setQueryData(['battles', gameId, view.battleId, 'map'], updated)
    queryClient.invalidateQueries({ queryKey: [...battleQueryKey, 'movement-range'] })
  }
  const place = useMutation({
    mutationFn: (p: ParticipantRef & HexCoord) => battleMapPlacementApi.place(gameId, view.battleId, p.kind, p.id, p.column, p.row),
    onSuccess,
  })
  const remove = useMutation({
    mutationFn: (p: ParticipantRef) => battleMapPlacementApi.remove(gameId, view.battleId, p.kind, p.id),
    onSuccess,
  })
  // Движение по правилам (в отличие от place — свободной расстановки мастером): доступно, только пока
  // выбран участник, чей сейчас ход, и им управляет текущий пользователь (свой персонаж или, для
  // мастера, существо). Сервер сам считает кратчайший маршрут и списывает очки движения.
  const move = useMutation({
    mutationFn: (hex: HexCoord) => battlesApi.move(gameId, view.battleId, hex.column, hex.row),
    // /move возвращает BattleDto (не BattleMapView) — вместо ручного патча кэша просто перезапрашиваем
    // и карту (новая позиция), и movement-range (после частичного хода остаток движения и, значит,
    // подсветка доступных гексов меняются) — SignalR-обновление сделало бы то же самое, но не
    // обязательно долетит быстрее собственного ответа мутации.
    onSuccess: () => queryClient.invalidateQueries({ queryKey: battleQueryKey }),
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
  const error = place.error ?? remove.error ?? move.error

  // Двигать (в отличие от свободно расставлять) можно только выбранного сейчас-активного участника,
  // которым управляет текущий пользователь (свой персонаж или, для мастера, существо), и только пока
  // бой идёт — сервер бы всё равно это перепроверил, но так UI не предлагает то, что заведомо откажет.
  const movable =
    !!selectedParticipant
    && selectedParticipant.column !== null
    && view.status === 'InProgress'
    && isActive(selectedParticipant)
    && selectedParticipant.controlledByCurrentUser
  const movementRange = useQuery({
    queryKey: ['battles', gameId, view.battleId, 'movement-range', selected?.kind, selected?.id],
    queryFn: () => battlesApi.movementRange(gameId, view.battleId, selected!.kind, selected!.id),
    enabled: movable,
  })
  const reachableByKey = useMemo(() => {
    const result = new Map<string, number>()
    for (const h of movementRange.data ?? []) result.set(hexKey(h.column, h.row), h.cost)
    return result
  }, [movementRange.data])

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
    if (e.button !== 0) return
    const hex = hexAt(e)
    if (!hex) return
    const occupant = participantByHex.get(hexKey(hex.column, hex.row))
    // Клик по чужой фишке — выбрать её (а не пытаться поставить/подвинуть выбранного поверх). Работает
    // и для игрока без права на place — выбор сам по себе ничего не меняет.
    if (occupant && !sameParticipant(occupant, selected)) {
      setSelected({ kind: occupant.kind, id: occupant.id })
      return
    }
    if (occupant) return
    if (movable) {
      if (reachableByKey.has(hexKey(hex.column, hex.row))) move.mutate(hex)
      return
    }
    if (!readOnly && selected) place.mutate({ ...selected, ...hex })
  }

  const onPointerMove = (e: ReactPointerEvent<SVGSVGElement>) => {
    const hex = hexAt(e)
    setHovered((prev) => (prev?.column === hex?.column && prev?.row === hex?.row ? prev : hex))
    setPointer({ x: e.clientX, y: e.clientY })
  }

  const { width: viewWidth, height: viewHeight } = mapViewSize(map.columns, map.rows)
  const hoveredHex = hovered ? hexesByKey.get(hexKey(hovered.column, hovered.row)) : undefined
  const hoveredParticipant = hovered ? participantByHex.get(hexKey(hovered.column, hovered.row)) : undefined
  const canMoveToHovered = movable && !!hovered && reachableByKey.has(hexKey(hovered.column, hovered.row))
  const canPlaceOnHovered =
    !movable
    && !readOnly
    && !!selected
    && !!hoveredHex
    && hoveredHex.isPassable
    && (!hoveredParticipant || sameParticipant(hoveredParticipant, selected))
  const canActOnHovered = canMoveToHovered || canPlaceOnHovered

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
            {p.controlledByCurrentUser && <span className="ml-1 text-xs text-violet-600">(вы)</span>}
            {isActive(p) && <span className="ml-1 text-xs text-amber-600">● ход</span>}
          </span>
          <span className="block text-xs text-neutral-500">
            {p.kind === 'Creature' ? 'Существо' : 'Персонаж'} · ПЗ {p.currentHP}/{p.maxHP}
            {p.column !== null && ` · (${p.column}, ${p.row})`}
            {' · движение '}
            {p.currentMovement}/{p.maxMovement}
          </span>
        </span>
      </>
    )
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
              ? 'Просмотр карты. Расставляет участников мастер; наведите на фишку, чтобы увидеть подробности. Когда наступит ваш ход — выберите своего персонажа, чтобы увидеть, куда он может дойти.'
              : 'Выберите участника и кликните по свободному проходимому гексу, чтобы выставить или переставить его. На гексе может стоять только один участник. В свой ход выбор активного участника вместо этого подсвечивает гексы для движения по правилам. Esc — снять выбор.'}
          </p>

          {selectedParticipant && (
            <section className="flex flex-col gap-2 rounded-md border border-violet-400 p-2">
              <div className="flex items-center gap-2">
                <ParticipantAvatar kind={selectedParticipant.kind} name={selectedParticipant.name} imageUrl={selectedParticipant.imageUrl} />
                <span className="font-medium">{selectedParticipant.name}</span>
              </div>
              {movable ? (
                <p className="text-xs text-neutral-500">
                  Ваш ход. Движение: {selectedParticipant.currentMovement}/{selectedParticipant.maxMovement}.{' '}
                  {reachableByKey.size > 1 ? 'Подсвеченные гексы — куда можно дойти прямо сейчас.' : 'Запас движения исчерпан.'}
                </p>
              ) : (
                <p className="text-xs text-neutral-500">
                  {readOnly
                    ? selectedParticipant.column !== null && `Стоит на гексе (${selectedParticipant.column}, ${selectedParticipant.row}).`
                    : selectedParticipant.column !== null
                      ? `Стоит на гексе (${selectedParticipant.column}, ${selectedParticipant.row}). Кликните по другому гексу, чтобы переставить.`
                      : 'Не на карте. Кликните по гексу, чтобы выставить.'}
                </p>
              )}
              {!readOnly && selectedParticipant.column !== null && (
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
            style={{
              cursor:
                movable || !readOnly
                  ? selected
                    ? canActOnHovered
                      ? 'copy'
                      : hoveredParticipant
                        ? 'pointer'
                        : 'not-allowed'
                    : hoveredParticipant
                      ? 'pointer'
                      : 'default'
                  : hoveredParticipant
                    ? 'pointer'
                    : 'default',
            }}
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
              {movable &&
                [...reachableByKey.keys()]
                  .filter((key) => key !== hexKey(selectedParticipant!.column!, selectedParticipant!.row!))
                  .map((key) => {
                    const [column, row] = key.split(',').map(Number)
                    return (
                      <polygon
                        key={`reachable-${key}`}
                        points={hexPolygonPoints(column, row, HEX_SIZE)}
                        fill="rgba(56,189,248,0.22)"
                        stroke="rgba(14,165,233,0.6)"
                        strokeWidth={1}
                        pointerEvents="none"
                      />
                    )
                  })}
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
              {selected && hovered && (movable || !readOnly) && (
                <polygon
                  points={hexPolygonPoints(hovered.column, hovered.row, HEX_SIZE)}
                  fill={canActOnHovered ? 'rgba(34,197,94,0.25)' : 'rgba(239,68,68,0.2)'}
                  stroke={canActOnHovered ? '#22c55e' : '#ef4444'}
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
              {movable && canMoveToHovered && ` · движение: ${reachableByKey.get(hexKey(hovered.column, hovered.row))}`}
            </div>
          )}
        </main>
      </div>
    </div>
  )
}
