import { api, buildQuery } from '../../lib/apiClient'
import type {
  AbilityFormValues,
  Condition,
  CreatureTemplate,
  CreatureTemplateFormValues,
  DamageType,
  DamageTypeModifierKind,
  PagedResult,
  PagingParams,
  Skill,
} from '../../types/api'

export const creatureTemplatesApi = {
  list: (filters: { gameId?: number } = {}, paging: PagingParams = {}) =>
    api.get<PagedResult<CreatureTemplate>>(`/api/creature-templates${buildQuery({ ...filters, ...paging })}`),
  get: (id: number) => api.get<CreatureTemplate>(`/api/creature-templates/${id}`),
  create: (gameId: number, payload: CreatureTemplateFormValues) =>
    api.post<CreatureTemplate>('/api/creature-templates', { ...payload, gameId }),
  update: (id: number, payload: CreatureTemplateFormValues) =>
    api.put<CreatureTemplate>(`/api/creature-templates/${id}`, payload),
  remove: (id: number) => api.delete<void>(`/api/creature-templates/${id}`),

  updatePartArmor: (creatureTemplateId: number, partId: number, armor: number) =>
    api.put<CreatureTemplate>(`/api/creature-templates/${creatureTemplateId}/parts/${partId}/armor`, { armor }),

  upsertSkill: (creatureTemplateId: number, skill: Skill, value: number) =>
    api.put<void>(`/api/creature-templates/${creatureTemplateId}/skills`, { skill, value }),
  deleteSkill: (creatureTemplateId: number, skill: Skill) =>
    api.delete<void>(`/api/creature-templates/${creatureTemplateId}/skills/${skill}`),

  setDamageTypeModifier: (creatureTemplateId: number, damageType: DamageType, modifier: DamageTypeModifierKind) =>
    api.put<void>(`/api/creature-templates/${creatureTemplateId}/damage-type-modifiers`, { damageType, modifier }),
  removeDamageTypeModifier: (creatureTemplateId: number, damageType: DamageType) =>
    api.delete<void>(`/api/creature-templates/${creatureTemplateId}/damage-type-modifiers/${damageType}`),

  addAbility: (creatureTemplateId: number, payload: AbilityFormValues) =>
    api.post<CreatureTemplate>(`/api/creature-templates/${creatureTemplateId}/abilities`, payload),
  updateAbility: (creatureTemplateId: number, abilityId: number, payload: AbilityFormValues) =>
    api.put<CreatureTemplate>(`/api/creature-templates/${creatureTemplateId}/abilities/${abilityId}`, payload),
  removeAbility: (creatureTemplateId: number, abilityId: number) =>
    api.delete<CreatureTemplate>(`/api/creature-templates/${creatureTemplateId}/abilities/${abilityId}`),

  addCondition: (creatureTemplateId: number, abilityId: number, condition: Condition, applyChance: number) =>
    api.post<CreatureTemplate>(`/api/creature-templates/${creatureTemplateId}/abilities/${abilityId}/conditions`, { condition, applyChance }),
  updateCondition: (creatureTemplateId: number, abilityId: number, conditionId: number, condition: Condition, applyChance: number) =>
    api.put<CreatureTemplate>(
      `/api/creature-templates/${creatureTemplateId}/abilities/${abilityId}/conditions/${conditionId}`,
      { condition, applyChance },
    ),
  removeCondition: (creatureTemplateId: number, abilityId: number, conditionId: number) =>
    api.delete<CreatureTemplate>(`/api/creature-templates/${creatureTemplateId}/abilities/${abilityId}/conditions/${conditionId}`),

  addDefensiveSkill: (creatureTemplateId: number, abilityId: number, skill: Skill) =>
    api.post<CreatureTemplate>(`/api/creature-templates/${creatureTemplateId}/abilities/${abilityId}/defensive-skills`, { skill }),
  removeDefensiveSkill: (creatureTemplateId: number, abilityId: number, defensiveSkillId: number) =>
    api.delete<CreatureTemplate>(`/api/creature-templates/${creatureTemplateId}/abilities/${abilityId}/defensive-skills/${defensiveSkillId}`),
}
