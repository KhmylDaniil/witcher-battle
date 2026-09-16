import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { Button, Card, ErrorText, Field, Input, PageHeader, Spinner, Textarea } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { bodyTemplatesApi } from '../bodyTemplates/api'
import { charactersApi } from '../characters/api'
import { creatureTemplatesApi } from '../creatureTemplates/api'
import { gamesApi } from './api'

const STATS = ['int', 'str', 'rea', 'dex', 'cra', 'emp', 'wil'] as const

export function GameDetailsPage() {
  const { gameId } = useParams<{ gameId: string }>()
  const id = Number(gameId)
  const navigate = useNavigate()
  const queryClient = useQueryClient()

  const game = useQuery({ queryKey: ['games', id], queryFn: () => gamesApi.get(id) })
  const characters = useQuery({ queryKey: ['characters', { gameId: id }], queryFn: () => charactersApi.list({ gameId: id }) })

  const isOwner = game.data?.membershipStatus === 'Owner'
  const isMember = game.data?.membershipStatus === 'Owner' || game.data?.membershipStatus === 'Member'

  const incomingRequests = useQuery({
    queryKey: ['games', id, 'join-requests'],
    queryFn: () => gamesApi.incomingRequests(id),
    enabled: isOwner,
  })

  const requestJoin = useMutation({
    mutationFn: () => gamesApi.requestJoin(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['games', id] }),
  })

  const accept = useMutation({
    mutationFn: (requestId: number) => gamesApi.acceptRequest(id, requestId),
    onSuccess: async () => {
      await Promise.all([
        queryClient.invalidateQueries({ queryKey: ['games', id, 'join-requests'] }),
        queryClient.invalidateQueries({ queryKey: ['games', id] }),
      ])
    },
  })
  const decline = useMutation({
    mutationFn: (requestId: number) => gamesApi.declineRequest(id, requestId),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['games', id, 'join-requests'] }),
  })

  const removeGame = useMutation({
    mutationFn: () => gamesApi.remove(id),
    onSuccess: () => navigate('/games'),
  })

  const bodyTemplates = useQuery({
    queryKey: ['body-templates', { gameId: id }],
    queryFn: () => bodyTemplatesApi.list({ gameId: id }),
    enabled: isOwner,
  })
  const creatureTemplates = useQuery({
    queryKey: ['creature-templates', { gameId: id }],
    queryFn: () => creatureTemplatesApi.list({ gameId: id }),
    enabled: isOwner,
  })

  const [showBodyTemplateForm, setShowBodyTemplateForm] = useState(false)
  const [bodyTemplateName, setBodyTemplateName] = useState('')
  const [bodyTemplateDescription, setBodyTemplateDescription] = useState('')
  const createBodyTemplate = useMutation({
    mutationFn: () => bodyTemplatesApi.create(id, { name: bodyTemplateName, description: bodyTemplateDescription }),
    onSuccess: async (created) => {
      await queryClient.invalidateQueries({ queryKey: ['body-templates', { gameId: id }] })
      setBodyTemplateName('')
      setBodyTemplateDescription('')
      setShowBodyTemplateForm(false)
      // Сразу переходим на страницу шаблона — там добавляются/редактируются части тела.
      navigate(`/games/${id}/body-templates/${created.id}`)
    },
  })
  const removeBodyTemplate = useMutation({
    mutationFn: (bodyTemplateId: number) => bodyTemplatesApi.remove(bodyTemplateId),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['body-templates', { gameId: id }] }),
  })

  const removeCreatureTemplate = useMutation({
    mutationFn: (creatureTemplateId: number) => creatureTemplatesApi.remove(creatureTemplateId),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['creature-templates', { gameId: id }] }),
  })

  if (game.isLoading) return <Spinner />
  if (!game.data) return null
  const g = game.data

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title={g.name}
        actions={
          isOwner && (
            <Button
              variant="danger"
              disabled={removeGame.isPending}
              onClick={() => {
                if (confirm(`Удалить игру "${g.name}"?`)) removeGame.mutate()
              }}
            >
              Удалить игру
            </Button>
          )
        }
      />

      {g.membershipStatus === 'None' && (
        <Card className="flex items-center justify-between gap-3">
          <p className="text-sm text-neutral-500">Вы пока не участвуете в этой игре.</p>
          <Button disabled={requestJoin.isPending} onClick={() => requestJoin.mutate()}>
            Запросить присоединение
          </Button>
        </Card>
      )}
      {g.membershipStatus === 'Declined' && (
        <Card className="flex items-center justify-between gap-3">
          <p className="text-sm text-red-600 dark:text-red-400">Ваша заявка на присоединение была отклонена мастером.</p>
          <Button disabled={requestJoin.isPending} onClick={() => requestJoin.mutate()}>
            Запросить присоединение снова
          </Button>
        </Card>
      )}
      {g.membershipStatus === 'RequestPending' && (
        <Card>
          <p className="text-sm text-neutral-500">Заявка на присоединение отправлена, ожидает решения мастера.</p>
        </Card>
      )}
      {requestJoin.error && (
        <ErrorText>{requestJoin.error instanceof ApiError ? requestJoin.error.message : 'Не удалось отправить заявку'}</ErrorText>
      )}

      {isOwner && (
        <Card>
          <h2 className="mb-3 font-semibold">Входящие заявки</h2>
          {incomingRequests.isLoading && <Spinner />}
          {incomingRequests.data && incomingRequests.data.length === 0 && (
            <p className="text-sm text-neutral-500">Заявок нет.</p>
          )}
          <div className="flex flex-col gap-2">
            {incomingRequests.data?.map((r) => (
              <div key={r.id} className="flex items-center justify-between gap-2 text-sm">
                <span>Пользователь #{r.userId}</span>
                <div className="flex gap-2">
                  <Button className="px-2 py-1" disabled={accept.isPending} onClick={() => accept.mutate(r.id)}>
                    Принять
                  </Button>
                  <Button
                    variant="secondary"
                    className="px-2 py-1"
                    disabled={decline.isPending}
                    onClick={() => decline.mutate(r.id)}
                  >
                    Отклонить
                  </Button>
                </div>
              </div>
            ))}
          </div>
        </Card>
      )}

      {isOwner && (
        <Card>
          <div className="mb-3 flex items-center justify-between">
            <h2 className="font-semibold">Шаблоны тела</h2>
            {!showBodyTemplateForm && (
              <Button className="px-2 py-1 text-xs" onClick={() => setShowBodyTemplateForm(true)}>
                Создать
              </Button>
            )}
          </div>

          {showBodyTemplateForm && (
            <form
              onSubmit={(e) => {
                e.preventDefault()
                createBodyTemplate.mutate()
              }}
              className="mb-4 flex flex-col gap-3 rounded-md border border-neutral-200 p-3 dark:border-neutral-800"
            >
              <Field label="Название">
                <Input value={bodyTemplateName} onChange={(e) => setBodyTemplateName(e.target.value)} required autoFocus />
              </Field>
              <Field label="Описание">
                <Textarea rows={2} value={bodyTemplateDescription} onChange={(e) => setBodyTemplateDescription(e.target.value)} />
              </Field>
              <p className="text-xs text-neutral-500">
                Части тела (голова, торс, руки, ноги) добавятся автоматически по дефолтному шаблону человека — их
                можно будет изменить, удалить или добавить свои на странице шаблона после создания.
              </p>
              {createBodyTemplate.error && (
                <ErrorText>
                  {createBodyTemplate.error instanceof ApiError ? createBodyTemplate.error.message : 'Не удалось создать шаблон тела'}
                </ErrorText>
              )}
              <div className="flex gap-2">
                <Button type="submit" disabled={createBodyTemplate.isPending}>
                  Создать
                </Button>
                <Button type="button" variant="secondary" onClick={() => setShowBodyTemplateForm(false)}>
                  Отмена
                </Button>
              </div>
            </form>
          )}

          {bodyTemplates.isLoading && <Spinner />}
          {bodyTemplates.data && bodyTemplates.data.length === 0 && (
            <p className="text-sm text-neutral-500">Шаблонов тела пока нет.</p>
          )}
          <div className="flex flex-col gap-2">
            {bodyTemplates.data?.map((bt) => (
              <div key={bt.id} className="flex items-center justify-between gap-2 text-sm">
                <Link to={`/games/${id}/body-templates/${bt.id}`} className="hover:text-violet-600">
                  {bt.name} <span className="text-neutral-400">— {bt.parts.length} частей тела</span>
                </Link>
                <Button
                  variant="danger"
                  className="px-2 py-1"
                  disabled={removeBodyTemplate.isPending}
                  onClick={() => {
                    if (confirm(`Удалить шаблон тела "${bt.name}"? Связанные шаблоны существ тоже удалятся.`)) {
                      removeBodyTemplate.mutate(bt.id)
                    }
                  }}
                >
                  Удалить
                </Button>
              </div>
            ))}
          </div>
        </Card>
      )}

      {isOwner && (
        <Card>
          <div className="mb-3 flex items-center justify-between">
            <h2 className="font-semibold">Шаблоны существ</h2>
            <Link to={`/games/${id}/creature-templates/new`}>
              <Button className="px-2 py-1 text-xs">Создать</Button>
            </Link>
          </div>

          {creatureTemplates.isLoading && <Spinner />}
          {creatureTemplates.data && creatureTemplates.data.length === 0 && (
            <p className="text-sm text-neutral-500">Шаблонов существ пока нет.</p>
          )}
          <div className="flex flex-col gap-2">
            {creatureTemplates.data?.map((ct) => (
              <div key={ct.id} className="flex items-center justify-between gap-2 text-sm">
                <Link to={`/games/${id}/creature-templates/${ct.id}`} className="hover:text-violet-600">
                  {ct.name} <span className="text-neutral-400">({ct.creatureType}, HP {ct.hp})</span>
                </Link>
                <Button
                  variant="danger"
                  className="px-2 py-1"
                  disabled={removeCreatureTemplate.isPending}
                  onClick={() => {
                    if (confirm(`Удалить шаблон существа "${ct.name}"?`)) removeCreatureTemplate.mutate(ct.id)
                  }}
                >
                  Удалить
                </Button>
              </div>
            ))}
          </div>
        </Card>
      )}

      <Card>
        <div className="mb-3 flex items-center justify-between">
          <h2 className="font-semibold">Персонажи</h2>
          {isMember && (
            <Link to={`/games/${id}/characters/new`}>
              <Button className="px-2 py-1 text-xs">Создать персонажа</Button>
            </Link>
          )}
        </div>

        {characters.isLoading && <Spinner />}
        {characters.data && characters.data.length === 0 && <p className="text-sm text-neutral-500">Персонажей пока нет.</p>}

        <div className="grid gap-3 sm:grid-cols-2 lg:grid-cols-3">
          {characters.data?.map((c) => (
            <Link key={c.id} to={`/games/${id}/characters/${c.id}`}>
              <Card className="h-full transition hover:border-violet-400">
                <h3 className="font-semibold">{c.name}</h3>
                <div className="mt-2 grid grid-cols-4 gap-1 text-xs text-neutral-500">
                  {STATS.map((s) => (
                    <span key={s}>
                      {s.toUpperCase()} {c[s]}
                    </span>
                  ))}
                </div>
              </Card>
            </Link>
          ))}
        </div>
      </Card>
    </div>
  )
}
