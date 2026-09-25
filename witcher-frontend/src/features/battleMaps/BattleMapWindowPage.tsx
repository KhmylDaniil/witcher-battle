import { useEffect, useMemo } from 'react'
import { useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useParams } from 'react-router-dom'
import { Button, ErrorText, Spinner } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { useBattleUpdates } from '../../lib/battleHub'
import { BattleMapBoard } from './BattleMapBoard'
import { battleMapPlacementApi } from './api'
import { subscribeBattleMapsChanged } from './editorWindow'

/**
 * Окно "Карта боя" (открывается со страницы боя в отдельном окне) — то же самое, что и панель карты,
 * встроенная прямо в страницу боя (см. BattleMapBoard, BattleDetailsPage), только в отдельном окне на
 * весь экран: удобно для второго монитора или когда карта большая. Мастер расставляет участников боя
 * по гексам подключённой карты — это расстановка, а не движение по правилам, работает и до, и после
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

  const data = view.data
  return (
    <div className="flex h-screen flex-col bg-neutral-100 dark:bg-neutral-950">
      <header className="flex flex-wrap items-center gap-3 border-b border-neutral-200 bg-white px-4 py-2 dark:border-neutral-800 dark:bg-neutral-900">
        <h1 className="font-semibold">{data.battleName}</h1>
        <span className="text-sm text-neutral-500">
          {data.map ? `Карта: ${data.map.name} (${data.map.columns}×${data.map.rows})` : 'Карта не подключена'}
          {data.status === 'InProgress' ? ' · бой идёт' : ' · подготовка'}
        </span>
        <div className="ml-auto flex gap-2">
          {window.opener ? (
            <Button variant="secondary" className="px-2 py-1" onClick={() => window.close()}>
              Закрыть окно
            </Button>
          ) : (
            <Link to={`/games/${gid}/battles/${bid}`}>
              <Button variant="secondary" className="px-2 py-1">
                К бою
              </Button>
            </Link>
          )}
        </div>
      </header>

      <BattleMapBoard gameId={gid} view={data} />
    </div>
  )
}
