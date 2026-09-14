import { useMutation, useQueryClient } from '@tanstack/react-query'
import { Select } from '../../../components/ui'
import { DAMAGE_TYPES, DAMAGE_TYPE_MODIFIERS, type CreatureTemplateDamageModifier, type DamageTypeModifierValue } from '../../../types/api'
import { creatureTemplatesApi } from '../api'

export function DamageModifiersTab({
  gameId,
  templateId,
  modifiers,
}: {
  gameId: string
  templateId: string
  modifiers: CreatureTemplateDamageModifier[]
}) {
  const queryClient = useQueryClient()
  const edit = useMutation({
    mutationFn: (payload: { damageType: (typeof DAMAGE_TYPES)[number]; damageTypeModifier: DamageTypeModifierValue }) =>
      creatureTemplatesApi.editDamageModifier(gameId, templateId, payload),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['creature-templates', gameId, templateId] }),
  })

  return (
    <table className="w-full max-w-md text-left text-sm">
      <thead>
        <tr className="border-b border-neutral-200 text-neutral-500 dark:border-neutral-800">
          <th className="py-2 pr-3">Тип урона</th>
          <th className="py-2">Модификатор</th>
        </tr>
      </thead>
      <tbody>
        {DAMAGE_TYPES.map((damageType) => {
          const current = modifiers.find((m) => m.damageType === damageType)?.damageTypeModifier ?? 'Normal'
          return (
            <tr key={damageType} className="border-b border-neutral-100 dark:border-neutral-900">
              <td className="py-2 pr-3">{damageType}</td>
              <td className="py-2">
                <Select
                  value={current}
                  disabled={edit.isPending}
                  onChange={(e) => edit.mutate({ damageType, damageTypeModifier: e.target.value as DamageTypeModifierValue })}
                >
                  {DAMAGE_TYPE_MODIFIERS.map((m) => (
                    <option key={m} value={m}>
                      {m}
                    </option>
                  ))}
                </Select>
              </td>
            </tr>
          )
        })}
      </tbody>
    </table>
  )
}
