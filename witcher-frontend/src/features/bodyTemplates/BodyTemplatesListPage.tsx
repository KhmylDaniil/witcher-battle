import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { Button, Card, ConfirmButton, ErrorText, Field, Input, PageHeader, Pagination, Spinner, Textarea } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { bodyTemplatesApi } from './api'

/** Страница "Шаблоны тела" игры — доступна только мастеру (бэкенд скоупит список по владельцу игры). */
export function BodyTemplatesListPage() {
  const { gameId } = useParams<{ gameId: string }>()
  const id = Number(gameId)
  const navigate = useNavigate()
  const queryClient = useQueryClient()
  const [page, setPage] = useState(1)

  const bodyTemplates = useQuery({
    queryKey: ['body-templates', { gameId: id }, page],
    queryFn: () => bodyTemplatesApi.list({ gameId: id }, { pageNumber: page }),
  })

  const [showForm, setShowForm] = useState(false)
  const [name, setName] = useState('')
  const [description, setDescription] = useState('')
  const create = useMutation({
    mutationFn: () => bodyTemplatesApi.create(id, { name, description }),
    onSuccess: async (created) => {
      await queryClient.invalidateQueries({ queryKey: ['body-templates', { gameId: id }] })
      setName('')
      setDescription('')
      setShowForm(false)
      // Сразу переходим на страницу шаблона — там добавляются/редактируются части тела.
      navigate(`/games/${id}/body-templates/${created.id}`)
    },
  })
  const remove = useMutation({
    mutationFn: (bodyTemplateId: number) => bodyTemplatesApi.remove(bodyTemplateId),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['body-templates', { gameId: id }] }),
  })

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title="Шаблоны тела"
        actions={
          <Link to={`/games/${id}`}>
            <Button variant="secondary">К игре</Button>
          </Link>
        }
      />

      <Card>
        <div className="mb-3 flex items-center justify-between">
          <h2 className="font-semibold">Список</h2>
          {!showForm && (
            <Button className="px-2 py-1 text-xs" onClick={() => setShowForm(true)}>
              Создать
            </Button>
          )}
        </div>

        {showForm && (
          <form
            onSubmit={(e) => {
              e.preventDefault()
              create.mutate()
            }}
            className="mb-4 flex flex-col gap-3 rounded-md border border-neutral-200 p-3 dark:border-neutral-800"
          >
            <Field label="Название">
              <Input value={name} onChange={(e) => setName(e.target.value)} required autoFocus />
            </Field>
            <Field label="Описание">
              <Textarea rows={2} value={description} onChange={(e) => setDescription(e.target.value)} />
            </Field>
            <p className="text-xs text-neutral-500">
              Части тела (голова, торс, руки, ноги) добавятся автоматически по дефолтному шаблону человека — их
              можно будет изменить, удалить или добавить свои на странице шаблона после создания.
            </p>
            {create.error && (
              <ErrorText>{create.error instanceof ApiError ? create.error.message : 'Не удалось создать шаблон тела'}</ErrorText>
            )}
            <div className="flex gap-2">
              <Button type="submit" disabled={create.isPending}>
                Создать
              </Button>
              <Button type="button" variant="secondary" onClick={() => setShowForm(false)}>
                Отмена
              </Button>
            </div>
          </form>
        )}

        {bodyTemplates.isLoading && <Spinner />}
        {bodyTemplates.data && bodyTemplates.data.items.length === 0 && (
          <p className="text-sm text-neutral-500">Шаблонов тела пока нет.</p>
        )}
        <div className="flex flex-col gap-2">
          {bodyTemplates.data?.items.map((bt) => (
            <div key={bt.id} className="flex items-center justify-between gap-2 text-sm">
              <Link to={`/games/${id}/body-templates/${bt.id}`} className="hover:text-violet-600">
                {bt.name} <span className="text-neutral-400">— {bt.parts.length} частей тела</span>
              </Link>
              <ConfirmButton
                className="px-2 py-1"
                confirmMessage={`Удалить шаблон тела "${bt.name}"? Связанные шаблоны существ тоже удалятся.`}
                onConfirm={() => remove.mutate(bt.id)}
                disabled={remove.isPending}
              >
                Удалить
              </ConfirmButton>
            </div>
          ))}
        </div>

        {bodyTemplates.data && (
          <Pagination
            pageNumber={bodyTemplates.data.pageNumber}
            pageSize={bodyTemplates.data.pageSize}
            totalCount={bodyTemplates.data.totalCount}
            onPageChange={setPage}
          />
        )}
      </Card>
    </div>
  )
}
