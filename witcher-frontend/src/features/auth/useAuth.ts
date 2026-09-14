import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { ApiError } from '../../lib/apiClient'
import { authApi, type LoginPayload, type RegisterPayload } from './api'

export const CURRENT_USER_KEY = ['auth', 'me'] as const

/** undefined = ещё не проверяли (загрузка), null = точно не аутентифицирован */
export function useCurrentUser() {
  const query = useQuery({
    queryKey: CURRENT_USER_KEY,
    queryFn: async () => {
      try {
        return await authApi.me()
      } catch (err) {
        if (err instanceof ApiError && err.status === 401) return null
        throw err
      }
    },
    staleTime: Infinity,
  })

  return { ...query, isAuthenticated: !!query.data }
}

export function useLogin() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (payload: LoginPayload) => authApi.login(payload),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: CURRENT_USER_KEY }),
  })
}

export function useRegister() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (payload: RegisterPayload) => authApi.register(payload),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: CURRENT_USER_KEY }),
  })
}

export function useLogout() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: () => authApi.logout(),
    onSuccess: () => {
      queryClient.setQueryData(CURRENT_USER_KEY, null)
      queryClient.clear()
    },
  })
}
