using System;

namespace Sindie.ApiService.Core.BaseData
{
	/// <summary>
	/// Навыки
	/// </summary>
	public static class Skills
	{
		#region intellect skills

		/// <summary>
		/// Айди навыка - внимание
		/// </summary>
		public static readonly Guid AwarenessId = new("c5f00eea-10d5-426e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - внимание
		/// </summary>
		public static readonly string AwarenessName = "Awareness";

		/// <summary>
		/// Айди навыка - Торговля
		/// </summary>
		public static readonly Guid BusinessId = new("c5f01eea-10d5-426e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Торговля
		/// </summary>
		public static readonly string BusinessName = "Business";

		/// <summary>
		/// Айди навыка - Дедукция
		/// </summary>
		public static readonly Guid DeductionId = new("754ef5e9-8960-4c38-a1be-a3c43c92b1cd");

		/// <summary>
		/// Название навыка - Дедукция
		/// </summary>
		public static readonly string DeductionName = "Deduction";

		/// <summary>
		/// Айди навыка - Образование
		/// </summary>
		public static readonly Guid EducationId = new("32ee830e-7bee-4924-9ddf-1070ceffecdd");

		/// <summary>
		/// Название навыка - Образование
		/// </summary>
		public static readonly string EducationName = "Education";

		/// <summary>
		/// Айди навыка - Общий язык
		/// </summary>
		public static readonly Guid CommonLanguageId = new("4fcbd3d6-fde0-47c1-899d-a8c82c291751");

		/// <summary>
		/// Название навыка - Общий язык
		/// </summary>
		public static readonly string CommonLanguageName = "CommonLanguage";

		/// <summary>
		/// Айди навыка - Старшая речь
		/// </summary>
		public static readonly Guid ElderLanguageId = new("c5f03eea-10d5-426e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Старшая речь
		/// </summary>
		public static readonly string ElderLanguageName = "ElderLanguage";

		/// <summary>
		/// Айди навыка - Краснолюдский язык
		/// </summary>
		public static readonly Guid DwarfenLanguageId = new("c5f04eea-10d5-426e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Краснолюдский язык
		/// </summary>
		public static readonly string DwarfenLanguageName = "DwarfenLanguage";

		/// <summary>
		/// Айди навыка - Монстрология
		/// </summary>
		public static readonly Guid MonsterLoreId = new("c5f05eea-10d5-426e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Монстрология
		/// </summary>
		public static readonly string MonsterLoreName = "MonsterLore";

		/// <summary>
		/// Айди навыка - Этикет
		/// </summary>
		public static readonly Guid SocialEtiquetteId = new("c5f06eea-10d5-426e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Этикет
		/// </summary>
		public static readonly string SocialEtiquetteName = "SocialEtiquette";

		/// <summary>
		/// Айди навыка - Знание улиц
		/// </summary>
		public static readonly Guid StreetwiseId = new("c5f07eea-10d5-426e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Знание улиц
		/// </summary>
		public static readonly string StreetwiseName = "Streetwise";

		/// <summary>
		/// Айди навыка - Тактика
		/// </summary>
		public static readonly Guid TacticsId = new("c5f08eea-10d5-426e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Тактика
		/// </summary>
		public static readonly string TacticsName = "Tactics";

		/// <summary>
		/// Айди навыка - Обучение
		/// </summary>
		public static readonly Guid TeachingId = new("c5f09eea-10d5-426e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Обучение
		/// </summary>
		public static readonly string TeachingName = "Teaching";

		/// <summary>
		/// Айди навыка - Выживание
		/// </summary>
		public static readonly Guid WildernessSurvivalId = new("c5f10eea-10d5-426e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Выживание
		/// </summary>
		public static readonly string WildernessSurvivalName = "WildernessSurvival";

		#endregion intellect skills

		#region reflex skills

		/// <summary>
		/// Айди навыка - Драка
		/// </summary>
		public static readonly Guid BrawlingId = new("c5f99eea-10d5-426e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Драка 
		/// </summary>
		public static readonly string BrawlingName = "Brawling";

		/// <summary>
		/// Айди навыка - Уклонение
		/// </summary>
		public static readonly Guid DodgeId = new("c5f99eea-10d5-427e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Уклонение
		/// </summary>
		public static readonly string DodgeName = "Dodge";

		/// <summary>
		/// Айди навыка - Ближний бой
		/// </summary>
		public static readonly Guid MeleeId = new("c5f99eea-10d5-428e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Ближний бой
		/// </summary>
		public static readonly string MeleeName = "Melee";

		/// <summary>
		/// Айди навыка - Верховая езда
		/// </summary>
		public static readonly Guid RidingId = new("c5f11eea-10d5-428e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Верховая езда
		/// </summary>
		public static readonly string RidingName = "Riding";

		/// <summary>
		/// Айди навыка - Мореходство
		/// </summary>
		public static readonly Guid SailingId = new("c5f12eea-10d5-428e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Мореходство
		/// </summary>
		public static readonly string SailingName = "Sailing";

		/// <summary>
		/// Айди навыка - Легкие клинки
		/// </summary>
		public static readonly Guid SmallBladesId = new("c5f99eea-10d5-429e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Легкие клинки
		/// </summary>
		public static readonly string SmallBladesName = "SmallBlades";

		/// <summary>
		/// Айди навыка - Древковое оружие
		/// </summary>
		public static readonly Guid StaffId = new("c5f99eea-10d5-420e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Древковое оружие
		/// </summary>
		public static readonly string StaffName = "Staff/Spear";

		/// <summary>
		/// Айди навыка - Владение мечом
		/// </summary>
		public static readonly Guid SwordId = new("c5f99eea-10d5-526e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Владение мечом
		/// </summary>
		public static readonly string SwordName = "Swordsmanship";

		#endregion reflex skills

		#region dexterity skills

		/// <summary>
		/// Айди навыка - Стрельба из лука
		/// </summary>
		public static readonly Guid ArcheryId = new("c5f99eea-10d5-626e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Стрельба из лука
		/// </summary>
		public static readonly string ArcheryName = "Archery";

		/// <summary>
		/// Айди навыка - Атлетика
		/// </summary>
		public static readonly Guid AthleticsId = new("c5f99eea-10d5-726e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Атлетика
		/// </summary>
		public static readonly string AthleticsName = "Athletics";

		/// <summary>
		/// Айди навыка - Стрельба из арбалета
		/// </summary>
		public static readonly Guid CrossbowId = new("c5f99eea-10d5-826e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Стрельба из арбалета
		/// </summary>
		public static readonly string CrossbowName = "Crossbow";

		/// <summary>
		/// Айди навыка - Ловкость рук
		/// </summary>
		public static readonly Guid SleightOfHandId = new("c5f13eea-10d5-726e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Ловкость рук
		/// </summary>
		public static readonly string SleightOfHandName = "SleightOfHand";

		/// <summary>
		/// Айди навыка - Незаметность
		/// </summary>
		public static readonly Guid StealthId = new("c5f14eea-10d5-826e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Незаметность
		/// </summary>
		public static readonly string StealthName = "Stealth";

		#endregion dexterity skills

		#region body skills

		/// <summary>
		/// Айди навыка - Стойкость
		/// </summary>
		public static readonly Guid EnduranceId = new("c5f99eea-10d5-926e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Стойкость
		/// </summary>
		public static readonly string EnduranceName = "Endurance";

		/// <summary>
		/// Айди навыка - Сила
		/// </summary>
		public static readonly Guid PhysiqueId = new("c5f99eea-10d5-506e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Сила
		/// </summary>
		public static readonly string PhysiqueName = "Physique";

		#endregion body skills

		#region empathy skills

		/// <summary>
		/// Айди навыка - Харизма
		/// </summary>
		public static readonly Guid CharismaId = new("c5f15eea-10d5-826e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Харизма
		/// </summary>
		public static readonly string CharismaName = "Charisma";

		/// <summary>
		/// Айди навыка - Обман
		/// </summary>
		public static readonly Guid DeceitId = new("c5f16eea-10d5-826e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Обман
		/// </summary>
		public static readonly string DeceitName = "Deceit";

		/// <summary>
		/// Айди навыка - Искусство
		/// </summary>
		public static readonly Guid FineArtsId = new("c5f17eea-10d5-826e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Искусство
		/// </summary>
		public static readonly string FineArtsName = "FineArts";

		/// <summary>
		/// Айди навыка - Азартные игры
		/// </summary>
		public static readonly Guid GamblingId = new("c5f18eea-10d5-826e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Азартные игры
		/// </summary>
		public static readonly string GamblingName = "Gambling";

		/// <summary>
		/// Айди навыка - Внешний вид
		/// </summary>
		public static readonly Guid StyleId = new("c5f19eea-10d5-826e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Внешний вид
		/// </summary>
		public static readonly string StyleName = "GroomingAndStyle";

		/// <summary>
		/// Айди навыка - Понимание людей
		/// </summary>
		public static readonly Guid HumanPerceptionId = new("c5f20eea-10d5-826e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Понимание людей
		/// </summary>
		public static readonly string HumanPerceptionName = "HumanPerception";

		/// <summary>
		/// Айди навыка - Лидерство
		/// </summary>
		public static readonly Guid LeadershipId = new("c5f21eea-10d5-826e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Лидерство
		/// </summary>
		public static readonly string LeadershipName = "Leadership";

		/// <summary>
		/// Айди навыка - Убеждение
		/// </summary>
		public static readonly Guid PersuasionId = new("c5f22eea-10d5-826e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Убеждение
		/// </summary>
		public static readonly string PersuasionName = "Persuasion";

		/// <summary>
		/// Айди навыка - Выступление
		/// </summary>
		public static readonly Guid PerfomanceId = new("c5f23eea-10d5-826e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Выступление
		/// </summary>
		public static readonly string PerfomanceName = "Perfomance";

		/// <summary>
		/// Айди навыка - Соблазнение
		/// </summary>
		public static readonly Guid SeductionId = new("c5f24eea-10d5-826e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Соблазнение
		/// </summary>
		public static readonly string SeductionName = "Seduction";

		#endregion empathy skills

		#region craft skills

		/// <summary>
		/// Айди навыка - Алхимия
		/// </summary>
		public static readonly Guid AlchemyId = new("c5f25eea-10d5-026e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Алхимия
		/// </summary>
		public static readonly string AlchemyName = "Alchemy";

		/// <summary>
		/// Айди навыка - Изготовление
		/// </summary>
		public static readonly Guid CraftingId = new("c5f26eea-10d5-026e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Изготовление
		/// </summary>
		public static readonly string CraftingName = "Crafting";

		/// <summary>
		/// Айди навыка - Маскировка
		/// </summary>
		public static readonly Guid DiguiseId = new("c5f27eea-10d5-026e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Маскировка
		/// </summary>
		public static readonly string DiguiseName = "Diguise";

		/// <summary>
		/// Айди навыка - Подделка
		/// </summary>
		public static readonly Guid ForgeryId = new("c5f28eea-10d5-026e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Подделка
		/// </summary>
		public static readonly string ForgeryName = "Forgery";

		/// <summary>
		/// Айди навыка - Взлом
		/// </summary>
		public static readonly Guid PickLockId = new("c5f29eea-10d5-026e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Взлом
		/// </summary>
		public static readonly string PickLockName = "PickLock";

		/// <summary>
		/// Айди навыка - Ловушки
		/// </summary>
		public static readonly Guid TrapCraftingId = new("c5f30eea-10d5-026e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Ловушки
		/// </summary>
		public static readonly string TrapCraftingName = "TrapCrafting";

		/// <summary>
		/// Айди навыка - Первая помощь
		/// </summary>
		public static readonly Guid FirstAidId = new("c5f99eea-10d5-026e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Первая помощь
		/// </summary>
		public static readonly string FirstAidName = "FirstAid";

		#endregion craft skills

		#region will skills

		/// <summary>
		/// Айди навыка - Храбрость
		/// </summary>
		public static readonly Guid CourageId = new("c5f31eea-10d5-026e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Храбрость
		/// </summary>
		public static readonly string CourageName = "Courage";

		/// <summary>
		/// Айди навыка - Порча
		/// </summary>
		public static readonly Guid HexWeavingId = new("c5f32eea-10d5-026e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Порча
		/// </summary>
		public static readonly string HexWeavingName = "HexWeaving";

		/// <summary>
		/// Айди навыка - Запугивание
		/// </summary>
		public static readonly Guid IntimidationId = new("c5f33eea-10d5-026e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Запугивание
		/// </summary>
		public static readonly string IntimidationName = "Intimidation";

		/// <summary>
		/// Айди навыка - Заклинания
		/// </summary>
		public static readonly Guid SpellId = new("c5f99eea-10d5-436e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Заклинания
		/// </summary>
		public static readonly string SpellName = "SpellCasting";

		/// <summary>
		/// Айди навыка - Сопротивление магии
		/// </summary>
		public static readonly Guid ResistMagicId = new("c5f99eea-10d5-446e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Сопротивление магии
		/// </summary>
		public static readonly string ResistMagicName = "ResistMagic";

		/// <summary>
		/// Айди навыка - Сопротивление убеждению
		/// </summary>
		public static readonly Guid ResistCoercionId = new("c5f99eea-10d5-456e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Сопротивление убеждению
		/// </summary>
		public static readonly string ResistCoercionName = "ResistCoercion";

		/// <summary>
		/// Айди навыка - Ритуалы
		/// </summary>
		public static readonly Guid RitualCraftingId = new("c5f34eea-10d5-026e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Ритуалы
		/// </summary>
		public static readonly string RitualCraftingName = "RitualCrafting";

		#endregion will skills

		/// <summary>
		/// Айди навыка - Подколка
		/// </summary>
		public static readonly Guid NeedlingId = new("c5f99eea-10d5-466e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Подколка
		/// </summary>
		public static readonly string NeedlingName = "Needling";

		/// <summary>
		/// Айди навыка - Прямо в глаз
		/// </summary>
		public static readonly Guid EyeGougeId = new("c5f99eea-10d5-476e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Прямо в глаз
		/// </summary>
		public static readonly string EyeGougeName = "EyeGouge";

		/// <summary>
		/// Айди навыка - Кровавая рана
		/// </summary>
		public static readonly Guid BleedingWoundId = new("c5f99eea-10d5-486e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Кровавая рана
		/// </summary>
		public static readonly string BleedingWoundName = "BleedingWound";

		/// <summary>
		/// Айди навыка - Лечащее прикосновение
		/// </summary>
		public static readonly Guid HealingHandsId = new("c5f99eea-10d5-496e-87a6-f6b8046c47da");

		/// <summary>
		/// Название навыка - Лечащее прикосновение
		/// </summary>
		public static readonly string HealingHandsName = "HealingHands";	
	}
}
