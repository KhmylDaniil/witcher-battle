import { api, buildQuery } from '../../lib/apiClient'
import type {
  BattleMap,
  BattleMapView,
  BattleMapSummary,
  CreateBattleMapFormValues,
  HexPaint,
  PagedResult,
  PagingParams,
  ParticipantKind,
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
  setMarker: (id: number, column: number, row: number, text: string) =>
    api.put<BattleMap>(`/api/battle-maps/${id}/hexes/${column}/${row}/marker`, { text }),
  removeMarker: (id: number, column: number, row: number) =>
    api.delete<BattleMap>(`/api/battle-maps/${id}/hexes/${column}/${row}/marker`),
}

/** Карта в бою: подключение карты к бою и расстановка участников по гексам. */
export const battleMapPlacementApi = {
  get: (gameId: number, battleId: number) => api.get<BattleMapView>(`/api/games/${gameId}/battles/${battleId}/map`),
  attach: (gameId: number, battleId: number, battleMapId: number | null) =>
    api.put<BattleMapView>(`/api/games/${gameId}/battles/${battleId}/map`, { battleMapId }),
  place: (gameId: number, battleId: number, kind: ParticipantKind, participantId: number, column: number, row: number) =>
    api.put<BattleMapView>(`/api/games/${gameId}/battles/${battleId}/map/participants/${kind}/${participantId}`, { column, row }),
  remove: (gameId: number, battleId: number, kind: ParticipantKind, participantId: number) =>
    api.delete<BattleMapView>(`/api/games/${gameId}/battles/${battleId}/map/participants/${kind}/${participantId}`),
}
