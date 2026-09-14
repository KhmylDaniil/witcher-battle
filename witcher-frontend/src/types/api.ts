// Типы зеркалят JSON-контракты нового API-слоя (Witcher.MVC/Controllers/Api/*), которые в свою очередь
// зеркалят Witcher.Core/Contracts/*Requests. Enum-ы на бэке сериализуются как строки (JsonStringEnumConverter
// в Startup.cs), поэтому здесь — строковые union-типы с точными именами значений из Witcher.Core/BaseData/Enums.cs.

export const CREATURE_TYPES = [
  'Human', 'Necrophage', 'Specter', 'Beast', 'Cursed', 'Hybrid',
  'Insectoid', 'Elementa', 'Relict', 'Orgoid', 'Draconid', 'Vampire',
] as const
export type CreatureType = (typeof CREATURE_TYPES)[number]

export const DAMAGE_TYPES = ['Slashing', 'Piercing', 'Bludgeoning', 'Elemental', 'Fire', 'Silver'] as const
export type DamageType = (typeof DAMAGE_TYPES)[number]

export const DAMAGE_TYPE_MODIFIERS = ['Normal', 'Vulnerability', 'Resistance', 'Immunity'] as const
export type DamageTypeModifierValue = (typeof DAMAGE_TYPE_MODIFIERS)[number]

export const BODY_PART_TYPES = ['Void', 'Head', 'Torso', 'Arm', 'Leg', 'Wing', 'Tail'] as const
export type BodyPartType = (typeof BODY_PART_TYPES)[number]

export const ATTACK_TYPES = ['Ability', 'Weapon'] as const
export type AttackType = (typeof ATTACK_TYPES)[number]

export type TurnState =
  | 'TurnNotBeginned'
  | 'ReadyForAction'
  | 'InProcessOfBaseAction'
  | 'BaseActionIsDone'
  | 'InProcessOfAdditionAction'
  | 'TurnIsDone'

export const SKILLS = [
  'Awareness', 'Business', 'Deduction', 'Education', 'CommonLanguage', 'ElderLanguage', 'DwarfenLanguage',
  'MonsterLore', 'SocialEtiquette', 'Streetwise', 'Tactics', 'Teaching', 'WildernessSurvival', 'Brawling',
  'Dodge', 'Melee', 'Riding', 'Sailing', 'SmallBlades', 'Staff', 'Sword', 'Archery', 'Athletics', 'Crossbow',
  'SleightOfHand', 'Stealth', 'Endurance', 'Physique', 'Charisma', 'Deceit', 'FineArts', 'Gambling', 'Style',
  'HumanPerception', 'Leadership', 'Persuasion', 'Perfomance', 'Seduction', 'Alchemy', 'Crafting', 'Disguise',
  'Forgery', 'PickLock', 'TrapCrafting', 'FirstAid', 'Courage', 'HexWeaving', 'Intimidation', 'Spell',
  'ResistMagic', 'ResistCoercion', 'RitualCrafting', 'Needling', 'EyeGouge', 'BleedingWound', 'HealingHands',
] as const
export type Skill = (typeof SKILLS)[number]

// ---- Auth (AuthApiController) ----

export interface CurrentUser {
  userId: string
  role: string
}

// ---- Games (GamesApiController) ----

export interface GameListItem {
  id: string
  name: string
  avatarId: string | null
  description: string
  users: Record<string, string>
  textFiles: string[]
  imgFiles: string[]
}

export interface GameMember {
  userId: string
  name: string
  roleName: string
}

export interface GameDetails {
  id: string
  gameMasterName: string
  name: string
  avatarId: string | null
  description: string
  members: GameMember[]
  textFiles: string[]
  imgFiles: string[]
}

// ---- Reference data (ReferenceDataApiController) ----

export interface BodyTemplateListItem {
  id: string
  name: string
  description: string
}

export interface AbilityListItem {
  id: string
  name: string
  description: string
  attackSkill: Skill
  damageType: DamageType
  attackDiceQuantity: number
  damageModifier: number
  attackSpeed: number
  accuracy: number
}

export interface CharacterListItem {
  id: string
  name: string
  ownerName: string
}

// ---- Creature templates (CreatureTemplatesApiController) ----

export interface CreatureTemplateListItem {
  id: string
  name: string
  description: string
  creatureType: CreatureType
  bodyTemplateName: string
  createdOn: string
  modifiedOn: string
}

export interface CreatureTemplateBodyPart {
  id: string
  name: string
  hitPenalty: number
  bodyPartType: BodyPartType
  damageModifier: number
  minToHit: number
  maxToHit: number
  armor: number
}

export interface CreatureTemplateSkillItem {
  id: string
  skill: Skill
  skillValue: number
}

export interface CreatureTemplateAppliedCondition {
  id: string
  condition: string
  applyChance: number
}

export interface CreatureTemplateAbility {
  id: string
  name: string
  description: string
  attackSkill: Skill
  attackDiceQuantity: number
  damageModifier: number
  attackSpeed: number
  accuracy: number
  appliedConditions: CreatureTemplateAppliedCondition[]
}

export interface CreatureTemplateDamageModifier {
  id: string
  damageType: DamageType
  damageTypeModifier: DamageTypeModifierValue
}

export interface CreatureTemplateDetails {
  id: string
  imgFileId: string | null
  bodyTemplateId: string
  name: string
  description: string
  creatureType: CreatureType
  hp: number
  sta: number
  int: number
  ref: number
  dex: number
  body: number
  emp: number
  cra: number
  will: number
  luck: number
  speed: number
  createdOn: string
  modifiedOn: string
  creatureTemplateParts: CreatureTemplateBodyPart[]
  creatureTemplateSkills: CreatureTemplateSkillItem[]
  abilities: CreatureTemplateAbility[]
  damageTypeModifiers: CreatureTemplateDamageModifier[]
}

export interface CreatureTemplateFormValues {
  imgFileId?: string | null
  bodyTemplateId: string
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
  abilities: string[]
}

// ---- Battles (BattlesApiController) ----

export interface BattleListItem {
  id: string
  name: string
  description: string
}

export interface MinMax {
  current: number
  max: number
}

export interface BattleCreature {
  id: string
  name: string
  creatureTemplateName: string
  description: string
  hp: MinMax
  effects: string
  initiative: number
  isCharacter: boolean
}

export interface BattleDetails {
  battleId: string
  name: string
  description: string
  creatures: BattleCreature[]
}

// ---- Run battle (RunBattleApiController) ----

export interface RunBattleState extends BattleDetails {
  battleLog: string
  creatureId: string
  currentCreatureName: string
}

export interface MakeTurnState {
  battleId: string
  creatureId: string
  currentCreatureName: string
  turnState: TurnState
  multiAttackAbilityId: string | null
  possibleTargets: Record<string, string>
  myAbilities: Record<string, string>
  equippedWeapons: Record<string, string>
}

export interface FormAttackState {
  creatureParts: Record<string, string | null>
  defensiveSkills: Record<string, Skill | null>
}

export interface FormHealState {
  effectsOnTarget: Record<string, string>
}

export interface AttackPayload {
  id: string
  targetId: string
  attackFormulaId: string
  creaturePartId?: string | null
  defensiveSkill?: Skill | null
  defenseValue?: number | null
  damageValue?: number | null
  attackValue?: number | null
  specialToHit: number
  specialToDamage: number
  isStrongAttack?: boolean | null
  attackType: AttackType
  isPartOfMultiattack: boolean
}

export interface HealPayload {
  creatureId: string
  targetId: string
  effectId: string
}
