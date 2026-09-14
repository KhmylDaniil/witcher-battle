import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useForm } from 'react-hook-form'
import { Link, useParams } from 'react-router-dom'
import { Button, Card, ErrorText, Field, Input, PageHeader, Select, Spinner } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { useGameId } from '../../routes/GameLayout'
import { creatureTemplatesApi } from '../creatureTemplates/api'
import { referenceApi } from '../reference/api'
import { battlesApi, type CreateCreaturePayload } from './api'

export function BattleDetailsPage() {
  const gameId = useGameId()
  const { battleId } = useParams<{ battleId: string }>()
  const queryClient = useQueryClient()

  const battle = useQuery({ queryKey: ['battles', gameId, battleId], queryFn: () => battlesApi.get(gameId, battleId!) })
  const templates = useQuery({ queryKey: ['creature-templates', gameId], queryFn: () => creatureTemplatesApi.list(gameId) })
  const characters = useQuery({ queryKey: ['reference', gameId, 'characters'], queryFn: () => referenceApi.characters(gameId) })

  const invalidate = () => queryClient.invalidateQueries({ queryKey: ['battles', gameId, battleId] })

  const addCreature = useMutation({
    mutationFn: (payload: CreateCreaturePayload) => battlesApi.addCreature(gameId, battleId!, payload),
    onSuccess: invalidate,
  })
  const removeCreature = useMutation({
    mutationFn: ({ creatureId, name }: { creatureId: string; name: string }) =>
      battlesApi.removeCreature(gameId, battleId!, creatureId, name),
    onSuccess: invalidate,
  })
  const addCharacter = useMutation({
    mutationFn: (characterId: string) => battlesApi.addCharacter(gameId, battleId!, characterId),
    onSuccess: invalidate,
  })

  const { register, handleSubmit, reset, formState } = useForm<CreateCreaturePayload>()
  const onAddCreature = handleSubmit(async (values) => {
    await addCreature.mutateAsync(values)
    reset()
  })

  if (battle.isLoading) return <Spinner />
  if (!battle.data) return null

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title={battle.data.name}
        actions={
          <Link to={`/games/${gameId}/battles/${battleId}/run`}>
            <Button>Начать/продолжить бой</Button>
          </Link>
        }
      />

      <Card>
        <p className="text-sm text-neutral-500">{battle.data.description || 'Без описания'}</p>
      </Card>

      <Card>
        <h2 className="mb-3 font-semibold">Участники</h2>
        {battle.data.creatures.length === 0 && <p className="text-sm text-neutral-500">Пока никого нет.</p>}
        <div className="overflow-x-auto">
          <table className="w-full text-left text-sm">
            <thead>
              <tr className="border-b border-neutral-200 text-neutral-500 dark:border-neutral-800">
                <th className="py-2 pr-3">Имя</th>
                <th className="py-2 pr-3">Шаблон</th>
                <th className="py-2 pr-3">HP</th>
                <th className="py-2 pr-3">Состояния</th>
                <th className="py-2 pr-3">Инициатива</th>
                <th className="py-2" />
              </tr>
            </thead>
            <tbody>
              {battle.data.creatures.map((c) => (
                <tr key={c.id} className="border-b border-neutral-100 dark:border-neutral-900">
                  <td className="py-2 pr-3">
                    {c.name} {c.isCharacter && <span className="text-xs text-violet-500">(персонаж)</span>}
                  </td>
                  <td className="py-2 pr-3">{c.creatureTemplateName}</td>
                  <td className="py-2 pr-3">
                    {c.hp.current}/{c.hp.max}
                  </td>
                  <td className="py-2 pr-3">{c.effects || '—'}</td>
                  <td className="py-2 pr-3">{c.initiative}</td>
                  <td className="py-2">
                    <button
                      className="text-red-600 hover:underline"
                      disabled={removeCreature.isPending}
                      onClick={() => {
                        if (confirm(`Убрать "${c.name}" из боя?`)) removeCreature.mutate({ creatureId: c.id, name: c.name })
                      }}
                    >
                      Убрать
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </Card>

      <Card>
        <h2 className="mb-3 font-semibold">Добавить существо по шаблону</h2>
        <form onSubmit={onAddCreature} className="flex flex-wrap items-end gap-3">
          <Field label="Шаблон">
            <Select {...register('creatureTemplateId', { required: true })}>
              <option value="">— выберите —</option>
              {templates.data?.map((t) => (
                <option key={t.id} value={t.id}>
                  {t.name}
                </option>
              ))}
            </Select>
          </Field>
          <Field label="Имя в бою">
            <Input {...register('name', { required: true })} />
          </Field>
          <Field label="Описание">
            <Input {...register('description')} />
          </Field>
          <Button type="submit" disabled={formState.isSubmitting || addCreature.isPending}>
            Добавить
          </Button>
        </form>
        {addCreature.error && (
          <div className="mt-2">
            <ErrorText>{addCreature.error instanceof ApiError ? addCreature.error.message : 'Не удалось добавить существо'}</ErrorText>
          </div>
        )}
      </Card>

      <Card>
        <h2 className="mb-3 font-semibold">Добавить персонажа игрока</h2>
        <div className="flex flex-wrap items-end gap-3">
          <Select
            disabled={addCharacter.isPending}
            defaultValue=""
            onChange={(e) => {
              if (e.target.value) {
                addCharacter.mutate(e.target.value)
                e.target.value = ''
              }
            }}
          >
            <option value="">— выберите персонажа —</option>
            {characters.data?.map((c) => (
              <option key={c.id} value={c.id}>
                {c.name} ({c.ownerName})
              </option>
            ))}
          </Select>
        </div>
      </Card>
    </div>
  )
}
