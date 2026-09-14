import { api, buildQuery } from '../../lib/apiClient'
import type { BattleDetails, BattleListItem } from '../../types/api'

export interface CreateBattlePayload {
  name: string
  description: string
}

export interface CreateCreaturePayload {
  creatureTemplateId: string
  name: string
  description: string
}

export const battlesApi = {
  list: (gameId: string) => api.get<BattleListItem[]>(`/api/games/${gameId}/battles`),
  get: (gameId: string, battleId: string) => api.get<BattleDetails>(`/api/games/${gameId}/battles/${battleId}`),
  create: (gameId: string, payload: CreateBattlePayload) => api.post<{ id: string }>(`/api/games/${gameId}/battles`, payload),
  remove: (gameId: string, battleId: string, name: string) =>
    api.delete<void>(`/api/games/${gameId}/battles/${battleId}${buildQuery({ name })}`),

  addCreature: (gameId: string, battleId: string, payload: CreateCreaturePayload) =>
    api.post<void>(`/api/games/${gameId}/battles/${battleId}/creatures`, payload),
  removeCreature: (gameId: string, battleId: string, creatureId: string, name: string) =>
    api.delete<void>(`/api/games/${gameId}/battles/${battleId}/creatures/${creatureId}${buildQuery({ name })}`),

  addCharacter: (gameId: string, battleId: string, characterId: string) =>
    api.post<void>(`/api/games/${gameId}/battles/${battleId}/characters`, { characterId }),
}
