import { api, buildQuery } from '../../lib/apiClient'
import type { GameDetails, GameListItem } from '../../types/api'

export interface CreateGamePayload {
  name: string
  description: string
}

export const gamesApi = {
  list: (filters: { name?: string } = {}) => api.get<GameListItem[]>(`/api/games${buildQuery(filters)}`),
  get: (gameId: string) => api.get<GameDetails>(`/api/games/${gameId}`),
  create: (payload: CreateGamePayload) => api.post<void>('/api/games', payload),
  askForJoin: (gameId: string, message: string) => api.post<void>('/api/games/join', { gameId, message }),
  changeMemberRole: (gameId: string, userId: string) => api.put<void>(`/api/games/${gameId}/members/${userId}`),
  removeMember: (gameId: string, userId: string, name: string) =>
    api.delete<void>(`/api/games/${gameId}/members/${userId}${buildQuery({ name })}`),
}
