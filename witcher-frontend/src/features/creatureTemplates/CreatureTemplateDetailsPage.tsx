import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { Button, Card, ErrorText, Field, Input, PageHeader, Select, Spinner, Textarea } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { CREATURE_TYPES, type CreatureTemplateFormValues } from '../../types/api'
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
      await queryClient.invalidateQueries({ queryKey: ['creature-templates', id] })
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

  if (creatureTemplate.isLoading) return <Spinner />
  if (!creatureTemplate.data) return null
  const ct = creatureTemplate.data
  const sortedParts = [...ct.parts].sort((a, b) => a.minToHit - b.minToHit)

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
            <Button
              variant="danger"
              disabled={remove.isPending}
              onClick={() => {
                if (confirm(`Удалить шаблон существа "${ct.name}"?`)) remove.mutate()
              }}
            >
              Удалить
            </Button>
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
    </div>
  )
}
