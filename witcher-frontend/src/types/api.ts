// Типы зеркалят JSON-контракты Wastelands.Service.MVC/Controllers/Api/*, которые в свою очередь зеркалят
// Wastelands.Service.Domain/Models/*. Enum-ы сериализуются как строки (JsonStringEnumConverter в Program.cs),
// включая ключи Dictionary<Skill,int> — глобальный конвертер покрывает и их.
// ВАЖНО: id здесь — number (long на бэке), а не Guid/string, как было у Witcher.

export const SKILLS = [
  // Int
  'Awareness', 'Tactics', 'Survival', 'Nature', 'Education', 'Lore', 'Streetwise', 'Politics', 'Deduction',
  // Str
  'Athletics', 'Melee', 'Brawl', 'Endurance',
  // Rea
  'Sword', 'Staff', 'SmallBlades', 'Dodge',
  // Dex
  'Acrobatics', 'SleightOfHand', 'Thrown', 'Stealth', 'Crossbow', 'Archery',
  // Cra
  'Alchemy', 'Locks', 'Crafting', 'Arts', 'FirstAid',
  // Emp
  'Presence', 'Perfomance', 'Leadership', 'Deceit', 'AnimalKen', 'Insight', 'Persuasion', 'Charisma',
  // Wil
  'Intimidation', 'ResistMagic', 'ResistCoercion', 'Courage',
] as const
export type Skill = (typeof SKILLS)[number]

/** Характеристика -> навыки, ей управляемые (для группировки в UI) */
export const SKILLS_BY_STAT: Record<string, Skill[]> = {
  Int: ['Awareness', 'Tactics', 'Survival', 'Nature', 'Education', 'Lore', 'Streetwise', 'Politics', 'Deduction'],
  Str: ['Athletics', 'Melee', 'Brawl', 'Endurance'],
  Rea: ['Sword', 'Staff', 'SmallBlades', 'Dodge'],
  Dex: ['Acrobatics', 'SleightOfHand', 'Thrown', 'Stealth', 'Crossbow', 'Archery'],
  Cra: ['Alchemy', 'Locks', 'Crafting', 'Arts', 'FirstAid'],
  Emp: ['Presence', 'Perfomance', 'Leadership', 'Deceit', 'AnimalKen', 'Insight', 'Persuasion', 'Charisma'],
  Wil: ['Intimidation', 'ResistMagic', 'ResistCoercion', 'Courage'],
}

// ---- Auth (AuthApiController) ----

export interface CurrentUser {
  userId: number
}

// ---- Games (GamesApiController) ----

export type GameMembershipStatus = 'None' | 'Owner' | 'Member' | 'RequestPending' | 'Declined'

export interface Game {
  id: number
  name: string
  createdByUserId: number
  membershipStatus: GameMembershipStatus
}

export interface GameFormValues {
  name: string
}

// ---- Join requests (GameJoinRequestsApiController) ----

export type GameJoinRequestStatus = 'Pending' | 'Accepted' | 'Declined'

export interface GameJoinRequest {
  id: number
  userId: number
  gameId: number
  status: GameJoinRequestStatus
}

// ---- Characters (CharactersApiController) ----

export interface Character {
  id: number
  userId: number
  /** null — игра, в которой был создан персонаж, удалена; персонаж хранится архивно. */
  gameId: number | null
  name: string
  hp: number
  sta: number
  int: number
  str: number
  rea: number
  dex: number
  cra: number
  emp: number
  wil: number
  skills: Partial<Record<Skill, number>>
  abilities: Ability[]
}

export interface CharacterFormValues {
  name: string
  hp: number
  sta: number
  int: number
  str: number
  rea: number
  dex: number
  cra: number
  emp: number
  wil: number
}

// ---- Body templates (BodyTemplatesApiController) — доступны только мастеру игры ----

export const BODY_PART_TYPES = ['Void', 'Head', 'Torso', 'Arm', 'Leg', 'Wing', 'Tail'] as const
export type BodyPartType = (typeof BODY_PART_TYPES)[number]

export interface BodyTemplatePart {
  id: number
  name: string
  bodyPartType: BodyPartType
  damageModifier: number
  hitPenalty: number
  minToHit: number
  maxToHit: number
}

export interface BodyTemplate {
  id: number
  gameId: number
  name: string
  description: string | null
  parts: BodyTemplatePart[]
}

export interface BodyTemplateFormValues {
  name: string
  description: string
}

export interface BodyTemplatePartFormValues {
  name: string
  bodyPartType: BodyPartType
  damageModifier: number
  hitPenalty: number
  minToHit: number
  maxToHit: number
}

// ---- Creature templates (CreatureTemplatesApiController) — доступны только мастеру игры ----

export const CREATURE_TYPES = [
  'Human', 'Necrophage', 'Specter', 'Beast', 'Cursed', 'Hybrid',
  'Insectoid', 'Elementa', 'Relict', 'Orgoid', 'Draconid', 'Vampire',
] as const
export type CreatureType = (typeof CREATURE_TYPES)[number]

export interface CreatureTemplatePart {
  id: number
  name: string
  bodyPartType: BodyPartType
  damageModifier: number
  hitPenalty: number
  minToHit: number
  maxToHit: number
  armor: number
}

