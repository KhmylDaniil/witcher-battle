import { useRef, useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { Button, Card, ConfirmButton, ErrorText, Input, PageHeader, Select, Spinner } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { getAvailableOptions } from '../../lib/options'
import { SKILLS_BY_STAT, type Skill } from '../../types/api'
import { useCurrentUser } from '../auth/useAuth'
import { gamesApi } from '../games/api'
import { itemTemplatesApi } from '../itemTemplates/api'
import { charactersApi } from './api'

const STATS = ['int', 'str', 'rea', 'dex', 'cra', 'emp', 'wil'] as const
const SKILL_VALUE_MIN = 1
const SKILL_VALUE_MAX = 10

export function CharacterDetailsPage() {
  const { characterId } = useParams<{ characterId: string }>()
  const id = Number(characterId)
  const navigate = useNavigate()
  const queryClient = useQueryClient()

  const { data: user } = useCurrentUser()
  const character = useQuery({ queryKey: ['characters', id], queryFn: () => charactersApi.get(id) })
  const invalidate = () => queryClient.invalidateQueries({ queryKey: ['characters', id] })

  const remove = useMutation({
    mutationFn: () => charactersApi.remove(id),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ['characters'] })
      navigate(character.data?.gameId ? `/games/${character.data.gameId}` : '/characters')
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

  const removeAbility = useMutation({
    mutationFn: (abilityId: number) => charactersApi.removeAbility(id, abilityId),
    onSuccess: invalidate,
  })

  const fileInputRef = useRef<HTMLInputElement>(null)
  const uploadImage = useMutation({
    mutationFn: (file: File) => charactersApi.uploadImage(id, file),
    onSuccess: invalidate,
  })
  const removeImage = useMutation({
    mutationFn: () => charactersApi.removeImage(id),
    onSuccess: invalidate,
  })

  const [newSkillStat, setNewSkillStat] = useState<string>('Int')
  const [newSkill, setNewSkill] = useState<Skill>('Awareness')
  const [newValue, setNewValue] = useState(1)

  const removeItem = useMutation({
    mutationFn: (itemId: number) => charactersApi.removeItem(id, itemId),
    onSuccess: invalidate,
  })
  const equipItem = useMutation({
    mutationFn: (itemId: number) => charactersApi.equipItem(id, itemId),
    onSuccess: invalidate,
  })
  const unequipItem = useMutation({
    mutationFn: (itemId: number) => charactersApi.unequipItem(id, itemId),
    onSuccess: invalidate,
  })

  // Владелец персонажа отличается от мастера, который тоже может открыть эту страницу (см.
  // карточку "Персонажи игроков" на GameDetailsPage) — только владельцу доступны мутации
  // характеристик/способностей/фото/удаление, только мастеру — добавление предметов в инвентарь.
  // Архивного персонажа (gameId == null) мастер открыть не может — GM-доступ идёт через игру, поэтому
  // тут владение проверяем без учёта gameId (иначе владелец потерял бы кнопку удаления архивного).
  const isOwner = user?.userId === character.data?.userId
  const gameId = character.data?.gameId ?? null

  // isGameMaster — отдельно от isOwner: мастер может быть ещё и владельцем своего же персонажа
  // (играет в своей игре), и тогда обе роли верны одновременно — добавление предмета должно быть
  // доступно ему в любом случае, поэтому это не "not owner", а прямая проверка авторства игры,
  // совпадающая с CharacterItemService.GetForGmMutationAsync на бэкенде.
  const game = useQuery({ queryKey: ['games', gameId], queryFn: () => gamesApi.get(gameId!), enabled: !!gameId })
  const isGameMaster = !!gameId && game.data?.createdByUserId === user?.userId

  const itemTemplates = useQuery({
    queryKey: ['item-templates', { gameId }, 'all'],
    queryFn: () => itemTemplatesApi.list({ gameId: gameId! }, { pageSize: 500 }),
    enabled: isGameMaster,
  })
  const [selectedItemTemplateId, setSelectedItemTemplateId] = useState<number | null>(null)
  const addItem = useMutation({
    mutationFn: (itemTemplateId: number) => charactersApi.addItem(id, itemTemplateId),
    onSuccess: invalidate,
  })

  if (character.isLoading) return <Spinner />
  if (!character.data) return null
  const c = character.data

  const usedSkills = new Set(Object.keys(c.skills) as Skill[])
  const availableInStat = getAvailableOptions(SKILLS_BY_STAT[newSkillStat], usedSkills)

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title={c.name}
        actions={
          <>
            {c.gameId ? (
              <>
                <Link to={`/games/${c.gameId}`}>
                  <Button variant="secondary">К игре</Button>
                </Link>
                {isOwner && (
                  <Link to={`/games/${c.gameId}/characters/${c.id}/edit`}>
                    <Button variant="secondary">Изменить</Button>
                  </Link>
                )}
              </>
            ) : (
              <Link to="/characters">
                <Button variant="secondary">Мои персонажи</Button>
              </Link>
            )}
            {isOwner && (
              <ConfirmButton
                confirmMessage={`Удалить персонажа "${c.name}"?`}
                onConfirm={() => remove.mutate()}
                disabled={remove.isPending}
              >
                Удалить
              </ConfirmButton>
            )}
          </>
        }
      />

      {!c.gameId && (
        <Card className="border-amber-300 bg-amber-50 dark:border-amber-800 dark:bg-amber-950">
          <p className="text-sm text-amber-800 dark:text-amber-300">
            Игра, в которой был этот персонаж, была удалена мастером. Персонаж сохранён в архивном виде —
            вы можете его просматривать, но не редактировать.
          </p>
        </Card>
      )}

      <Card>
        <h2 className="mb-3 font-semibold">Изображение</h2>
        <div className="flex items-center gap-4">
          {c.imageUrl && (
            <img src={c.imageUrl} alt="" className="h-24 w-24 rounded-md border border-neutral-200 object-cover dark:border-neutral-800" />
          )}
          {isOwner && c.gameId && (
            <div className="flex flex-col items-start gap-2">
              <input
                ref={fileInputRef}
                type="file"
                accept="image/*"
                className="hidden"
                onChange={(e) => {
                  const file = e.target.files?.[0]
                  if (file) uploadImage.mutate(file)
                  e.target.value = ''
                }}
              />
              <Button
                variant="secondary"
                className="px-2 py-1 text-xs"
                disabled={uploadImage.isPending}
                onClick={() => fileInputRef.current?.click()}
              >
                {c.imageUrl ? 'Заменить изображение' : 'Загрузить изображение'}
              </Button>
              {c.imageUrl && (
                <ConfirmButton link confirmMessage="Удалить изображение?" onConfirm={() => removeImage.mutate()} disabled={removeImage.isPending}>
                  Удалить изображение
                </ConfirmButton>
              )}
            </div>
          )}
        </div>
        {(uploadImage.error ?? removeImage.error) && (
          <div className="mt-2">
            <ErrorText>
              {(uploadImage.error ?? removeImage.error) instanceof ApiError
                ? (uploadImage.error ?? removeImage.error as ApiError).message
                : 'Не удалось обновить изображение'}
            </ErrorText>
          </div>
        )}
      </Card>

      <Card>
        <h2 className="mb-2 font-semibold">Характеристики</h2>
        <div className="mb-3 grid grid-cols-4 gap-3 text-sm sm:grid-cols-7">
          <div>
            <span className="text-neutral-400">HP</span> <span className="font-medium">{c.hp}</span>
          </div>
          <div>
            <span className="text-neutral-400">STA</span> <span className="font-medium">{c.sta}</span>
          </div>
        </div>
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
                  {!isOwner || !c.gameId ? null : editingSkill === skill ? (
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

        {isOwner && c.gameId && (
          <>
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
          </>
        )}
      </Card>

      <Card>
        <div className="mb-3 flex items-center justify-between">
          <h2 className="font-semibold">Способности</h2>
          {isOwner && c.gameId && (
            <Link to={`/characters/${c.id}/abilities/new`}>
              <Button className="px-2 py-1 text-xs">Добавить способность</Button>
            </Link>
          )}
        </div>

        {c.abilities.length === 0 && <p className="text-sm text-neutral-500">Способностей пока нет.</p>}
        <div className="flex flex-col gap-2">
          {c.abilities.map((a) =>
            a.isFromEquippedWeapon ? (
              <div key={a.id} className="flex items-center justify-between gap-2 text-sm">
                <span>
                  {a.name}{' '}
                  <span className="text-neutral-400">
                    — {a.attacksPerTurn}× {a.damageDiceCount}д6+{a.damageModifier} {a.damageType} ({a.attackSkill})
                  </span>
                </span>
                <span className="text-xs text-neutral-400">от оружия</span>
              </div>
            ) : (
              <div key={a.id} className="flex items-center justify-between gap-2 text-sm">
                <Link to={`/characters/${c.id}/abilities/${a.id}`} className="hover:text-violet-600">
                  {a.name}{' '}
                  <span className="text-neutral-400">
                    — {a.attacksPerTurn}× {a.damageDiceCount}д6+{a.damageModifier} {a.damageType} ({a.attackSkill})
                  </span>
                </Link>
                {isOwner && c.gameId && (
                  <ConfirmButton
                    link
                    confirmMessage={`Удалить способность "${a.name}"?`}
                    onConfirm={() => removeAbility.mutate(a.id)}
                    disabled={removeAbility.isPending}
                  >
                    Удалить
                  </ConfirmButton>
                )}
              </div>
            ),
          )}
        </div>
      </Card>

      <Card>
        <div className="mb-3 flex items-center justify-between">
          <h2 className="font-semibold">Инвентарь</h2>
        </div>

        {isGameMaster && (
          <div className="mb-4 flex flex-wrap items-end gap-2 rounded-md border border-neutral-200 p-3 dark:border-neutral-800">
            <Select
              value={selectedItemTemplateId ?? ''}
              onChange={(e) => setSelectedItemTemplateId(e.target.value ? Number(e.target.value) : null)}
            >
              <option value="">— выберите шаблон —</option>
              {itemTemplates.data?.items.map((it) => (
                <option key={it.id} value={it.id}>
                  {it.name} ({it.itemType})
                </option>
              ))}
            </Select>
            <Button
              className="px-2 py-1 text-xs"
              disabled={!selectedItemTemplateId || addItem.isPending}
              onClick={() => selectedItemTemplateId && addItem.mutate(selectedItemTemplateId)}
            >
              Добавить предмет
            </Button>
          </div>
        )}
        {addItem.error && (
          <div className="mb-3">
            <ErrorText>{addItem.error instanceof ApiError ? addItem.error.message : 'Не удалось добавить предмет'}</ErrorText>
          </div>
        )}

        {c.items.length === 0 && <p className="text-sm text-neutral-500">Инвентарь пуст.</p>}
        <div className="flex flex-col gap-2">
          {c.items.map((i) => (
            <div key={i.id} className="flex items-center justify-between gap-2 text-sm">
              <span>
                {i.name} <span className="text-neutral-400">({i.itemType}, вес {i.weight})</span>
                {i.itemType === 'Weapon' && (
                  <span className="text-neutral-400">
                    {' '}
                    — {i.damageDiceCount}д6+{i.damageModifier} {i.damageType} ({i.attackSkill}
                    {i.isMultiAttack ? ', мультиатака' : ''})
                  </span>
                )}
                {i.isEquipped && (
                  <span className="ml-2 rounded-full bg-violet-100 px-2 py-0.5 text-xs font-medium text-violet-700 dark:bg-violet-950 dark:text-violet-300">
                    Экипировано
                  </span>
                )}
              </span>
              <div className="flex items-center gap-3">
                {isOwner && c.gameId && (
                  <button
                    className="text-violet-600 hover:underline"
                    disabled={equipItem.isPending || unequipItem.isPending}
                    onClick={() => (i.isEquipped ? unequipItem.mutate(i.id) : equipItem.mutate(i.id))}
                  >
                    {i.isEquipped ? 'Снять' : 'Экипировать'}
                  </button>
                )}
                {c.gameId && (
                  <ConfirmButton
                    link
                    confirmMessage={`Удалить предмет "${i.name}" из инвентаря?`}
                    onConfirm={() => removeItem.mutate(i.id)}
                    disabled={removeItem.isPending}
                  >
                    Удалить
                  </ConfirmButton>
                )}
              </div>
            </div>
          ))}
        </div>
      </Card>
    </div>
  )
}
