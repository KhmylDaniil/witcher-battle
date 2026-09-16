import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { Button, Card, ConfirmButton, ErrorText, Field, Input, PageHeader, Select, Spinner, Textarea } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { getAvailableOptions } from '../../lib/options'
import {
  CREATURE_TYPES,
  DAMAGE_TYPES,
  DAMAGE_TYPE_MODIFIERS,
  SKILLS_BY_STAT,
  type CreatureTemplateFormValues,
  type DamageType,
  type DamageTypeModifierKind,
  type Skill,
} from '../../types/api'
import { creatureTemplatesApi } from './api'

const STATS = ['hp', 'sta', 'int', 'ref', 'dex', 'body', 'emp', 'cra', 'will', 'speed', 'luck'] as const

function toFormValues(ct: {
  bodyTemplateId: number
  creatureType: CreatureTemplateFormValues['creatureType']
  name: string
  description: string | null
  hp: number
  sta: number
  int: number
  ref: number
  dex: number
  body: number
  emp: number
  cra: number
  will: number
  speed: number
  luck: number
}): CreatureTemplateFormValues {
  return {
    bodyTemplateId: ct.bodyTemplateId,
    creatureType: ct.creatureType,
    name: ct.name,
    description: ct.description ?? '',
    hp: ct.hp,
    sta: ct.sta,
    int: ct.int,
    ref: ct.ref,
    dex: ct.dex,
    body: ct.body,
    emp: ct.emp,
    cra: ct.cra,
    will: ct.will,
    speed: ct.speed,
    luck: ct.luck,
  }
}

