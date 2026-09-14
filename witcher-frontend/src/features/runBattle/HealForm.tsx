import { useState } from 'react'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { Button, ErrorText, Field, Select } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import type { MakeTurnState } from '../../types/api'
import { runBattleApi } from './api'

interface Props {
  gameId: string
  battleId: string
  turn: MakeTurnState
  onDone: () => void
  onCancel: () => void
}

export function HealForm({ gameId, battleId, turn, onDone, onCancel }: Props) {
  const queryClient = useQueryClient()
  const [targetId, setTargetId] = useState('')
  const [effectId, setEffectId] = useState('')

  const formHeal = useMutation({
    mutationFn: (target: string) => runBattleApi.formHeal(gameId, battleId, target),
  })

  const heal = useMutation({
    mutationFn: () => runBattleApi.heal(gameId, battleId, { creatureId: turn.creatureId, targetId, effectId }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['run-battle', gameId, battleId] })
      onDone()
    },
  })

  return (
    <div className="flex flex-col gap-3">
      <Field label="Цель">
        <Select
          value={targetId}
          onChange={(e) => {
            setTargetId(e.target.value)
            setEffectId('')
            if (e.target.value) formHeal.mutate(e.target.value)
          }}
        >
          <option value="">— выберите —</option>
          {Object.entries(turn.possibleTargets).map(([id, name]) => (
            <option key={id} value={id}>
              {name}
            </option>
          ))}
        </Select>
      </Field>

      {targetId && (
        <Field label="Эффект">
          <Select value={effectId} onChange={(e) => setEffectId(e.target.value)} disabled={formHeal.isPending}>
            <option value="">— выберите —</option>
            {Object.entries(formHeal.data?.effectsOnTarget ?? {}).map(([id, name]) => (
              <option key={id} value={id}>
                {name}
              </option>
            ))}
          </Select>
        </Field>
      )}

      {heal.error && <ErrorText>{heal.error instanceof ApiError ? heal.error.message : 'Не удалось снять эффект'}</ErrorText>}

      <div className="flex gap-2">
        <Button disabled={!effectId || heal.isPending} onClick={() => heal.mutate()}>
          Попытаться снять
        </Button>
        <Button variant="secondary" onClick={onCancel}>
          Отмена
        </Button>
      </div>
    </div>
  )
}
