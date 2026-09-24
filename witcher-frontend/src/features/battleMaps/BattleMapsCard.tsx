import { useEffect, useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useNavigate } from 'react-router-dom'
import { Button, Card, ConfirmButton, ErrorText, Field, Input, Pagination, Select, Spinner, Textarea } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import {
  BATTLE_MAP_MAX_DIMENSION,
  BATTLE_MAP_MIN_DIMENSION,
  HEX_TERRAIN_STYLE_LABELS,
  HEX_TERRAIN_STYLES,
  type HexTerrainStyle,
} from '../../types/api'
import { battleMapsApi } from './api'
import { battleMapEditorPath, openBattleMapEditor, reserveEditorWindow, subscribeBattleMapsChanged } from './editorWindow'

/** Карточка "Карты боя" на странице игры — только для мастера. Редактор карты открывается в отдельном окне. */
export function BattleMapsCard({ gameId }: { gameId: number }) {
  const navigate = useNavigate()
  const queryClient = useQueryClient()
  const [page, setPage] = useState(1)

  const battleMaps = useQuery({
    queryKey: ['battle-maps', { gameId }, page],
    queryFn: () => battleMapsApi.list({ gameId }, { pageNumber: page }),
  })

  // Переименование/ресайз в окне редактора — обновляем список здесь.
  useEffect(
    () => subscribeBattleMapsChanged(gameId, () => queryClient.invalidateQueries({ queryKey: ['battle-maps', { gameId }] })),
    [gameId, queryClient],
  )

  const [showForm, setShowForm] = useState(false)
  const [name, setName] = useState('')
  const [description, setDescription] = useState('')
  const [columns, setColumns] = useState(20)
  const [rows, setRows] = useState(15)
  const [terrainStyle, setTerrainStyle] = useState<HexTerrainStyle>('Grass')

  const create = useMutation({
    mutationFn: (_editorWindow: Window | null) => battleMapsApi.create(gameId, { name, description, columns, rows, terrainStyle }),
    onSuccess: async (created, editorWindow) => {
      await queryClient.invalidateQueries({ queryKey: ['battle-maps', { gameId }] })
      setName('')
      setDescription('')
      setShowForm(false)
      const path = battleMapEditorPath(gameId, created.id)
      if (editorWindow && !editorWindow.closed) editorWindow.location.href = path
      else navigate(path)
    },
    onError: (_error, editorWindow) => editorWindow?.close(),
  })
  const remove = useMutation({
    mutationFn: (battleMapId: number) => battleMapsApi.remove(battleMapId),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['battle-maps', { gameId }] }),
  })

  const openEditor = (battleMapId: number) => {
    if (!openBattleMapEditor(gameId, battleMapId)) navigate(battleMapEditorPath(gameId, battleMapId))
  }

  return (
    <Card>
      <div className="mb-3 flex items-center justify-between">
        <h2 className="font-semibold">Карты боя</h2>
        {!showForm && (
          <Button className="px-2 py-1 text-xs" onClick={() => setShowForm(true)}>
            Создать
          </Button>
        )}
      </div>

      {showForm && (
        <form
          onSubmit={(e) => {
            e.preventDefault()
            create.mutate(reserveEditorWindow())
          }}
          className="mb-4 flex flex-col gap-3 rounded-md border border-neutral-200 p-3 dark:border-neutral-800"
        >
          <Field label="Название">
            <Input value={name} onChange={(e) => setName(e.target.value)} maxLength={50} required autoFocus />
          </Field>
          <Field label="Описание">
            <Textarea rows={2} value={description} onChange={(e) => setDescription(e.target.value)} maxLength={500} />
          </Field>
          <div className="grid grid-cols-2 gap-3 sm:grid-cols-3">
            <Field label="Ширина (гексов)">
              <Input
                type="number"
                min={BATTLE_MAP_MIN_DIMENSION}
                max={BATTLE_MAP_MAX_DIMENSION}
                value={columns}
                onChange={(e) => setColumns(Number(e.target.value))}
                required
              />
            </Field>
            <Field label="Высота (гексов)">
              <Input
                type="number"
                min={BATTLE_MAP_MIN_DIMENSION}
                max={BATTLE_MAP_MAX_DIMENSION}
                value={rows}
                onChange={(e) => setRows(Number(e.target.value))}
                required
              />
            </Field>
            <Field label="Стиль местности">
              <Select value={terrainStyle} onChange={(e) => setTerrainStyle(e.target.value as HexTerrainStyle)}>
                {HEX_TERRAIN_STYLES.map((s) => (
                  <option key={s} value={s}>
                    {HEX_TERRAIN_STYLE_LABELS[s]}
                  </option>
                ))}
              </Select>
            </Field>
          </div>
          <p className="text-xs text-neutral-500">
            Карта заполнится простым террейном выбранного стиля и откроется в отдельном окне редактора.
          </p>
          {create.error && (
            <ErrorText>{create.error instanceof ApiError ? create.error.message : 'Не удалось создать карту'}</ErrorText>
          )}
          <div className="flex gap-2">
            <Button type="submit" disabled={create.isPending}>
              Создать
            </Button>
            <Button type="button" variant="secondary" onClick={() => setShowForm(false)}>
              Отмена
            </Button>
          </div>
        </form>
      )}

      {battleMaps.isLoading && <Spinner />}
      {battleMaps.data && battleMaps.data.items.length === 0 && <p className="text-sm text-neutral-500">Карт пока нет.</p>}
      <div className="flex flex-col gap-2">
        {battleMaps.data?.items.map((m) => (
          <div key={m.id} className="flex items-center justify-between gap-2 text-sm">
            <button type="button" className="text-left hover:text-violet-600" onClick={() => openEditor(m.id)}>
              {m.name}{' '}
              <span className="text-neutral-400">
                — {m.columns}×{m.rows}
              </span>
            </button>
            <div className="flex gap-2">
              <Button variant="secondary" className="px-2 py-1" onClick={() => openEditor(m.id)}>
                Открыть редактор
              </Button>
              <ConfirmButton
                className="px-2 py-1"
                confirmMessage={`Удалить карту "${m.name}"?`}
                onConfirm={() => remove.mutate(m.id)}
                disabled={remove.isPending}
              >
                Удалить
              </ConfirmButton>
            </div>
          </div>
        ))}
      </div>

      {battleMaps.data && (
        <Pagination
          pageNumber={battleMaps.data.pageNumber}
          pageSize={battleMaps.data.pageSize}
          totalCount={battleMaps.data.totalCount}
          onPageChange={setPage}
        />
      )}
    </Card>
  )
}