export function CreatureTemplateDetailsPage() {
  const { gameId, creatureTemplateId } = useParams<{ gameId: string; creatureTemplateId: string }>()
  const id = Number(creatureTemplateId)
  const gameIdNum = Number(gameId)
  const navigate = useNavigate()
  const queryClient = useQueryClient()

  const creatureTemplate = useQuery({ queryKey: ['creature-templates', id], queryFn: () => creatureTemplatesApi.get(id) })

  const remove = useMutation({
    mutationFn: () => creatureTemplatesApi.remove(id),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ['creature-templates'] })
      navigate(`/games/${gameId}`)
    },
  })

  const [editingTemplate, setEditingTemplate] = useState(false)
  const [templateValues, setTemplateValues] = useState<CreatureTemplateFormValues | null>(null)
  const updateTemplate = useMutation({
    mutationFn: () => creatureTemplatesApi.update(id, templateValues!),
    onSuccess: async () => {
      // Инвалидируем и детальную карточку, и список на экране игры — иначе имя/тип/HP там
      // обновятся только когда react-query сам решит, что список устарел (staleTime).
      await Promise.all([
        queryClient.invalidateQueries({ queryKey: ['creature-templates', id] }),
        queryClient.invalidateQueries({ queryKey: ['creature-templates', { gameId: gameIdNum }] }),
      ])
      setEditingTemplate(false)
    },
  })

  const [editingPartId, setEditingPartId] = useState<number | null>(null)
  const [armorValue, setArmorValue] = useState(0)
  const updateArmor = useMutation({
    mutationFn: (partId: number) => creatureTemplatesApi.updatePartArmor(id, partId, armorValue),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['creature-templates', id] })
      setEditingPartId(null)
    },
  })

  const [editingSkill, setEditingSkill] = useState<Skill | null>(null)
  const [editSkillValue, setEditSkillValue] = useState(1)
  const upsertSkill = useMutation({
    mutationFn: ({ skill, value }: { skill: Skill; value: number }) => creatureTemplatesApi.upsertSkill(id, skill, value),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['creature-templates', id] })
      setEditingSkill(null)
    },
  })
  const deleteSkill = useMutation({
    mutationFn: (skill: Skill) => creatureTemplatesApi.deleteSkill(id, skill),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['creature-templates', id] }),
  })
  const [newSkillStat, setNewSkillStat] = useState<string>('Int')
  const [newSkill, setNewSkill] = useState<Skill>('Awareness')
  const [newSkillValue, setNewSkillValue] = useState(1)

  const [newDamageType, setNewDamageType] = useState<DamageType>('Slashing')
  const [newModifier, setNewModifier] = useState<DamageTypeModifierKind>('Vulnerability')
  const setDamageTypeModifier = useMutation({
    mutationFn: () => creatureTemplatesApi.setDamageTypeModifier(id, newDamageType, newModifier),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['creature-templates', id] }),
  })
  const removeDamageTypeModifier = useMutation({
    mutationFn: (damageType: DamageType) => creatureTemplatesApi.removeDamageTypeModifier(id, damageType),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['creature-templates', id] }),
  })

  const removeAbility = useMutation({
    mutationFn: (abilityId: number) => creatureTemplatesApi.removeAbility(id, abilityId),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['creature-templates', id] }),
  })

  if (creatureTemplate.isLoading) return <Spinner />
  if (!creatureTemplate.data) return null
  const ct = creatureTemplate.data
  const sortedParts = [...ct.parts].sort((a, b) => a.minToHit - b.minToHit)

  const usedSkills = new Set(Object.keys(ct.skills) as Skill[])
  const availableInStat = getAvailableOptions(SKILLS_BY_STAT[newSkillStat], usedSkills)
  const usedDamageTypes = new Set(Object.keys(ct.damageTypeModifiers) as DamageType[])
  const availableDamageTypes = getAvailableOptions(DAMAGE_TYPES, usedDamageTypes)

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title={`${ct.name} (${ct.creatureType})`}
        actions={
          <>
            <Link to={`/games/${gameId}`}>
              <Button variant="secondary">К игре</Button>
            </Link>
            {!editingTemplate && (
              <Button
                variant="secondary"
                onClick={() => {
                  setTemplateValues(toFormValues(ct))
                  setEditingTemplate(true)
                }}
              >
                Изменить
              </Button>
            )}
            <ConfirmButton
              confirmMessage={`Удалить шаблон существа "${ct.name}"?`}
              onConfirm={() => remove.mutate()}
              disabled={remove.isPending}
            >
              Удалить
            </ConfirmButton>
          </>
        }
      />

      {editingTemplate && templateValues ? (
        <Card>
          <form
            onSubmit={(e) => {
              e.preventDefault()
              updateTemplate.mutate()
            }}
            className="flex flex-col gap-4"
          >
            <Field label="Название">
              <Input
                value={templateValues.name}
                onChange={(e) => setTemplateValues({ ...templateValues, name: e.target.value })}
                required
                autoFocus
              />
            </Field>
            <Field label="Описание">
              <Textarea
                rows={2}
                value={templateValues.description}
                onChange={(e) => setTemplateValues({ ...templateValues, description: e.target.value })}
              />
            </Field>
            <Field label="Тип существа">
              <Select
                value={templateValues.creatureType}
                onChange={(e) =>
                  setTemplateValues({ ...templateValues, creatureType: e.target.value as CreatureTemplateFormValues['creatureType'] })
                }
              >
                {CREATURE_TYPES.map((t) => (
                  <option key={t} value={t}>
                    {t}
                  </option>
                ))}
              </Select>
            </Field>

            <div>
              <h3 className="mb-2 text-sm font-medium text-neutral-700 dark:text-neutral-300">Характеристики</h3>
              <div className="grid grid-cols-4 gap-3 sm:grid-cols-6">
                {STATS.map((s) => (
                  <Field key={s} label={s.toUpperCase()}>
                    <Input
                      type="number"
                      min={s === 'speed' || s === 'luck' ? 0 : 1}
                      value={templateValues[s]}
                      onChange={(e) => setTemplateValues({ ...templateValues, [s]: Number(e.target.value) })}
                    />
                  </Field>
                ))}
              </div>
            </div>

            {updateTemplate.error && (
              <ErrorText>{updateTemplate.error instanceof ApiError ? updateTemplate.error.message : 'Не удалось сохранить'}</ErrorText>
            )}

            <div className="flex gap-2">
              <Button type="submit" disabled={updateTemplate.isPending}>
                Сохранить
              </Button>
              <Button type="button" variant="secondary" onClick={() => setEditingTemplate(false)}>
                Отмена
              </Button>
            </div>
          </form>
        </Card>
      ) : (
        <>
          {ct.description && <p className="text-sm text-neutral-500">{ct.description}</p>}

          <Card>
            <h2 className="mb-2 font-semibold">Характеристики</h2>
            <div className="grid grid-cols-4 gap-3 text-sm sm:grid-cols-6">
              {STATS.map((s) => (
                <div key={s}>
                  <span className="text-neutral-400">{s.toUpperCase()}</span> <span className="font-medium">{ct[s]}</span>
                </div>
              ))}
            </div>
          </Card>
        </>
      )}

      <Card>
        <h2 className="mb-3 font-semibold">Части тела</h2>
        <table className="w-full text-left text-sm">
          <thead className="text-xs text-neutral-400">
            <tr>
              <th className="py-1 pr-3">Часть</th>
              <th className="py-1 pr-3">Тип</th>
              <th className="py-1 pr-3">Урон ×</th>
              <th className="py-1 pr-3">Пенальти</th>
              <th className="py-1 pr-3">To hit</th>
              <th className="py-1 pr-3">Броня</th>
              <th className="py-1 pr-3" />
            </tr>
          </thead>
          <tbody>
            {sortedParts.map((p) => (
              <tr key={p.id} className="border-t border-neutral-100 dark:border-neutral-900">
                <td className="py-1 pr-3">{p.name}</td>
                <td className="py-1 pr-3">{p.bodyPartType}</td>
                <td className="py-1 pr-3">{p.damageModifier}</td>
                <td className="py-1 pr-3">{p.hitPenalty}</td>
                <td className="py-1 pr-3">
                  {p.minToHit}–{p.maxToHit}
                </td>
                <td className="py-1 pr-3">
                  {editingPartId === p.id ? (
                    <Input
                      type="number"
                      min={0}
                      className="w-16"
                      value={armorValue}
                      onChange={(e) => setArmorValue(Number(e.target.value))}
                      autoFocus
                    />
                  ) : (
                    p.armor
                  )}
                </td>
                <td className="py-1 pr-3">
                  {editingPartId === p.id ? (
                    <div className="flex gap-2">
                      <Button
                        className="px-2 py-1"
                        disabled={updateArmor.isPending || armorValue < 0}
                        onClick={() => updateArmor.mutate(p.id)}
                      >
                        OK
                      </Button>
                      <Button variant="secondary" className="px-2 py-1" onClick={() => setEditingPartId(null)}>
                        Отмена
                      </Button>
                    </div>
                  ) : (
                    <button
                      className="text-violet-600 hover:underline"
                      onClick={() => {
                        setEditingPartId(p.id)
                        setArmorValue(p.armor)
                      }}
                    >
                      Изменить
                    </button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        {updateArmor.error && (
          <div className="mt-2">
            <ErrorText>{updateArmor.error instanceof ApiError ? updateArmor.error.message : 'Не удалось изменить броню'}</ErrorText>
          </div>
        )}
      </Card>

      <Card>
        <h2 className="mb-3 font-semibold">Навыки</h2>
        {Object.keys(ct.skills).length === 0 && <p className="mb-3 text-sm text-neutral-500">Навыков пока нет.</p>}
        <table className="w-full max-w-md text-left text-sm">
          <tbody>
            {(Object.entries(ct.skills) as [Skill, number][]).map(([skill, value]) => (
              <tr key={skill} className="border-b border-neutral-100 dark:border-neutral-900">
                <td className="py-2 pr-3">{skill}</td>
                <td className="py-2 pr-3">
                  {editingSkill === skill ? (
                    <Input
                      type="number"
                      min={1}
                      max={10}
                      className="w-20"
                      value={editSkillValue}
                      onChange={(e) => setEditSkillValue(Number(e.target.value))}
                      autoFocus
                    />
                  ) : (
                    value
                  )}
                </td>
                <td className="py-2">
                  {editingSkill === skill ? (
                    <div className="flex gap-2">
                      <Button
                        className="px-2 py-1"
                        disabled={upsertSkill.isPending || editSkillValue < 1 || editSkillValue > 10}
                        onClick={() => upsertSkill.mutate({ skill, value: editSkillValue })}
                      >
                        OK
                      </Button>
                      <Button variant="secondary" className="px-2 py-1" onClick={() => setEditingSkill(null)}>
                        Отмена
                      </Button>
                    </div>
                  ) : (
                    <div className="flex gap-3">
                      <button
                        className="text-violet-600 hover:underline"
                        onClick={() => {
                          setEditingSkill(skill)
                          setEditSkillValue(value)
                        }}
                      >
                        Изменить
                      </button>
                      <button
                        className="text-red-600 hover:underline"
                        disabled={deleteSkill.isPending}
                        onClick={() => deleteSkill.mutate(skill)}
                      >
                        Удалить
                      </button>
                    </div>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        <div className="mt-4 flex flex-wrap items-end gap-2">
          <Select
            value={newSkillStat}
            onChange={(e) => {
              setNewSkillStat(e.target.value)
              const first = SKILLS_BY_STAT[e.target.value].find((s) => !usedSkills.has(s))
              if (first) setNewSkill(first)
            }}
          >
            {Object.keys(SKILLS_BY_STAT).map((stat) => (
              <option key={stat} value={stat}>
                {stat.toUpperCase()}
              </option>
            ))}
          </Select>
          <Select value={newSkill} onChange={(e) => setNewSkill(e.target.value as Skill)}>
            {availableInStat.length === 0 && <option value="">— все добавлены —</option>}
            {availableInStat.map((s) => (
              <option key={s} value={s}>
                {s}
              </option>
            ))}
          </Select>
          <Input
            type="number"
            min={1}
            max={10}
            className="w-20"
            value={newSkillValue}
            onChange={(e) => setNewSkillValue(Number(e.target.value))}
          />
          <Button
            disabled={availableInStat.length === 0 || upsertSkill.isPending || newSkillValue < 1 || newSkillValue > 10}
            onClick={() => upsertSkill.mutate({ skill: newSkill, value: newSkillValue })}
          >
            Добавить навык
          </Button>
        </div>

        {upsertSkill.error && (
          <div className="mt-2">
            <ErrorText>{upsertSkill.error instanceof ApiError ? upsertSkill.error.message : 'Не удалось сохранить навык'}</ErrorText>
          </div>
        )}
      </Card>

      <Card>
        <h2 className="mb-3 font-semibold">Модификаторы урона</h2>
        {Object.keys(ct.damageTypeModifiers).length === 0 && (
          <p className="mb-3 text-sm text-neutral-500">Модификаторов пока нет.</p>
        )}
        <table className="w-full max-w-md text-left text-sm">
          <tbody>
            {(Object.entries(ct.damageTypeModifiers) as [DamageType, DamageTypeModifierKind][]).map(([damageType, modifier]) => (
              <tr key={damageType} className="border-b border-neutral-100 dark:border-neutral-900">
                <td className="py-2 pr-3">{damageType}</td>
                <td className="py-2 pr-3">{modifier}</td>
                <td className="py-2">
                  <button
                    className="text-red-600 hover:underline"
                    disabled={removeDamageTypeModifier.isPending}
                    onClick={() => removeDamageTypeModifier.mutate(damageType)}
                  >
                    Удалить
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        <div className="mt-4 flex flex-wrap items-end gap-2">
          <Select value={newDamageType} onChange={(e) => setNewDamageType(e.target.value as DamageType)}>
            {availableDamageTypes.length === 0 && <option value="">— все добавлены —</option>}
            {availableDamageTypes.map((t) => (
              <option key={t} value={t}>
                {t}
              </option>
            ))}
          </Select>
          <Select value={newModifier} onChange={(e) => setNewModifier(e.target.value as DamageTypeModifierKind)}>
            {DAMAGE_TYPE_MODIFIERS.map((m) => (
              <option key={m} value={m}>
                {m}
              </option>
            ))}
          </Select>
          <Button
            disabled={availableDamageTypes.length === 0 || setDamageTypeModifier.isPending}
            onClick={() => setDamageTypeModifier.mutate()}
          >
            Добавить модификатор
          </Button>
        </div>

        {setDamageTypeModifier.error && (
          <div className="mt-2">
            <ErrorText>
              {setDamageTypeModifier.error instanceof ApiError ? setDamageTypeModifier.error.message : 'Не удалось сохранить модификатор'}
            </ErrorText>
          </div>
        )}
      </Card>

      <Card>
        <div className="mb-3 flex items-center justify-between">
          <h2 className="font-semibold">Способности</h2>
          <Link to={`/games/${gameId}/creature-templates/${id}/abilities/new`}>
            <Button className="px-2 py-1 text-xs">Добавить способность</Button>
          </Link>
        </div>

        {ct.abilities.length === 0 && <p className="text-sm text-neutral-500">Способностей пока нет.</p>}
        <div className="flex flex-col gap-2">
          {ct.abilities.map((a) => (
            <div key={a.id} className="flex items-center justify-between gap-2 text-sm">
              <Link to={`/games/${gameId}/creature-templates/${id}/abilities/${a.id}`} className="hover:text-violet-600">
                {a.name}{' '}
                <span className="text-neutral-400">
                  — {a.attacksPerTurn}× {a.damageDiceCount}д6+{a.damageModifier} {a.damageType} ({a.attackSkill})
                </span>
              </Link>
              <ConfirmButton
                link
                confirmMessage={`Удалить способность "${a.name}"?`}
                onConfirm={() => removeAbility.mutate(a.id)}
                disabled={removeAbility.isPending}
              >
                Удалить
              </ConfirmButton>
            </div>
          ))}
        </div>
      </Card>
    </div>
  )
}
