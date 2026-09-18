import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useParams } from 'react-router-dom'
import { Button, Card, ConfirmButton, PageHeader, Pagination, Spinner } from '../../components/ui'
import { itemTemplatesApi } from './api'

/** Страница "Шаблоны предметов" игры — доступна только мастеру (бэкенд скоупит список по владельцу игры). */
export function ItemTemplatesListPage() {
  const { gameId } = useParams<{ gameId: string }>()
  const id = Number(gameId)
  const queryClient = useQueryClient()
  const [page, setPage] = useState(1)

  const itemTemplates = useQuery({
    queryKey: ['item-templates', { gameId: id }, page],
    queryFn: () => itemTemplatesApi.list({ gameId: id }, { pageNumber: page }),
  })

  const remove = useMutation({
    mutationFn: (itemTemplateId: number) => itemTemplatesApi.remove(itemTemplateId),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['item-templates', { gameId: id }] }),
  })

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title="Шаблоны предметов"
        actions={
          <Link to={`/games/${id}`}>
            <Button variant="secondary">К игре</Button>
          </Link>
        }
      />

      <Card>
        <div className="mb-3 flex items-center justify-between">
          <h2 className="font-semibold">Список</h2>
          <Link to={`/games/${id}/item-templates/new`}>
            <Button className="px-2 py-1 text-xs">Создать</Button>
          </Link>
        </div>

        {itemTemplates.isLoading && <Spinner />}
        {itemTemplates.data && itemTemplates.data.items.length === 0 && (
          <p className="text-sm text-neutral-500">Шаблонов предметов пока нет.</p>
        )}
        <div className="flex flex-col gap-2">
          {itemTemplates.data?.items.map((it) => (
            <div key={it.id} className="flex items-center justify-between gap-2 text-sm">
              <Link to={`/games/${id}/item-templates/${it.id}`} className="hover:text-violet-600">
                {it.name} <span className="text-neutral-400">({it.itemType}, вес {it.weight}, цена {it.cost})</span>
              </Link>
              <ConfirmButton
                className="px-2 py-1"
                confirmMessage={`Удалить шаблон предмета "${it.name}"?`}
                onConfirm={() => remove.mutate(it.id)}
                disabled={remove.isPending}
              >
                Удалить
              </ConfirmButton>
            </div>
          ))}
        </div>

        {itemTemplates.data && (
          <Pagination
            pageNumber={itemTemplates.data.pageNumber}
            pageSize={itemTemplates.data.pageSize}
            totalCount={itemTemplates.data.totalCount}
            onPageChange={setPage}
          />
        )}
      </Card>
    </div>
  )
}
