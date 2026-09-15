import { api, buildQuery } from '../../lib/apiClient'
import type { Game, GameFormValues, GameJoinRequest } from '../../types/api'

export const gamesApi = {
  list: (filters: { name?: string } = {}) => api.get<Game[]>(`/api/games${buildQuery(filters)}`),
  mine: () => api.get<Game[]>('/api/games/mine'),
  get: (id: number) => api.get<Game>(`/api/games/${id}`),
  create: (payload: GameFormValues) => api.post<Game>('/api/games', payload),
  update: (id: number, payload: GameFormValues) => api.put<Game>(`/api/games/${id}`, payload),
  remove: (id: number) => api.delete<void>(`/api/games/${id}`),

  requestJoin: (gameId: number) => api.post<GameJoinRequest>(`/api/games/${gameId}/join-requests`),
  incomingRequests: (gameId: number) => api.get<GameJoinRequest[]>(`/api/games/${gameId}/join-requests`),
  acceptRequest: (gameId: number, requestId: number) => api.post<void>(`/api/games/${gameId}/join-requests/${requestId}/accept`),
  declineRequest: (gameId: number, requestId: number) => api.post<void>(`/api/games/${gameId}/join-requests/${requestId}/decline`),
  myRequests: () => api.get<GameJoinRequest[]>('/api/join-requests/mine'),
}
