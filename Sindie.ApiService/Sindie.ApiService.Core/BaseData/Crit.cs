using System;

namespace Sindie.ApiService.Core.BaseData
{
	/// <summary>
	/// Критические повреждения
	/// </summary>
	public static class Crit
	{
		public const string SimpleHead1 = "Уродующий шрам";
		public static readonly Guid SimpleHead1Id = new("208ed04e-73aa-4e57-bb58-26c807fcf589");

		public const string SimpleHead2 = "Треснувшая челюсть";
		public static readonly Guid SimpleHead2Id = new("208ed04e-73aa-4e57-bb58-26c807fcf588");

		public const string SimpleTorso1 = "Инородный объект";
		public static readonly Guid SimpleTorso1Id = new("208ed04e-73aa-4e57-bb58-26c807fcf587");

		public const string SimpleTorso2 = "Треснувшие ребра";
		public static readonly Guid SimpleTorso2Id = new("208ed04e-73aa-4e57-bb58-26c807fcf586");

		public const string SimpleArm = "Вывих руки";
		public static readonly Guid SimpleArmId = new("208ed04e-73aa-4e57-bb58-26c807fcf585");
		
		public const string SimpleLeg = "Вывих ноги";
		public static readonly Guid SimpleLegId = new("208ed04e-73aa-4e57-bb58-26c807fcf584");

		public const string SimpleWing = "Вывих крыла";
		public static readonly Guid SimpleWingId = new("208ed04e-73aa-4e57-bb58-26c807fcf583");

		public const string SimpleTail = "Вывих хвоста";
		public static readonly Guid SimpleTailId = new("208ed04e-73aa-4e57-bb58-26c807fcf582");

		public const string ComplexHead1 = "Выбитые зубы";
		public static readonly Guid ComplexHead1Id = new("208ed04e-73aa-4e57-bb58-26c807fcf581");

		public const string ComplexHead2 = "Небольшая травма головы";
		public static readonly Guid ComplexHead2Id = new("208ed04e-73aa-4e57-bb58-26c807fcf580");

		public const string ComplexTorso1 = "Сломанные ребра";
		public static readonly Guid ComplexTorso1Id = new("208ed04e-73aa-4e57-bb58-26c807fcf579");

		public const string ComplexTorso2 = "Разрыв селезенки";
		public static readonly Guid ComplexTorso2Id = new("208ed04e-73aa-4e57-bb58-26c807fcf578");
		
		public const string ComplexArm = "Перелом руки";
		public static readonly Guid ComplexArmId = new("208ed04e-73aa-4e57-bb58-26c807fcf577");
		
		public const string ComplexLeg = "Перелом ноги";
		public static readonly Guid ComplexLegId = new("208ed04e-73aa-4e57-bb58-26c807fcf576");
		
		public const string ComplexWing = "Перелом крыла";
		public static readonly Guid ComplexWingId = new("208ed04e-73aa-4e57-bb58-26c807fcf575");
		
		public const string ComplexTail = "Перелом хвоста";
		public static readonly Guid ComplexTailId = new("208ed04e-73aa-4e57-bb58-26c807fcf574");

		public const string DifficultHead1 = "Контузия";
		public static readonly Guid DifficultHead1Id = new("208ed04e-73aa-4e57-bb58-26c807fcf573");
		
		public const string DifficultHead2 = "Проломленный череп";
		public static readonly Guid DifficultHead2Id = new("208ed04e-73aa-4e57-bb58-26c807fcf572");
		
		public const string DifficultTorso1 = "Сосущая рана грудной клетки";
		public static readonly Guid DifficultTorso1Id = new("208ed04e-73aa-4e57-bb58-26c807fcf571");
		
		public const string DifficultTorso2 = "Рана в живот";
		public static readonly Guid DifficultTorso2Id = new("208ed04e-73aa-4e57-bb58-26c807fcf570");
		
		public const string DifficultArm = "Открытый перелом руки";
		public static readonly Guid DifficultArmId = new("208ed04e-73aa-4e57-bb58-26c807fcf569");
		
		public const string DifficultLeg = "Открытый перелом ноги";
		public static readonly Guid DifficultLegId = new("208ed04e-73aa-4e57-bb58-26c807fcf568");

		public const string DifficultWing = "Открытый перелом крыла";
		public static readonly Guid DifficultWingId = new("208ed04e-73aa-4e57-bb58-26c807fcf567");

		public const string DifficultTail = "Открытый перелом хвоста";
		public static readonly Guid DifficultTailId = new("208ed04e-73aa-4e57-bb58-26c807fcf566");

		public const string DeadlyHead1 = "Потеря головы";
		public static readonly Guid DeadlyHead1Id = new("208ed04e-73aa-4e57-bb58-26c807fcf565");
		
		public const string DeadlyHead2 = "Повреждение глаза";
		public static readonly Guid DeadlyHead2Id = new("208ed04e-73aa-4e57-bb58-26c807fcf564");
		
		public const string DeadlyTorso1 = "Септический шок";
		public static readonly Guid DeadlyTorso1Id = new("208ed04e-73aa-4e57-bb58-26c807fcf563");
		
		public const string DeadlyTorso2 = "Травма сердца";
		public static readonly Guid DeadlyTorso2Id = new("208ed04e-73aa-4e57-bb58-26c807fcf562");
		
		public const string DeadlyArm = "Потеря руки";
		public static readonly Guid DeadlyArmId = new("208ed04e-73aa-4e57-bb58-26c807fcf561");
		
		public const string DeadlyLeg = "Потеря ноги";
		public static readonly Guid DeadlyLegId = new("208ed04e-73aa-4e57-bb58-26c807fcf560");
		
		public const string DeadlyWing = "Потеря крыла";
		public static readonly Guid DeadlyWingId = new("208ed04e-73aa-4e57-bb58-26c807fcf559");
		
		public const string DeadlyTail = "Потеря хвоста";
		public static readonly Guid DeadlyTailId = new("208ed04e-73aa-4e57-bb58-26c807fcf558");

		/// <summary>
		/// Тяжесть крита
		/// </summary>
		[Flags]
		public enum Severity
		{
			Unstabilizied = 1,
			Simple = 2,
			Complex = 4,
			Difficult = 8,
			Deadly = 16
		}
		/*

		симпл -2, стабилизировнный -1
		комплекс -3, стабилизировнный -2
		дифф /4, стабилизировнный /2
		простой /4, стабилизировнный /4
		*/
	}
}
