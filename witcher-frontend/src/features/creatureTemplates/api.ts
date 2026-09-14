import { api, buildQuery } from '../../lib/apiClient'
import type {
  CreatureTemplateDetails,
  CreatureTemplateFormValues,
  CreatureTemplateListItem,
  DamageType,
  DamageTypeModifierValue,
  Skill,
} from '../../types/api'

export interface PartArmorPayload {
  id?: string | null
  name?: string
  armorValue: number
}

export interface SkillUpsertPayload {
  id?: string | null
  skill: Skill
  value: number
}

export interface DamageModifierPayload {
  damageType: DamageType
  damageTypeModifier: DamageTypeModifierValue
}

export const creatureTemplatesApi = {
  list: (gameId: string, filters: { name?: string } = {}) =>
    api.get<CreatureTemplateListItem[]>(`/api/games/${gameId}/creature-templates${buildQuery(filters)}`),
  get: (gameId: string, id: string) => api.get<CreatureTemplateDetails>(`/api/games/${gameId}/creature-templates/${id}`),
  create: (gameId: string, payload: CreatureTemplateFormValues) =>
    api.post<{ id: string }>(`/api/games/${gameId}/creature-templates`, payload),
  update: (gameId: string, id: string, payload: CreatureTemplateFormValues) =>
    api.put<void>(`/api/games/${gameId}/creature-templates/${id}`, payload),
  remove: (gameId: string, id: string, name: string) =>
    api.delete<void>(`/api/games/${gameId}/creature-templates/${id}${buildQuery({ name })}`),

  editPart: (gameId: string, creatureTemplateId: string, payload: PartArmorPayload) =>
    api.put<void>(`/api/games/${gameId}/creature-templates/${creatureTemplateId}/parts`, payload),

  upsertSkill: (gameId: string, creatureTemplateId: string, payload: SkillUpsertPayload) =>
    api.put<void>(`/api/games/${gameId}/creature-templates/${creatureTemplateId}/skills`, payload),
  deleteSkill: (gameId: string, creatureTemplateId: string, skillId: string) =>
    api.delete<void>(`/api/games/${gameId}/creature-templates/${creatureTemplateId}/skills/${skillId}`),

  editDamageModifier: (gameId: string, creatureTemplateId: string, payload: DamageModifierPayload) =>
    api.put<void>(`/api/games/${gameId}/creature-templates/${creatureTemplateId}/damage-modifiers`, payload),
}
