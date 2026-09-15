import { api, buildQuery } from '../../lib/apiClient'
import type { CreatureTemplate, CreatureTemplateFormValues } from '../../types/api'

export const creatureTemplatesApi = {
  list: (filters: { gameId?: number } = {}) => api.get<CreatureTemplate[]>(`/api/creature-templates${buildQuery(filters)}`),
  get: (id: number) => api.get<CreatureTemplate>(`/api/creature-templates/${id}`),
  create: (gameId: number, payload: CreatureTemplateFormValues) =>
    api.post<CreatureTemplate>('/api/creature-templates', { ...payload, gameId }),
  update: (id: number, payload: CreatureTemplateFormValues) =>
    api.put<CreatureTemplate>(`/api/creature-templates/${id}`, payload),
  remove: (id: number) => api.delete<void>(`/api/creature-templates/${id}`),

  updatePartArmor: (creatureTemplateId: number, partId: number, armor: number) =>
    api.put<CreatureTemplate>(`/api/creature-templates/${creatureTemplateId}/parts/${partId}/armor`, { armor }),
}