export interface CreatureTemplate {
  id: number
  gameId: number
  bodyTemplateId: number
  creatureType: CreatureType
  name: string
  description: string | null
  hp: number
  sta: number
  int: number
  ref: number
  dex: number
  body: number
  emp: number
  cra: number
  will: number
  speed: number
  luck: number
  parts: CreatureTemplatePart[]
  skills: Partial<Record<Skill, number>>
  damageTypeModifiers: Partial<Record<DamageType, DamageTypeModifierKind>>
  abilities: Ability[]
}

export interface CreatureTemplateFormValues {
  bodyTemplateId: number
  creatureType: CreatureType
  name: string
  description: string
  hp: number
  sta: number
  int: number
  ref: number
  dex: number
  body: number
  emp: number
  cra: number
  will: number
  speed: number
  luck: number
}

// ---- Damage types / modifiers ----

export const DAMAGE_TYPES = ['Slashing', 'Piercing', 'Bludgeoning', 'Elemental', 'Fire', 'Silver'] as const
export type DamageType = (typeof DAMAGE_TYPES)[number]

export const DAMAGE_TYPE_MODIFIERS = ['Vulnerability', 'Resistance', 'Immunity'] as const
export type DamageTypeModifierKind = (typeof DAMAGE_TYPE_MODIFIERS)[number]

// ---- Conditions (эффекты, накладываемые атакующими способностями) ----

export const CONDITIONS = [
  'Bleed', 'BleedingWound', 'Poison', 'Fire', 'Freeze', 'Stun', 'Staggered', 'Intoxication',
  'Hallutination', 'Nausea', 'Sufflocation', 'Blinded', 'Dying',

  'SimpleLeg', 'SimpleArm', 'SimpleWing', 'SimpleTail', 'SimpleHead1', 'SimpleHead2', 'SimpleTorso1', 'SimpleTorso2',
  'ComplexLeg', 'ComplexArm', 'ComplexWing', 'ComplexTail', 'ComplexHead1', 'ComplexHead2', 'ComplexTorso1', 'ComplexTorso2',
  'DifficultLeg', 'DifficultArm', 'DifficultWing', 'DifficultTail', 'DifficultHead1', 'DifficultHead2', 'DifficultTorso1', 'DifficultTorso2',
  'DeadlyLeg', 'DeadlyArm', 'DeadlyWing', 'DeadlyTail', 'DeadlyHead1', 'DeadlyHead2', 'DeadlyTorso1', 'DeadlyTorso2',
] as const
export type Condition = (typeof CONDITIONS)[number]

// ---- Abilities (атакующие способности шаблона существа) ----

export interface AbilityAppliedCondition {
  id: number
  condition: Condition
  applyChance: number
}

export interface AbilityDefensiveSkill {
  id: number
  skill: Skill
}

export interface Ability {
  id: number
  name: string
  attackSkill: Skill
  attacksPerTurn: number
  damageDiceCount: number
  damageModifier: number
  damageType: DamageType
  appliedConditions: AbilityAppliedCondition[]
  defensiveSkills: AbilityDefensiveSkill[]
}

export interface AbilityFormValues {
  name: string
  attackSkill: Skill
  attacksPerTurn: number
  damageDiceCount: number
  damageModifier: number
  damageType: DamageType
}

// ---- Battles (BattlesApiController) ----

export type BattleStatus = 'Draft' | 'InProgress'

export interface BattleCreature {
  id: number
  creatureTemplateId: number
  name: string
  creatureType: CreatureType
  maxHP: number
  currentHP: number
  maxSta: number
  currentSta: number
  initiative: number | null
  appliedConditions: Condition[]
}

export interface BattleCharacterEntry {
  id: number
  characterId: number
  characterName: string
  characterUserId: number
  maxHP: number
  currentHP: number
  maxSta: number
  currentSta: number
  initiative: number | null
  appliedConditions: Condition[]
}

export type ParticipantKind = 'Creature' | 'Character'

export type BattleAttackPhase = 'AwaitingChoices' | 'AwaitingDamageRoll' | 'SwingResolved'

export interface CreaturePartOption {
  id: number
  name: string
}

export interface BattleAttack {
  id: number
  attackerKind: ParticipantKind
  attackerId: number
  attackerName: string
  abilityId: number
  abilityName: string
  /** Справочное значение характеристика+навык атакующего — менять нельзя. */
  attackerSkillValue: number
  attacksAllowed: number
  attacksUsed: number
  defenderKind: ParticipantKind
  defenderId: number
  defenderName: string
  /** Заполнено, только если защитник — существо. */
  availableCreatureParts: CreaturePartOption[] | null
  targetedCreaturePartId: number | null
  attackRoll: number | null
  attackerConfirmed: boolean
  availableDefensiveSkills: Skill[]
  defensiveSkill: Skill | null
  defenseRoll: number | null
  defenderConfirmed: boolean
  phase: BattleAttackPhase
  lastHitSucceeded: boolean | null
  resolvedCreaturePartId: number | null
  damageRoll: number | null
}

export interface BattleLogEntry {
  id: number
  message: string
  createdAt: string
}

export interface Battle {
  id: number
  gameId: number
  name: string
  status: BattleStatus
  currentRound: number
  currentInitiative: number | null
  creatures: BattleCreature[]
  characters: BattleCharacterEntry[]
  attack: BattleAttack | null
  logEntries: BattleLogEntry[]
}

export interface BattleFormValues {
  name: string
}

export interface AddCreatureToBattleFormValues {
  creatureTemplateId: number
  name?: string
}

export interface UpdateBattleCreatureFormValues {
  name: string
  currentHP: number
  currentSta: number
}
