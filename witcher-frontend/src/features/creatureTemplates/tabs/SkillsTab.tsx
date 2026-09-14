import { useState } from 'react'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { Button, Input, Select } from '../../../components/ui'
import { SKILLS, type CreatureTemplateSkillItem, type Skill } from '../../../types/api'
import { creatureTemplatesApi } from '../api'

export function SkillsTab({ gameId, templateId, skills }: { gameId: string; templateId: string; skills: CreatureTemplateSkillItem[] }) {
  const queryClient = useQueryClient()
  const invalidate = () => queryClient.invalidateQueries({ queryKey: ['creature-templates', gameId, templateId] })

  const [editingId, setEditingId] = useState<string | null>(null)
  const [editValue, setEditValue] = useState(1)
  const [newSkill, setNewSkill] = useState<Skill>('Awareness')
  const [newValue, setNewValue] = useState(1)

  const upsert = useMutation({
    mutationFn: (payload: { id?: string | null; skill: Skill; value: number }) =>
      creatureTemplatesApi.upsertSkill(gameId, templateId, payload),
    onSuccess: () => {
      invalidate()
      setEditingId(null)
    },
  })
  const remove = useMutation({
    mutationFn: (skillId: string) => creatureTemplatesApi.deleteSkill(gameId, templateId, skillId),
    onSuccess: invalidate,
  })

  const usedSkills = new Set(skills.map((s) => s.skill))
  const availableSkills = SKILLS.filter((s) => !usedSkills.has(s))

  return (
    <div className="flex flex-col gap-4">
      <table className="w-full text-left text-sm">
        <thead>
          <tr className="border-b border-neutral-200 text-neutral-500 dark:border-neutral-800">
            <th className="py-2 pr-3">Навык</th>
            <th className="py-2 pr-3">Значение</th>
            <th className="py-2" />
          </tr>
        </thead>
        <tbody>
          {skills.map((s) => (
            <tr key={s.id} className="border-b border-neutral-100 dark:border-neutral-900">
              <td className="py-2 pr-3">{s.skill}</td>
              <td className="py-2 pr-3">
                {editingId === s.id ? (
                  <Input type="number" className="w-20" value={editValue} onChange={(e) => setEditValue(Number(e.target.value))} autoFocus />
                ) : (
                  s.skillValue
                )}
              </td>
              <td className="py-2">
                {editingId === s.id ? (
                  <div className="flex gap-2">
                    <Button
                      className="px-2 py-1"
                      disabled={upsert.isPending}
                      onClick={() => upsert.mutate({ id: s.id, skill: s.skill, value: editValue })}
                    >
                      OK
                    </Button>
                    <Button variant="secondary" className="px-2 py-1" onClick={() => setEditingId(null)}>
                      Отмена
                    </Button>
                  </div>
                ) : (
                  <div className="flex gap-3">
                    <button
                      className="text-violet-600 hover:underline"
                      onClick={() => {
                        setEditingId(s.id)
                        setEditValue(s.skillValue)
                      }}
                    >
                      Изменить
                    </button>
                    <button className="text-red-600 hover:underline" disabled={remove.isPending} onClick={() => remove.mutate(s.id)}>
                      Удалить
                    </button>
                  </div>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {availableSkills.length > 0 && (
        <div className="flex flex-wrap items-end gap-2">
          <Select value={newSkill} onChange={(e) => setNewSkill(e.target.value as Skill)}>
            {availableSkills.map((s) => (
              <option key={s} value={s}>
                {s}
              </option>
            ))}
          </Select>
          <Input type="number" className="w-20" value={newValue} onChange={(e) => setNewValue(Number(e.target.value))} />
          <Button disabled={upsert.isPending} onClick={() => upsert.mutate({ id: null, skill: newSkill, value: newValue })}>
            Добавить навык
          </Button>
        </div>
      )}
    </div>
  )
}
