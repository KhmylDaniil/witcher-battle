import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useForm } from 'react-hook-form'
import { Link } from 'react-router-dom'
import { Button, Card, ErrorText, Field, Input, PageHeader, Spinner } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { useGameId } from '../../routes/GameLayout'
import { battlesApi, type CreateBattlePayload } from './api'

export function BattlesListPage() {
  const gameId = useGameId()
  const queryClient = useQueryClient()
  const battles = useQuery({ queryKey: ['battles', gameId], queryFn: () => battlesApi.list(gameId) })
  const createBattle = useMutation({
    mutationFn: (payload: CreateBattlePayload) => battlesApi.create(gameId, payload),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['battles', gameId] }),
  })
  const { register, handleSubmit, reset, formState } = useForm<CreateBattlePayload>()

  const onSubmit = handleSubmit(async (values) => {
    await createBattle.mutateAsync(values)
    reset()
  })

  return (
    <div className="flex flex-col gap-6">
      <PageHeader title="Бои" />

      {battles.isLoading && <Spinner />}
      {battles.data && battles.data.length === 0 && <p className="text-neutral-500">Боёв пока нет.</p>}

      <div className="grid gap-3 sm:grid-cols-2">
        {battles.data?.map((b) => (
          <Link key={b.id} to={`/games/${gameId}/battles/${b.id}`}>
            <Card className="h-full transition hover:border-violet-400">
              <h2 className="font-semibold">{b.name}</h2>
              <p className="mt-1 text-sm text-neutral-500">{b.description || 'Без описания'}</p>
            </Card>
          </Link>
        ))}
      </div>

      <Card>
        <h2 className="mb-3 font-semibold">Создать бой</h2>
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
        {createBattle.error && (
          <div className="mt-2">
            <ErrorText>{createBattle.error instanceof ApiError ? createBattle.error.message : 'Не удалось создать бой'}</ErrorText>
          </div>
        )}
      </Card>
    </div>
  )
}
