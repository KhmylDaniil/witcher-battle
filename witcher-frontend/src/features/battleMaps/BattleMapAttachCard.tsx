import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useNavigate } from 'react-router-dom'
import { Button, Card, ErrorText, Select, Spinner } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import type { Battle } from '../../types/api'
import { battleMapPlacementApi, battleMapsApi } from './api'
import { battleMapWindowPath, openBattleMapWindow } from './editorWindow'

/**
 * Карточка "Карта боя" на странице боя — только мастеру: подключить к бою одну из карт игры и открыть
 * окно расстановки участников. Смена карты снимает всех участников с поля (позиции относятся к карте).
 */
export function BattleMapAttachCard({ gameId, battle }: { gameId: number; battle: Battle }) {
  const navigate = useNavigate()
  const queryClient = useQueryClient()

  const maps = useQuery({
    queryKey: ['battle-maps', { gameId }, 'all'],
    queryFn: () => battleMapsApi.list({ gameId }, { pageSize: 500 }),
  })
  const [chosenMapId, setChosenMapId] = useState<number | null>(null)
  const selectedMapId = chosenMapId ?? battle.battleMapId ?? maps.data?.items[0]?.id ?? null

  const attach = useMutation({
    mutationFn: (battleMapId: number | null) => battleMapPlacementApi.attach(gameId, battle.id, battleMapId),
    onSuccess: async () => {
      setChosenMapId(null)
      await queryClient.invalidateQueries({ queryKey: ['battles', gameId, battle.id] })
    },
  })

  const openWindow = () => {
    if (!openBattleMapWindow(gameId, battle.id)) navigate(battleMapWindowPath(gameId, battle.id))
  }

  const attachedMap = maps.data?.items.find((m) => m.id === battle.battleMapId)
  const placedCount =
    battle.creatures.filter((c) => c.mapColumn !== null).length + battle.characters.filter((c) => c.mapColumn !== null).length
  const totalCount = battle.creatures.length + battle.characters.length

  const confirmChange = (battleMapId: number | null) => {
    if (placedCount > 0 && !confirm('Все участники будут сняты с текущей карты. Продолжить?')) return
    attach.mutate(battleMapId)
  }

  return (
    <Card>
      <h2 className="mb-3 font-semibold">Карта боя</h2>
      {maps.isLoading && <Spinner />}

      {battle.battleMapId !== null && (
        <div className="mb-3 flex flex-wrap items-center justify-between gap-2 text-sm">
          <span>
            Подключена: <span className="font-medium">{attachedMap?.name ?? `карта #${battle.battleMapId}`}</span>
            <span className="text-neutral-500">
              {' '}
              — на карте {placedCount} из {totalCount} участников
            </span>
          </span>
          <div className="flex gap-2">
            <Button className="px-2 py-1" onClick={openWindow}>
              Открыть карту боя
            </Button>
            <Button variant="secondary" className="px-2 py-1" disabled={attach.isPending} onClick={() => confirmChange(null)}>
              Отключить
            </Button>
          </div>
        </div>
      )}

      {maps.data && maps.data.items.length === 0 && (
        <p className="text-sm text-neutral-500">В игре пока нет карт — создайте карту на странице игры.</p>
      )}
      {maps.data && maps.data.items.length > 0 && (
        <div className="flex flex-wrap items-center gap-2 text-sm">
          <Select value={selectedMapId ?? ''} onChange={(e) => setChosenMapId(Number(e.target.value))} aria-label="Карта для боя">
            {maps.data.items.map((m) => (
              <option key={m.id} value={m.id}>
                {m.name} ({m.columns}×{m.rows})
              </option>
            ))}
          </Select>
          <Button
            variant={battle.battleMapId === null ? 'primary' : 'secondary'}
            className="px-2 py-1"
            disabled={attach.isPending || selectedMapId === null || selectedMapId === battle.battleMapId}
            onClick={() => confirmChange(selectedMapId)}
          >
            {battle.battleMapId === null ? 'Подключить карту' : 'Сменить карту'}
          </Button>
        </div>
      )}

      {attach.error && (
        <div className="mt-2">
          <ErrorText>{attach.error instanceof ApiError ? attach.error.message : 'Не удалось подключить карту'}</ErrorText>
        </div>
      )}
    </Card>
  )
}
