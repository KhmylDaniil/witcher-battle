import { useEffect } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useForm } from 'react-hook-form'
import { useNavigate, useParams } from 'react-router-dom'
import { Button, Card, ErrorText, Field, Input, PageHeader, Select, Spinner, Textarea } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { useGameId } from '../../routes/GameLayout'
import { referenceApi } from '../reference/api'
import { CREATURE_TYPES, type CreatureTemplateFormValues } from '../../types/api'
import { creatureTemplatesApi } from './api'

const NUMBER_FIELDS = ['hp', 'sta', 'int', 'ref', 'dex', 'body', 'emp', 'cra', 'will', 'speed', 'luck'] as const

export function CreatureTemplateFormPage() {
  const gameId = useGameId()
  const { templateId } = useParams<{ templateId: string }>()
  const isEdit = !!templateId
  const navigate = useNavigate()
  const queryClient = useQueryClient()

  const bodyTemplates = useQuery({ queryKey: ['reference', gameId, 'body-templates'], queryFn: () => referenceApi.bodyTemplates(gameId) })
  const abilities = useQuery({ queryKey: ['reference', gameId, 'abilities'], queryFn: () => referenceApi.abilities(gameId) })
  const existing = useQuery({
    queryKey: ['creature-templates', gameId, templateId],
    queryFn: () => creatureTemplatesApi.get(gameId, templateId!),
    enabled: isEdit,
  })

  const { register, handleSubmit, reset, formState } = useForm<CreatureTemplateFormValues>({
    defaultValues: {
      creatureType: 'Human',
      hp: 10,
      sta: 1,
      int: 1,
      ref: 1,
      dex: 1,
      body: 1,
      emp: 1,
      cra: 1,
      will: 1,
      speed: 0,
      luck: 0,
      abilities: [],
    },
  })

  useEffect(() => {
    if (existing.data) {
      reset({
        bodyTemplateId: existing.data.bodyTemplateId,
        creatureType: existing.data.creatureType,
        name: existing.data.name,
        description: existing.data.description,
        hp: existing.data.hp,
        sta: existing.data.sta,
        int: existing.data.int,
        ref: existing.data.ref,
        dex: existing.data.dex,
        body: existing.data.body,
        emp: existing.data.emp,
        cra: existing.data.cra,
        will: existing.data.will,
        speed: existing.data.speed,
        luck: existing.data.luck,
        abilities: existing.data.abilities.map((a) => a.id),
      })
    }
  }, [existing.data, reset])

  const save = useMutation({
    mutationFn: async (values: CreatureTemplateFormValues): Promise<{ id: string }> => {
      const payload: CreatureTemplateFormValues = {
        ...values,
        ...Object.fromEntries(NUMBER_FIELDS.map((f) => [f, Number(values[f])])),
      }
      if (isEdit) {
        await creatureTemplatesApi.update(gameId, templateId!, payload)
        return { id: templateId! }
      }
      return creatureTemplatesApi.create(gameId, payload)
    },
    onSuccess: async (result) => {
      await queryClient.invalidateQueries({ queryKey: ['creature-templates', gameId] })
      navigate(`/games/${gameId}/creature-templates/${result.id}`)
    },
  })

  const onSubmit = handleSubmit((values) => save.mutate(values))

  if (isEdit && existing.isLoading) return <Spinner />
  if (bodyTemplates.isLoading || abilities.isLoading) return <Spinner />

  return (
    <div className="flex flex-col gap-4">
      <PageHeader title={isEdit ? 'Изменить шаблон существа' : 'Новый шаблон существа'} />
      <Card>
        <form onSubmit={onSubmit} className="flex flex-col gap-4">
          <div className="grid gap-3 sm:grid-cols-2">
            <Field label="Название">
              <Input {...register('name', { required: true })} />
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
            <Field label="Шаблон тела">
              <Select {...register('bodyTemplateId', { required: true })}>
                <option value="">— выберите —</option>
                {bodyTemplates.data?.map((bt) => (
                  <option key={bt.id} value={bt.id}>
                    {bt.name}
                  </option>
                ))}
              </Select>
            </Field>
          </div>

          <Field label="Описание">
            <Textarea rows={3} {...register('description')} />
          </Field>

          <div>
            <h3 className="mb-2 text-sm font-medium text-neutral-700 dark:text-neutral-300">Характеристики</h3>
            <div className="grid grid-cols-3 gap-3 sm:grid-cols-6">
              {NUMBER_FIELDS.map((f) => (
                <Field key={f} label={f.toUpperCase()}>
                  <Input type="number" {...register(f, { required: true, valueAsNumber: true })} />
                </Field>
              ))}
            </div>
          </div>

          <Field label="Способности (Ctrl/Cmd + клик — выбрать несколько)">
            <Select multiple {...register('abilities')} size={Math.min(6, Math.max(3, abilities.data?.length ?? 3))}>
              {abilities.data?.map((a) => (
                <option key={a.id} value={a.id}>
                  {a.name}
                </option>
              ))}
            </Select>
          </Field>

          {save.error && (
            <ErrorText>{save.error instanceof ApiError ? save.error.message : 'Не удалось сохранить шаблон'}</ErrorText>
          )}

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
