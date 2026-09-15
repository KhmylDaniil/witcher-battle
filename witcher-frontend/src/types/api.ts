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
  gameId: number
  name: string
  int: number
  str: number
  rea: number
  dex: number
  cra: number
  emp: number
  wil: number
  skills: Partial<Record<Skill, number>>
}

export interface CharacterFormValues {
  name: string
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
