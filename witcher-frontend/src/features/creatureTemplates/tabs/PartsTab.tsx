import { useState } from 'react'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { Button, Input } from '../../../components/ui'
import type { CreatureTemplateBodyPart } from '../../../types/api'
import { creatureTemplatesApi } from '../api'

export function PartsTab({ gameId, templateId, parts }: { gameId: string; templateId: string; parts: CreatureTemplateBodyPart[] }) {
  const queryClient = useQueryClient()
  const [editingId, setEditingId] = useState<string | null>(null)
  const [armorValue, setArmorValue] = useState(0)

  const editPart = useMutation({
    mutationFn: (payload: { id: string; armorValue: number }) => creatureTemplatesApi.editPart(gameId, templateId, payload),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['creature-templates', gameId, templateId] })
      setEditingId(null)
    },
  })

  return (
    <div className="overflow-x-auto">
      <table className="w-full text-left text-sm">
        <thead>
          <tr className="border-b border-neutral-200 text-neutral-500 dark:border-neutral-800">
            <th className="py-2 pr-3">Часть</th>
            <th className="py-2 pr-3">Тип</th>
            <th className="py-2 pr-3">Пенальти</th>
            <th className="py-2 pr-3">Модификатор урона</th>
            <th className="py-2 pr-3">Диапазон попадания</th>
            <th className="py-2 pr-3">Броня</th>
            <th className="py-2" />
          </tr>
        </thead>
        <tbody>
          {parts.map((p) => (
            <tr key={p.id} className="border-b border-neutral-100 dark:border-neutral-900">
              <td className="py-2 pr-3">{p.name}</td>
              <td className="py-2 pr-3">{p.bodyPartType}</td>
              <td className="py-2 pr-3">{p.hitPenalty}</td>
              <td className="py-2 pr-3">×{p.damageModifier}</td>
              <td className="py-2 pr-3">
                {p.minToHit}–{p.maxToHit}
              </td>
              <td className="py-2 pr-3">
                {editingId === p.id ? (
                  <Input
                    type="number"
                    className="w-20"
                    value={armorValue}
                    onChange={(e) => setArmorValue(Number(e.target.value))}
                    autoFocus
                  />
                ) : (
                  p.armor
                )}
              </td>
              <td className="py-2">
                {editingId === p.id ? (
                  <div className="flex gap-2">
                    <Button
                      className="px-2 py-1"
                      disabled={editPart.isPending}
                      onClick={() => editPart.mutate({ id: p.id, armorValue })}
                    >
                      OK
                    </Button>
                    <Button variant="secondary" className="px-2 py-1" onClick={() => setEditingId(null)}>
                      Отмена
                    </Button>
                  </div>
                ) : (
                  <button
                    className="text-violet-600 hover:underline"
                    onClick={() => {
                      setEditingId(p.id)
                      setArmorValue(p.armor)
                    }}
                  >
                    Изменить броню
                  </button>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
