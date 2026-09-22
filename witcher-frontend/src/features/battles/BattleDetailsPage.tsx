import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { Button, Card, ConfirmButton, ErrorText, Field, Input, PageHeader, Select, Spinner } from '../../components/ui'
import { useCurrentUser } from '../auth/useAuth'
import { ApiError } from '../../lib/apiClient'
import { useBattleUpdates } from '../../lib/battleHub'
import { charactersApi } from '../characters/api'
import { creatureTemplatesApi } from '../creatureTemplates/api'
import { gamesApi } from '../games/api'
import { CONDITION_REMOVAL_RULES, CONDITIONS, type Ability, type Condition, type ParticipantKind, type Skill } from '../../types/api'
import { AttackModal } from './AttackModal'
import { ParticipantSheetModal } from './ParticipantSheetModal'
import { battlesApi } from './api'

type Participant = {
  kind: 'creature' | 'character'
  /** Id самой записи (Creature.Id либо BattleCharacter.Id) — для React key. */
  id: number
  /** Id, по которому бэкенд принимает запросы на этого участника (Creature.Id либо Character.Id). */
  refId: number
  name: string
  maxHP: number
  currentHP: number
  maxSta: number
  currentSta: number
  initiative: number | null
  appliedConditions: Condition[]
  creatureTemplateId?: number
  characterUserId?: number
  /** Только для персонажей — уже потратил основное действие в этот ход, доступно доп. действие или конец хода. */
  hasActedThisTurn?: boolean
  /** Только для существ — износ брони по частям тела в этом бою (для расчёта текущей брони в карточке). */
  armorReductionByPartId?: Partial<Record<number, number>>
}

