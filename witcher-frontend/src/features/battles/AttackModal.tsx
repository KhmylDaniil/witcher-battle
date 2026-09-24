import { useState } from 'react'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { Button, ErrorText, Field, Input, Modal, Select } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { HUMAN_BODY_PARTS, HUMAN_BODY_PART_LABELS, type Battle, type BattleAttack, type HumanBodyPart, type ParticipantKind, type Skill } from '../../types/api'
import { battlesApi } from './api'

interface TargetOption {
  kind: ParticipantKind
  id: number
  name: string
}

function getTargetOptions(battle: Battle, exclude: { kind: ParticipantKind; id: number }): TargetOption[] {
  return [
    ...battle.creatures
      .filter((c) => !(exclude.kind === 'Creature' && exclude.id === c.id))
      .map((c): TargetOption => ({ kind: 'Creature', id: c.id, name: c.name })),
    ...battle.characters
      .filter((bc) => !(exclude.kind === 'Character' && exclude.id === bc.characterId))
      .map((bc): TargetOption => ({ kind: 'Character', id: bc.characterId, name: bc.characterName })),
  ]
}

export function AttackModal({
  gameId,
  battleId,
  battle,
  attack,
  isAttackerController,
  isDefenderController,
  isStunSaveOwnerController,
}: {
  gameId: number
  battleId: number
  battle: Battle
  attack: BattleAttack
  isAttackerController: boolean
  isDefenderController: boolean
  isStunSaveOwnerController: boolean
}) {
  const queryClient = useQueryClient()
  const invalidate = () => queryClient.invalidateQueries({ queryKey: ['battles', gameId, battleId] })

  const [partId, setPartId] = useState('')
  const [humanBodyPart, setHumanBodyPart] = useState('')
  const [attackRoll, setAttackRoll] = useState('')
  const [defensiveSkill, setDefensiveSkill] = useState<Skill>(attack.availableDefensiveSkills[0])
  const [defenseRoll, setDefenseRoll] = useState('')
  const [isParry, setIsParry] = useState(false)
  const [damageRoll, setDamageRoll] = useState('')
  const [stunSaveRollInput, setStunSaveRollInput] = useState('')
  const [nextTarget, setNextTarget] = useState<string>(`${attack.defenderKind}:${attack.defenderId}`)

  const confirmAttacker = useMutation({
    mutationFn: async () => {
      await battlesApi.setAttackerChoices(gameId, battleId, {
        targetedCreaturePartId: partId ? Number(partId) : null,
        targetedHumanBodyPart: humanBodyPart ? (humanBodyPart as HumanBodyPart) : null,
        attackRoll: attackRoll ? Number(attackRoll) : null,
      })
      await battlesApi.confirmAttacker(gameId, battleId)
    },
    onSuccess: invalidate,
  })

  const confirmDefender = useMutation({
    mutationFn: async () => {
      // Оглушённый защитник не выбирает навык — его защита фиксирована на 10 (см. attack.defenderIsStunned).
      if (!attack.defenderIsStunned) {
        await battlesApi.setDefenderChoice(gameId, battleId, {
          defensiveSkill: isParry ? (attack.parrySkill ?? defensiveSkill) : defensiveSkill,
          defenseRoll: defenseRoll ? Number(defenseRoll) : null,
          isParry,
        })
      }
      await battlesApi.confirmDefender(gameId, battleId)
    },
    onSuccess: invalidate,
  })

  const continueDamage = useMutation({
    mutationFn: async () => {
      await battlesApi.setDamageRoll(gameId, battleId, damageRoll ? Number(damageRoll) : null)
      await battlesApi.continueDamage(gameId, battleId)
    },
    onSuccess: invalidate,
  })

  const submitStunSave = useMutation({
    mutationFn: async () => {
      await battlesApi.setStunSaveRoll(gameId, battleId, stunSaveRollInput ? Number(stunSaveRollInput) : null)
      await battlesApi.resolveStunSave(gameId, battleId)
    },
    onSuccess: invalidate,
  })

  const nextSwing = useMutation({
    mutationFn: () => {
      const [kind, id] = nextTarget.split(':') as [ParticipantKind, string]
      return battlesApi.nextSwing(gameId, battleId, { defenderKind: kind, defenderId: Number(id) })
    },
    onSuccess: invalidate,
  })

  const endActivation = useMutation({
    mutationFn: () => battlesApi.endActivation(gameId, battleId),
    onSuccess: invalidate,
  })

  const error =
    confirmAttacker.error ?? confirmDefender.error ?? continueDamage.error ?? submitStunSave.error ?? nextSwing.error ?? endActivation.error
  const isPending =
    confirmAttacker.isPending
    || confirmDefender.isPending
    || continueDamage.isPending
    || submitStunSave.isPending
    || nextSwing.isPending
    || endActivation.isPending

  const targetOptions = getTargetOptions(battle, { kind: attack.attackerKind, id: attack.attackerId })

  return (
    <Modal>
      <h2 className="mb-3 font-semibold">
        {attack.attackerName} атакует {attack.defenderName} — {attack.abilityName}
      </h2>
      {attack.isBonusAction && (
        <p className="mb-3 text-sm text-amber-600 dark:text-amber-400">
          Дополнительное действие: потрачено 3 выносливости, к атаке применён модификатор −3.
        </p>
      )}

      {(attack.phase === 'AwaitingChoices' || attack.phase === 'AwaitingDamageRoll') && (
        <div className="flex flex-col gap-4">
          {/* Секция атакующего */}
          {isAttackerController && attack.phase === 'AwaitingChoices' ? (
            attack.attackerConfirmed ? (
              <p className="text-sm text-green-700 dark:text-green-400">✓ Атакующий подтвердил выбор</p>
            ) : (
              <div className="rounded-md border border-neutral-200 p-3 dark:border-neutral-800">
                <p className="mb-2 text-sm text-neutral-500">
                  {attack.attackerName}: {attack.abilityName} (характеристика+навык = {attack.attackerSkillValue})
                </p>
                {attack.availableCreatureParts && (
                  <div className="mb-2">
                    <Field label="Часть тела защитника">
                      <Select value={partId} onChange={(e) => setPartId(e.target.value)}>
                        <option value="">— случайно —</option>
                        {attack.availableCreatureParts.map((p) => (
                          <option key={p.id} value={p.id}>
                            {p.name}
                          </option>
                        ))}
                      </Select>
                    </Field>
                  </div>
                )}
                {attack.defenderKind === 'Character' && (
                  <div className="mb-2">
                    <Field label="Часть тела защитника (прицельная атака штрафует к попаданию)">
                      <Select value={humanBodyPart} onChange={(e) => setHumanBodyPart(e.target.value)}>
                        <option value="">— случайно —</option>
                        {HUMAN_BODY_PARTS.map((p) => (
                          <option key={p} value={p}>
                            {HUMAN_BODY_PART_LABELS[p]}
                          </option>
                        ))}
                      </Select>
                    </Field>
                  </div>
                )}
                <div className="mb-2">
                  <Field label="Бросок атаки (необязательно; d10 может «взрываться» — не ограничен 1–10)">
                    <Input
                      type="number"
                      className="w-24"
                      value={attackRoll}
                      onChange={(e) => setAttackRoll(e.target.value)}
                      placeholder="кубик"
                    />
                  </Field>
                </div>
                <Button disabled={isPending} onClick={() => confirmAttacker.mutate()}>
                  Подтвердить
                </Button>
              </div>
            )
          ) : (
            !isAttackerController &&
            attack.phase === 'AwaitingChoices' && <p className="text-sm text-neutral-500">Ожидание выбора атакующего…</p>
          )}

          {/* Секция защитника */}
          {isDefenderController && attack.phase === 'AwaitingChoices' ? (
            attack.defenderConfirmed ? (
              <p className="text-sm text-green-700 dark:text-green-400">✓ Защитник подтвердил выбор</p>
            ) : attack.defenderIsStunned ? (
              <div className="rounded-md border border-neutral-200 p-3 dark:border-neutral-800">
                <p className="mb-2 text-sm text-amber-600 dark:text-amber-400">
                  {attack.defenderName} оглушён и не бросает защиту — итог фиксирован на 10.
                </p>
                <Button disabled={isPending} onClick={() => confirmDefender.mutate()}>
                  Подтвердить
                </Button>
              </div>
            ) : (
              <div className="rounded-md border border-neutral-200 p-3 dark:border-neutral-800">
                <p className="mb-2 text-sm text-neutral-500">{attack.defenderName}: выбор защиты</p>
                {attack.canParry && (
                  <label className="mb-2 flex items-center gap-2 text-sm">
                    <input type="checkbox" checked={isParry} onChange={(e) => setIsParry(e.target.checked)} />
                    Парировать оружием ({attack.parrySkill}, база {attack.parrySkillValue}, штраф −3) — при успехе атака
                    отражена без урона, атакующий получает Ошеломление
                  </label>
                )}
                {!isParry && (
                  <div className="mb-2">
                    <Field label="Защитный навык">
                      <Select value={defensiveSkill} onChange={(e) => setDefensiveSkill(e.target.value as Skill)}>
                        {attack.availableDefensiveSkills.map((s) => (
                          <option key={s} value={s}>
                            {s} (база {attack.defensiveSkillValues[s] ?? '—'})
                          </option>
                        ))}
                      </Select>
                    </Field>
                  </div>
                )}
                <div className="mb-2">
                  <Field label="Бросок защиты (необязательно; d10 может «взрываться» — не ограничен 1–10)">
                    <Input
                      type="number"
                      className="w-24"
                      value={defenseRoll}
                      onChange={(e) => setDefenseRoll(e.target.value)}
                      placeholder="кубик"
                    />
                  </Field>
                </div>
                <Button disabled={isPending} onClick={() => confirmDefender.mutate()}>
                  Подтвердить
                </Button>
              </div>
            )
          ) : (
            !isDefenderController &&
            attack.phase === 'AwaitingChoices' && <p className="text-sm text-neutral-500">Ожидание выбора защитника…</p>
          )}

          {/* Фаза урона */}
          {attack.phase === 'AwaitingDamageRoll' &&
            (isAttackerController ? (
              <div className="rounded-md border border-neutral-200 p-3 dark:border-neutral-800">
                <p className="mb-2 text-sm text-green-700 dark:text-green-400">Попадание! Бросок урона.</p>
                <div className="mb-2">
                  <Field
                    label={`Бросок урона (необязательно; ${attack.abilityDamageDiceCount}к6: ${attack.abilityDamageDiceCount}–${attack.abilityDamageDiceCount * 6})`}
                  >
                    <Input
                      type="number"
                      min={attack.abilityDamageDiceCount}
                      max={attack.abilityDamageDiceCount * 6}
                      className="w-24"
                      value={damageRoll}
                      onChange={(e) => setDamageRoll(e.target.value)}
                      placeholder="кубики"
                    />
                  </Field>
                </div>
                <Button disabled={isPending} onClick={() => continueDamage.mutate()}>
                  Продолжить
                </Button>
              </div>
            ) : (
              <p className="text-sm text-neutral-500">Атакующий бросает урон…</p>
            ))}
        </div>
      )}

      {attack.phase === 'AwaitingStunSave' && (
        <div className="flex flex-col gap-3">
          <p className="text-sm text-amber-600 dark:text-amber-400">
            {attack.stunSaveOwnerName ?? attack.defenderName} проходит проверку Оглушения.
          </p>
          {isStunSaveOwnerController ? (
            <div className="rounded-md border border-neutral-200 p-3 dark:border-neutral-800">
              <div className="mb-2">
                <Field label="Чистый бросок д10 (stun save; необязательно — иначе бросит сервер)">
                  <Input
                    type="number"
                    className="w-24"
                    value={stunSaveRollInput}
                    onChange={(e) => setStunSaveRollInput(e.target.value)}
                    placeholder="кубик"
                  />
                </Field>
              </div>
              <Button disabled={isPending} onClick={() => submitStunSave.mutate()}>
                Бросить
              </Button>
            </div>
          ) : (
            <p className="text-sm text-neutral-500">Ожидание броска…</p>
          )}
        </div>
      )}

      {attack.phase === 'SwingResolved' && (
        <div className="flex flex-col gap-3">
          <p className="text-sm">
            Результат: {attack.lastHitSucceeded ? 'попадание' : attack.isParry ? 'парировано' : 'промах'}. Подробности — в логе боя.
          </p>
          {attack.stunSaveSucceeded !== null && (
            <p className="text-sm">
              Проверка Оглушения ({attack.stunSaveOwnerName ?? attack.defenderName}): бросок {attack.stunSaveRoll} —{' '}
              {attack.stunSaveSucceeded ? 'Оглушение наложено.' : 'Оглушение не наложено.'}
            </p>
          )}
          {isAttackerController && (
            <div className="flex flex-col gap-2">
              <p className="text-xs text-neutral-500">
                Атак использовано: {attack.attacksUsed}/{attack.attacksAllowed}
              </p>
              {attack.attacksUsed < attack.attacksAllowed && (
                <Field label="Цель следующей атаки">
                  <Select value={nextTarget} onChange={(e) => setNextTarget(e.target.value)}>
                    {targetOptions.map((t) => (
                      <option key={`${t.kind}:${t.id}`} value={`${t.kind}:${t.id}`}>
                        {t.name}
                      </option>
                    ))}
                  </Select>
                </Field>
              )}
              <div className="flex gap-2">
                {attack.attacksUsed < attack.attacksAllowed && (
                  <Button disabled={isPending} onClick={() => nextSwing.mutate()}>
                    Ещё одна атака
                  </Button>
                )}
                <Button variant="secondary" disabled={isPending} onClick={() => endActivation.mutate()}>
                  Закончить
                </Button>
              </div>
            </div>
          )}
        </div>
      )}

      {error && <div className="mt-3"><ErrorText>{error instanceof ApiError ? error.message : 'Не удалось выполнить действие'}</ErrorText></div>}
    </Modal>
  )
}
