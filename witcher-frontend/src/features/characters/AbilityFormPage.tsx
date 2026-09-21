import { useMutation, useQueryClient } from '@tanstack/react-query'
import { useForm } from 'react-hook-form'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { Button, Card, ErrorText, Field, Input, PageHeader, Select } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { DAMAGE_TYPES, SKILLS, type AbilityFormValues } from '../../types/api'
import { charactersApi } from './api'

export function AbilityFormPage() {
  const { characterId } = useParams<{ characterId: string }>()
  const characterIdNum = Number(characterId)
  const navigate = useNavigate()
  const queryClient = useQueryClient()

  const { register, handleSubmit, formState } = useForm<AbilityFormValues>({
    defaultValues: {
      name: '',
      attackSkill: 'Melee',
      attacksPerTurn: 1,
      damageDiceCount: 1,
      attackModifier: 0,
      damageModifier: 0,
      damageType: 'Slashing',
    },
  })

  const save = useMutation({
    mutationFn: (values: AbilityFormValues) => charactersApi.addAbility(characterIdNum, values),
    onSuccess: async (result) => {
      await queryClient.invalidateQueries({ queryKey: ['characters', characterIdNum] })
      const created = result.abilities[result.abilities.length - 1]
      navigate(`/characters/${characterIdNum}/abilities/${created.id}`)
    },
  })

  const onSubmit = handleSubmit((values) =>
    save.mutate({
      ...values,
      attacksPerTurn: Number(values.attacksPerTurn),
      damageDiceCount: Number(values.damageDiceCount),
      attackModifier: Number(values.attackModifier),
      damageModifier: Number(values.damageModifier),
    }),
  )

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title="Новая способность"
        actions={
          <Link to={`/characters/${characterIdNum}`}>
            <Button variant="secondary">К персонажу</Button>
          </Link>
        }
      />
      <Card>
        <form onSubmit={onSubmit} className="flex flex-col gap-4">
          <Field label="Название">
            <Input {...register('name', { required: true })} autoFocus />
          </Field>

          <div className="grid grid-cols-2 gap-3 sm:grid-cols-3">
            <Field label="Навык атаки">
              <Select {...register('attackSkill', { required: true })}>
                {SKILLS.map((s) => (
                  <option key={s} value={s}>
                    {s}
                  </option>
                ))}
              </Select>
            </Field>
            <Field label="Атак в ход">
              <Input type="number" min={1} {...register('attacksPerTurn', { required: true, valueAsNumber: true, min: 1 })} />
            </Field>
            <Field label="Кубиков д6 урона">
              <Input type="number" min={1} {...register('damageDiceCount', { required: true, valueAsNumber: true, min: 1 })} />
            </Field>
            <Field label="Модификатор атаки">
              <Input type="number" {...register('attackModifier', { required: true, valueAsNumber: true })} />
            </Field>
            <Field label="Модификатор урона">
              <Input type="number" {...register('damageModifier', { required: true, valueAsNumber: true })} />
            </Field>
            <Field label="Тип урона">
              <Select {...register('damageType', { required: true })}>
                {DAMAGE_TYPES.map((t) => (
                  <option key={t} value={t}>
                    {t}
                  </option>
                ))}
              </Select>
            </Field>
          </div>

          {save.error && (
            <ErrorText>{save.error instanceof ApiError ? save.error.message : 'Не удалось сохранить способность'}</ErrorText>
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
