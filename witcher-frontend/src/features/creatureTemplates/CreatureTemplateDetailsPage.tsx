import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { Button, Card, ErrorText, Input, PageHeader, Spinner } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { creatureTemplatesApi } from './api'

const STATS = ['hp', 'sta', 'int', 'ref', 'dex', 'body', 'emp', 'cra', 'will', 'speed', 'luck'] as const

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

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title={`${ct.name} (${ct.creatureType})`}
        actions={
          <>
            <Link to={`/games/${gameId}`}>
              <Button variant="secondary">К игре</Button>
            </Link>
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
            {ct.parts.map((p) => (
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
