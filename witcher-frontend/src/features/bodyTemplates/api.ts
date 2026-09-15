import { api, buildQuery } from '../../lib/apiClient'
import type { BodyTemplate, BodyTemplateFormValues } from '../../types/api'

export const bodyTemplatesApi = {
  list: (filters: { gameId?: number } = {}) => api.get<BodyTemplate[]>(`/api/body-templates${buildQuery(filters)}`),
  get: (id: number) => api.get<BodyTemplate>(`/api/body-templates/${id}`),
  create: (gameId: number, payload: BodyTemplateFormValues) =>
    api.post<BodyTemplate>('/api/body-templates', { ...payload, gameId }),
  update: (id: number, payload: BodyTemplateFormValues) => api.put<BodyTemplate>(`/api/body-templates/${id}`, payload),
  remove: (id: number) => api.delete<void>(`/api/body-templates/${id}`),
}
