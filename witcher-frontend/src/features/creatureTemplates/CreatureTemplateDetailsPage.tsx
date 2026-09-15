import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { Button, Card, PageHeader, Spinner } from '../../components/ui'
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
                <td className="py-1 pr-3">{p.armor}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </Card>
    </div>
  )
}
