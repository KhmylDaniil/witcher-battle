import { useState } from 'react'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { Button, ErrorText, Field, Input, Select } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import type { AttackType, MakeTurnState, Skill } from '../../types/api'
import { runBattleApi } from './api'

interface Props {
  gameId: string
  battleId: string
  turn: MakeTurnState
  onDone: () => void
  onCancel: () => void
}

export function AttackForm({ gameId, battleId, turn, onDone, onCancel }: Props) {
  const queryClient = useQueryClient()
  const hasAbilities = Object.keys(turn.myAbilities).length > 0
  const hasWeapons = Object.keys(turn.equippedWeapons).length > 0
  // Продолжение мультиатаки — формула фиксирована (та же способность), только цель выбирается заново
  // (см. Views/RunBattle/MakeTurn.cshtml: MultiAttackAbilityId != null ветка)
  const isMultiattack = !!turn.multiAttackAbilityId

  const [attackType, setAttackType] = useState<AttackType>(hasAbilities ? 'Ability' : 'Weapon')
  const [formulaId, setFormulaId] = useState(isMultiattack ? turn.multiAttackAbilityId! : '')
  const [targetId, setTargetId] = useState('')
  const [step, setStep] = useState<'setup' | 'details'>('setup')

  const [creaturePartId, setCreaturePartId] = useState('')
  const [defensiveSkill, setDefensiveSkill] = useState('')
  const [specialToHit, setSpecialToHit] = useState(0)
  const [specialToDamage, setSpecialToDamage] = useState(0)
  const [isStrongAttack, setIsStrongAttack] = useState(false)

  const formAttack = useMutation({
    mutationFn: () => runBattleApi.formAttack(gameId, battleId, { attackerId: turn.creatureId, targetId, attackFormulaId: formulaId, attackType }),
    onSuccess: () => setStep('details'),
  })

  const attack = useMutation({
    mutationFn: () =>
      runBattleApi.attack(gameId, battleId, {
        id: turn.creatureId,
        targetId,
        attackFormulaId: formulaId,
        creaturePartId: creaturePartId || null,
        defensiveSkill: (defensiveSkill || null) as Skill | null,
        specialToHit,
        specialToDamage,
        isStrongAttack: attackType === 'Weapon' ? isStrongAttack : null,
        attackType,
        isPartOfMultiattack: isMultiattack,
      }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['run-battle', gameId, battleId] })
      onDone()
    },
  })

  const formulaOptions = attackType === 'Ability' ? turn.myAbilities : turn.equippedWeapons

  if (step === 'setup') {
    return (
      <div className="flex flex-col gap-3">
        {isMultiattack ? (
          <p className="rounded-md bg-violet-50 px-3 py-2 text-sm text-violet-700 dark:bg-violet-950 dark:text-violet-300">
            Продолжение мультиатаки: {turn.myAbilities[turn.multiAttackAbilityId!]}
          </p>
        ) : (
          <>
            {hasAbilities && hasWeapons && (
              <div className="flex gap-4 text-sm">
                <label className="flex items-center gap-1">
                  <input type="radio" checked={attackType === 'Ability'} onChange={() => { setAttackType('Ability'); setFormulaId('') }} />
                  Способность
                </label>
                <label className="flex items-center gap-1">
                  <input type="radio" checked={attackType === 'Weapon'} onChange={() => { setAttackType('Weapon'); setFormulaId('') }} />
                  Оружие
                </label>
              </div>
            )}
            <Field label={attackType === 'Ability' ? 'Способность' : 'Оружие'}>
              <Select value={formulaId} onChange={(e) => setFormulaId(e.target.value)}>
                <option value="">— выберите —</option>
                {Object.entries(formulaOptions).map(([id, name]) => (
                  <option key={id} value={id}>
                    {name}
                  </option>
                ))}
              </Select>
            </Field>
          </>
        )}
        <Field label="Цель">
          <Select value={targetId} onChange={(e) => setTargetId(e.target.value)}>
            <option value="">— выберите —</option>
            {Object.entries(turn.possibleTargets)
              .filter(([id]) => id !== turn.creatureId)
              .map(([id, name]) => (
              <option key={id} value={id}>
                {name}
              </option>
            ))}
          </Select>
        </Field>
        {formAttack.error && (
          <ErrorText>{formAttack.error instanceof ApiError ? formAttack.error.message : 'Не удалось подготовить атаку'}</ErrorText>
        )}
        <div className="flex gap-2">
          <Button disabled={!formulaId || !targetId || formAttack.isPending} onClick={() => formAttack.mutate()}>
            Далее
          </Button>
          <Button variant="secondary" onClick={onCancel}>
            Отмена
          </Button>
        </div>
      </div>
    )
  }

  const parts = formAttack.data?.creatureParts ?? {}
  const skills = formAttack.data?.defensiveSkills ?? {}

  return (
    <div className="flex flex-col gap-3">
      <Field label="Часть тела (пусто — по броску)">
        <Select value={creaturePartId} onChange={(e) => setCreaturePartId(e.target.value)}>
          <option value="">— случайно —</option>
          {Object.entries(parts)
            .filter(([, id]) => id)
            .map(([name, id]) => (
              <option key={id!} value={id!}>
                {name}
              </option>
            ))}
        </Select>
      </Field>
      <Field label="Защита цели">
        <Select value={defensiveSkill} onChange={(e) => setDefensiveSkill(e.target.value)}>
          <option value="">— без защиты —</option>
          {Object.entries(skills)
            .filter(([, skill]) => skill)
            .map(([name, skill]) => (
              <option key={skill!} value={skill!}>
                {name}
              </option>
            ))}
        </Select>
      </Field>
      <div className="grid grid-cols-2 gap-3">
        <Field label="Бонус к попаданию">
          <Input type="number" value={specialToHit} onChange={(e) => setSpecialToHit(Number(e.target.value))} />
        </Field>
        <Field label="Бонус к урону">
          <Input type="number" value={specialToDamage} onChange={(e) => setSpecialToDamage(Number(e.target.value))} />
        </Field>
      </div>
      {attackType === 'Weapon' && (
        <label className="flex items-center gap-2 text-sm">
          <input type="checkbox" checked={isStrongAttack} onChange={(e) => setIsStrongAttack(e.target.checked)} />
          Сильная атака
        </label>
      )}
      {attack.error && <ErrorText>{attack.error instanceof ApiError ? attack.error.message : 'Не удалось провести атаку'}</ErrorText>}
      <div className="flex gap-2">
        <Button disabled={attack.isPending} onClick={() => attack.mutate()}>
          Атаковать
        </Button>
        <Button variant="secondary" onClick={() => setStep('setup')}>
          Назад
        </Button>
      </div>
    </div>
  )
}
