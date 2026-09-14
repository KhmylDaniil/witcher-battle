import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useForm } from 'react-hook-form'
import { Link } from 'react-router-dom'
import { Button, Card, ErrorText, Field, Input, PageHeader, Spinner } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { gamesApi, type CreateGamePayload } from './api'

export function GamesListPage() {
  const queryClient = useQueryClient()
  const games = useQuery({ queryKey: ['games'], queryFn: () => gamesApi.list() })
  const createGame = useMutation({
    mutationFn: (payload: CreateGamePayload) => gamesApi.create(payload),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['games'] }),
  })
  const { register, handleSubmit, reset, formState } = useForm<CreateGamePayload>()

  const onSubmit = handleSubmit(async (values) => {
    await createGame.mutateAsync(values)
    reset()
  })

  return (
    <div className="flex flex-col gap-6">
      <PageHeader title="Мои игры" />

      {games.isLoading && <Spinner />}
      {games.data && games.data.length === 0 && <p className="text-neutral-500">Пока нет ни одной игры.</p>}

      <div className="grid gap-3 sm:grid-cols-2">
        {games.data?.map((game) => (
          <Link key={game.id} to={`/games/${game.id}`}>
            <Card className="h-full transition hover:border-violet-400">
              <h2 className="font-semibold">{game.name}</h2>
              <p className="mt-1 text-sm text-neutral-500">{game.description || 'Без описания'}</p>
              <p className="mt-2 text-xs text-neutral-400">{Object.keys(game.users).length} участников</p>
            </Card>
          </Link>
        ))}
      </div>

      <Card>
        <h2 className="mb-3 font-semibold">Создать новую игру</h2>
        <form onSubmit={onSubmit} className="flex flex-col gap-3 sm:flex-row sm:items-end">
          <Field label="Название">
            <Input {...register('name', { required: true })} />
          </Field>
          <Field label="Описание">
            <Input {...register('description')} />
          </Field>
          <Button type="submit" disabled={formState.isSubmitting}>
            Создать
          </Button>
        </form>
        {createGame.error && (
          <div className="mt-2">
            <ErrorText>{createGame.error instanceof ApiError ? createGame.error.message : 'Не удалось создать игру'}</ErrorText>
          </div>
        )}
      </Card>
    </div>
  )
}
