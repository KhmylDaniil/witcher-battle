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

// ---- Characters (CharactersApiController) ----

export interface Character {
  id: number
  userId: number
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
