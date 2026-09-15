import { api, buildQuery } from '../../lib/apiClient'
import type { Character, CharacterFormValues, Skill } from '../../types/api'

export const charactersApi = {
  list: (filters: { name?: string } = {}) => api.get<Character[]>(`/api/characters${buildQuery(filters)}`),
  get: (id: number) => api.get<Character>(`/api/characters/${id}`),
  create: (payload: CharacterFormValues) => api.post<Character>('/api/characters', payload),
  update: (id: number, payload: CharacterFormValues) => api.put<Character>(`/api/characters/${id}`, payload),
  remove: (id: number) => api.delete<void>(`/api/characters/${id}`),

  upsertSkill: (characterId: number, skill: Skill, value: number) =>
    api.put<void>(`/api/characters/${characterId}/skills`, { skill, value }),
  deleteSkill: (characterId: number, skill: Skill) => api.delete<void>(`/api/characters/${characterId}/skills/${skill}`),
}
