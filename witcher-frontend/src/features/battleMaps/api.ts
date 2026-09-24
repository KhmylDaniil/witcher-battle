import { api, buildQuery } from '../../lib/apiClient'
import type {
  BattleMap,
  BattleMapSummary,
  CreateBattleMapFormValues,
  HexPaint,
  PagedResult,
  PagingParams,
  UpdateBattleMapFormValues,
} from '../../types/api'

export const battleMapsApi = {
  list: (filters: { gameId?: number } = {}, paging: PagingParams = {}) =>
    api.get<PagedResult<BattleMapSummary>>(`/api/battle-maps${buildQuery({ ...filters, ...paging })}`),
  get: (id: number) => api.get<BattleMap>(`/api/battle-maps/${id}`),
  create: (gameId: number, payload: CreateBattleMapFormValues) =>
    api.post<BattleMap>('/api/battle-maps', { ...payload, gameId }),
  update: (id: number, payload: UpdateBattleMapFormValues) => api.put<BattleMap>(`/api/battle-maps/${id}`, payload),
  paintHexes: (id: number, hexes: HexPaint[]) => api.put<BattleMap>(`/api/battle-maps/${id}/hexes`, { hexes }),
  remove: (id: number) => api.delete<void>(`/api/battle-maps/${id}`),
}
