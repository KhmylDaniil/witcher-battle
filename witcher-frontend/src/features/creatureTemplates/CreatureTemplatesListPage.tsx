import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useParams } from 'react-router-dom'
import { Button, Card, ConfirmButton, PageHeader, Pagination, Spinner } from '../../components/ui'
import { creatureTemplatesApi } from './api'

/** Страница "Шаблоны существ" игры — доступна только мастеру (бэкенд скоупит список по владельцу игры). */
export function CreatureTemplatesListPage() {
  const { gameId } = useParams<{ gameId: string }>()
  const id = Number(gameId)
  const queryClient = useQueryClient()
  const [page, setPage] = useState(1)

  const creatureTemplates = useQuery({
    queryKey: ['creature-templates', { gameId: id }, page],
    queryFn: () => creatureTemplatesApi.list({ gameId: id }, { pageNumber: page }),
  })

  const remove = useMutation({
    mutationFn: (creatureTemplateId: number) => creatureTemplatesApi.remove(creatureTemplateId),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['creature-templates', { gameId: id }] }),
  })

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title="Шаблоны существ"
        actions={
          <Link to={`/games/${id}`}>
            <Button variant="secondary">К игре</Button>
          </Link>
        }
      />

      <Card>
        <div className="mb-3 flex items-center justify-between">
          <h2 className="font-semibold">Список</h2>
          <Link to={`/games/${id}/creature-templates/new`}>
            <Button className="px-2 py-1 text-xs">Создать</Button>
          </Link>
        </div>

        {creatureTemplates.isLoading && <Spinner />}
        {creatureTemplates.data && creatureTemplates.data.items.length === 0 && (
          <p className="text-sm text-neutral-500">Шаблонов существ пока нет.</p>
        )}
        <div className="flex flex-col gap-2">
          {creatureTemplates.data?.items.map((ct) => (
            <div key={ct.id} className="flex items-center justify-between gap-2 text-sm">
              <Link to={`/games/${id}/creature-templates/${ct.id}`} className="hover:text-violet-600">
                {ct.name} <span className="text-neutral-400">({ct.creatureType}, HP {ct.hp})</span>
              </Link>
              <ConfirmButton
                className="px-2 py-1"
                confirmMessage={`Удалить шаблон существа "${ct.name}"?`}
                onConfirm={() => remove.mutate(ct.id)}
                disabled={remove.isPending}
              >
                Удалить
              </ConfirmButton>
            </div>
          ))}
        </div>

        {creatureTemplates.data && (
          <Pagination
            pageNumber={creatureTemplates.data.pageNumber}
            pageSize={creatureTemplates.data.pageSize}
            totalCount={creatureTemplates.data.totalCount}
            onPageChange={setPage}
          />
        )}
      </Card>
    </div>
  )
}
