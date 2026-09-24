import { useEffect } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useForm } from 'react-hook-form'
import { useNavigate, useParams } from 'react-router-dom'
import { Button, Card, ErrorText, Field, Input, PageHeader, Spinner } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import type { CharacterFormValues } from '../../types/api'
import { charactersApi } from './api'

const STATS = ['int', 'str', 'rea', 'dex', 'cra', 'emp', 'wil'] as const
const STAT_MIN = 1
const STAT_MAX = 15
const HP_STA = ['hp', 'sta'] as const
const MOVEMENT_MIN = 0
const MOVEMENT_MAX = 20

export function CharacterFormPage() {
  const { gameId, characterId } = useParams<{ gameId: string; characterId: string }>()
  const gameIdNum = Number(gameId)
  const isEdit = !!characterId
  const id = characterId ? Number(characterId) : undefined
  const navigate = useNavigate()
  const queryClient = useQueryClient()

  const existing = useQuery({
    queryKey: ['characters', id],
    queryFn: () => charactersApi.get(id!),
    enabled: isEdit,
  })

  const { register, handleSubmit, reset, formState } = useForm<CharacterFormValues>({
    defaultValues: { name: '', hp: 10, sta: 10, int: 1, str: 1, rea: 1, dex: 1, cra: 1, emp: 1, wil: 1, movement: 0 },
  })

  useEffect(() => {
    if (existing.data) {
      const { name, hp, sta, int, str, rea, dex, cra, emp, wil, movement } = existing.data
      reset({ name, hp, sta, int, str, rea, dex, cra, emp, wil, movement })
    }
  }, [existing.data, reset])

  const save = useMutation({
    mutationFn: (values: CharacterFormValues) => {
      const payload: CharacterFormValues = {
        ...values,
        ...Object.fromEntries([...HP_STA, ...STATS, 'movement' as const].map((s) => [s, Number(values[s])])),
      }
      return isEdit ? charactersApi.update(id!, payload) : charactersApi.create(gameIdNum, payload)
    },
    onSuccess: async (result) => {
      await queryClient.invalidateQueries({ queryKey: ['characters'] })
      navigate(`/games/${gameIdNum}/characters/${result.id}`)
    },
  })

  const onSubmit = handleSubmit((values) => save.mutate(values))

  if (isEdit && existing.isLoading) return <Spinner />

  return (
    <div className="flex flex-col gap-4">
      <PageHeader title={isEdit ? 'Изменить персонажа' : 'Новый персонаж'} />
      <Card>
        <form onSubmit={onSubmit} className="flex flex-col gap-4">
          <Field label="Имя">
            <Input {...register('name', { required: true })} />
          </Field>

          <div>
            <h3 className="mb-2 text-sm font-medium text-neutral-700 dark:text-neutral-300">Здоровье и выносливость</h3>
            <div className="grid grid-cols-2 gap-3 sm:w-1/2">
              {HP_STA.map((s) => (
                <Field key={s} label={s.toUpperCase()}>
                  <Input
                    type="number"
                    min={1}
                    {...register(s, {
                      required: true,
                      valueAsNumber: true,
                      min: { value: 1, message: 'Должно быть больше нуля' },
                    })}
                  />
                  {formState.errors[s] && <span className="text-xs text-red-600">{formState.errors[s]?.message}</span>}
                </Field>
              ))}
            </div>
          </div>

          <div>
            <h3 className="mb-2 text-sm font-medium text-neutral-700 dark:text-neutral-300">Движение</h3>
            <div className="grid grid-cols-2 gap-3 sm:w-1/4">
              <Field label="Движение">
                <Input
                  type="number"
                  min={MOVEMENT_MIN}
                  max={MOVEMENT_MAX}
                  {...register('movement', {
                    required: true,
                    valueAsNumber: true,
                    min: { value: MOVEMENT_MIN, message: `От ${MOVEMENT_MIN} до ${MOVEMENT_MAX}` },
                    max: { value: MOVEMENT_MAX, message: `От ${MOVEMENT_MIN} до ${MOVEMENT_MAX}` },
                  })}
                />
                {formState.errors.movement && <span className="text-xs text-red-600">{formState.errors.movement?.message}</span>}
              </Field>
            </div>
          </div>

          <div>
            <h3 className="mb-2 text-sm font-medium text-neutral-700 dark:text-neutral-300">Характеристики</h3>
            <div className="grid grid-cols-3 gap-3 sm:grid-cols-7">
              {STATS.map((s) => (
                <Field key={s} label={s.toUpperCase()}>
                  <Input
                    type="number"
                    min={STAT_MIN}
                    max={STAT_MAX}
                    {...register(s, {
                      required: true,
                      valueAsNumber: true,
                      min: { value: STAT_MIN, message: `От ${STAT_MIN} до ${STAT_MAX}` },
                      max: { value: STAT_MAX, message: `От ${STAT_MIN} до ${STAT_MAX}` },
                    })}
                  />
                  {formState.errors[s] && <span className="text-xs text-red-600">{formState.errors[s]?.message}</span>}
                </Field>
              ))}
            </div>
          </div>

          {save.error && <ErrorText>{save.error instanceof ApiError ? save.error.message : 'Не удалось сохранить персонажа'}</ErrorText>}

          <div>
            <Button type="submit" disabled={formState.isSubmitting || save.isPending}>
              Сохранить
            </Button>
          </div>
        </form>
      </Card>
    </div>
  )
}
