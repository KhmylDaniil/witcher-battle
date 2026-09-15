import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { Button, Card, ErrorText, Input, PageHeader, Select, Spinner } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { SKILLS_BY_STAT, type Skill } from '../../types/api'
import { charactersApi } from './api'

const STATS = ['int', 'str', 'rea', 'dex', 'cra', 'emp', 'wil'] as const
const SKILL_VALUE_MIN = 1
const SKILL_VALUE_MAX = 10

export function CharacterDetailsPage() {
  const { characterId } = useParams<{ characterId: string }>()
  const id = Number(characterId)
  const navigate = useNavigate()
  const queryClient = useQueryClient()

  const character = useQuery({ queryKey: ['characters', id], queryFn: () => charactersApi.get(id) })
  const invalidate = () => queryClient.invalidateQueries({ queryKey: ['characters', id] })

  const remove = useMutation({
    mutationFn: () => charactersApi.remove(id),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ['characters'] })
      navigate(character.data ? `/games/${character.data.gameId}` : '/games')
    },
  })

  const [editingSkill, setEditingSkill] = useState<Skill | null>(null)
  const [editValue, setEditValue] = useState(1)
  const upsertSkill = useMutation({
    mutationFn: ({ skill, value }: { skill: Skill; value: number }) => charactersApi.upsertSkill(id, skill, value),
    onSuccess: () => {
      invalidate()
      setEditingSkill(null)
    },
  })
  const deleteSkill = useMutation({
    mutationFn: (skill: Skill) => charactersApi.deleteSkill(id, skill),
    onSuccess: invalidate,
  })

  const [newSkillStat, setNewSkillStat] = useState<string>('Int')
  const [newSkill, setNewSkill] = useState<Skill>('Awareness')
  const [newValue, setNewValue] = useState(1)

  if (character.isLoading) return <Spinner />
  if (!character.data) return null
  const c = character.data

  const usedSkills = new Set(Object.keys(c.skills) as Skill[])
  const availableInStat = SKILLS_BY_STAT[newSkillStat].filter((s) => !usedSkills.has(s))

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title={c.name}
        actions={
          <>
            <Link to={`/games/${c.gameId}`}>
              <Button variant="secondary">К игре</Button>
            </Link>
            <Link to={`/games/${c.gameId}/characters/${c.id}/edit`}>
              <Button variant="secondary">Изменить</Button>
            </Link>
            <Button
              variant="danger"
              disabled={remove.isPending}
              onClick={() => {
                if (confirm(`Удалить персонажа "${c.name}"?`)) remove.mutate()
              }}
            >
              Удалить
            </Button>
          </>
        }
      />

      <Card>
        <h2 className="mb-2 font-semibold">Характеристики</h2>
        <div className="grid grid-cols-4 gap-3 text-sm sm:grid-cols-7">
          {STATS.map((s) => (
            <div key={s}>
              <span className="text-neutral-400">{s.toUpperCase()}</span> <span className="font-medium">{c[s]}</span>
            </div>
          ))}
        </div>
      </Card>

      <Card>
        <h2 className="mb-3 font-semibold">Навыки</h2>
        {Object.keys(c.skills).length === 0 && <p className="mb-3 text-sm text-neutral-500">Навыков пока нет.</p>}
        <table className="w-full max-w-md text-left text-sm">
          <tbody>
            {(Object.entries(c.skills) as [Skill, number][]).map(([skill, value]) => (
              <tr key={skill} className="border-b border-neutral-100 dark:border-neutral-900">
                <td className="py-2 pr-3">{skill}</td>
                <td className="py-2 pr-3">
                  {editingSkill === skill ? (
                    <Input
                      type="number"
                      min={SKILL_VALUE_MIN}
                      max={SKILL_VALUE_MAX}
                      className="w-20"
                      value={editValue}
                      onChange={(e) => setEditValue(Number(e.target.value))}
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
                        disabled={upsertSkill.isPending || editValue < SKILL_VALUE_MIN || editValue > SKILL_VALUE_MAX}
                        onClick={() => upsertSkill.mutate({ skill, value: editValue })}
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
                          setEditValue(value)
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
            min={SKILL_VALUE_MIN}
            max={SKILL_VALUE_MAX}
            className="w-20"
            value={newValue}
            onChange={(e) => setNewValue(Number(e.target.value))}
          />
          <Button
            disabled={
              availableInStat.length === 0 ||
              upsertSkill.isPending ||
              newValue < SKILL_VALUE_MIN ||
              newValue > SKILL_VALUE_MAX
            }
            onClick={() => upsertSkill.mutate({ skill: newSkill, value: newValue })}
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
    </div>
  )
}
