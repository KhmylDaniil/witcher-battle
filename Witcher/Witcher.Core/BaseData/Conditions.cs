using System;
using System.Collections.Generic;

namespace Witcher.Core.BaseData
{
	public static class CritNames
	{
		/// <summary>
		/// Названия критических эффектов
		/// </summary>
		public static readonly Dictionary<Condition, string> Names = new Dictionary<Condition, string>
		{
			{Condition.SimpleHead1, "Уродующий шрам" },
			{Condition.SimpleHead2, "Треснувшая челюсть" },
			{Condition.SimpleTorso1, "Инородный объект" },
			{Condition.SimpleTorso2, "Треснувшие ребра" },
			{Condition.SimpleArm, "Вывих руки" },
			{Condition.SimpleLeg, "Вывих ноги" },
			{Condition.SimpleWing, "Вывих крыла" },
			{Condition.SimpleTail, "Вывих хвоста" },
			{Condition.ComplexHead1, "Выбитые зубы" },
			{Condition.ComplexHead2, "Небольшая травма головы" },
			{Condition.ComplexTorso1, "Сломанные ребра" },
			{Condition.ComplexTorso2, "Разрыв селезенки" },
			{Condition.ComplexArm, "Перелом руки" },
			{Condition.ComplexLeg, "Перелом ноги" },
			{Condition.ComplexWing, "Перелом крыла" },
			{Condition.ComplexTail, "Перелом хвоста" },
			{Condition.DifficultHead1, "Контузия" },
			{Condition.DifficultHead2, "Проломленный череп" },
			{Condition.DifficultTorso1, "Сосущая рана грудной клетки" },
			{Condition.DifficultTorso2, "Рана в живот" },
			{Condition.DifficultArm, "Открытый перелом руки" },
			{Condition.DifficultLeg, "Открытый перелом ноги" },
			{Condition.DifficultWing, "Открытый перелом крыла" },
			{Condition.DifficultTail, "Открытый перелом хвоста" },
			{Condition.DeadlyHead1, "Повреждение глаза" },
			{Condition.DeadlyHead2, "Потеря головы" },
			{Condition.DeadlyTorso1, "Септический шок" },
			{Condition.DeadlyTorso2, "Травма сердца" },
			{Condition.DeadlyArm, "Потеря руки" },
			{Condition.DeadlyLeg, "Потеря ноги" },
			{Condition.DeadlyWing, "Потеря крыла" },
			{Condition.DeadlyTail, "Потеря хвоста" },
			{Condition.Bleed, "Кровотечение" },
			{Condition.BleedingWound, "Кровавая рана" },
			{Condition.Blinded, "Ослепление" },
			{Condition.Dying, "При смерти" },
			{Condition.Fire, "Горение" },
			{Condition.Freeze, "Заморозка" },
			{Condition.Hallutination, "Галлюцинации" },
			{Condition.Intoxication, "Опьянение" },
			{Condition.Nausea, "Тошнота" },
			{Condition.Poison, "Отравление" },
			{Condition.Staggered, "Ошеломление" },
			{Condition.Stun, "Дизориентация" },
			{Condition.Sufflocation, "Удушье" }
		};

		public static string GetConditionFullName(Condition condition)
			=> Names.TryGetValue(condition, out string result)
			? result
			: throw new ArgumentException("No such condition");
	}


	/// <summary>
	/// Состояния (справочник)
	/// </summary>
	public enum Condition
	{
		Bleed,
		BleedingWound,
		Poison,
		Fire,
		Freeze,
		Stun,
		Staggered,
		Intoxication,
		Hallutination,
		Nausea,
		Sufflocation,
		Blinded,
		Dying,

		SimpleLeg,
		SimpleArm,
		SimpleWing,
		SimpleTail,
		SimpleHead1,
		SimpleHead2,
		SimpleTorso1,
		SimpleTorso2,

		ComplexLeg,
		ComplexArm,
		ComplexWing,
		ComplexTail,
		ComplexHead1,
		ComplexHead2,
		ComplexTorso1,
		ComplexTorso2,

		DifficultLeg,
		DifficultArm,
		DifficultWing,
		DifficultTail,
		DifficultHead1,
		DifficultHead2,
		DifficultTorso1,
		DifficultTorso2,

		DeadlyLeg,
		DeadlyArm,
		DeadlyWing,
		DeadlyTail,
		DeadlyHead1,
		DeadlyHead2,
		DeadlyTorso1,
		DeadlyTorso2
	}
}
