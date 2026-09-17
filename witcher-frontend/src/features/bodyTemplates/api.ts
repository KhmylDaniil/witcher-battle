import { api, buildQuery } from '../../lib/apiClient'
import type { BodyTemplate, BodyTemplateFormValues, BodyTemplatePartFormValues, PagedResult, PagingParams } from '../../types/api'

export const bodyTemplatesApi = {
  list: (filters: { gameId?: number } = {}, paging: PagingParams = {}) =>
    api.get<PagedResult<BodyTemplate>>(`/api/body-templates${buildQuery({ ...filters, ...paging })}`),
  get: (id: number) => api.get<BodyTemplate>(`/api/body-templates/${id}`),
  create: (gameId: number, payload: BodyTemplateFormValues) =>
    api.post<BodyTemplate>('/api/body-templates', { ...payload, gameId }),
  update: (id: number, payload: BodyTemplateFormValues) => api.put<BodyTemplate>(`/api/body-templates/${id}`, payload),
  remove: (id: number) => api.delete<void>(`/api/body-templates/${id}`),

  addPart: (bodyTemplateId: number, payload: BodyTemplatePartFormValues) =>
    api.post<BodyTemplate>(`/api/body-templates/${bodyTemplateId}/parts`, payload),
  updatePart: (bodyTemplateId: number, partId: number, payload: BodyTemplatePartFormValues) =>
    api.put<BodyTemplate>(`/api/body-templates/${bodyTemplateId}/parts/${partId}`, payload),
  removePart: (bodyTemplateId: number, partId: number) =>
    api.delete<BodyTemplate>(`/api/body-templates/${bodyTemplateId}/parts/${partId}`),
}
