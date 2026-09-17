import { api, buildQuery } from '../../lib/apiClient'
import type { AbilityFormValues, Character, CharacterFormValues, Condition, PagedResult, PagingParams, Skill } from '../../types/api'

export const charactersApi = {
  list: (filters: { name?: string; gameId?: number } = {}, paging: PagingParams = {}) =>
    api.get<PagedResult<Character>>(`/api/characters${buildQuery({ ...filters, ...paging })}`),
  get: (id: number) => api.get<Character>(`/api/characters/${id}`),
  /** Персонажи всех игроков этой игры — доступно только мастеру (например, для добавления в бой). */
  gameCharacters: (gameId: number) => api.get<Character[]>(`/api/games/${gameId}/characters`),
  create: (gameId: number, payload: CharacterFormValues) => api.post<Character>('/api/characters', { ...payload, gameId }),
  update: (id: number, payload: CharacterFormValues) => api.put<Character>(`/api/characters/${id}`, payload),
  remove: (id: number) => api.delete<void>(`/api/characters/${id}`),

  uploadImage: (id: number, file: File) => api.upload<Character>(`/api/characters/${id}/image`, file),
  removeImage: (id: number) => api.delete<Character>(`/api/characters/${id}/image`),

  upsertSkill: (characterId: number, skill: Skill, value: number) =>
    api.put<void>(`/api/characters/${characterId}/skills`, { skill, value }),
  deleteSkill: (characterId: number, skill: Skill) => api.delete<void>(`/api/characters/${characterId}/skills/${skill}`),

  addAbility: (characterId: number, payload: AbilityFormValues) =>
    api.post<Character>(`/api/characters/${characterId}/abilities`, payload),
  updateAbility: (characterId: number, abilityId: number, payload: AbilityFormValues) =>
    api.put<Character>(`/api/characters/${characterId}/abilities/${abilityId}`, payload),
  removeAbility: (characterId: number, abilityId: number) =>
    api.delete<Character>(`/api/characters/${characterId}/abilities/${abilityId}`),

  addCondition: (characterId: number, abilityId: number, condition: Condition, applyChance: number) =>
    api.post<Character>(`/api/characters/${characterId}/abilities/${abilityId}/conditions`, { condition, applyChance }),
  updateCondition: (characterId: number, abilityId: number, conditionId: number, condition: Condition, applyChance: number) =>
    api.put<Character>(`/api/characters/${characterId}/abilities/${abilityId}/conditions/${conditionId}`, { condition, applyChance }),
  removeCondition: (characterId: number, abilityId: number, conditionId: number) =>
    api.delete<Character>(`/api/characters/${characterId}/abilities/${abilityId}/conditions/${conditionId}`),

  addDefensiveSkill: (characterId: number, abilityId: number, skill: Skill) =>
    api.post<Character>(`/api/characters/${characterId}/abilities/${abilityId}/defensive-skills`, { skill }),
  removeDefensiveSkill: (characterId: number, abilityId: number, defensiveSkillId: number) =>
    api.delete<Character>(`/api/characters/${characterId}/abilities/${abilityId}/defensive-skills/${defensiveSkillId}`),
}
