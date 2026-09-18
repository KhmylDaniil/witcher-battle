import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { Button, Card, ConfirmButton, ErrorText, Field, Input, PageHeader, Select, Spinner, Textarea } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import {
  CONDITIONS,
  DAMAGE_TYPES,
  SKILLS,
  WEAPON_KINDS,
  type Condition,
  type ItemTemplateFormValues,
} from '../../types/api'
import { itemTemplatesApi } from './api'

const EMPTY_CONDITION: { condition: Condition; applyChance: number } = { condition: 'Bleed', applyChance: 25 }

export function ItemTemplateDetailsPage() {
  const { gameId, itemTemplateId } = useParams<{ gameId: string; itemTemplateId: string }>()
  const id = Number(itemTemplateId)
  const navigate = useNavigate()
  const queryClient = useQueryClient()

  const itemTemplate = useQuery({ queryKey: ['item-templates', id], queryFn: () => itemTemplatesApi.get(id) })
  const invalidate = () =>
    Promise.all([
      queryClient.invalidateQueries({ queryKey: ['item-templates', id] }),
      queryClient.invalidateQueries({ queryKey: ['item-templates', { gameId: Number(gameId) }] }),
    ])

  const [editing, setEditing] = useState(false)
  const [values, setValues] = useState<ItemTemplateFormValues | null>(null)
  const update = useMutation({
    mutationFn: () => itemTemplatesApi.update(id, values!),
    onSuccess: async () => {
      await invalidate()
      setEditing(false)
    },
  })

  const remove = useMutation({
    mutationFn: () => itemTemplatesApi.remove(id),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ['item-templates'] })
      navigate(`/games/${gameId}/item-templates`)
    },
  })

  const [editingConditionId, setEditingConditionId] = useState<number | null>(null)
  const [editCondition, setEditCondition] = useState<{ condition: Condition; applyChance: number }>(EMPTY_CONDITION)
  const updateCondition = useMutation({
    mutationFn: (conditionId: number) => itemTemplatesApi.updateCondition(id, conditionId, editCondition.condition, editCondition.applyChance),
    onSuccess: () => {
      invalidate()
      setEditingConditionId(null)
    },
  })
  const removeCondition = useMutation({
    mutationFn: (conditionId: number) => itemTemplatesApi.removeCondition(id, conditionId),
    onSuccess: invalidate,
  })
  const [showAddCondition, setShowAddCondition] = useState(false)
  const [newCondition, setNewCondition] = useState<{ condition: Condition; applyChance: number }>(EMPTY_CONDITION)
  const addCondition = useMutation({
    mutationFn: () => itemTemplatesApi.addCondition(id, newCondition.condition, newCondition.applyChance),
    onSuccess: () => {
      invalidate()
      setNewCondition(EMPTY_CONDITION)
      setShowAddCondition(false)
    },
  })

  if (itemTemplate.isLoading) return <Spinner />
  if (!itemTemplate.data) return null
  const it = itemTemplate.data
  const isWeapon = it.itemType === 'Weapon'

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title={it.name}
        actions={
          <>
            <Link to={`/games/${gameId}/item-templates`}>
              <Button variant="secondary">К шаблонам предметов</Button>
            </Link>
            {!editing && (
              <Button
                variant="secondary"
                onClick={() => {
                  setValues({
                    name: it.name,
                    description: it.description ?? '',
                    itemType: it.itemType,
                    weight: it.weight,
                    cost: it.cost,
                    attackSkill: it.attackSkill ?? undefined,
                    attacksPerTurn: it.attacksPerTurn ?? undefined,
                    damageDiceCount: it.damageDiceCount ?? undefined,
                    damageModifier: it.damageModifier ?? undefined,
                    damageType: it.damageType ?? undefined,
                    weaponKind: it.weaponKind ?? undefined,
                    attackRange: it.attackRange ?? undefined,
                    handsRequired: it.handsRequired ?? undefined,
                    durability: it.durability ?? undefined,
                  })
                  setEditing(true)
                }}
              >
                Изменить
              </Button>
            )}
            <ConfirmButton confirmMessage={`Удалить шаблон предмета "${it.name}"?`} onConfirm={() => remove.mutate()} disabled={remove.isPending}>
              Удалить
            </ConfirmButton>
          </>
        }
      />

      {editing && values ? (
        <Card>
          <form
            onSubmit={(e) => {
              e.preventDefault()
              update.mutate()
            }}
            className="flex flex-col gap-4"
          >
            <Field label="Название">
              <Input value={values.name} onChange={(e) => setValues({ ...values, name: e.target.value })} required autoFocus />
            </Field>
            <Field label="Описание">
              <Textarea rows={2} value={values.description} onChange={(e) => setValues({ ...values, description: e.target.value })} />
            </Field>
            <div className="grid grid-cols-2 gap-3 sm:grid-cols-3">
              <Field label="Вес">
                <Input
                  type="number"
                  step="0.1"
                  min={0}
                  value={values.weight}
                  onChange={(e) => setValues({ ...values, weight: Number(e.target.value) })}
                />
              </Field>
              <Field label="Стоимость">
                <Input type="number" min={0} value={values.cost} onChange={(e) => setValues({ ...values, cost: Number(e.target.value) })} />
              </Field>
            </div>

            {isWeapon && (
              <div>
                <h3 className="mb-2 text-sm font-medium text-neutral-700 dark:text-neutral-300">Параметры оружия</h3>
                <div className="grid grid-cols-2 gap-3 sm:grid-cols-3">
                  <Field label="Навык атаки">
                    <Select
                      value={values.attackSkill}
                      onChange={(e) => setValues({ ...values, attackSkill: e.target.value as ItemTemplateFormValues['attackSkill'] })}
                    >
                      {SKILLS.map((s) => (
                        <option key={s} value={s}>
                          {s}
                        </option>
                      ))}
                    </Select>
                  </Field>
                  <Field label="Атак в ход">
                    <Input
                      type="number"
                      min={1}
                      value={values.attacksPerTurn}
                      onChange={(e) => setValues({ ...values, attacksPerTurn: Number(e.target.value) })}
                    />
                  </Field>
                  <Field label="Кубиков д6 урона">
                    <Input
                      type="number"
                      min={1}
                      value={values.damageDiceCount}
                      onChange={(e) => setValues({ ...values, damageDiceCount: Number(e.target.value) })}
                    />
                  </Field>
                  <Field label="Модификатор урона">
                    <Input
                      type="number"
                      value={values.damageModifier}
                      onChange={(e) => setValues({ ...values, damageModifier: Number(e.target.value) })}
                    />
                  </Field>
                  <Field label="Тип урона">
                    <Select
                      value={values.damageType}
                      onChange={(e) => setValues({ ...values, damageType: e.target.value as ItemTemplateFormValues['damageType'] })}
                    >
                      {DAMAGE_TYPES.map((t) => (
                        <option key={t} value={t}>
                          {t}
                        </option>
                      ))}
                    </Select>
                  </Field>
                  <Field label="Вид">
                    <Select
                      value={values.weaponKind}
                      onChange={(e) => setValues({ ...values, weaponKind: e.target.value as ItemTemplateFormValues['weaponKind'] })}
                    >
                      {WEAPON_KINDS.map((k) => (
                        <option key={k} value={k}>
                          {k}
                        </option>
                      ))}
                    </Select>
                  </Field>
                  <Field label="Дальность атаки">
                    <Input
                      type="number"
                      min={1}
                      value={values.attackRange}
                      onChange={(e) => setValues({ ...values, attackRange: Number(e.target.value) })}
                    />
                  </Field>
                  <Field label="Рук для использования">
                    <Input
                      type="number"
                      min={1}
                      max={2}
                      value={values.handsRequired}
                      onChange={(e) => setValues({ ...values, handsRequired: Number(e.target.value) })}
                    />
                  </Field>
                  <Field label="Прочность">
                    <Input
                      type="number"
                      min={1}
                      value={values.durability}
                      onChange={(e) => setValues({ ...values, durability: Number(e.target.value) })}
                    />
                  </Field>
                </div>
              </div>
            )}

            {update.error && <ErrorText>{update.error instanceof ApiError ? update.error.message : 'Не удалось сохранить'}</ErrorText>}

            <div className="flex gap-2">
              <Button type="submit" disabled={update.isPending}>
                Сохранить
              </Button>
              <Button type="button" variant="secondary" onClick={() => setEditing(false)}>
                Отмена
              </Button>
            </div>
          </form>
        </Card>
      ) : (
        <Card>
          <h2 className="mb-2 font-semibold">Параметры</h2>
          <div className="grid grid-cols-2 gap-3 text-sm sm:grid-cols-3">
            <div>
              <span className="text-neutral-400">Тип</span> <span className="font-medium">{it.itemType}</span>
            </div>
            <div>
              <span className="text-neutral-400">Вес</span> <span className="font-medium">{it.weight}</span>
            </div>
            <div>
              <span className="text-neutral-400">Стоимость</span> <span className="font-medium">{it.cost}</span>
            </div>
            {it.description && (
              <div className="col-span-2 sm:col-span-3">
                <span className="text-neutral-400">Описание</span> <span className="font-medium">{it.description}</span>
              </div>
            )}
            {isWeapon && (
              <>
                <div>
                  <span className="text-neutral-400">Навык атаки</span> <span className="font-medium">{it.attackSkill}</span>
                </div>
                <div>
                  <span className="text-neutral-400">Атак в ход</span> <span className="font-medium">{it.attacksPerTurn}</span>
                </div>
                <div>
                  <span className="text-neutral-400">Урон</span>{' '}
                  <span className="font-medium">
                    {it.damageDiceCount}д6+{it.damageModifier} {it.damageType}
                  </span>
                </div>
                <div>
                  <span className="text-neutral-400">Вид</span> <span className="font-medium">{it.weaponKind}</span>
                </div>
                <div>
                  <span className="text-neutral-400">Дальность атаки</span> <span className="font-medium">{it.attackRange}</span>
                </div>
                <div>
                  <span className="text-neutral-400">Рук для использования</span> <span className="font-medium">{it.handsRequired}</span>
                </div>
                <div>
                  <span className="text-neutral-400">Прочность</span> <span className="font-medium">{it.durability}</span>
                </div>
              </>
            )}
          </div>
        </Card>
      )}

      {isWeapon && (
        <Card>
          <div className="mb-3 flex items-center justify-between">
            <h2 className="font-semibold">Накладываемые эффекты</h2>
            {!showAddCondition && (
              <Button className="px-2 py-1 text-xs" onClick={() => setShowAddCondition(true)}>
                Добавить эффект
              </Button>
            )}
          </div>

          {showAddCondition && (
            <div className="mb-4 flex flex-wrap items-end gap-2 rounded-md border border-neutral-200 p-3 dark:border-neutral-800">
              <Field label="Эффект">
                <Select value={newCondition.condition} onChange={(e) => setNewCondition({ ...newCondition, condition: e.target.value as Condition })}>
                  {CONDITIONS.map((c) => (
                    <option key={c} value={c}>
                      {c}
                    </option>
                  ))}
                </Select>
              </Field>
              <Field label="Шанс, %">
                <Input
                  type="number"
                  min={1}
                  max={100}
                  className="w-24"
                  value={newCondition.applyChance}
                  onChange={(e) => setNewCondition({ ...newCondition, applyChance: Number(e.target.value) })}
                />
              </Field>
              <Button
                disabled={addCondition.isPending || newCondition.applyChance < 1 || newCondition.applyChance > 100}
                onClick={() => addCondition.mutate()}
              >
                Добавить
              </Button>
              <Button
                variant="secondary"
                onClick={() => {
                  setShowAddCondition(false)
                  setNewCondition(EMPTY_CONDITION)
                }}
              >
                Отмена
              </Button>
            </div>
          )}
          {addCondition.error && (
            <div className="mb-3">
              <ErrorText>{addCondition.error instanceof ApiError ? addCondition.error.message : 'Не удалось добавить эффект'}</ErrorText>
            </div>
          )}

          {it.appliedConditions.length === 0 && <p className="text-sm text-neutral-500">Эффектов пока нет.</p>}
          <table className="w-full max-w-md text-left text-sm">
            <tbody>
              {it.appliedConditions.map((c) => (
                <tr key={c.id} className="border-b border-neutral-100 dark:border-neutral-900">
                  {editingConditionId === c.id ? (
                    <td colSpan={3} className="py-2">
                      <div className="flex flex-wrap items-end gap-2">
                        <Select
                          value={editCondition.condition}
                          onChange={(e) => setEditCondition({ ...editCondition, condition: e.target.value as Condition })}
                        >
                          {CONDITIONS.map((cond) => (
                            <option key={cond} value={cond}>
                              {cond}
                            </option>
                          ))}
                        </Select>
                        <Input
                          type="number"
                          min={1}
                          max={100}
                          className="w-24"
                          value={editCondition.applyChance}
                          onChange={(e) => setEditCondition({ ...editCondition, applyChance: Number(e.target.value) })}
                        />
                        <Button
                          className="px-2 py-1"
                          disabled={updateCondition.isPending || editCondition.applyChance < 1 || editCondition.applyChance > 100}
                          onClick={() => updateCondition.mutate(c.id)}
                        >
                          OK
                        </Button>
                        <Button variant="secondary" className="px-2 py-1" onClick={() => setEditingConditionId(null)}>
                          Отмена
                        </Button>
                      </div>
                    </td>
                  ) : (
                    <>
                      <td className="py-2 pr-3">{c.condition}</td>
                      <td className="py-2 pr-3">{c.applyChance}%</td>
                      <td className="py-2">
                        <div className="flex gap-3">
                          <button
                            className="text-violet-600 hover:underline"
                            onClick={() => {
                              setEditingConditionId(c.id)
                              setEditCondition({ condition: c.condition, applyChance: c.applyChance })
                            }}
                          >
                            Изменить
                          </button>
                          <button
                            className="text-red-600 hover:underline"
                            disabled={removeCondition.isPending}
                            onClick={() => removeCondition.mutate(c.id)}
                          >
                            Удалить
                          </button>
                        </div>
                      </td>
                    </>
                  )}
                </tr>
              ))}
            </tbody>
          </table>
        </Card>
      )}
    </div>
  )
}
