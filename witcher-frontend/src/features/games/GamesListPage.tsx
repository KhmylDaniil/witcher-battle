import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link } from 'react-router-dom'
import { Button, Card, ErrorText, Field, Input, PageHeader, Spinner } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import type { GameMembershipStatus } from '../../types/api'
import { gamesApi } from './api'

const STATUS_LABEL: Record<GameMembershipStatus, string> = {
  Owner: 'Вы — мастер',
  Member: 'Вы участник',
  RequestPending: 'Заявка отправлена',
  None: '',
}

const STATUS_CLASS: Record<GameMembershipStatus, string> = {
  Owner: 'bg-violet-100 text-violet-700 dark:bg-violet-950 dark:text-violet-300',
  Member: 'bg-green-100 text-green-700 dark:bg-green-950 dark:text-green-300',
  RequestPending: 'bg-amber-100 text-amber-700 dark:bg-amber-950 dark:text-amber-300',
  None: '',
}

export function GamesListPage() {
  const queryClient = useQueryClient()
  const games = useQuery({ queryKey: ['games'], queryFn: () => gamesApi.list() })

  const [showForm, setShowForm] = useState(false)
  const [name, setName] = useState('')
  const create = useMutation({
    mutationFn: () => gamesApi.create({ name }),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ['games'] })
      setName('')
      setShowForm(false)
    },
  })

  const requestJoin = useMutation({
    mutationFn: (gameId: number) => gamesApi.requestJoin(gameId),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['games'] }),
  })

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title="Игры"
        actions={
          !showForm && (
            <Button onClick={() => setShowForm(true)}>Создать игру</Button>
          )
        }
      />

      {showForm && (
        <Card>
          <form
            onSubmit={(e) => {
              e.preventDefault()
              create.mutate()
            }}
            className="flex flex-col gap-3"
          >
            <Field label="Название игры">
              <Input value={name} onChange={(e) => setName(e.target.value)} required autoFocus />
            </Field>
            {create.error && <ErrorText>{create.error instanceof ApiError ? create.error.message : 'Не удалось создать игру'}</ErrorText>}
            <div className="flex gap-2">
              <Button type="submit" disabled={create.isPending}>
                Создать
              </Button>
              <Button type="button" variant="secondary" onClick={() => setShowForm(false)}>
                Отмена
              </Button>
            </div>
          </form>
        </Card>
      )}

      {games.isLoading && <Spinner />}
      {games.data && games.data.length === 0 && <p className="text-neutral-500">Пока нет ни одной игры.</p>}

      <div className="grid gap-3 sm:grid-cols-2 lg:grid-cols-3">
        {games.data?.map((g) => (
          <Card key={g.id} className="flex h-full flex-col justify-between gap-3">
            <Link to={`/games/${g.id}`} className="font-semibold hover:text-violet-600">
              {g.name}
            </Link>
            <div className="flex items-center justify-between gap-2">
              {g.membershipStatus !== 'None' ? (
                <span className={`rounded-full px-2 py-0.5 text-xs font-medium ${STATUS_CLASS[g.membershipStatus]}`}>
                  {STATUS_LABEL[g.membershipStatus]}
                </span>
              ) : (
                <Button
                  className="px-2 py-1 text-xs"
                  disabled={requestJoin.isPending}
                  onClick={() => requestJoin.mutate(g.id)}
                >
                  Запросить присоединение
                </Button>
              )}
            </div>
          </Card>
        ))}
      </div>

      {requestJoin.error && (
        <ErrorText>{requestJoin.error instanceof ApiError ? requestJoin.error.message : 'Не удалось отправить заявку'}</ErrorText>
      )}
    </div>
  )
}
