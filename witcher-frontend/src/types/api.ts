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
  movement: number
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
  movement: number
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
  movement: number
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
  movement: number
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
  /** Прочность на момент добавления в инвентарь — верхняя граница для ремонта. */
  maxDurability: number | null
  appliedConditions: ItemAppliedCondition[]
  armorParts: ItemArmorPart[]
  damageTypeModifiers: Partial<Record<DamageType, DamageTypeModifierKind>>
  isEquipped: boolean
}

// ---- Damage types / modifiers ----

export const DAMAGE_TYPES = ['Slashing', 'Piercing', 'Bludgeoning', 'Fire'] as const
export type DamageType = (typeof DAMAGE_TYPES)[number]

export const DAMAGE_TYPE_MODIFIERS = ['Vulnerability', 'Resistance', 'Immunity'] as const
export type DamageTypeModifierKind = (typeof DAMAGE_TYPE_MODIFIERS)[number]

// ---- Conditions (эффекты, накладываемые атакующими способностями) ----

export const CONDITIONS = [
  'Bleed', 'Poison', 'Fire', 'Stun', 'Staggered', 'Sufflocation', 'Blinded', 'Dying',

  // Критические ранения (новая система) — Simple/Medium/Difficult x часть тела x тип урона.
  'SimpleHeadPiercing', 'SimpleHeadSlashing', 'SimpleHeadBludgeoning', 'SimpleHeadFire',
  'SimpleTorsoPiercing', 'SimpleTorsoSlashing', 'SimpleTorsoBludgeoning', 'SimpleTorsoFire',
  'SimpleArmPiercing', 'SimpleArmSlashing', 'SimpleArmBludgeoning', 'SimpleArmFire',
  'SimpleLegPiercing', 'SimpleLegSlashing', 'SimpleLegBludgeoning', 'SimpleLegFire',
  'SimpleWingPiercing', 'SimpleWingSlashing', 'SimpleWingBludgeoning', 'SimpleWingFire',
  'SimpleTailPiercing', 'SimpleTailSlashing', 'SimpleTailBludgeoning', 'SimpleTailFire',

  'MediumHeadPiercing', 'MediumHeadSlashing', 'MediumHeadBludgeoning', 'MediumHeadFire',
  'MediumTorsoPiercing', 'MediumTorsoSlashing', 'MediumTorsoBludgeoning', 'MediumTorsoFire',
  'MediumArmPiercing', 'MediumArmSlashing', 'MediumArmBludgeoning', 'MediumArmFire',
  'MediumLegPiercing', 'MediumLegSlashing', 'MediumLegBludgeoning', 'MediumLegFire',
  'MediumWingPiercing', 'MediumWingSlashing', 'MediumWingBludgeoning', 'MediumWingFire',
  'MediumTailPiercing', 'MediumTailSlashing', 'MediumTailBludgeoning', 'MediumTailFire',

  'DifficultHeadPiercing', 'DifficultHeadSlashing', 'DifficultHeadBludgeoning', 'DifficultHeadFire',
  'DifficultTorsoPiercing', 'DifficultTorsoSlashing', 'DifficultTorsoBludgeoning', 'DifficultTorsoFire',
  'DifficultArmPiercing', 'DifficultArmSlashing', 'DifficultArmBludgeoning', 'DifficultArmFire',
  'DifficultLegPiercing', 'DifficultLegSlashing', 'DifficultLegBludgeoning', 'DifficultLegFire',
  'DifficultWingPiercing', 'DifficultWingSlashing', 'DifficultWingBludgeoning', 'DifficultWingFire',
  'DifficultTailPiercing', 'DifficultTailSlashing', 'DifficultTailBludgeoning', 'DifficultTailFire',

  'Prone',
] as const
export type Condition = (typeof CONDITIONS)[number]

/**
 * Зеркало backend ConditionRemovalCatalog — какими навыками и с какой сложностью можно снять
 * состояние броском (Bleed/Poison), и можно ли выбрать в качестве цели кого-то другого.
 * Состояния без записи здесь (Fire, Sufflocation, Staggered/Blinded, Dying, крит. ранения) через
 * этот бросок не снимаются — Fire снимается обычным действием без броска, Sufflocation только
 * мастером вручную, Staggered/Blinded спадают сами.
 */
export interface ConditionRemovalRule {
  skill: Skill
  difficulty: number
  selfOnly: boolean
}

export const CONDITION_REMOVAL_RULES: Partial<Record<Condition, ConditionRemovalRule[]>> = {
  Poison: [
    { skill: 'Endurance', difficulty: 15, selfOnly: true },
    { skill: 'FirstAid', difficulty: 14, selfOnly: false },
  ],
  Bleed: [{ skill: 'FirstAid', difficulty: 14, selfOnly: false }],
}

/**
 * Состояния, снимаемые обычным действием без броска (всегда успешно, только с себя) — зеркало
 * backend ConditionRemovalCatalog.IsAutoClearable.
 */
export const AUTO_CLEARABLE_CONDITIONS: Condition[] = ['Fire', 'Prone']

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
  /** Позиция на подключённой к бою карте; null — не выставлен. */
  mapColumn: number | null
  mapRow: number | null
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
  /** Позиция на подключённой к бою карте; null — не выставлен. */
  mapColumn: number | null
  mapRow: number | null
  appliedConditions: Condition[]
  /** Уже потратил основное действие в этот ход — доступно доп. действие за 3 выносливости или конец хода. */
  hasActedThisTurn: boolean
}

export type ParticipantKind = 'Creature' | 'Character'

