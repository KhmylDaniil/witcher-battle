import { api, buildQuery } from '../../lib/apiClient'
import type {
  AttackPayload,
  AttackType,
  FormAttackState,
  FormHealState,
  HealPayload,
  MakeTurnState,
  RunBattleState,
} from '../../types/api'

const base = (gameId: string, battleId: string) => `/api/games/${gameId}/battles/${battleId}`

export const runBattleApi = {
  run: (gameId: string, battleId: string) => api.get<RunBattleState>(`${base(gameId, battleId)}/run`),

  makeTurn: (gameId: string, battleId: string, creatureId: string) =>
    api.get<MakeTurnState>(`${base(gameId, battleId)}/turn${buildQuery({ creatureId })}`),

  formAttack: (
    gameId: string,
    battleId: string,
    params: { attackerId: string; targetId: string; attackFormulaId: string; attackType: AttackType },
  ) => api.get<FormAttackState>(`${base(gameId, battleId)}/form-attack${buildQuery(params)}`),

  attack: (gameId: string, battleId: string, payload: AttackPayload) => api.post<void>(`${base(gameId, battleId)}/attack`, payload),

  formHeal: (gameId: string, battleId: string, targetCreatureId: string) =>
    api.get<FormHealState>(`${base(gameId, battleId)}/form-heal${buildQuery({ targetCreatureId })}`),

  heal: (gameId: string, battleId: string, payload: HealPayload) => api.post<void>(`${base(gameId, battleId)}/heal`, payload),

  passInMultiAttack: (gameId: string, battleId: string, creatureId: string) =>
    api.post<void>(`${base(gameId, battleId)}/pass-in-multiattack`, creatureId),

  endTurn: (gameId: string, battleId: string, creatureId: string) =>
    api.post<void>(`${base(gameId, battleId)}/end-turn`, creatureId),
}
