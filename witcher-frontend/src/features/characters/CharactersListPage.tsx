import { useQuery } from '@tanstack/react-query'
import { Link } from 'react-router-dom'
import { Button, Card, PageHeader, Spinner } from '../../components/ui'
import { charactersApi } from './api'

const STATS = ['int', 'str', 'rea', 'dex', 'cra', 'emp', 'wil'] as const

export function CharactersListPage() {
  const characters = useQuery({ queryKey: ['characters'], queryFn: () => charactersApi.list() })

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title="Персонажи"
        actions={
          <Link to="/characters/new">
            <Button>Создать</Button>
          </Link>
        }
      />

      {characters.isLoading && <Spinner />}
      {characters.data && characters.data.length === 0 && <p className="text-neutral-500">Пока нет ни одного персонажа.</p>}

      <div className="grid gap-3 sm:grid-cols-2 lg:grid-cols-3">
        {characters.data?.map((c) => (
          <Link key={c.id} to={`/characters/${c.id}`}>
            <Card className="h-full transition hover:border-violet-400">
              <h2 className="font-semibold">{c.name}</h2>
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
    </div>
  )
}
