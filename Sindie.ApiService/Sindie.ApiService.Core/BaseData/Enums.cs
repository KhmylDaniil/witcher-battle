using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System;

namespace Sindie.ApiService.Core.BaseData
{
	/// <summary>
	/// Перечисления
	/// </summary>
	public static class Enums
	{
		/// <summary>
		/// Типы существ
		/// </summary>
		public enum CreatureType
		{
			Human,
			Necrophage,
			Specter,
			Beast,
			Cursed,
			Hybrid,
			Insectoid,
			Elementa,
			Relict,
			Orgoid,
			Draconid,
			Vampire
		}

		public enum DamageType
		{
			Slashing,
			Piercing,
			Bludgeoning,
			Elemental,
			Fire,
			Silver
		}

		/// <summary>
		/// Модификатор в зависимости от типа урона
		/// </summary>
		public enum DamageTypeModifier
		{
			Normal,
			Vulnerability,
			Resistance,
			Immunity
		}
		
		/// <summary>
		/// Типы частей тела
		/// </summary>
		public enum BodyPartType
		{
			Void,
			Head,
			Torso,
			Arm,
			Leg,
			Wing,
			Tail
		}

		/// <summary>
		/// Тяжесть критического эффекта
		/// </summary>
		[Flags]
		public enum Severity
		{
			Unstabilizied = 1,
			Simple = 12,
			Complex = 14,
			Difficult = 16,
			Deadly = 18
		}

		/// <summary>
		/// Стабилизирован ли критический эффект
		/// </summary>
		/// <param name="severity">Тяжесть критического эффекта</param>
		/// <returns></returns>
		public static bool IsStabile(Severity severity)
			=> severity == Severity.Simple || severity == Severity.Complex || severity == Severity.Difficult || severity == Severity.Deadly;

		
		public enum Skill
		{
			Awareness,
			Business,
			Deduction,
			Education,
			CommonLanguage,
			ElderLanguage,
			DwarfenLanguage,
			MonsterLore,
			SocialEtiquette,
			Streetwise,
			Tactics,
			Teaching,
			WildernessSurvival,
			Brawling,
			Dodge,
			Melee,
			Riding,
			Sailing,
			SmallBlades,
			Staff,
			Sword,
			Archery,
			Athletics,
			Crossbow,
			SleightOfHand,
			Stealth,
			Endurance,
			Physique,
			Charisma,
			Deceit,
			FineArts,
			Gambling,
			Style,
			HumanPerception,
			Leadership,
			Persuasion,
			Perfomance,
			Seduction,
			Alchemy,
			Crafting,
			Disguise,
			Forgery,
			PickLock,
			TrapCrafting,
			FirstAid,
			Courage,
			HexWeaving,
			Intimidation,
			Spell,
			ResistMagic,
			ResistCoercion,
			RitualCrafting,
			Needling,
			EyeGouge,
			BleedingWound,
			HealingHands
		}
		
		/// <summary>
		/// Характеристики
		/// </summary>
		public enum Stats
		{
			Int = 1,
			Ref,
			Dex,
			Body,
			Emp,
			Cra,
			Will,
			Void
		};

		/// <summary>
		/// Соответствие скилла и характеристики
		/// </summary>
		/// <param name="skill">Скилл</param>
		/// <returns>Соответствующая характеристика</returns>
		public static Stats CorrespondingStat(Skill skill) => skill switch
		{
			Skill.Awareness => Stats.Int,
			Skill.Business => Stats.Int,
			Skill.Deduction => Stats.Int,
			Skill.Education => Stats.Int,
			Skill.CommonLanguage => Stats.Int,
			Skill.ElderLanguage => Stats.Int,
			Skill.DwarfenLanguage => Stats.Int,
			Skill.MonsterLore => Stats.Int,
			Skill.SocialEtiquette => Stats.Int,
			Skill.Streetwise => Stats.Int,
			Skill.Tactics => Stats.Int,
			Skill.Teaching => Stats.Int,
			Skill.WildernessSurvival => Stats.Int,
			Skill.Brawling => Stats.Ref,
			Skill.Dodge => Stats.Ref,
			Skill.Melee => Stats.Ref,
			Skill.Riding => Stats.Ref,
			Skill.Sailing => Stats.Ref,
			Skill.SmallBlades => Stats.Ref,
			Skill.Staff => Stats.Ref,
			Skill.Sword => Stats.Ref,
			Skill.Archery => Stats.Dex,
			Skill.Athletics => Stats.Dex,
			Skill.Crossbow => Stats.Dex,
			Skill.SleightOfHand => Stats.Dex,
			Skill.Stealth => Stats.Dex,
			Skill.Endurance => Stats.Body,
			Skill.Physique => Stats.Body,
			Skill.Charisma => Stats.Emp,
			Skill.Deceit => Stats.Emp,
			Skill.FineArts => Stats.Emp,
			Skill.Gambling => Stats.Emp,
			Skill.Style => Stats.Emp,
			Skill.HumanPerception => Stats.Emp,
			Skill.Leadership => Stats.Emp,
			Skill.Persuasion => Stats.Emp,
			Skill.Perfomance => Stats.Emp,
			Skill.Seduction => Stats.Emp,
			Skill.Alchemy => Stats.Cra,
			Skill.Crafting => Stats.Cra,
			Skill.Disguise => Stats.Cra,
			Skill.Forgery => Stats.Cra,
			Skill.PickLock => Stats.Cra,
			Skill.TrapCrafting => Stats.Cra,
			Skill.FirstAid => Stats.Cra,
			Skill.Courage => Stats.Will,
			Skill.HexWeaving => Stats.Will,
			Skill.Intimidation => Stats.Will,
			Skill.Spell => Stats.Will,
			Skill.ResistMagic => Stats.Will,
			Skill.ResistCoercion => Stats.Will,
			Skill.RitualCrafting => Stats.Will,
			Skill.Needling => Stats.Emp,
			Skill.EyeGouge => Stats.Dex,
			Skill.BleedingWound => Stats.Int,
			Skill.HealingHands => Stats.Cra,
			_ => throw new ExceptionFieldOutOfRange<Skill>(nameof(skill)),
		};
	}
}
