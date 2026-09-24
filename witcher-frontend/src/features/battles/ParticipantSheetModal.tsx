import { useQuery } from '@tanstack/react-query'
import { Modal, Spinner } from '../../components/ui'
import type { Ability, Condition, DamageType, DamageTypeModifierKind, ParticipantKind } from '../../types/api'
import { battlesApi } from './api'

const DAMAGE_TYPE_MODIFIER_LABEL: Record<DamageTypeModifierKind, string> = {
  Vulnerability: 'уязвимость',
  Resistance: 'сопротивление',
  Immunity: 'иммунитет',
}

const CREATURE_STAT_KEYS = ['int', 'ref', 'dex', 'body', 'emp', 'cra', 'will', 'speed', 'luck', 'movement'] as const
const CHARACTER_STAT_KEYS = ['int', 'str', 'rea', 'dex', 'cra', 'emp', 'wil', 'movement'] as const

function formatAbility(a: Ability): string {
  const modifier = a.damageModifier >= 0 ? `+${a.damageModifier}` : `${a.damageModifier}`
  const attackModifier = a.attackModifier >= 0 ? `+${a.attackModifier}` : `${a.attackModifier}`
  return `${a.name} — ${a.attacksPerTurn}× атака${attackModifier}, ${a.damageDiceCount}д6${modifier} ${a.damageType} (${a.attackSkill})`
}

export function ParticipantSheetModal({
  gameId,
  battleId,
  kind,
  refId,
  currentHP,
  maxHP,
  currentSta,
  maxSta,
  appliedConditions,
  armorReductionByPartId,
  onClose,
}: {
  gameId: number
  battleId: number
  kind: ParticipantKind
  refId: number
  currentHP: number
  maxHP: number
  currentSta: number
  maxSta: number
  appliedConditions: Condition[]
  armorReductionByPartId?: Partial<Record<number, number>>
  onClose: () => void
}) {
  const isCreature = kind === 'Creature'

  const creatureSheet = useQuery({
    queryKey: ['battles', gameId, battleId, 'creature-sheet', refId],
    queryFn: () => battlesApi.creatureSheet(gameId, battleId, refId),
    enabled: isCreature,
  })
  const characterSheet = useQuery({
    queryKey: ['battles', gameId, battleId, 'character-sheet', refId],
    queryFn: () => battlesApi.characterSheet(gameId, battleId, refId),
    enabled: !isCreature,
  })

  const isLoading = isCreature ? creatureSheet.isLoading : characterSheet.isLoading
  const isError = isCreature ? creatureSheet.isError : characterSheet.isError
  const name = isCreature ? creatureSheet.data?.name : characterSheet.data?.name
  const imageUrl = isCreature ? creatureSheet.data?.imageUrl : characterSheet.data?.imageUrl
  const abilities: Ability[] = (isCreature ? creatureSheet.data?.abilities : characterSheet.data?.abilities) ?? []
  const statKeys = isCreature ? CREATURE_STAT_KEYS : CHARACTER_STAT_KEYS
  const statSource = (isCreature ? creatureSheet.data : characterSheet.data) as Record<string, unknown> | undefined

  return (
    <Modal className="max-w-2xl" onClose={onClose}>
      <div className="mb-3 flex items-start justify-between gap-3">
        <div className="flex items-center gap-3">
          {imageUrl && (
            <img src={imageUrl} alt="" className="h-16 w-16 rounded-md border border-neutral-200 object-cover dark:border-neutral-800" />
          )}
          <h2 className="text-lg font-semibold">{name ?? '…'}</h2>
        </div>
        <button className="text-neutral-400 hover:text-neutral-700 dark:hover:text-neutral-200" onClick={onClose}>
          ✕
        </button>
      </div>

      {isLoading && <Spinner />}
      {isError && <p className="text-sm text-red-600">Не удалось загрузить карточку.</p>}

      {!isLoading && !isError && (
        <div className="flex flex-col gap-4 text-sm">
          <div className="flex gap-6">
            <p>
              HP: <span className="font-medium">{currentHP}</span>/{maxHP}
            </p>
            <p>
              Sta: <span className="font-medium">{currentSta}</span>/{maxSta}
            </p>
          </div>

          {statSource && (
            <div>
              <h3 className="mb-1 font-medium text-neutral-700 dark:text-neutral-300">Характеристики</h3>
              <div className="grid grid-cols-3 gap-2 sm:grid-cols-5">
                {statKeys.map((s) => (
                  <div key={s} className="rounded-md border border-neutral-200 px-2 py-1 text-center dark:border-neutral-800">
                    <div className="text-xs text-neutral-400">{s.toUpperCase()}</div>
                    <div className="font-medium">{String(statSource[s] ?? '—')}</div>
                  </div>
                ))}
              </div>
            </div>
          )}

          {isCreature && creatureSheet.data && (
            <div>
              <h3 className="mb-1 font-medium text-neutral-700 dark:text-neutral-300">Части тела</h3>
              {creatureSheet.data.parts.length === 0 && <p className="text-neutral-500">Нет частей тела.</p>}
              <ul className="flex flex-col gap-1">
                {creatureSheet.data.parts.map((part) => {
                  const reduction = armorReductionByPartId?.[part.id] ?? 0
                  const current = Math.max(0, part.armor - reduction)
                  return (
                    <li key={part.id} className="flex justify-between border-b border-neutral-100 py-0.5 dark:border-neutral-900">
                      <span>{part.name}</span>
                      <span>
                        броня {current}/{part.armor}
                      </span>
                    </li>
                  )
                })}
              </ul>
            </div>
          )}

          {isCreature && creatureSheet.data && Object.keys(creatureSheet.data.damageTypeModifiers).length > 0 && (
            <div>
              <h3 className="mb-1 font-medium text-neutral-700 dark:text-neutral-300">Модификаторы типа урона</h3>
              <ul className="flex flex-col gap-0.5">
                {(Object.entries(creatureSheet.data.damageTypeModifiers) as [DamageType, DamageTypeModifierKind][]).map(
                  ([damageType, modifier]) => (
                    <li key={damageType}>
                      {DAMAGE_TYPE_MODIFIER_LABEL[modifier]} к {damageType}
                    </li>
                  ),
                )}
              </ul>
            </div>
          )}

          <div>
            <h3 className="mb-1 font-medium text-neutral-700 dark:text-neutral-300">Способности</h3>
            {abilities.length === 0 && <p className="text-neutral-500">Способностей нет.</p>}
            <ul className="flex flex-col gap-0.5">
              {abilities.map((a) => (
                <li key={a.id}>{formatAbility(a)}</li>
              ))}
            </ul>
          </div>

          <div>
            <h3 className="mb-1 font-medium text-neutral-700 dark:text-neutral-300">Состояния</h3>
            {appliedConditions.length === 0 && <p className="text-neutral-500">Нет активных состояний.</p>}
            <div className="flex flex-wrap gap-1">
              {appliedConditions.map((c, i) => (
                <span
                  key={`${c}-${i}`}
                  className="rounded-full bg-amber-100 px-2 py-0.5 text-xs font-medium text-amber-700 dark:bg-amber-950 dark:text-amber-300"
                >
                  {c}
                </span>
              ))}
            </div>
          </div>
        </div>
      )}
    </Modal>
  )
}
