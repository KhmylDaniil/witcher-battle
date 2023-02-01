using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System;
using System.Collections.Generic;

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

		
		public enum Skills
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
		public static Stats CorrespondingStat(Skills skill) => skill switch
		{
			Skills.Awareness => Stats.Int,
			Skills.Business => Stats.Int,
			Skills.Deduction => Stats.Int,
			Skills.Education => Stats.Int,
			Skills.CommonLanguage => Stats.Int,
			Skills.ElderLanguage => Stats.Int,
			Skills.DwarfenLanguage => Stats.Int,
			Skills.MonsterLore => Stats.Int,
			Skills.SocialEtiquette => Stats.Int,
			Skills.Streetwise => Stats.Int,
			Skills.Tactics => Stats.Int,
			Skills.Teaching => Stats.Int,
			Skills.WildernessSurvival => Stats.Int,
			Skills.Brawling => Stats.Ref,
			Skills.Dodge => Stats.Ref,
			Skills.Melee => Stats.Ref,
			Skills.Riding => Stats.Ref,
			Skills.Sailing => Stats.Ref,
			Skills.SmallBlades => Stats.Ref,
			Skills.Staff => Stats.Ref,
			Skills.Sword => Stats.Ref,
			Skills.Archery => Stats.Dex,
			Skills.Athletics => Stats.Dex,
			Skills.Crossbow => Stats.Dex,
			Skills.SleightOfHand => Stats.Dex,
			Skills.Stealth => Stats.Dex,
			Skills.Endurance => Stats.Body,
			Skills.Physique => Stats.Body,
			Skills.Charisma => Stats.Emp,
			Skills.Deceit => Stats.Emp,
			Skills.FineArts => Stats.Emp,
			Skills.Gambling => Stats.Emp,
			Skills.Style => Stats.Emp,
			Skills.HumanPerception => Stats.Emp,
			Skills.Leadership => Stats.Emp,
			Skills.Persuasion => Stats.Emp,
			Skills.Perfomance => Stats.Emp,
			Skills.Seduction => Stats.Emp,
			Skills.Alchemy => Stats.Cra,
			Skills.Crafting => Stats.Cra,
			Skills.Disguise => Stats.Cra,
			Skills.Forgery => Stats.Cra,
			Skills.PickLock => Stats.Cra,
			Skills.TrapCrafting => Stats.Cra,
			Skills.FirstAid => Stats.Cra,
			Skills.Courage => Stats.Will,
			Skills.HexWeaving => Stats.Will,
			Skills.Intimidation => Stats.Will,
			Skills.Spell => Stats.Will,
			Skills.ResistMagic => Stats.Will,
			Skills.ResistCoercion => Stats.Will,
			Skills.RitualCrafting => Stats.Will,
			Skills.Needling => Stats.Emp,
			Skills.EyeGouge => Stats.Dex,
			Skills.BleedingWound => Stats.Int,
			Skills.HealingHands => Stats.Cra,
			_ => throw new ExceptionFieldOutOfRange<Skills>(nameof(skill)),
		};


		/// <summary>
		/// Названия критических эффектов
		/// </summary>
		public static readonly Dictionary<string, string> CritNames = new Dictionary<string, string>
		{
			{"SimpleHead1", "Уродующий шрам" },
			{"SimpleHead2", "Треснувшая челюсть" },
			{"SimpleTorso1", "Инородный объект" },
			{"SimpleTorso2", "Треснувшие ребра" },
			{"SimpleArm", "Вывих руки" },
			{"SimpleLeg", "Вывих ноги" },
			{"SimpleWing", "Вывих крыла" },
			{"SimpleTail", "Вывих хвоста" },
			{"ComplexHead1", "Выбитые зубы" },
			{"ComplexHead2", "Небольшая травма головы" },
			{"ComplexTorso1", "Сломанные ребра" },
			{"ComplexTorso2", "Разрыв селезенки" },
			{"ComplexArm", "Перелом руки" },
			{"ComplexLeg", "Перелом ноги" },
			{"ComplexWing", "Перелом крыла" },
			{"ComplexTail", "Перелом хвоста" },
			{"DifficultHead1", "Контузия" },
			{"DifficultHead2", "Проломленный череп" },
			{"DifficultTorso1", "Сосущая рана грудной клетки" },
			{"DifficultTorso2", "Рана в живот" },
			{"DifficultArm", "Открытый перелом руки" },
			{"DifficultLeg", "Открытый перелом ноги" },
			{"DifficultWing", "Открытый перелом крыла" },
			{"DifficultTail", "Открытый перелом хвоста" },
			{"DeadlyHead1", "Потеря головы" },
			{"DeadlyHead2", "Повреждение глаза" },
			{"DeadlyTorso1", "Септический шок" },
			{"DeadlyTorso2", "Травма сердца" },
			{"DeadlyArm", "Потеря руки" },
			{"DeadlyLeg", "Потеря ноги" },
			{"DeadlyWing", "Потеря крыла" },
			{"DeadlyTail", "Потеря хвоста" }
	};

		/// <summary>
		/// Соотношение навыков и характеристик
		/// </summary>
		public static readonly Dictionary<Skills, Stats> SkillStats = new Dictionary<Skills, Stats>
		{
			{Skills.Awareness, Stats.Int},
			{Skills.Business, Stats.Int},
			{Skills.Deduction, Stats.Int},
			{Skills.Education, Stats.Int},
			{Skills.CommonLanguage, Stats.Int},
			{Skills.ElderLanguage, Stats.Int},
			{Skills.DwarfenLanguage, Stats.Int},
			{Skills.MonsterLore, Stats.Int},
			{Skills.SocialEtiquette, Stats.Int},
			{Skills.Tactics, Stats.Int},
			{Skills.Teaching, Stats.Int},
			{Skills.WildernessSurvival, Stats.Int},
			{Skills.Brawling, Stats.Ref},
			{Skills.Dodge, Stats.Ref},
			{Skills.Melee, Stats.Ref},
			{Skills.Riding, Stats.Ref},
			{Skills.Sailing, Stats.Ref},
			{Skills.SmallBlades, Stats.Ref},
			{Skills.Staff, Stats.Ref},
			{Skills.Sword, Stats.Ref},
			{Skills.Archery, Stats.Dex},
			{Skills.Athletics, Stats.Dex},
			{Skills.Crossbow, Stats.Dex},
			{Skills.SleightOfHand, Stats.Dex},
			{Skills.Stealth, Stats.Dex},
			{Skills.Physique, Stats.Body},
			{Skills.Endurance, Stats.Body},
			{Skills.Charisma, Stats.Emp},
			{Skills.Deceit, Stats.Emp},
			{Skills.FineArts, Stats.Emp},
			{Skills.Gambling, Stats.Emp},
			{Skills.Style, Stats.Emp},
			{Skills.HumanPerception, Stats.Emp},
			{Skills.Leadership, Stats.Emp},
			{Skills.Persuasion, Stats.Emp},
			{Skills.Perfomance, Stats.Emp},
			{Skills.Seduction, Stats.Emp},
			{Skills.Alchemy, Stats.Cra},
			{Skills.Crafting, Stats.Cra},
			{Skills.Disguise, Stats.Cra},
			{Skills.FirstAid, Stats.Cra},
			{Skills.Forgery, Stats.Cra},
			{Skills.PickLock, Stats.Cra},
			{Skills.TrapCrafting, Stats.Cra},
			{Skills.Courage, Stats.Will},
			{Skills.HexWeaving, Stats.Will},
			{Skills.Intimidation, Stats.Will},
			{Skills.Spell, Stats.Will},
			{Skills.ResistMagic, Stats.Will},
			{Skills.ResistCoercion, Stats.Will},
			{Skills.RitualCrafting, Stats.Will},
			{Skills.Needling, Stats.Emp},
			{Skills.EyeGouge, Stats.Dex},
			{Skills.BleedingWound, Stats.Int},
			{Skills.HealingHands, Stats.Cra}
		};
	}
}