export type BattleAttackPhase = 'AwaitingChoices' | 'AwaitingDamageRoll' | 'SwingResolved' | 'AwaitingStunSave'

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
  /** Есть ли у защитника экипированное оружие ближнего боя — доступно ли парирование вместо обычного защитного навыка. */
  canParry: boolean
  /** Навык, которым защитник будет парировать (навык атаки его оружия), — заполнено, только если canParry. */
  parrySkill: Skill | null
  /** Справочное значение навыка парирования до вычета штрафа -3 — заполнено, только если canParry. */
  parrySkillValue: number | null
  /** true — защитник выбрал парирование вместо обычного защитного навыка. */
  isParry: boolean
  defenderConfirmed: boolean
  /** Оглушённый защитник не выбирает навык/не бросает защиту — защита фиксирована на 10, шаг выбора нужно скрыть. */
  defenderIsStunned: boolean
  phase: BattleAttackPhase
  lastHitSucceeded: boolean | null
  resolvedCreaturePartId: number | null
  damageRoll: number | null
  /** Заполнено, когда phase === 'AwaitingStunSave' или проверка уже пройдена. */
  stunSaveRoll: number | null
  /** true — Оглушение наложено этой атакой, false — не наложено, null — проверка ещё не пройдена. */
  stunSaveSucceeded: boolean | null
  /**
   * Кто проходит текущую/последнюю проверку Оглушения — обычно защитник, но при критическом провале
   * атаки/защиты им может стать любая сторона. Null, пока проверка Оглушения ни разу не начиналась.
   */
  stunSaveOwnerKind: ParticipantKind | null
  stunSaveOwnerId: number | null
  stunSaveOwnerName: string | null
  /** true — дополнительное действие персонажа за выносливость, со штрафом -3 к атаке. */
  isBonusAction: boolean
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
  /** Подключённая карта боя (BattleMap.id); null — бой без карты. */
  battleMapId: number | null
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

// ---- Battle maps (BattleMapsApiController) — гексагональные карты боя, доступны только мастеру игры ----

/** Зеркалит Wastelands.Service.Domain/Enums/HexTerrainType.cs — влияет на передвижение. */
export const HEX_TERRAIN_TYPES = ['Open', 'Difficult', 'Impassable', 'Wall', 'Void'] as const
export type HexTerrainType = (typeof HEX_TERRAIN_TYPES)[number]

export const HEX_TERRAIN_TYPE_LABELS: Record<HexTerrainType, string> = {
  Open: 'Простой террейн',
  Difficult: 'Сложный террейн',
  Impassable: 'Непроходимый террейн',
  Wall: 'Стена',
  Void: 'Черное пространство',
}

/** Зеркалит Wastelands.Service.Domain/Enums/HexTerrainStyle.cs — только внешний вид гекса. */
export const HEX_TERRAIN_STYLES = ['Grass', 'Sand', 'Water', 'AncientStreet', 'FuturisticMetal'] as const
export type HexTerrainStyle = (typeof HEX_TERRAIN_STYLES)[number]

export const HEX_TERRAIN_STYLE_LABELS: Record<HexTerrainStyle, string> = {
  Grass: 'Трава',
  Sand: 'Песок',
  Water: 'Вода',
  AncientStreet: 'Улица античного города',
  FuturisticMetal: 'Металл футуристического комплекса',
}

export interface BattleMapHex {
  column: number
  row: number
  terrainType: HexTerrainType
  terrainStyle: HexTerrainStyle
  isPassable: boolean
  movementCost: number | null
  /** Текст маркера мастера на гексе; null — маркера нет. На гексе не больше одного объекта. */
  markerText: string | null
}

/** Карта без гексов — то, что отдаёт список карт игры. */
export interface BattleMapSummary {
  id: number
  gameId: number
  name: string
  description: string | null
  columns: number
  rows: number
}

export interface BattleMap extends BattleMapSummary {
  /** Ровно columns × rows гексов, построчно (row, затем column). */
  hexes: BattleMapHex[]
}

/** Зеркалит BattleMap.MinDimension/MaxDimension на бэке. */
export const BATTLE_MAP_MIN_DIMENSION = 1
export const BATTLE_MAP_MAX_DIMENSION = 60

export interface CreateBattleMapFormValues {
  name: string
  description: string
  columns: number
  rows: number
  terrainStyle: HexTerrainStyle
}

export interface UpdateBattleMapFormValues {
  name: string
  description: string
  columns: number
  rows: number
  /** Стиль простого террейна для гексов, появившихся при увеличении карты. */
  fillTerrainStyle: HexTerrainStyle
}

export interface HexPaint {
  column: number
  row: number
  terrainType: HexTerrainType
  terrainStyle: HexTerrainStyle
}

/** Зеркалит BattleMapHex.MaxMarkerTextLength на бэке. */
export const BATTLE_MAP_MARKER_MAX_LENGTH = 500

// ---- Карта в бою (BattleMapPlacementApiController) — только мастер игры ----

export interface BattleMapParticipant {
  kind: ParticipantKind
  /** Creature.id или Character.id — как attackerId/defenderId в атаке. */
  id: number
  name: string
  imageUrl: string | null
  currentHP: number
  maxHP: number
  initiative: number | null
  column: number | null
  row: number | null
  /** Своим участником управляет текущий пользователь (свой персонаж игрока; для мастера — существа). */
  controlledByCurrentUser: boolean
}

export interface BattleMapView {
  battleId: number
  battleName: string
  status: BattleStatus
  currentInitiative: number | null
  /** true — мастер (подключение карты и расстановка); false — игрок, только просмотр, без маркеров. */
  canEdit: boolean
  map: BattleMap | null
  participants: BattleMapParticipant[]
}
