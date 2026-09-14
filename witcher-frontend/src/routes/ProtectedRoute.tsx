import { Navigate, Outlet } from 'react-router-dom'
import { Spinner } from '../components/ui'
import { useCurrentUser } from '../features/auth/useAuth'

export function ProtectedRoute() {
  const { data: user, isLoading } = useCurrentUser()

  if (isLoading) return <Spinner />
  if (!user) return <Navigate to="/login" replace />

  return <Outlet />
}
