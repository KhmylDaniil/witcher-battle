import { useEffect } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useForm } from 'react-hook-form'
import { useNavigate, useParams } from 'react-router-dom'
import { Button, Card, ErrorText, Field, Input, PageHeader, Select, Spinner, Textarea } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { CREATURE_TYPES, type CreatureTemplateFormValues } from '../../types/api'
import { bodyTemplatesApi } from '../bodyTemplates/api'
import { creatureTemplatesApi } from './api'

const STATS = ['hp', 'sta', 'int', 'ref', 'dex', 'body', 'emp', 'cra', 'will', 'speed', 'luck'] as const

export function CreatureTemplateFormPage() {
  const { gameId } = useParams<{ gameId: string }>()
  const gameIdNum = Number(gameId)
  const navigate = useNavigate()
  const queryClient = useQueryClient()

  const bodyTemplates = useQuery({
    queryKey: ['body-templates', { gameId: gameIdNum }],
    queryFn: () => bodyTemplatesApi.list({ gameId: gameIdNum }),
  })

  const { register, handleSubmit, formState, setValue } = useForm<CreatureTemplateFormValues>({
    defaultValues: {
      bodyTemplateId: 0,
      creatureType: 'Beast',
      name: '',
      description: '',
      hp: 10,
      sta: 10,
      int: 1,
      ref: 1,
      dex: 1,
      body: 1,
      emp: 1,
      cra: 1,
      will: 1,
      speed: 0,
      luck: 0,
    },
  })

  // Выбранное по умолчанию значение <select> должно совпадать с одним из уже загруженных вариантов —
  // иначе браузер оставляет выбор пустым (не откатывается на первый option), даже если он единственный.
  useEffect(() => {
    if (bodyTemplates.data && bodyTemplates.data.length > 0) {
      setValue('bodyTemplateId', bodyTemplates.data[0].id)
    }
  }, [bodyTemplates.data, setValue])

  const save = useMutation({
    mutationFn: (values: CreatureTemplateFormValues) => creatureTemplatesApi.create(gameIdNum, values),
    onSuccess: async (result) => {
      await queryClient.invalidateQueries({ queryKey: ['creature-templates'] })
      navigate(`/games/${gameIdNum}/creature-templates/${result.id}`)
    },
  })

  const onSubmit = handleSubmit((values) =>
    save.mutate({ ...values, bodyTemplateId: Number(values.bodyTemplateId) }),
  )

  if (bodyTemplates.isLoading) return <Spinner />

  return (
    <div className="flex flex-col gap-4">
      <PageHeader title="Новый шаблон существа" />
      <Card>
        {bodyTemplates.data && bodyTemplates.data.length === 0 ? (
          <p className="text-sm text-neutral-500">
            Сначала создайте хотя бы один шаблон тела — шаблон существа наследует от него части тела.
          </p>
        ) : (
          <form onSubmit={onSubmit} className="flex flex-col gap-4">
            <Field label="Название">
              <Input {...register('name', { required: true })} />
            </Field>
            <Field label="Описание">
              <Textarea rows={2} {...register('description')} />
            </Field>

            <div className="grid grid-cols-2 gap-3 sm:grid-cols-3">
              <Field label="Шаблон тела">
                <Select {...register('bodyTemplateId', { required: true, valueAsNumber: true })}>
                  {bodyTemplates.data?.map((bt) => (
                    <option key={bt.id} value={bt.id}>
                      {bt.name}
                    </option>
                  ))}
                </Select>
              </Field>
              <Field label="Тип существа">
                <Select {...register('creatureType', { required: true })}>
                  {CREATURE_TYPES.map((t) => (
                    <option key={t} value={t}>
                      {t}
                    </option>
                  ))}
                </Select>
              </Field>
            </div>

            <div>
              <h3 className="mb-2 text-sm font-medium text-neutral-700 dark:text-neutral-300">Характеристики</h3>
              <div className="grid grid-cols-4 gap-3 sm:grid-cols-6">
                {STATS.map((s) => (
                  <Field key={s} label={s.toUpperCase()}>
                    <Input type="number" {...register(s, { required: true, valueAsNumber: true, min: s === 'speed' || s === 'luck' ? 0 : 1 })} />
                  </Field>
                ))}
              </div>
            </div>

            {save.error && (
              <ErrorText>{save.error instanceof ApiError ? save.error.message : 'Не удалось сохранить шаблон существа'}</ErrorText>
            )}

            <div>
              <Button type="submit" disabled={formState.isSubmitting || save.isPending}>
                Сохранить
              </Button>
            </div>
          </form>
        )}
      </Card>
    </div>
  )
}
