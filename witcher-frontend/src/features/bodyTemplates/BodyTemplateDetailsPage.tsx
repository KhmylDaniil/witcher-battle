import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { Button, Card, ErrorText, Field, Input, PageHeader, Select, Spinner, Textarea } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { BODY_PART_TYPES, type BodyTemplatePartFormValues } from '../../types/api'
import { bodyTemplatesApi } from './api'

const SELECTABLE_BODY_PART_TYPES = BODY_PART_TYPES.filter((t) => t !== 'Void')

const EMPTY_PART: BodyTemplatePartFormValues = {
  name: '',
  bodyPartType: 'Head',
  damageModifier: 1,
  hitPenalty: 1,
  minToHit: 1,
  maxToHit: 1,
}

export function BodyTemplateDetailsPage() {
  const { gameId, bodyTemplateId } = useParams<{ gameId: string; bodyTemplateId: string }>()
  const id = Number(bodyTemplateId)
  const gameIdNum = Number(gameId)
  const navigate = useNavigate()
  const queryClient = useQueryClient()

  const bodyTemplate = useQuery({ queryKey: ['body-templates', id], queryFn: () => bodyTemplatesApi.get(id) })
  // Инвалидируем и детальную карточку, и список на экране игры — иначе название/число частей
  // на экране игры обновится только когда react-query сам решит, что список устарел (staleTime).
  const invalidate = () =>
    Promise.all([
      queryClient.invalidateQueries({ queryKey: ['body-templates', id] }),
      queryClient.invalidateQueries({ queryKey: ['body-templates', { gameId: gameIdNum }] }),
    ])

  const [editingTemplate, setEditingTemplate] = useState(false)
  const [templateName, setTemplateName] = useState('')
  const [templateDescription, setTemplateDescription] = useState('')
  const updateTemplate = useMutation({
    mutationFn: () => bodyTemplatesApi.update(id, { name: templateName, description: templateDescription }),
    onSuccess: async () => {
      await invalidate()
      setEditingTemplate(false)
    },
  })

  const removeTemplate = useMutation({
    mutationFn: () => bodyTemplatesApi.remove(id),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ['body-templates'] })
      navigate(`/games/${gameId}`)
    },
  })

  const [showAddPartForm, setShowAddPartForm] = useState(false)
  const [newPart, setNewPart] = useState<BodyTemplatePartFormValues>(EMPTY_PART)
  const addPart = useMutation({
    mutationFn: () => bodyTemplatesApi.addPart(id, newPart),
    onSuccess: async () => {
      await invalidate()
      setNewPart(EMPTY_PART)
      setShowAddPartForm(false)
    },
  })

  const [editingPartId, setEditingPartId] = useState<number | null>(null)
  const [editPart, setEditPart] = useState<BodyTemplatePartFormValues>(EMPTY_PART)
  const updatePart = useMutation({
    mutationFn: (partId: number) => bodyTemplatesApi.updatePart(id, partId, editPart),
    onSuccess: async () => {
      await invalidate()
      setEditingPartId(null)
    },
  })

  const removePart = useMutation({
    mutationFn: (partId: number) => bodyTemplatesApi.removePart(id, partId),
    onSuccess: invalidate,
  })

  if (bodyTemplate.isLoading) return <Spinner />
  if (!bodyTemplate.data) return null
  const bt = bodyTemplate.data

  const diceValues = Array.from({ length: 10 }, (_, i) => i + 1)
  const partForValue = (n: number) => bt.parts.find((p) => p.minToHit <= n && n <= p.maxToHit)
  const sortedParts = [...bt.parts].sort((a, b) => a.minToHit - b.minToHit)

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title={bt.name}
        actions={
          <>
            <Link to={`/games/${gameId}`}>
              <Button variant="secondary">К игре</Button>
            </Link>
            <Button
              variant="danger"
              disabled={removeTemplate.isPending}
              onClick={() => {
                if (confirm(`Удалить шаблон тела "${bt.name}"? Связанные шаблоны существ тоже удалятся.`)) removeTemplate.mutate()
              }}
            >
              Удалить
            </Button>
          </>
        }
      />

      <Card>
        {editingTemplate ? (
          <form
            onSubmit={(e) => {
              e.preventDefault()
              updateTemplate.mutate()
            }}
            className="flex flex-col gap-3"
          >
            <Field label="Название">
              <Input value={templateName} onChange={(e) => setTemplateName(e.target.value)} required autoFocus />
            </Field>
            <Field label="Описание">
              <Textarea rows={2} value={templateDescription} onChange={(e) => setTemplateDescription(e.target.value)} />
            </Field>
            {updateTemplate.error && (
              <ErrorText>{updateTemplate.error instanceof ApiError ? updateTemplate.error.message : 'Не удалось сохранить'}</ErrorText>
            )}
            <div className="flex gap-2">
              <Button type="submit" className="px-2 py-1" disabled={updateTemplate.isPending}>
                Сохранить
              </Button>
              <Button type="button" variant="secondary" className="px-2 py-1" onClick={() => setEditingTemplate(false)}>
                Отмена
              </Button>
            </div>
          </form>
        ) : (
          <div className="flex items-center justify-between gap-3">
            <p className="text-sm text-neutral-500">{bt.description || 'Без описания'}</p>
            <Button
              variant="secondary"
              className="px-2 py-1"
              onClick={() => {
                setEditingTemplate(true)
                setTemplateName(bt.name)
                setTemplateDescription(bt.description ?? '')
              }}
            >
              Изменить
            </Button>
          </div>
        )}
      </Card>

      <Card>
        <h2 className="mb-3 font-semibold">Покрытие кубика д10</h2>
        <div className="grid grid-cols-5 gap-2 sm:grid-cols-10">
          {diceValues.map((n) => {
            const part = partForValue(n)
            return (
              <div
                key={n}
                className={`flex min-w-0 flex-col items-center rounded-md border p-2 text-center text-xs ${
                  part
                    ? 'border-green-300 bg-green-50 dark:border-green-800 dark:bg-green-950'
                    : 'border-red-300 bg-red-50 dark:border-red-800 dark:bg-red-950'
                }`}
                title={part ? part.name : 'Значение не назначено ни одной части'}
              >
                <span className="font-semibold">{n}</span>
                <span className="w-full truncate text-neutral-500">{part ? part.name : '—'}</span>
              </div>
            )
          })}
        </div>
      </Card>

      <Card>
        <div className="mb-3 flex items-center justify-between">
          <h2 className="font-semibold">Части тела</h2>
          {!showAddPartForm && (
            <Button className="px-2 py-1 text-xs" onClick={() => setShowAddPartForm(true)}>
              Добавить часть
            </Button>
          )}
        </div>

        {showAddPartForm && (
          <PartForm
            value={newPart}
            onChange={setNewPart}
            onSubmit={() => addPart.mutate()}
            onCancel={() => {
              setShowAddPartForm(false)
              setNewPart(EMPTY_PART)
            }}
            submitLabel="Добавить"
            pending={addPart.isPending}
            error={addPart.error}
          />
        )}

        <table className="w-full text-left text-sm">
          <thead className="text-xs text-neutral-400">
            <tr>
              <th className="py-1 pr-3">Часть</th>
              <th className="py-1 pr-3">Тип</th>
              <th className="py-1 pr-3">Урон ×</th>
              <th className="py-1 pr-3">Сложность попадания</th>
              <th className="py-1 pr-3">Д10</th>
              <th className="py-1 pr-3" />
            </tr>
          </thead>
          <tbody>
            {sortedParts.map((p) =>
              editingPartId === p.id ? (
                <tr key={p.id} className="border-t border-neutral-100 dark:border-neutral-900">
                  <td colSpan={6} className="py-2">
                    <PartForm
                      value={editPart}
                      onChange={setEditPart}
                      onSubmit={() => updatePart.mutate(p.id)}
                      onCancel={() => setEditingPartId(null)}
                      submitLabel="Сохранить"
                      pending={updatePart.isPending}
                      error={updatePart.error}
                    />
                  </td>
                </tr>
              ) : (
                <tr key={p.id} className="border-t border-neutral-100 dark:border-neutral-900">
                  <td className="py-1 pr-3">{p.name}</td>
                  <td className="py-1 pr-3">{p.bodyPartType}</td>
                  <td className="py-1 pr-3">{p.damageModifier}</td>
                  <td className="py-1 pr-3">{p.hitPenalty}</td>
                  <td className="py-1 pr-3">
                    {p.minToHit}–{p.maxToHit}
                  </td>
                  <td className="py-1 pr-3">
                    <div className="flex gap-3">
                      <button
                        className="text-violet-600 hover:underline"
                        onClick={() => {
                          setEditingPartId(p.id)
                          setEditPart({
                            name: p.name,
                            bodyPartType: p.bodyPartType,
                            damageModifier: p.damageModifier,
                            hitPenalty: p.hitPenalty,
                            minToHit: p.minToHit,
                            maxToHit: p.maxToHit,
                          })
                        }}
                      >
                        Изменить
                      </button>
                      <button
                        className="text-red-600 hover:underline"
                        disabled={removePart.isPending}
                        onClick={() => {
                          if (confirm(`Удалить часть тела "${p.name}"?`)) removePart.mutate(p.id)
                        }}
                      >
                        Удалить
                      </button>
                    </div>
                  </td>
                </tr>
              ),
            )}
          </tbody>
        </table>
        {bt.parts.length === 0 && <p className="mt-3 text-sm text-neutral-500">Частей тела пока нет.</p>}
        {removePart.error && (
          <div className="mt-2">
            <ErrorText>{removePart.error instanceof ApiError ? removePart.error.message : 'Не удалось удалить часть'}</ErrorText>
          </div>
        )}
      </Card>
    </div>
  )
}

