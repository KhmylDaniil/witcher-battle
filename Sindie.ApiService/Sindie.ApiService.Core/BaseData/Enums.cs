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
		public static readonly Dictionary<Guid, Stats> SkillStats = new Dictionary<Guid, Stats>
		{
			{Skills.AwarenessId, Stats.Int},
			{Skills.BusinessId, Stats.Int},
			{Skills.DeductionId, Stats.Int},
			{Skills.EducationId, Stats.Int},
			{Skills.CommonLanguageId, Stats.Int},
			{Skills.ElderLanguageId, Stats.Int},
			{Skills.DwarfenLanguageId, Stats.Int},
			{Skills.MonsterLoreId, Stats.Int},
			{Skills.SocialEtiquetteId, Stats.Int},
			{Skills.TacticsId, Stats.Int},
			{Skills.TeachingId, Stats.Int},
			{Skills.WildernessSurvivalId, Stats.Int},
			{Skills.BrawlingId, Stats.Ref},
			{Skills.DodgeId, Stats.Ref},
			{Skills.MeleeId, Stats.Ref},
			{Skills.RidingId, Stats.Ref},
			{Skills.SailingId, Stats.Ref},
			{Skills.SmallBladesId, Stats.Ref},
			{Skills.StaffId, Stats.Ref},
			{Skills.SwordId, Stats.Ref},
			{Skills.ArcheryId, Stats.Dex},
			{Skills.AthleticsId, Stats.Dex},
			{Skills.CrossbowId, Stats.Dex},
			{Skills.SleightOfHandId, Stats.Dex},
			{Skills.StealthId, Stats.Dex},
			{Skills.PhysiqueId, Stats.Body},
			{Skills.EnduranceId, Stats.Body},
			{Skills.CharismaId, Stats.Emp},
			{Skills.DeceitId, Stats.Emp},
			{Skills.FineArtsId, Stats.Emp},
			{Skills.GamblingId, Stats.Emp},
			{Skills.StyleId, Stats.Emp},
			{Skills.HumanPerceptionId, Stats.Emp},
			{Skills.LeadershipId, Stats.Emp},
			{Skills.PersuasionId, Stats.Emp},
			{Skills.PerfomanceId, Stats.Emp},
			{Skills.SeductionId, Stats.Emp},
			{Skills.AlchemyId, Stats.Cra},
			{Skills.CraftingId, Stats.Cra},
			{Skills.DiguiseId, Stats.Cra},
			{Skills.FirstAidId, Stats.Cra},
			{Skills.ForgeryId, Stats.Cra},
			{Skills.PickLockId, Stats.Cra},
			{Skills.TrapCraftingId, Stats.Cra},
			{Skills.CourageId, Stats.Will},
			{Skills.HexWeavingId, Stats.Will},
			{Skills.IntimidationId, Stats.Will},
			{Skills.SpellId, Stats.Will},
			{Skills.ResistMagicId, Stats.Will},
			{Skills.ResistCoercionId, Stats.Will},
			{Skills.RitualCraftingId, Stats.Will},
			{Skills.NeedlingId, Stats.Emp},
			{Skills.EyeGougeId, Stats.Dex},
			{Skills.BleedingWoundId, Stats.Int},
			{Skills.HealingHandsId, Stats.Cra}
		};
	}
}
