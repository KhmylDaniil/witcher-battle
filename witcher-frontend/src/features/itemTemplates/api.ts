import { api, buildQuery } from '../../lib/apiClient'
import type { Condition, ItemTemplate, ItemTemplateFormValues, PagedResult, PagingParams } from '../../types/api'

export const itemTemplatesApi = {
  list: (filters: { gameId?: number } = {}, paging: PagingParams = {}) =>
    api.get<PagedResult<ItemTemplate>>(`/api/item-templates${buildQuery({ ...filters, ...paging })}`),
  get: (id: number) => api.get<ItemTemplate>(`/api/item-templates/${id}`),
  create: (gameId: number, payload: ItemTemplateFormValues) =>
    api.post<ItemTemplate>('/api/item-templates', { ...payload, gameId }),
  update: (id: number, payload: ItemTemplateFormValues) => api.put<ItemTemplate>(`/api/item-templates/${id}`, payload),
  remove: (id: number) => api.delete<void>(`/api/item-templates/${id}`),

  addCondition: (itemTemplateId: number, condition: Condition, applyChance: number) =>
    api.post<ItemTemplate>(`/api/item-templates/${itemTemplateId}/applied-conditions`, { condition, applyChance }),
  updateCondition: (itemTemplateId: number, conditionId: number, condition: Condition, applyChance: number) =>
    api.put<ItemTemplate>(`/api/item-templates/${itemTemplateId}/applied-conditions/${conditionId}`, { condition, applyChance }),
  removeCondition: (itemTemplateId: number, conditionId: number) =>
    api.delete<ItemTemplate>(`/api/item-templates/${itemTemplateId}/applied-conditions/${conditionId}`),
}
