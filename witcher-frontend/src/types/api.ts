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

// ---- Пагинация (единый механизм для списков существ, шаблонов тела, игр, персонажей) ----

/** Зеркалит Wastelands.Service.Application/Models/Dto/PagedResultDto.cs */
export interface PagedResult<T> {
  items: T[]
  totalCount: number
  pageNumber: number
  pageSize: number
}

/** Зеркалит Wastelands.Core.Contracts/Models/PagedRequest.cs; стандартный размер страницы — 20. */
export interface PagingParams {
  pageNumber?: number
  pageSize?: number
  orderBy?: string
  orderDirection?: 'Ascending' | 'Descending'
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
  imageUrl: string | null
  hp: number
  currentHP: number
  sta: number
  int: number
  str: number
  rea: number
  dex: number
  cra: number
  emp: number
  wil: number
  /** (Str+Wil)/2 с округлением вниз — вычисляется на сервере, не редактируется напрямую. */
  recovery: number
  /** (Str+Wil)/2 с округлением вниз — вычисляется на сервере, не редактируется напрямую. */
  stun: number
  skills: Partial<Record<Skill, number>>
  abilities: Ability[]
  items: Item[]
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
  imageUrl: string | null
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

// ---- Item templates (ItemTemplatesApiController) — доступны только мастеру игры ----

export const ITEM_TYPES = ['Weapon', 'Armor', 'Other'] as const
export type ItemType = (typeof ITEM_TYPES)[number]

export const WEAPON_KINDS = ['Melee', 'Ranged'] as const
export type WeaponKind = (typeof WEAPON_KINDS)[number]

export const HUMAN_BODY_PARTS = ['Head', 'Torso', 'RightArm', 'LeftArm', 'RightLeg', 'LeftLeg'] as const
export type HumanBodyPart = (typeof HUMAN_BODY_PARTS)[number]

export const HUMAN_BODY_PART_LABELS: Record<HumanBodyPart, string> = {
  Head: 'Голова',
  Torso: 'Торс',
  RightArm: 'Правая рука',
  LeftArm: 'Левая рука',
  RightLeg: 'Правая нога',
  LeftLeg: 'Левая нога',
}

export interface ItemTemplateAppliedCondition {
  id: number
  condition: Condition
  applyChance: number
}

/** Покрытие шаблона брони — часть тела + значение брони (оно же стартовая/максимальная прочность). */
export interface ItemTemplateArmorPart {
  id: number
  part: HumanBodyPart
  armor: number
}

export interface ItemTemplate {
  id: number
  gameId: number
  name: string
  description: string | null
  itemType: ItemType
  weight: number
  cost: number
  /** Заполнены только когда itemType === 'Weapon'. */
  attackSkill: Skill | null
  /** Возможна ли мультиатака (быстрая/сильная атака при экипировке). */
  isMultiAttack: boolean | null
  damageDiceCount: number | null
  attackModifier: number | null
  damageModifier: number | null
  damageType: DamageType | null
  weaponKind: WeaponKind | null
  attackRange: number | null
  handsRequired: number | null
  durability: number | null
  appliedConditions: ItemTemplateAppliedCondition[]
  /** Заполнены только когда itemType === 'Armor'. */
  armorParts: ItemTemplateArmorPart[]
  damageTypeModifiers: Partial<Record<DamageType, DamageTypeModifierKind>>
}

export interface ItemTemplateFormValues {
  name: string
  description: string
  itemType: ItemType
  weight: number
  cost: number
  attackSkill?: Skill
  isMultiAttack?: boolean
  damageDiceCount?: number
  attackModifier?: number
  damageModifier?: number
  damageType?: DamageType
  weaponKind?: WeaponKind
  attackRange?: number
  handsRequired?: number
  durability?: number
}

// ---- Items (экземпляры предметов в инвентаре персонажа, CharactersApiController) ----

export interface ItemAppliedCondition {
  id: number
  condition: Condition
  applyChance: number
}

/** Покрытие экземпляра брони — снапшот ItemTemplateArmorPart плюс собственная текущая прочность. */
export interface ItemArmorPart {
  id: number
  part: HumanBodyPart
  armor: number
  currentDurability: number
}

/** Снапшот-копия ItemTemplate в момент добавления в инвентарь — правки шаблона на неё не влияют. */
export interface Item {
  id: number
  itemTemplateId: number
  name: string
  description: string | null
  itemType: ItemType
  weight: number
  cost: number
  attackSkill: Skill | null
  isMultiAttack: boolean | null
  damageDiceCount: number | null
  attackModifier: number | null
  damageModifier: number | null
  damageType: DamageType | null
  weaponKind: WeaponKind | null
  attackRange: number | null
  handsRequired: number | null
  durability: number | null
  appliedConditions: ItemAppliedCondition[]
  armorParts: ItemArmorPart[]
  damageTypeModifiers: Partial<Record<DamageType, DamageTypeModifierKind>>
  isEquipped: boolean
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
  attackModifier: number
  damageModifier: number
  damageType: DamageType
  appliedConditions: AbilityAppliedCondition[]
  defensiveSkills: AbilityDefensiveSkill[]
  /** Сгенерирована экипировкой оружия — редактируется/удаляется только через снятие предмета. */
  isFromEquippedWeapon: boolean
}

export interface AbilityFormValues {
  name: string
  attackSkill: Skill
  attacksPerTurn: number
  damageDiceCount: number
  attackModifier: number
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
  recovery: number
  stun: number
  initiative: number | null
  appliedConditions: Condition[]
  /** Износ брони по частям тела в этом бою (partId -> сколько очков брони потеряно). */
  armorReductionByPartId: Partial<Record<number, number>>
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
  /** Сколько к6 бросает способность — допустимый диапазон броска урона: от diceCount до diceCount*6. */
  abilityDamageDiceCount: number
  attacksAllowed: number
  attacksUsed: number
  defenderKind: ParticipantKind
  defenderId: number
  defenderName: string
  /** Заполнено, только если защитник — существо. */
  availableCreatureParts: CreaturePartOption[] | null
  targetedCreaturePartId: number | null
  /** Заполнено, только если защитник — персонаж; варианты — фиксированный HUMAN_BODY_PARTS. */
  targetedHumanBodyPart: HumanBodyPart | null
  attackRoll: number | null
  attackerConfirmed: boolean
  availableDefensiveSkills: Skill[]
  /** Справочное значение характеристика+навык защитника для каждого доступного защитного навыка. */
  defensiveSkillValues: Partial<Record<Skill, number>>
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
