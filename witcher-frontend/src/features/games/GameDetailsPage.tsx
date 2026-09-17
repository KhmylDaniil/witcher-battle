import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { Button, Card, ConfirmButton, ErrorText, PageHeader, Pagination, Spinner } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { BattlesCard } from '../battles/BattlesCard'
import { CreatureTemplatesCard } from '../creatureTemplates/CreatureTemplatesCard'
import { BodyTemplatesCard } from '../bodyTemplates/BodyTemplatesCard'
import { charactersApi } from '../characters/api'
import { gamesApi } from './api'

const STATS = ['int', 'str', 'rea', 'dex', 'cra', 'emp', 'wil'] as const

export function GameDetailsPage() {
  const { gameId } = useParams<{ gameId: string }>()
  const id = Number(gameId)
  const navigate = useNavigate()
  const queryClient = useQueryClient()

  const game = useQuery({ queryKey: ['games', id], queryFn: () => gamesApi.get(id) })
  const [charactersPage, setCharactersPage] = useState(1)
  const characters = useQuery({
    queryKey: ['characters', { gameId: id }, charactersPage],
    queryFn: () => charactersApi.list({ gameId: id }, { pageNumber: charactersPage }),
  })

  const isOwner = game.data?.membershipStatus === 'Owner'
  const isMember = game.data?.membershipStatus === 'Owner' || game.data?.membershipStatus === 'Member'

  const incomingRequests = useQuery({
    queryKey: ['games', id, 'join-requests'],
    queryFn: () => gamesApi.incomingRequests(id),
    enabled: isOwner,
  })

  const requestJoin = useMutation({
    mutationFn: () => gamesApi.requestJoin(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['games', id] }),
  })

  const accept = useMutation({
    mutationFn: (requestId: number) => gamesApi.acceptRequest(id, requestId),
    onSuccess: async () => {
      await Promise.all([
        queryClient.invalidateQueries({ queryKey: ['games', id, 'join-requests'] }),
        queryClient.invalidateQueries({ queryKey: ['games', id] }),
      ])
    },
  })
  const decline = useMutation({
    mutationFn: (requestId: number) => gamesApi.declineRequest(id, requestId),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['games', id, 'join-requests'] }),
  })

  const removeGame = useMutation({
    mutationFn: () => gamesApi.remove(id),
    onSuccess: () => navigate('/games'),
  })

  if (game.isLoading) return <Spinner />
  if (!game.data) return null
  const g = game.data

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title={g.name}
        actions={
          isOwner && (
            <ConfirmButton confirmMessage={`Удалить игру "${g.name}"?`} onConfirm={() => removeGame.mutate()} disabled={removeGame.isPending}>
              Удалить игру
            </ConfirmButton>
          )
        }
      />

      {g.membershipStatus === 'None' && (
        <Card className="flex items-center justify-between gap-3">
          <p className="text-sm text-neutral-500">Вы пока не участвуете в этой игре.</p>
          <Button disabled={requestJoin.isPending} onClick={() => requestJoin.mutate()}>
            Запросить присоединение
          </Button>
        </Card>
      )}
      {g.membershipStatus === 'Declined' && (
        <Card className="flex items-center justify-between gap-3">
          <p className="text-sm text-red-600 dark:text-red-400">Ваша заявка на присоединение была отклонена мастером.</p>
          <Button disabled={requestJoin.isPending} onClick={() => requestJoin.mutate()}>
            Запросить присоединение снова
          </Button>
        </Card>
      )}
      {g.membershipStatus === 'RequestPending' && (
        <Card>
          <p className="text-sm text-neutral-500">Заявка на присоединение отправлена, ожидает решения мастера.</p>
        </Card>
      )}
      {requestJoin.error && (
        <ErrorText>{requestJoin.error instanceof ApiError ? requestJoin.error.message : 'Не удалось отправить заявку'}</ErrorText>
      )}

      {isOwner && (
        <Card>
          <h2 className="mb-3 font-semibold">Входящие заявки</h2>
          {incomingRequests.isLoading && <Spinner />}
          {incomingRequests.data && incomingRequests.data.length === 0 && (
            <p className="text-sm text-neutral-500">Заявок нет.</p>
          )}
          <div className="flex flex-col gap-2">
            {incomingRequests.data?.map((r) => (
              <div key={r.id} className="flex items-center justify-between gap-2 text-sm">
                <span>Пользователь #{r.userId}</span>
                <div className="flex gap-2">
                  <Button className="px-2 py-1" disabled={accept.isPending} onClick={() => accept.mutate(r.id)}>
                    Принять
                  </Button>
                  <Button
                    variant="secondary"
                    className="px-2 py-1"
                    disabled={decline.isPending}
                    onClick={() => decline.mutate(r.id)}
                  >
                    Отклонить
                  </Button>
                </div>
              </div>
            ))}
          </div>
        </Card>
      )}

      {isOwner && <BodyTemplatesCard gameId={id} />}
      {isOwner && <CreatureTemplatesCard gameId={id} />}
      {isMember && <BattlesCard gameId={id} isOwner={isOwner} />}

      <Card>
        <div className="mb-3 flex items-center justify-between">
          <h2 className="font-semibold">Персонажи</h2>
          {isMember && (
            <Link to={`/games/${id}/characters/new`}>
              <Button className="px-2 py-1 text-xs">Создать персонажа</Button>
            </Link>
          )}
        </div>

        {characters.isLoading && <Spinner />}
        {characters.data && characters.data.items.length === 0 && <p className="text-sm text-neutral-500">Персонажей пока нет.</p>}

        <div className="grid gap-3 sm:grid-cols-2 lg:grid-cols-3">
          {characters.data?.items.map((c) => (
            <Link key={c.id} to={`/games/${id}/characters/${c.id}`}>
              <Card className="h-full transition hover:border-violet-400">
                <h3 className="font-semibold">{c.name}</h3>
                <div className="mt-2 grid grid-cols-4 gap-1 text-xs text-neutral-500">
                  {STATS.map((s) => (
                    <span key={s}>
                      {s.toUpperCase()} {c[s]}
                    </span>
                  ))}
                </div>
              </Card>
            </Link>
          ))}
        </div>

        {characters.data && (
          <Pagination
            pageNumber={characters.data.pageNumber}
            pageSize={characters.data.pageSize}
            totalCount={characters.data.totalCount}
            onPageChange={setCharactersPage}
          />
        )}
      </Card>
    </div>
  )
}