function PartForm({
  value,
  onChange,
  onSubmit,
  onCancel,
  submitLabel,
  pending,
  error,
}: {
  value: BodyTemplatePartFormValues
  onChange: (value: BodyTemplatePartFormValues) => void
  onSubmit: () => void
  onCancel: () => void
  submitLabel: string
  pending: boolean
  error: unknown
}) {
  return (
    <form
      onSubmit={(e) => {
        e.preventDefault()
        onSubmit()
      }}
      className="mb-4 flex flex-col gap-3 rounded-md border border-neutral-200 p-3 dark:border-neutral-800"
    >
      <div className="grid grid-cols-2 gap-3 sm:grid-cols-3">
        <Field label="Название">
          <Input value={value.name} onChange={(e) => onChange({ ...value, name: e.target.value })} required autoFocus />
        </Field>
        <Field label="Тип части тела">
          <Select
            value={value.bodyPartType}
            onChange={(e) => onChange({ ...value, bodyPartType: e.target.value as BodyTemplatePartFormValues['bodyPartType'] })}
          >
            {SELECTABLE_BODY_PART_TYPES.map((t) => (
              <option key={t} value={t}>
                {t}
              </option>
            ))}
          </Select>
        </Field>
        <Field label="Модификатор урона">
          <Input
            type="number"
            step="0.1"
            min={0.1}
            value={value.damageModifier}
            onChange={(e) => onChange({ ...value, damageModifier: Number(e.target.value) })}
          />
        </Field>
        <Field label="Сложность попадания">
          <Input
            type="number"
            min={1}
            value={value.hitPenalty}
            onChange={(e) => onChange({ ...value, hitPenalty: Number(e.target.value) })}
          />
        </Field>
        <Field label="Д10 от">
          <Input
            type="number"
            min={1}
            max={10}
            value={value.minToHit}
            onChange={(e) => onChange({ ...value, minToHit: Number(e.target.value) })}
          />
        </Field>
        <Field label="Д10 до">
          <Input
            type="number"
            min={1}
            max={10}
            value={value.maxToHit}
            onChange={(e) => onChange({ ...value, maxToHit: Number(e.target.value) })}
          />
        </Field>
      </div>

      {error != null && <ErrorText>{error instanceof ApiError ? error.message : 'Не удалось сохранить часть тела'}</ErrorText>}

      <div className="flex gap-2">
        <Button type="submit" className="px-2 py-1" disabled={pending}>
          {submitLabel}
        </Button>
        <Button type="button" variant="secondary" className="px-2 py-1" onClick={onCancel}>
          Отмена
        </Button>
      </div>
    </form>
  )
}
