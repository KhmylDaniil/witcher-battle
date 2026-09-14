import { useQuery } from '@tanstack/react-query'
import { Link } from 'react-router-dom'
import { Button, Card, PageHeader, Spinner } from '../../components/ui'
import { useGameId } from '../../routes/GameLayout'
import { creatureTemplatesApi } from './api'

export function CreatureTemplatesListPage() {
  const gameId = useGameId()
  const templates = useQuery({
    queryKey: ['creature-templates', gameId],
    queryFn: () => creatureTemplatesApi.list(gameId),
  })

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title="Шаблоны существ"
        actions={
          <Link to={`/games/${gameId}/creature-templates/new`}>
            <Button>Создать</Button>
          </Link>
        }
      />

      {templates.isLoading && <Spinner />}
      {templates.data && templates.data.length === 0 && <p className="text-neutral-500">Пока нет ни одного шаблона.</p>}

      <div className="grid gap-3 sm:grid-cols-2 lg:grid-cols-3">
        {templates.data?.map((t) => (
          <Link key={t.id} to={`/games/${gameId}/creature-templates/${t.id}`}>
            <Card className="h-full transition hover:border-violet-400">
              <h2 className="font-semibold">{t.name}</h2>
              <p className="mt-1 text-xs text-neutral-400">{t.creatureType} · {t.bodyTemplateName}</p>
              <p className="mt-2 text-sm text-neutral-500">{t.description || 'Без описания'}</p>
            </Card>
          </Link>
        ))}
      </div>
    </div>
  )
}
