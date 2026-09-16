import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useNavigate } from 'react-router-dom'
import { Button, Card, ErrorText, Field, Input, Spinner } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { battlesApi } from './api'

/** Карточка "Бои" на странице игры — виден список мастеру и участникам, создание — только мастеру. */
export function BattlesCard({ gameId, isOwner }: { gameId: number; isOwner: boolean }) {
  const navigate = useNavigate()
  const queryClient = useQueryClient()

  const battles = useQuery({
    queryKey: ['battles', { gameId }],
    queryFn: () => battlesApi.list(gameId),
  })

  const [showForm, setShowForm] = useState(false)
  const [name, setName] = useState('')
  const create = useMutation({
    mutationFn: () => battlesApi.create(gameId, { name }),
    onSuccess: async (created) => {
      await queryClient.invalidateQueries({ queryKey: ['battles', { gameId }] })
      setName('')
      setShowForm(false)
      navigate(`/games/${gameId}/battles/${created.id}`)
    },
  })

  return (
    <Card>
      <div className="mb-3 flex items-center justify-between">
        <h2 className="font-semibold">Бои</h2>
        {isOwner && !showForm && (
          <Button className="px-2 py-1 text-xs" onClick={() => setShowForm(true)}>
            Создать
          </Button>
        )}
      </div>

      {showForm && (
        <form
          onSubmit={(e) => {
            e.preventDefault()
            create.mutate()
          }}
          className="mb-4 flex flex-col gap-3 rounded-md border border-neutral-200 p-3 dark:border-neutral-800"
        >
          <Field label="Название боя">
            <Input value={name} onChange={(e) => setName(e.target.value)} required autoFocus />
          </Field>
          {create.error && <ErrorText>{create.error instanceof ApiError ? create.error.message : 'Не удалось создать бой'}</ErrorText>}
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

      {battles.isLoading && <Spinner />}
      {battles.data && battles.data.length === 0 && <p className="text-sm text-neutral-500">Боёв пока нет.</p>}
      <div className="flex flex-col gap-2">
        {battles.data?.map((b) => (
          <Link
            key={b.id}
            to={`/games/${gameId}/battles/${b.id}`}
            className="flex items-center justify-between gap-2 text-sm hover:text-violet-600"
          >
            <span>{b.name}</span>
            <span className="text-xs text-neutral-400">{b.status === 'Draft' ? 'подготовка' : 'идёт бой'}</span>
          </Link>
        ))}
      </div>
    </Card>
  )
}
