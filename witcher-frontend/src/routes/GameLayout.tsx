import { NavLink, Outlet, useParams } from 'react-router-dom'

/** gameId всегда присутствует внутри /games/:gameId/* — сюда попадаем только через ProtectedRoute > GameLayout */
export function useGameId(): string {
  const { gameId } = useParams<{ gameId: string }>()
  if (!gameId) throw new Error('useGameId() вызван вне маршрута /games/:gameId/*')
  return gameId
}

const tabClass = ({ isActive }: { isActive: boolean }) =>
  `rounded-md px-3 py-1.5 text-sm font-medium ${
    isActive ? 'bg-violet-600 text-white' : 'text-neutral-600 hover:bg-neutral-100 dark:text-neutral-400 dark:hover:bg-neutral-800'
  }`

export function GameLayout() {
  const gameId = useGameId()

  return (
    <div className="flex flex-col gap-4">
      <nav className="flex flex-wrap gap-1 border-b border-neutral-200 pb-2 dark:border-neutral-800">
        <NavLink to={`/games/${gameId}`} end className={tabClass}>
          Обзор
        </NavLink>
        <NavLink to={`/games/${gameId}/creature-templates`} className={tabClass}>
          Шаблоны существ
        </NavLink>
        <NavLink to={`/games/${gameId}/battles`} className={tabClass}>
          Бои
        </NavLink>
      </nav>
      <Outlet />
    </div>
  )
}
