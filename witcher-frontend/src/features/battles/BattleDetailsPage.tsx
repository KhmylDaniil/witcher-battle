import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useParams } from 'react-router-dom'
import { Button, Card, ErrorText, Field, Input, PageHeader, Select, Spinner } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { charactersApi } from '../characters/api'
import { creatureTemplatesApi } from '../creatureTemplates/api'
import { gamesApi } from '../games/api'
import { CONDITIONS, type Condition } from '../../types/api'
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
}

export function BattleDetailsPage() {
  const { gameId, battleId } = useParams<{ gameId: string; battleId: string }>()
  const gameIdNum = Number(gameId)
  const id = Number(battleId)
  const queryClient = useQueryClient()

  const game = useQuery({ queryKey: ['games', gameIdNum], queryFn: () => gamesApi.get(gameIdNum) })
  const isOwner = game.data?.membershipStatus === 'Owner'

  const battle = useQuery({
    queryKey: ['battles', gameIdNum, id],
    queryFn: () => battlesApi.get(gameIdNum, id),
    retry: false,
  })
  const invalidate = () => queryClient.invalidateQueries({ queryKey: ['battles', gameIdNum, id] })

  const creatureTemplates = useQuery({
    queryKey: ['creature-templates', { gameId: gameIdNum }],
    queryFn: () => creatureTemplatesApi.list({ gameId: gameIdNum }),
    enabled: isOwner,
  })
  const gameCharacters = useQuery({
    queryKey: ['games', gameIdNum, 'characters'],
    queryFn: () => charactersApi.gameCharacters(gameIdNum),
    enabled: isOwner,
  })

  const start = useMutation({ mutationFn: () => battlesApi.start(gameIdNum, id), onSuccess: invalidate })

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
    })),
  ].sort((x, y) => {
    if (x.initiative != null && y.initiative != null) return x.initiative - y.initiative
    if (x.initiative != null) return -1
    if (y.initiative != null) return 1
    return x.name.localeCompare(y.name)
  })

  const addedCharacterIds = new Set(b.characters.map((bc) => bc.characterId))
  const availableCharacters = (gameCharacters.data ?? []).filter((c) => !addedCharacterIds.has(c.id))

  return (
    <div className="flex flex-col gap-4">
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
          </>
        }
      />

      {start.error && (
        <ErrorText>{start.error instanceof ApiError ? start.error.message : 'Не удалось начать бой'}</ErrorText>
      )}

      <Card>
        <p className="mb-3 text-sm text-neutral-500">{b.status === 'Draft' ? 'Подготовка к бою.' : 'Бой идёт.'}</p>
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
              return (
                <tr key={draftKey} className="border-t border-neutral-100 dark:border-neutral-900">
                  <td className="py-2 pr-3">
                    {p.name} <span className="text-xs text-neutral-400">({p.kind === 'creature' ? 'существо' : 'персонаж'})</span>
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
              {creatureTemplates.data?.map((ct) => (
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
