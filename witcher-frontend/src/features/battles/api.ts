import { api } from '../../lib/apiClient'
import type {
  AddCreatureToBattleFormValues,
  Battle,
  BattleFormValues,
  Character,
  Condition,
  CreatureTemplate,
  HumanBodyPart,
  ParticipantKind,
  Skill,
  UpdateBattleCreatureFormValues,
} from '../../types/api'

export const battlesApi = {
  list: (gameId: number) => api.get<Battle[]>(`/api/games/${gameId}/battles`),
  get: (gameId: number, id: number) => api.get<Battle>(`/api/games/${gameId}/battles/${id}`),
  create: (gameId: number, payload: BattleFormValues) => api.post<Battle>(`/api/games/${gameId}/battles`, payload),
  remove: (gameId: number, id: number) => api.delete<void>(`/api/games/${gameId}/battles/${id}`),

  addCreature: (gameId: number, battleId: number, payload: AddCreatureToBattleFormValues) =>
    api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/creatures`, payload),
  updateCreature: (gameId: number, battleId: number, creatureId: number, payload: UpdateBattleCreatureFormValues) =>
    api.put<Battle>(`/api/games/${gameId}/battles/${battleId}/creatures/${creatureId}`, payload),
  removeCreature: (gameId: number, battleId: number, creatureId: number) =>
    api.delete<Battle>(`/api/games/${gameId}/battles/${battleId}/creatures/${creatureId}`),
  addCreatureCondition: (gameId: number, battleId: number, creatureId: number, condition: Condition) =>
    api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/creatures/${creatureId}/conditions`, { condition }),
  removeCreatureCondition: (gameId: number, battleId: number, creatureId: number, condition: Condition) =>
    api.delete<Battle>(`/api/games/${gameId}/battles/${battleId}/creatures/${creatureId}/conditions/${condition}`),

  addCharacter: (gameId: number, battleId: number, characterId: number) =>
    api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/characters`, { characterId }),
  removeCharacter: (gameId: number, battleId: number, characterId: number) =>
    api.delete<Battle>(`/api/games/${gameId}/battles/${battleId}/characters/${characterId}`),
  addCharacterCondition: (gameId: number, battleId: number, characterId: number, condition: Condition) =>
    api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/characters/${characterId}/conditions`, { condition }),
  removeCharacterCondition: (gameId: number, battleId: number, characterId: number, condition: Condition) =>
    api.delete<Battle>(`/api/games/${gameId}/battles/${battleId}/characters/${characterId}/conditions/${condition}`),

  start: (gameId: number, battleId: number) => api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/start`),

  startAttack: (
    gameId: number,
    battleId: number,
    payload: { abilityId: number; defenderKind: ParticipantKind; defenderId: number },
  ) => api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/attacks`, payload),
  setAttackerChoices: (
    gameId: number,
    battleId: number,
    payload: { targetedCreaturePartId: number | null; targetedHumanBodyPart: HumanBodyPart | null; attackRoll: number | null },
  ) => api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/attacks/current/attacker-choices`, payload),
  confirmAttacker: (gameId: number, battleId: number) =>
    api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/attacks/current/attacker-confirm`),
  setDefenderChoice: (
    gameId: number,
    battleId: number,
    payload: { defensiveSkill: Skill; defenseRoll: number | null },
  ) => api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/attacks/current/defender-choice`, payload),
  confirmDefender: (gameId: number, battleId: number) =>
    api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/attacks/current/defender-confirm`),
  setDamageRoll: (gameId: number, battleId: number, damageRoll: number | null) =>
    api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/attacks/current/damage-roll`, { damageRoll }),
  continueDamage: (gameId: number, battleId: number) =>
    api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/attacks/current/continue`),
  setStunSaveRoll: (gameId: number, battleId: number, roll: number | null) =>
    api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/attacks/current/stun-save-roll`, { roll }),
  resolveStunSave: (gameId: number, battleId: number) =>
    api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/attacks/current/stun-save-resolve`),
  rollOwnStunSave: (gameId: number, battleId: number, roll: number | null) =>
    api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/stun-save`, { roll }),
  nextSwing: (gameId: number, battleId: number, payload: { defenderKind: ParticipantKind; defenderId: number }) =>
    api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/attacks/current/next-swing`, payload),
  endActivation: (gameId: number, battleId: number) =>
    api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/attacks/current/end`),
  skipTurn: (gameId: number, battleId: number) => api.post<Battle>(`/api/games/${gameId}/battles/${battleId}/skip-turn`),

  creatureSheet: (gameId: number, battleId: number, creatureId: number) =>
    api.get<CreatureTemplate>(`/api/games/${gameId}/battles/${battleId}/creatures/${creatureId}/sheet`),
  characterSheet: (gameId: number, battleId: number, characterId: number) =>
    api.get<Character>(`/api/games/${gameId}/battles/${battleId}/characters/${characterId}/sheet`),
}
