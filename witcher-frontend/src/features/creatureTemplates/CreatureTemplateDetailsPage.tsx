import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { Button, Card, PageHeader, Spinner } from '../../components/ui'
import { useGameId } from '../../routes/GameLayout'
import { creatureTemplatesApi } from './api'
import { PartsTab } from './tabs/PartsTab'
import { SkillsTab } from './tabs/SkillsTab'
import { DamageModifiersTab } from './tabs/DamageModifiersTab'

const STATS = ['hp', 'sta', 'int', 'ref', 'dex', 'body', 'emp', 'cra', 'will', 'speed', 'luck'] as const
type Tab = 'parts' | 'skills' | 'abilities' | 'damage'

export function CreatureTemplateDetailsPage() {
  const gameId = useGameId()
  const { templateId } = useParams<{ templateId: string }>()
  const navigate = useNavigate()
  const queryClient = useQueryClient()
  const [tab, setTab] = useState<Tab>('parts')

  const template = useQuery({
    queryKey: ['creature-templates', gameId, templateId],
    queryFn: () => creatureTemplatesApi.get(gameId, templateId!),
  })

  const remove = useMutation({
    mutationFn: () => creatureTemplatesApi.remove(gameId, templateId!, template.data!.name),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ['creature-templates', gameId] })
      navigate(`/games/${gameId}/creature-templates`)
    },
  })

  if (template.isLoading) return <Spinner />
  if (!template.data) return null
  const t = template.data

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title={t.name}
        actions={
          <>
            <Link to={`/games/${gameId}/creature-templates/${t.id}/edit`}>
              <Button variant="secondary">Изменить</Button>
            </Link>
            <Button
              variant="danger"
              disabled={remove.isPending}
              onClick={() => {
                if (confirm(`Удалить шаблон "${t.name}"?`)) remove.mutate()
              }}
            >
              Удалить
            </Button>
          </>
        }
      />

      <Card>
        <p className="text-sm text-neutral-500">{t.description || 'Без описания'}</p>
        <p className="mt-1 text-xs text-neutral-400">Тип: {t.creatureType}</p>
        <div className="mt-3 grid grid-cols-3 gap-2 text-sm sm:grid-cols-6">
          {STATS.map((s) => (
            <div key={s}>
              <span className="text-neutral-400">{s.toUpperCase()}</span> <span className="font-medium">{t[s]}</span>
            </div>
          ))}
        </div>
      </Card>

      <div className="flex gap-1 border-b border-neutral-200 dark:border-neutral-800">
        {(
          [
            ['parts', 'Части тела'],
            ['skills', 'Навыки'],
            ['abilities', 'Способности'],
            ['damage', 'Модификаторы урона'],
          ] as [Tab, string][]
        ).map(([value, label]) => (
          <button
            key={value}
            onClick={() => setTab(value)}
            className={`px-3 py-1.5 text-sm font-medium ${
              tab === value
                ? 'border-b-2 border-violet-600 text-violet-600'
                : 'text-neutral-500 hover:text-neutral-800 dark:hover:text-neutral-200'
            }`}
          >
            {label}
          </button>
        ))}
      </div>

      <Card>
        {tab === 'parts' && <PartsTab gameId={gameId} templateId={t.id} parts={t.creatureTemplateParts} />}
        {tab === 'skills' && <SkillsTab gameId={gameId} templateId={t.id} skills={t.creatureTemplateSkills} />}
        {tab === 'damage' && <DamageModifiersTab gameId={gameId} templateId={t.id} modifiers={t.damageTypeModifiers} />}
        {tab === 'abilities' && (
          <ul className="flex flex-col gap-2 text-sm">
            {t.abilities.length === 0 && <li className="text-neutral-500">Способности не выбраны.</li>}
            {t.abilities.map((a) => (
              <li key={a.id} className="border-b border-neutral-100 pb-2 dark:border-neutral-900">
                <p className="font-medium">{a.name}</p>
                <p className="text-neutral-500">{a.description}</p>
                <p className="text-xs text-neutral-400">
                  {a.attackSkill} · {a.attackDiceQuantity} кубов · урон +{a.damageModifier} · скорость {a.attackSpeed} · точность{' '}
                  {a.accuracy}
                </p>
              </li>
            ))}
          </ul>
        )}
      </Card>
    </div>
  )
}
