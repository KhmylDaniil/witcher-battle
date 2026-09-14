import { Link, Outlet } from 'react-router-dom'
import { Button } from '../components/ui'
import { useCurrentUser, useLogout } from '../features/auth/useAuth'

export function AppLayout() {
  const { data: user } = useCurrentUser()
  const logout = useLogout()

  return (
    <div className="min-h-screen">
      <header className="flex items-center justify-between border-b border-neutral-200 px-4 py-3 dark:border-neutral-800">
        <Link to="/games" className="font-semibold">
          Witcher Battle
        </Link>
        {user && (
          <Button variant="secondary" onClick={() => logout.mutate()} disabled={logout.isPending}>
            Выйти
          </Button>
        )}
      </header>
      <main className="mx-auto max-w-5xl px-4 py-6">
        <Outlet />
      </main>
    </div>
  )
}
