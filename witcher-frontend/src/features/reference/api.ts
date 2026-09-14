import { api } from '../../lib/apiClient'
import type { AbilityListItem, BodyTemplateListItem, CharacterListItem } from '../../types/api'

/** Read-only справочники для выпадающих списков в формах (ReferenceDataApiController) */
export const referenceApi = {
  bodyTemplates: (gameId: string) => api.get<BodyTemplateListItem[]>(`/api/games/${gameId}/reference/body-templates`),
  abilities: (gameId: string) => api.get<AbilityListItem[]>(`/api/games/${gameId}/reference/abilities`),
  characters: (gameId: string) => api.get<CharacterListItem[]>(`/api/games/${gameId}/reference/characters`),
}
