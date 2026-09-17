using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Services
{
	internal enum StatGroup
	{
		Int,
		Str,
		Rea,
		Dex,
		Cra,
		Emp,
		Wil,
	}

	/// <summary>
	/// Вычисление "характеристика+навык" для боевых расчётов. Группировка навыков по статам одна и та
	/// же у Character и CreatureTemplate (см. комментарии в Skill.cs), но сами статы называются
	/// по-разному (Str/Rea/Wil у персонажа vs Body/Ref/Will у существа) — отсюда два адаптера.
	/// </summary>
	internal static class SkillHelpers
	{
		public static StatGroup GetStatGroup(Skill skill) => skill switch
		{
			Skill.Awareness or Skill.Tactics or Skill.Survival or Skill.Nature or Skill.Education
				or Skill.Lore or Skill.Streetwise or Skill.Politics or Skill.Deduction => StatGroup.Int,

			Skill.Athletics or Skill.Melee or Skill.Brawl or Skill.Endurance => StatGroup.Str,

			Skill.Sword or Skill.Staff or Skill.SmallBlades or Skill.Dodge => StatGroup.Rea,

			Skill.Acrobatics or Skill.SleightOfHand or Skill.Thrown or Skill.Stealth
				or Skill.Crossbow or Skill.Archery => StatGroup.Dex,

			Skill.Alchemy or Skill.Locks or Skill.Crafting or Skill.Arts or Skill.FirstAid => StatGroup.Cra,

			Skill.Presence or Skill.Perfomance or Skill.Leadership or Skill.Deceit or Skill.AnimalKen
				or Skill.Insight or Skill.Persuasion or Skill.Charisma => StatGroup.Emp,

			Skill.Intimidation or Skill.ResistMagic or Skill.ResistCoercion or Skill.Courage => StatGroup.Wil,

			_ => throw new ArgumentOutOfRangeException(nameof(skill), skill, null),
		};

		public static int GetCharacterStatValue(Character character, StatGroup statGroup) => statGroup switch
		{
			StatGroup.Int => character.Int,
			StatGroup.Str => character.Str,
			StatGroup.Rea => character.Rea,
			StatGroup.Dex => character.Dex,
			StatGroup.Cra => character.Cra,
			StatGroup.Emp => character.Emp,
			StatGroup.Wil => character.Wil,
			_ => throw new ArgumentOutOfRangeException(nameof(statGroup), statGroup, null),
		};

		public static int GetCreatureTemplateStatValue(CreatureTemplate template, StatGroup statGroup) => statGroup switch
		{
			StatGroup.Int => template.Int,
			StatGroup.Str => template.Body,
			StatGroup.Rea => template.Ref,
			StatGroup.Dex => template.Dex,
			StatGroup.Cra => template.Cra,
			StatGroup.Emp => template.Emp,
			StatGroup.Wil => template.Will,
			_ => throw new ArgumentOutOfRangeException(nameof(statGroup), statGroup, null),
		};

		public static int GetCharacterSkillValue(Character character, Skill skill)
		{
			var statValue = GetCharacterStatValue(character, GetStatGroup(skill));
			return statValue + (character.Skills.TryGetValue(skill, out var skillValue) ? skillValue : 0);
		}

		public static int GetCreatureTemplateSkillValue(CreatureTemplate template, Skill skill)
		{
			var statValue = GetCreatureTemplateStatValue(template, GetStatGroup(skill));
			return statValue + (template.Skills.TryGetValue(skill, out var skillValue) ? skillValue : 0);
		}
	}
}
