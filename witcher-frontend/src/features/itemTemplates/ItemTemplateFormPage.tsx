import { useMutation, useQueryClient } from '@tanstack/react-query'
import { useForm } from 'react-hook-form'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { Button, Card, ErrorText, Field, Input, PageHeader, Select, Textarea } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { DAMAGE_TYPES, ITEM_TYPES, SKILLS, WEAPON_KINDS, type ItemTemplateFormValues } from '../../types/api'
import { itemTemplatesApi } from './api'

export function ItemTemplateFormPage() {
  const { gameId } = useParams<{ gameId: string }>()
  const gameIdNum = Number(gameId)
  const navigate = useNavigate()
  const queryClient = useQueryClient()

  const { register, handleSubmit, formState, watch } = useForm<ItemTemplateFormValues>({
    defaultValues: {
      name: '',
      description: '',
      itemType: 'Weapon',
      weight: 1,
      cost: 0,
      attackSkill: 'Melee',
      isMultiAttack: false,
      damageDiceCount: 1,
      attackModifier: 0,
      damageModifier: 0,
      damageType: 'Slashing',
      weaponKind: 'Melee',
      attackRange: 1,
      handsRequired: 1,
      durability: 10,
    },
  })

  const itemType = watch('itemType')

  const save = useMutation({
    mutationFn: (values: ItemTemplateFormValues) => itemTemplatesApi.create(gameIdNum, values),
    onSuccess: async (result) => {
      await queryClient.invalidateQueries({ queryKey: ['item-templates'] })
      navigate(`/games/${gameIdNum}/item-templates/${result.id}`)
    },
  })

  const onSubmit = handleSubmit((values) =>
    save.mutate({
      ...values,
      attackModifier: Number.isNaN(Number(values.attackModifier)) ? 0 : Number(values.attackModifier),
    }),
  )

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title="Новый шаблон предмета"
        actions={
          <Link to={`/games/${gameIdNum}/item-templates`}>
            <Button variant="secondary">К шаблонам предметов</Button>
          </Link>
        }
      />
      <Card>
        <form onSubmit={onSubmit} className="flex flex-col gap-4">
          <Field label="Название">
            <Input {...register('name', { required: true })} />
          </Field>
          <Field label="Описание">
            <Textarea rows={2} {...register('description')} />
          </Field>

          <div className="grid grid-cols-2 gap-3 sm:grid-cols-3">
            <Field label="Тип">
              <Select {...register('itemType', { required: true })}>
                {ITEM_TYPES.map((t) => (
                  <option key={t} value={t}>
                    {t}
                  </option>
                ))}
              </Select>
            </Field>
            <Field label="Вес">
              <Input type="number" min={0} {...register('weight', { required: true, valueAsNumber: true, min: 0 })} />
            </Field>
            <Field label="Стоимость">
              <Input type="number" min={0} {...register('cost', { required: true, valueAsNumber: true, min: 0 })} />
            </Field>
          </div>

          {itemType === 'Weapon' && (
            <div>
              <h3 className="mb-2 text-sm font-medium text-neutral-700 dark:text-neutral-300">Параметры оружия</h3>
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
                <Field label="Кубиков д6 урона">
                  <Input type="number" min={1} {...register('damageDiceCount', { required: true, valueAsNumber: true, min: 1 })} />
                </Field>
                <Field label="Модификатор атаки">
                  <Input type="number" placeholder="0" {...register('attackModifier', { valueAsNumber: true })} />
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
                <Field label="Вид">
                  <Select {...register('weaponKind', { required: true })}>
                    {WEAPON_KINDS.map((k) => (
                      <option key={k} value={k}>
                        {k}
                      </option>
                    ))}
                  </Select>
                </Field>
                <Field label="Дальность атаки">
                  <Input type="number" min={1} {...register('attackRange', { required: true, valueAsNumber: true, min: 1 })} />
                </Field>
                <Field label="Рук для использования">
                  <Input type="number" min={1} max={2} {...register('handsRequired', { required: true, valueAsNumber: true, min: 1, max: 2 })} />
                </Field>
                <Field label="Прочность">
                  <Input type="number" min={1} {...register('durability', { required: true, valueAsNumber: true, min: 1 })} />
                </Field>
              </div>
              <label className="mt-3 flex items-center gap-2 text-sm">
                <input type="checkbox" className="h-4 w-4" {...register('isMultiAttack')} />
                Возможна мультиатака
              </label>
            </div>
          )}

          {save.error && (
            <ErrorText>{save.error instanceof ApiError ? save.error.message : 'Не удалось сохранить шаблон предмета'}</ErrorText>
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
