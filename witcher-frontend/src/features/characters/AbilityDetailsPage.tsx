import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { Button, Card, ConfirmButton, ErrorText, Field, Input, PageHeader, Select, Spinner } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { getAvailableOptions } from '../../lib/options'
import { CONDITIONS, DAMAGE_TYPES, SKILLS, type AbilityFormValues, type Condition, type Skill } from '../../types/api'
import { charactersApi } from './api'

const EMPTY_CONDITION: { condition: Condition; applyChance: number } = { condition: 'Bleed', applyChance: 25 }

export function AbilityDetailsPage() {
  const { characterId, abilityId } = useParams<{ characterId: string; abilityId: string }>()
  const characterIdNum = Number(characterId)
  const abilityIdNum = Number(abilityId)
  const navigate = useNavigate()
  const queryClient = useQueryClient()

  const character = useQuery({
    queryKey: ['characters', characterIdNum],
    queryFn: () => charactersApi.get(characterIdNum),
  })
  const invalidate = () => queryClient.invalidateQueries({ queryKey: ['characters', characterIdNum] })

  const [editingAbility, setEditingAbility] = useState(false)
  const [abilityValues, setAbilityValues] = useState<AbilityFormValues | null>(null)
  const updateAbility = useMutation({
    mutationFn: () => charactersApi.updateAbility(characterIdNum, abilityIdNum, abilityValues!),
    onSuccess: async () => {
      await invalidate()
      setEditingAbility(false)
    },
  })

  const removeAbility = useMutation({
    mutationFn: () => charactersApi.removeAbility(characterIdNum, abilityIdNum),
    onSuccess: async () => {
      await invalidate()
      navigate(`/characters/${characterIdNum}`)
    },
  })

  const [editingConditionId, setEditingConditionId] = useState<number | null>(null)
  const [editCondition, setEditCondition] = useState<{ condition: Condition; applyChance: number }>(EMPTY_CONDITION)
  const updateCondition = useMutation({
    mutationFn: (conditionId: number) =>
      charactersApi.updateCondition(characterIdNum, abilityIdNum, conditionId, editCondition.condition, editCondition.applyChance),
    onSuccess: () => {
      invalidate()
      setEditingConditionId(null)
    },
  })
  const removeCondition = useMutation({
    mutationFn: (conditionId: number) => charactersApi.removeCondition(characterIdNum, abilityIdNum, conditionId),
    onSuccess: invalidate,
  })
  const [showAddCondition, setShowAddCondition] = useState(false)
  const [newCondition, setNewCondition] = useState<{ condition: Condition; applyChance: number }>(EMPTY_CONDITION)
  const addCondition = useMutation({
    mutationFn: () => charactersApi.addCondition(characterIdNum, abilityIdNum, newCondition.condition, newCondition.applyChance),
    onSuccess: () => {
      invalidate()
      setNewCondition(EMPTY_CONDITION)
      setShowAddCondition(false)
    },
  })

  const removeDefensiveSkill = useMutation({
    mutationFn: (defensiveSkillId: number) => charactersApi.removeDefensiveSkill(characterIdNum, abilityIdNum, defensiveSkillId),
    onSuccess: invalidate,
  })
  const [newDefensiveSkill, setNewDefensiveSkill] = useState<Skill>('Dodge')
  const addDefensiveSkill = useMutation({
    mutationFn: () => charactersApi.addDefensiveSkill(characterIdNum, abilityIdNum, newDefensiveSkill),
    onSuccess: () => {
      invalidate()
      setNewDefensiveSkill('Dodge')
    },
  })

  if (character.isLoading) return <Spinner />
  const c = character.data
  const ability = c?.abilities.find((a) => a.id === abilityIdNum)
  if (!c || !ability) return null

  const usedDefensiveSkills = new Set(ability.defensiveSkills.map((d) => d.skill))
  const availableDefensiveSkills = getAvailableOptions(SKILLS, usedDefensiveSkills)

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title={ability.name}
        actions={
          <>
            <Link to={`/characters/${characterIdNum}`}>
              <Button variant="secondary">К персонажу</Button>
            </Link>
            {!editingAbility && (
              <Button
                variant="secondary"
                onClick={() => {
                  setAbilityValues({
                    name: ability.name,
                    attackSkill: ability.attackSkill,
                    attacksPerTurn: ability.attacksPerTurn,
                    damageDiceCount: ability.damageDiceCount,
                    damageModifier: ability.damageModifier,
                    damageType: ability.damageType,
                  })
                  setEditingAbility(true)
                }}
              >
                Изменить
              </Button>
            )}
            <ConfirmButton
              confirmMessage={`Удалить способность "${ability.name}"?`}
              onConfirm={() => removeAbility.mutate()}
              disabled={removeAbility.isPending}
            >
              Удалить
            </ConfirmButton>
          </>
        }
      />

      {editingAbility && abilityValues ? (
        <Card>
          <form
            onSubmit={(e) => {
              e.preventDefault()
              updateAbility.mutate()
            }}
            className="flex flex-col gap-4"
          >
            <Field label="Название">
              <Input
                value={abilityValues.name}
                onChange={(e) => setAbilityValues({ ...abilityValues, name: e.target.value })}
                required
                autoFocus
              />
            </Field>
            <div className="grid grid-cols-2 gap-3 sm:grid-cols-3">
              <Field label="Навык атаки">
                <Select
                  value={abilityValues.attackSkill}
                  onChange={(e) => setAbilityValues({ ...abilityValues, attackSkill: e.target.value as Skill })}
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
                  value={abilityValues.attacksPerTurn}
                  onChange={(e) => setAbilityValues({ ...abilityValues, attacksPerTurn: Number(e.target.value) })}
                />
              </Field>
              <Field label="Кубиков д6 урона">
                <Input
                  type="number"
                  min={1}
                  value={abilityValues.damageDiceCount}
                  onChange={(e) => setAbilityValues({ ...abilityValues, damageDiceCount: Number(e.target.value) })}
                />
              </Field>
              <Field label="Модификатор урона">
                <Input
                  type="number"
                  value={abilityValues.damageModifier}
                  onChange={(e) => setAbilityValues({ ...abilityValues, damageModifier: Number(e.target.value) })}
                />
              </Field>
              <Field label="Тип урона">
                <Select
                  value={abilityValues.damageType}
                  onChange={(e) => setAbilityValues({ ...abilityValues, damageType: e.target.value as AbilityFormValues['damageType'] })}
                >
                  {DAMAGE_TYPES.map((t) => (
                    <option key={t} value={t}>
                      {t}
                    </option>
                  ))}
                </Select>
              </Field>
            </div>

            {updateAbility.error && (
              <ErrorText>{updateAbility.error instanceof ApiError ? updateAbility.error.message : 'Не удалось сохранить'}</ErrorText>
            )}

            <div className="flex gap-2">
              <Button type="submit" disabled={updateAbility.isPending}>
                Сохранить
              </Button>
              <Button type="button" variant="secondary" onClick={() => setEditingAbility(false)}>
                Отмена
              </Button>
            </div>
          </form>
        </Card>
      ) : (
        <Card>
          <h2 className="mb-2 font-semibold">Параметры атаки</h2>
          <div className="grid grid-cols-2 gap-3 text-sm sm:grid-cols-3">
            <div>
              <span className="text-neutral-400">Навык атаки</span> <span className="font-medium">{ability.attackSkill}</span>
            </div>
            <div>
              <span className="text-neutral-400">Атак в ход</span> <span className="font-medium">{ability.attacksPerTurn}</span>
            </div>
            <div>
              <span className="text-neutral-400">Урон</span>{' '}
              <span className="font-medium">
                {ability.damageDiceCount}д6+{ability.damageModifier} {ability.damageType}
              </span>
            </div>
          </div>
        </Card>
      )}

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

        {ability.appliedConditions.length === 0 && <p className="text-sm text-neutral-500">Эффектов пока нет.</p>}
        <table className="w-full max-w-md text-left text-sm">
          <tbody>
            {ability.appliedConditions.map((c) => (
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

      <Card>
        <h2 className="mb-3 font-semibold">Защитные навыки</h2>
        {ability.defensiveSkills.length === 0 && <p className="mb-3 text-sm text-neutral-500">Защитных навыков пока нет.</p>}
        <div className="mb-3 flex flex-wrap gap-2">
          {ability.defensiveSkills.map((d) => (
            <span
              key={d.id}
              className="flex items-center gap-2 rounded-full bg-neutral-100 px-3 py-1 text-sm dark:bg-neutral-800"
            >
              {d.skill}
              <button
                className="text-red-600 hover:underline"
                disabled={removeDefensiveSkill.isPending}
                onClick={() => removeDefensiveSkill.mutate(d.id)}
              >
                ×
              </button>
            </span>
          ))}
        </div>

        <div className="flex flex-wrap items-end gap-2">
          <Select value={newDefensiveSkill} onChange={(e) => setNewDefensiveSkill(e.target.value as Skill)}>
            {availableDefensiveSkills.length === 0 && <option value="">— все добавлены —</option>}
            {availableDefensiveSkills.map((s) => (
              <option key={s} value={s}>
                {s}
              </option>
            ))}
          </Select>
          <Button
            disabled={availableDefensiveSkills.length === 0 || addDefensiveSkill.isPending}
            onClick={() => addDefensiveSkill.mutate()}
          >
            Добавить навык
          </Button>
        </div>
        {addDefensiveSkill.error && (
          <div className="mt-2">
            <ErrorText>
              {addDefensiveSkill.error instanceof ApiError ? addDefensiveSkill.error.message : 'Не удалось добавить защитный навык'}
            </ErrorText>
          </div>
        )}
      </Card>
    </div>
  )
}
