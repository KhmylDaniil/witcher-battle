import { api } from '../../lib/apiClient'
import type {
  AddCreatureToBattleFormValues,
  Battle,
  BattleFormValues,
  Condition,
  UpdateBattleCreatureFormValues,
} from '../../types/api'

export const battlesApi = {
  list: (gameId: number) => api.get<Battle[]>(`/api/games/${gameId}/battles`),
  get: (gameId: number, id: number) => api.get<Battle>(`/api/games/${gameId}/battles/${id}`),
  create: (gameId: number, payload: BattleFormValues) => api.post<Battle>(`/api/games/${gameId}/battles`, payload),
  remove: (gameId: number, id: number) => api.delete<void>(`/api/games/${gameId}/battles/${id}`),

  addCreature: (gameId: number, battleId: number, payload: AddCreatureToBattleFormValues) =>
    api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/creatures`, payload),
  updateCreature: (gameId: number, battleId: number, creatureId: number, payload: UpdateBattleCreatureFormValues) =>
    api.put<Battle>(`/api/games/${gameId}/battles/${battleId}/creatures/${creatureId}`, payload),
  removeCreature: (gameId: number, battleId: number, creatureId: number) =>
    api.delete<Battle>(`/api/games/${gameId}/battles/${battleId}/creatures/${creatureId}`),
  addCreatureCondition: (gameId: number, battleId: number, creatureId: number, condition: Condition) =>
    api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/creatures/${creatureId}/conditions`, { condition }),
  removeCreatureCondition: (gameId: number, battleId: number, creatureId: number, condition: Condition) =>
    api.delete<Battle>(`/api/games/${gameId}/battles/${battleId}/creatures/${creatureId}/conditions/${condition}`),

  addCharacter: (gameId: number, battleId: number, characterId: number) =>
    api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/characters`, { characterId }),
  removeCharacter: (gameId: number, battleId: number, characterId: number) =>
    api.delete<Battle>(`/api/games/${gameId}/battles/${battleId}/characters/${characterId}`),
  addCharacterCondition: (gameId: number, battleId: number, characterId: number, condition: Condition) =>
    api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/characters/${characterId}/conditions`, { condition }),
  removeCharacterCondition: (gameId: number, battleId: number, characterId: number, condition: Condition) =>
    api.delete<Battle>(`/api/games/${gameId}/battles/${battleId}/characters/${characterId}/conditions/${condition}`),

  start: (gameId: number, battleId: number) => api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/start`),
}