export function BattleDetailsPage() {
  const { gameId, battleId } = useParams<{ gameId: string; battleId: string }>()
  const gameIdNum = Number(gameId)
  const id = Number(battleId)
  const queryClient = useQueryClient()
  const navigate = useNavigate()

  const game = useQuery({ queryKey: ['games', gameIdNum], queryFn: () => gamesApi.get(gameIdNum) })
  const isOwner = game.data?.membershipStatus === 'Owner'
  const { data: currentUser } = useCurrentUser()

  const battle = useQuery({
    queryKey: ['battles', gameIdNum, id],
    queryFn: () => battlesApi.get(gameIdNum, id),
    retry: false,
    // SignalR — основной механизм обновления; интервал — подстраховка на случай обрыва/пропуска
    // хаб-события, поэтому достаточно редкий и не создаёт постоянную нагрузку.
    refetchInterval: (query) => (query.state.data?.status === 'InProgress' ? 20_000 : false),
  })
  const invalidate = () => queryClient.invalidateQueries({ queryKey: ['battles', gameIdNum, id] })
  useBattleUpdates(id, invalidate)

  // Выпадающий список для добавления существа в бой должен показывать все шаблоны игры —
  // запрашиваем через тот же пагинируемый эндпоинт, но с большим pageSize.
  const creatureTemplates = useQuery({
    queryKey: ['creature-templates', { gameId: gameIdNum }, 'all'],
    queryFn: () => creatureTemplatesApi.list({ gameId: gameIdNum }, { pageSize: 500 }),
    enabled: isOwner,
  })
  const gameCharacters = useQuery({
    queryKey: ['games', gameIdNum, 'characters'],
    queryFn: () => charactersApi.gameCharacters(gameIdNum),
    enabled: isOwner,
  })

  const start = useMutation({ mutationFn: () => battlesApi.start(gameIdNum, id), onSuccess: invalidate })
  const endBattle = useMutation({
    mutationFn: () => battlesApi.remove(gameIdNum, id),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ['battles', { gameId: gameIdNum }] })
      navigate(`/games/${gameId}`)
    },
  })

  const [addCreatureTemplateId, setAddCreatureTemplateId] = useState<number | null>(null)
  const [addCreatureName, setAddCreatureName] = useState('')
  const addCreature = useMutation({
    mutationFn: () =>
      battlesApi.addCreature(gameIdNum, id, { creatureTemplateId: addCreatureTemplateId!, name: addCreatureName || undefined }),
    onSuccess: async () => {
      await invalidate()
      setAddCreatureName('')
    },
  })
  const removeCreature = useMutation({ mutationFn: (creatureId: number) => battlesApi.removeCreature(gameIdNum, id, creatureId), onSuccess: invalidate })

  const [editingCreatureId, setEditingCreatureId] = useState<number | null>(null)
  const [editHp, setEditHp] = useState(0)
  const [editSta, setEditSta] = useState(0)
  const updateCreature = useMutation({
    mutationFn: (params: { creatureId: number; name: string }) =>
      battlesApi.updateCreature(gameIdNum, id, params.creatureId, { name: params.name, currentHP: editHp, currentSta: editSta }),
    onSuccess: async () => {
      await invalidate()
      setEditingCreatureId(null)
    },
  })

  const [addCharacterId, setAddCharacterId] = useState<number | null>(null)
  const addCharacter = useMutation({
    mutationFn: () => battlesApi.addCharacter(gameIdNum, id, addCharacterId!),
    onSuccess: invalidate,
  })
  const removeCharacter = useMutation({ mutationFn: (characterId: number) => battlesApi.removeCharacter(gameIdNum, id, characterId), onSuccess: invalidate })

  // Способности активного по инициативе участника — нужны для панели выбора атаки, только если
  // это мой ход и атака ещё не начата. Существо — способности живут в шаблоне; персонаж — в самом
  // персонаже (доступен, только если это моя учётка).
  const activeCreature = battle.data?.creatures.find((c) => c.initiative === battle.data?.currentInitiative) ?? null
  const activeCharacterEntry = battle.data?.characters.find((c) => c.initiative === battle.data?.currentInitiative) ?? null
  const isActiveCreatureController = !!activeCreature && isOwner
  const isActiveCharacterController = !!activeCharacterEntry && activeCharacterEntry.characterUserId === currentUser?.userId
  const noActiveAttack = !battle.data?.attack

  const activeCreatureTemplate = useQuery({
    queryKey: ['creature-templates', activeCreature?.creatureTemplateId],
    queryFn: () => creatureTemplatesApi.get(activeCreature!.creatureTemplateId),
    enabled: isActiveCreatureController && noActiveAttack,
  })
  const activeCharacterDetail = useQuery({
    queryKey: ['characters', activeCharacterEntry?.characterId],
    queryFn: () => charactersApi.get(activeCharacterEntry!.characterId),
    enabled: isActiveCharacterController && noActiveAttack,
  })

  const [attackAbilityId, setAttackAbilityId] = useState<number | null>(null)
  const [attackTarget, setAttackTarget] = useState('')
  const startAttack = useMutation({
    mutationFn: () => {
      const [kind, refId] = attackTarget.split(':') as [ParticipantKind, string]
      return battlesApi.startAttack(gameIdNum, id, { abilityId: attackAbilityId!, defenderKind: kind, defenderId: Number(refId) })
    },
    onSuccess: async () => {
      await invalidate()
      setAttackAbilityId(null)
      setAttackTarget('')
    },
  })
  const skipTurn = useMutation({ mutationFn: () => battlesApi.skipTurn(gameIdNum, id), onSuccess: invalidate })

  const [removeConditionCondition, setRemoveConditionCondition] = useState<Condition | ''>('')
  const [removeConditionSkill, setRemoveConditionSkill] = useState<Skill | ''>('')
  const [removeConditionTarget, setRemoveConditionTarget] = useState('')
  const [removeConditionRoll, setRemoveConditionRoll] = useState('')
  const attemptRemoveCondition = useMutation({
    mutationFn: () => {
      const [kind, refId] = removeConditionTarget.split(':') as [ParticipantKind, string]
      return battlesApi.attemptRemoveCondition(gameIdNum, id, {
        condition: removeConditionCondition as Condition,
        skill: removeConditionSkill as Skill,
        targetKind: kind,
        targetId: Number(refId),
        roll: removeConditionRoll ? Number(removeConditionRoll) : null,
      })
    },
    onSuccess: async () => {
      await invalidate()
      setRemoveConditionCondition('')
      setRemoveConditionSkill('')
      setRemoveConditionTarget('')
      setRemoveConditionRoll('')
    },
  })

  const [ownStunSaveRoll, setOwnStunSaveRoll] = useState('')
  const rollOwnStunSave = useMutation({
    mutationFn: () => battlesApi.rollOwnStunSave(gameIdNum, id, ownStunSaveRoll ? Number(ownStunSaveRoll) : null),
    onSuccess: async () => {
      await invalidate()
      setOwnStunSaveRoll('')
    },
  })

  const [sheetTarget, setSheetTarget] = useState<{ kind: 'creature' | 'character'; refId: number } | null>(null)

  const [conditionDrafts, setConditionDrafts] = useState<Record<string, Condition>>({})
  const addCondition = useMutation({
    mutationFn: (params: { kind: 'creature' | 'character'; refId: number; condition: Condition }) =>
      params.kind === 'creature'
        ? battlesApi.addCreatureCondition(gameIdNum, id, params.refId, params.condition)
        : battlesApi.addCharacterCondition(gameIdNum, id, params.refId, params.condition),
    onSuccess: invalidate,
  })
  const removeCondition = useMutation({
    mutationFn: (params: { kind: 'creature' | 'character'; refId: number; condition: Condition }) =>
      params.kind === 'creature'
        ? battlesApi.removeCreatureCondition(gameIdNum, id, params.refId, params.condition)
        : battlesApi.removeCharacterCondition(gameIdNum, id, params.refId, params.condition),
    onSuccess: invalidate,
  })

  if (battle.isLoading) return <Spinner />

  if (!battle.data) {
    const notFound = battle.error instanceof ApiError && battle.error.status === 404
    return (
      <Card>
        <p className="text-sm text-neutral-500">
          {notFound ? 'Бой недоступен — он ещё не начался или не существует.' : 'Не удалось загрузить бой.'}
        </p>
        <Link to={`/games/${gameId}`} className="mt-3 inline-block">
          <Button variant="secondary">К игре</Button>
        </Link>
      </Card>
    )
  }

  const b = battle.data

  const participants: Participant[] = [
    ...b.creatures.map((c): Participant => ({
      kind: 'creature',
      id: c.id,
      refId: c.id,
      name: c.name,
      maxHP: c.maxHP,
      currentHP: c.currentHP,
      maxSta: c.maxSta,
      currentSta: c.currentSta,
      initiative: c.initiative,
      appliedConditions: c.appliedConditions,
      creatureTemplateId: c.creatureTemplateId,
      armorReductionByPartId: c.armorReductionByPartId,
    })),
    ...b.characters.map((bc): Participant => ({
      kind: 'character',
      id: bc.id,
      refId: bc.characterId,
      name: bc.characterName,
      maxHP: bc.maxHP,
      currentHP: bc.currentHP,
      maxSta: bc.maxSta,
      currentSta: bc.currentSta,
      initiative: bc.initiative,
      appliedConditions: bc.appliedConditions,
      characterUserId: bc.characterUserId,
      hasActedThisTurn: bc.hasActedThisTurn,
    })),
  ].sort((x, y) => {
    if (x.initiative != null && y.initiative != null) return x.initiative - y.initiative
    if (x.initiative != null) return -1
    if (y.initiative != null) return 1
    return x.name.localeCompare(y.name)
  })

  const addedCharacterIds = new Set(b.characters.map((bc) => bc.characterId))
  const availableCharacters = (gameCharacters.data ?? []).filter((c) => !addedCharacterIds.has(c.id))

  // Активный по инициативе участник и проверка "я его контролирую" — существом всегда распоряжается
  // мастер (isOwner), персонажем — его владелец (characterUserId совпадает с текущим пользователем).
  const activeParticipant = participants.find((p) => p.initiative === b.currentInitiative) ?? null
  const isActiveController = !!activeParticipant
    && (activeParticipant.kind === 'creature' ? isOwner : activeParticipant.characterUserId === currentUser?.userId)

  const attack = b.attack
  const isAttackerController = !!attack
    && (attack.attackerKind === 'Creature' ? isOwner : participants.some((p) => p.kind === 'character' && p.refId === attack.attackerId && p.characterUserId === currentUser?.userId))
  const isDefenderController = !!attack
    && (attack.defenderKind === 'Creature' ? isOwner : participants.some((p) => p.kind === 'character' && p.refId === attack.defenderId && p.characterUserId === currentUser?.userId))

  return (
    <div className="flex flex-col gap-4 pb-56">
      {/* relative z-[70] — выше и модалки атаки (z-50), и лога боя (z-[60]), чтобы "Закончить бой"
          оставался доступен мастеру, даже если бой завис на неразрешённой атаке. */}
      <div className="relative z-[70]">
      <PageHeader
        title={b.name}
        actions={
          <>
            <Link to={`/games/${gameId}`}>
              <Button variant="secondary">К игре</Button>
            </Link>
            {isOwner && b.status === 'Draft' && (
              <Button disabled={start.isPending || participants.length === 0} onClick={() => start.mutate()}>
                Начать бой
              </Button>
            )}
            {isOwner && b.status === 'InProgress' && (
              <ConfirmButton
                confirmMessage="Закончить бой? Существа из боя удалятся, персонажи останутся в текущем состоянии."
                onConfirm={() => endBattle.mutate()}
                disabled={endBattle.isPending}
              >
                Закончить бой
              </ConfirmButton>
            )}
          </>
        }
      />
      </div>

      {start.error && (
        <ErrorText>{start.error instanceof ApiError ? start.error.message : 'Не удалось начать бой'}</ErrorText>
      )}
      {endBattle.error && (
        <ErrorText>{endBattle.error instanceof ApiError ? endBattle.error.message : 'Не удалось закончить бой'}</ErrorText>
      )}

      <Card>
        <p className="mb-3 text-sm text-neutral-500">
          {b.status === 'Draft' ? 'Подготовка к бою.' : `Бой идёт — раунд ${b.currentRound}.`}
        </p>
        <table className="w-full text-left text-sm">
          <thead className="text-xs text-neutral-400">
            <tr>
              <th className="py-1 pr-3">Имя</th>
              <th className="py-1 pr-3">HP</th>
              <th className="py-1 pr-3">Sta</th>
              <th className="py-1 pr-3">Состояние</th>
              <th className="py-1 pr-3">Инициатива</th>
              {isOwner && <th className="py-1 pr-3" />}
            </tr>
          </thead>
          <tbody>
            {participants.map((p) => {
              const draftKey = `${p.kind}-${p.refId}`
              const isEditingHp = p.kind === 'creature' && editingCreatureId === p.refId
              const isActive = b.status === 'InProgress' && p.initiative === b.currentInitiative
              return (
                <tr
                  key={draftKey}
                  className={`border-t border-neutral-100 dark:border-neutral-900 ${isActive ? 'bg-violet-50 dark:bg-violet-950' : ''}`}
                >
                  <td className="py-2 pr-3">
                    {(() => {
                      // Ссылка на карточку существа/персонажа — только если она у зрителя доступна:
                      // шаблон существа виден только мастеру (секретные характеристики монстра),
                      // лист персонажа — его владельцу или мастеру игры.
                      const detailsHref =
                        p.kind === 'creature'
                          ? isOwner
                            ? `/games/${gameId}/creature-templates/${p.creatureTemplateId}`
                            : null
                          : isOwner || p.characterUserId === currentUser?.userId
                            ? `/games/${gameId}/characters/${p.refId}`
                            : null
                      return detailsHref ? (
                        <Link to={detailsHref} className="hover:text-violet-600 hover:underline">
                          {p.name}
                        </Link>
                      ) : (
                        p.name
                      )
                    })()}{' '}
                    <span className="text-xs text-neutral-400">({p.kind === 'creature' ? 'существо' : 'персонаж'})</span>{' '}
                    <button
                      className="text-xs text-neutral-400 hover:text-violet-600"
                      title="Карточка участника"
                      onClick={() => setSheetTarget({ kind: p.kind, refId: p.refId })}
                    >
                      ℹ️
                    </button>
                    {isActive && <span className="ml-1 text-xs text-violet-600 dark:text-violet-400">● ход</span>}
                  </td>
                  <td className="py-2 pr-3">
                    {isEditingHp ? (
                      <Input type="number" min={0} max={p.maxHP} className="w-16" value={editHp} onChange={(e) => setEditHp(Number(e.target.value))} />
                    ) : (
                      `${p.currentHP}/${p.maxHP}`
                    )}
                  </td>
                  <td className="py-2 pr-3">
                    {isEditingHp ? (
                      <Input type="number" min={0} max={p.maxSta} className="w-16" value={editSta} onChange={(e) => setEditSta(Number(e.target.value))} />
                    ) : (
                      `${p.currentSta}/${p.maxSta}`
                    )}
                  </td>
                  <td className="py-2 pr-3">
                    {p.appliedConditions.length > 0 && <span title={p.appliedConditions.join(', ')}>⚠️</span>}
                  </td>
                  <td className="py-2 pr-3">{p.initiative ?? '—'}</td>
                  {isOwner && (
                    <td className="py-2 pr-3">
                      <div className="flex flex-wrap items-center gap-2">
                        {p.kind === 'creature' &&
                          (isEditingHp ? (
                            <>
                              <Button
                                className="px-2 py-1"
                                disabled={updateCreature.isPending}
                                onClick={() => updateCreature.mutate({ creatureId: p.refId, name: p.name })}
                              >
                                OK
                              </Button>
                              <Button variant="secondary" className="px-2 py-1" onClick={() => setEditingCreatureId(null)}>
                                Отмена
                              </Button>
                            </>
                          ) : (
                            <button
                              className="text-violet-600 hover:underline"
                              onClick={() => {
                                setEditingCreatureId(p.refId)
                                setEditHp(p.currentHP)
                                setEditSta(p.currentSta)
                              }}
                            >
                              HP/Sta
                            </button>
                          ))}

                        <Select
                          className="w-32"
                          value={conditionDrafts[draftKey] ?? CONDITIONS[0]}
                          onChange={(e) => setConditionDrafts({ ...conditionDrafts, [draftKey]: e.target.value as Condition })}
                        >
                          {CONDITIONS.map((c) => (
                            <option key={c} value={c}>
                              {c}
                            </option>
                          ))}
                        </Select>
                        <button
                          className="text-violet-600 hover:underline"
                          onClick={() => addCondition.mutate({ kind: p.kind, refId: p.refId, condition: conditionDrafts[draftKey] ?? CONDITIONS[0] })}
                        >
                          + состояние
                        </button>
                        {p.appliedConditions.length > 0 && (
                          <button
                            className="text-red-600 hover:underline"
                            onClick={() =>
                              removeCondition.mutate({ kind: p.kind, refId: p.refId, condition: p.appliedConditions[p.appliedConditions.length - 1] })
                            }
                          >
                            − состояние
                          </button>
                        )}

                        {b.status === 'Draft' && (
                          <button
                            className="text-red-600 hover:underline"
                            onClick={() =>
                              p.kind === 'creature' ? removeCreature.mutate(p.refId) : removeCharacter.mutate(p.refId)
                            }
                          >
                            Удалить
                          </button>
                        )}
                      </div>
                    </td>
                  )}
                </tr>
              )
            })}
          </tbody>
        </table>
      </Card>

      {b.status === 'InProgress' && !attack && isActiveController && activeParticipant && (
        <Card>
          <h2 className="mb-3 font-semibold">Ваш ход: {activeParticipant.name}</h2>
          {activeParticipant.appliedConditions.includes('Stun') ? (
            <>
              <p className="mb-2 text-sm text-amber-600 dark:text-amber-400">
                {activeParticipant.name} оглушён и может только пройти проверку Оглушения — ход завершится сразу после броска.
              </p>
              <div className="mb-3 flex flex-wrap items-end gap-2">
                <Field label="Чистый бросок д10 (stun save; необязательно — иначе бросит сервер)">
                  <Input
                    type="number"
                    className="w-24"
                    value={ownStunSaveRoll}
                    onChange={(e) => setOwnStunSaveRoll(e.target.value)}
                    placeholder="кубик"
                  />
                </Field>
                <Button disabled={rollOwnStunSave.isPending} onClick={() => rollOwnStunSave.mutate()}>
                  Бросить
                </Button>
              </div>
              {rollOwnStunSave.error && (
                <ErrorText>{rollOwnStunSave.error instanceof ApiError ? rollOwnStunSave.error.message : 'Не удалось пройти проверку Оглушения'}</ErrorText>
              )}
            </>
          ) : (
            (() => {
              const abilities: Ability[] =
                activeParticipant.kind === 'creature' ? (activeCreatureTemplate.data?.abilities ?? []) : (activeCharacterDetail.data?.abilities ?? [])
              const targetOptions = participants.filter(
                (p) => !(p.kind === activeParticipant.kind && p.refId === activeParticipant.refId),
              )
              // Персонаж (не существо) после основного действия может взять ещё одно за 3 выносливости,
              // с модификатором -3 (см. BattleCombatService.StartAttackAsync) — тот же выбор способности/
              // цели, только другая подпись кнопок и предупреждение о цене/штрафе.
              const isBonusActionWindow = activeParticipant.kind === 'character' && activeParticipant.hasActedThisTurn === true
              const notEnoughStaForBonusAction = isBonusActionWindow && activeParticipant.currentSta < 3
              return (
                <>
                  {isBonusActionWindow && (
                    <p className="mb-2 text-sm text-amber-600 dark:text-amber-400">
                      Основное действие уже потрачено. Можно взять дополнительное действие за 3 выносливости
                      (модификатор −3, в т.ч. на оба удара мультиатаки) либо закончить ход.
                    </p>
                  )}
                  <div className="mb-3 flex flex-wrap items-end gap-2">
                    <Field label="Способность">
                      <Select className="w-48" value={attackAbilityId ?? ''} onChange={(e) => setAttackAbilityId(e.target.value ? Number(e.target.value) : null)}>
                        <option value="">— выберите способность —</option>
                        {abilities.map((a) => (
                          <option key={a.id} value={a.id}>
                            {a.name}
                          </option>
                        ))}
                      </Select>
                    </Field>
                    <Field label="Цель">
                      <Select className="w-48" value={attackTarget} onChange={(e) => setAttackTarget(e.target.value)}>
                        <option value="">— выберите цель —</option>
                        {targetOptions.map((t) => (
                          <option key={`${t.kind}-${t.refId}`} value={`${t.kind === 'creature' ? 'Creature' : 'Character'}:${t.refId}`}>
                            {t.name}
                          </option>
                        ))}
                      </Select>
                    </Field>
                    <Button
                      disabled={!attackAbilityId || !attackTarget || startAttack.isPending || notEnoughStaForBonusAction}
                      onClick={() => startAttack.mutate()}
                    >
                      {isBonusActionWindow ? 'Доп. действие (3 STA, −3)' : 'Атаковать'}
                    </Button>
                    <Button variant="secondary" disabled={skipTurn.isPending} onClick={() => skipTurn.mutate()}>
                      {isBonusActionWindow ? 'Закончить ход' : 'Пропустить ход'}
                    </Button>
                  </div>
                  {notEnoughStaForBonusAction && (
                    <p className="mb-2 text-sm text-neutral-500">Недостаточно выносливости для дополнительного действия.</p>
                  )}
                  {startAttack.error && (
                    <ErrorText>{startAttack.error instanceof ApiError ? startAttack.error.message : 'Не удалось начать атаку'}</ErrorText>
                  )}

                  {(() => {
                    // Кровотечение/Отравление снимаются броском навыка вместо атаки — тоже основное
                    // или доп. действие персонажа (см. isBonusActionWindow выше), а для существа —
                    // всегда весь ход целиком (BattleCombatService.AttemptRemoveConditionAsync).
                    const removableConditions = activeParticipant.appliedConditions.filter((c) => CONDITION_REMOVAL_RULES[c])
                    if (removableConditions.length === 0) return null
                    const rules = removeConditionCondition ? (CONDITION_REMOVAL_RULES[removeConditionCondition] ?? []) : []
                    const selectedRule = rules.find((r) => r.skill === removeConditionSkill) ?? null
                    const removalTargetOptions = selectedRule?.selfOnly
                      ? participants.filter((p) => p.kind === activeParticipant.kind && p.refId === activeParticipant.refId)
                      : participants
                    return (
                      <div className="mt-3 border-t border-neutral-100 pt-3 dark:border-neutral-900">
                        <h3 className="mb-2 text-sm font-semibold">Снять состояние</h3>
                        <div className="flex flex-wrap items-end gap-2">
                          <Field label="Состояние">
                            <Select
                              className="w-36"
                              value={removeConditionCondition}
                              onChange={(e) => {
                                setRemoveConditionCondition(e.target.value as Condition)
                                setRemoveConditionSkill('')
                                setRemoveConditionTarget('')
                              }}
                            >
                              <option value="">— выберите —</option>
                              {removableConditions.map((c) => (
                                <option key={c} value={c}>
                                  {c}
                                </option>
                              ))}
                            </Select>
                          </Field>
                          <Field label="Навык">
                            <Select
                              className="w-40"
                              value={removeConditionSkill}
                              disabled={!removeConditionCondition}
                              onChange={(e) => {
                                setRemoveConditionSkill(e.target.value as Skill)
                                setRemoveConditionTarget('')
                              }}
                            >
                              <option value="">— выберите —</option>
                              {rules.map((r) => (
                                <option key={r.skill} value={r.skill}>
                                  {r.skill} (сложность {r.difficulty})
                                </option>
                              ))}
                            </Select>
                          </Field>
                          <Field label="Цель">
                            <Select
                              className="w-44"
                              value={removeConditionTarget}
                              disabled={!removeConditionSkill}
                              onChange={(e) => setRemoveConditionTarget(e.target.value)}
                            >
                              <option value="">— выберите —</option>
                              {removalTargetOptions.map((t) => (
                                <option key={`${t.kind}-${t.refId}`} value={`${t.kind === 'creature' ? 'Creature' : 'Character'}:${t.refId}`}>
                                  {t.kind === activeParticipant.kind && t.refId === activeParticipant.refId ? `${t.name} (себя)` : t.name}
                                </option>
                              ))}
                            </Select>
                          </Field>
                          <Field label="Чистый бросок д10 (необязательно — иначе бросит сервер)">
                            <Input
                              type="number"
                              className="w-24"
                              value={removeConditionRoll}
                              onChange={(e) => setRemoveConditionRoll(e.target.value)}
                              placeholder="кубик"
                            />
                          </Field>
                          <Button
                            disabled={
                              !removeConditionCondition
                              || !removeConditionSkill
                              || !removeConditionTarget
                              || attemptRemoveCondition.isPending
                              || notEnoughStaForBonusAction
                            }
                            onClick={() => attemptRemoveCondition.mutate()}
                          >
                            {isBonusActionWindow ? 'Снять (доп. действие, 3 STA, −3)' : 'Снять состояние'}
                          </Button>
                        </div>
                        {attemptRemoveCondition.error && (
                          <ErrorText>
                            {attemptRemoveCondition.error instanceof ApiError ? attemptRemoveCondition.error.message : 'Не удалось снять состояние'}
                          </ErrorText>
                        )}
                      </div>
                    )
                  })()}
                </>
              )
            })()
          )}
        </Card>
      )}

      {/* Зафиксирован по центру внизу экрана и лежит выше модального окна атаки (z-[60] > z-50) —
          лог должен оставаться доступным всем участникам боя, даже пока открыто AttackModal. */}
      <div className="fixed inset-x-0 bottom-4 z-[60] flex justify-center px-4">
        <Card className="max-h-48 w-full max-w-2xl overflow-y-auto shadow-lg">
          <h2 className="mb-2 font-semibold">Лог боя</h2>
          {b.logEntries.length === 0 && <p className="text-sm text-neutral-500">Пока пусто.</p>}
          <ul className="flex flex-col-reverse gap-1 text-sm">
            {b.logEntries.map((entry) => (
              <li key={entry.id} className="border-t border-neutral-100 pt-1 first:border-t-0 first:pt-0 dark:border-neutral-900">
                <span className="text-xs text-neutral-400">{new Date(entry.createdAt).toLocaleTimeString()}</span> {entry.message}
              </li>
            ))}
          </ul>
        </Card>
      </div>

      {attack && (isAttackerController || isDefenderController) && (
        <AttackModal
          gameId={gameIdNum}
          battleId={id}
          battle={b}
          attack={attack}
          isAttackerController={isAttackerController}
          isDefenderController={isDefenderController}
        />
      )}

      {sheetTarget && (() => {
        const p = participants.find((x) => x.kind === sheetTarget.kind && x.refId === sheetTarget.refId)
        if (!p) return null
        return (
          <ParticipantSheetModal
            gameId={gameIdNum}
            battleId={id}
            kind={p.kind === 'creature' ? 'Creature' : 'Character'}
            refId={p.refId}
            currentHP={p.currentHP}
            maxHP={p.maxHP}
            currentSta={p.currentSta}
            maxSta={p.maxSta}
            appliedConditions={p.appliedConditions}
            armorReductionByPartId={p.armorReductionByPartId}
            onClose={() => setSheetTarget(null)}
          />
        )
      })()}

      {isOwner && b.status === 'Draft' && (
        <Card>
          <h2 className="mb-3 font-semibold">Добавить участников</h2>

          <div className="mb-4 flex flex-wrap items-end gap-2">
            <Select
              className="w-56"
              value={addCreatureTemplateId ?? ''}
              onChange={(e) => setAddCreatureTemplateId(e.target.value ? Number(e.target.value) : null)}
            >
              <option value="">— выберите шаблон существа —</option>
              {creatureTemplates.data?.items.map((ct) => (
                <option key={ct.id} value={ct.id}>
                  {ct.name} ({ct.creatureType})
                </option>
              ))}
            </Select>
            <Field label="Имя (необязательно)">
              <Input className="w-40" value={addCreatureName} onChange={(e) => setAddCreatureName(e.target.value)} />
            </Field>
            <Button disabled={!addCreatureTemplateId || addCreature.isPending} onClick={() => addCreature.mutate()}>
              Добавить существо
            </Button>
          </div>
          {addCreature.error && (
            <ErrorText>{addCreature.error instanceof ApiError ? addCreature.error.message : 'Не удалось добавить существо'}</ErrorText>
          )}

          <div className="flex flex-wrap items-end gap-2">
            <Select className="w-56" value={addCharacterId ?? ''} onChange={(e) => setAddCharacterId(e.target.value ? Number(e.target.value) : null)}>
              <option value="">— выберите персонажа —</option>
              {availableCharacters.map((c) => (
                <option key={c.id} value={c.id}>
                  {c.name}
                </option>
              ))}
            </Select>
            <Button disabled={!addCharacterId || addCharacter.isPending} onClick={() => addCharacter.mutate()}>
              Добавить персонажа
            </Button>
          </div>
          {addCharacter.error && (
            <ErrorText>{addCharacter.error instanceof ApiError ? addCharacter.error.message : 'Не удалось добавить персонажа'}</ErrorText>
          )}
        </Card>
      )}
    </div>
  )
}
