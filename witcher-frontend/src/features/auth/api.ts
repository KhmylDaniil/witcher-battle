import { api } from '../../lib/apiClient'
import type { CurrentUser } from '../../types/api'

export interface RegisterPayload {
  name: string
  email?: string
  login: string
  password: string
}

export interface LoginPayload {
  login: string
  password: string
}

export const authApi = {
  me: () => api.get<CurrentUser>('/api/auth/me'),
  login: (payload: LoginPayload) => api.post<void>('/api/auth/login', payload),
  register: (payload: RegisterPayload) => api.post<void>('/api/auth/register', payload),
  logout: () => api.post<void>('/api/auth/logout'),
}
