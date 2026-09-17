import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { Link } from 'react-router-dom'
import { Card, PageHeader, Pagination, Spinner } from '../../components/ui'
import { charactersApi } from './api'

const STATS = ['int', 'str', 'rea', 'dex', 'cra', 'emp', 'wil'] as const

export function CharactersListPage() {
  const [page, setPage] = useState(1)
  // Без фильтров бэкенд уже скоупит список на персонажей текущего пользователя —
  // сюда попадают и персонажи из живых игр, и архивные (у которых игру снёс мастер).
  const characters = useQuery({ queryKey: ['characters', page], queryFn: () => charactersApi.list({}, { pageNumber: page }) })

  return (
    <div className="flex flex-col gap-4">
      <PageHeader title="Мои персонажи" />

      {characters.isLoading && <Spinner />}
      {characters.data && characters.data.items.length === 0 && (
        <p className="text-sm text-neutral-500">У вас пока нет персонажей — создайте их на странице игры.</p>
      )}

      <div className="grid gap-3 sm:grid-cols-2 lg:grid-cols-3">
        {characters.data?.items.map((c) => (
          <Link key={c.id} to={`/characters/${c.id}`}>
            <Card className="h-full transition hover:border-violet-400">
              <div className="mb-2 flex items-center justify-between gap-2">
                <h3 className="font-semibold">{c.name}</h3>
                {!c.gameId && (
                  <span className="rounded-full bg-amber-100 px-2 py-0.5 text-xs font-medium text-amber-700 dark:bg-amber-950 dark:text-amber-300">
                    Архив
                  </span>
                )}
              </div>
              <div className="grid grid-cols-4 gap-1 text-xs text-neutral-500">
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

      {characters.data && (
        <Pagination
          pageNumber={characters.data.pageNumber}
          pageSize={characters.data.pageSize}
          totalCount={characters.data.totalCount}
          onPageChange={setPage}
        />
      )}
    </div>
  )
}
