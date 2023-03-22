using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Exceptions.EntityExceptions;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Критический эффект
	/// </summary>
	public abstract class CritEffect : Effect
	{
		private CreaturePart _creaturePart;

		/// <summary>
		/// Поле для <see cref="_creaturePart"/>
		/// </summary>
		public const string CreaturePartField = nameof(_creaturePart);

		/// <summary>
		/// Пустой конструктор для EF
		/// </summary>
		protected CritEffect()
		{
		}

		/// <summary>
		/// Базовый конструктор для эффекта
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="aimedPart">Часть тела</param>
		public CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, name)
		{
			CreaturePart = aimedPart;
		}

		/// <summary>
		/// Тяжесть критического эффекта
		/// </summary>
		public Severity Severity { get; protected set; }

		/// <summary>
		/// Тип части тела
		/// </summary
		public BodyPartType BodyPartLocation { get; protected set; } 

		/// <summary>
		/// Айди части тела
		/// </summary>
		public Guid CreaturePartId { get; protected set; }

		/// <summary>
		/// Часть тела
		/// </summary>
		public CreaturePart CreaturePart
		{
			get => _creaturePart;
			set
			{
				_creaturePart = value ?? throw new EntityNotIncludedException<CritEffect>(nameof(CreaturePart));

				CreaturePartId = value.Id;
			}
		}

		/// <summary>
		/// Создание критического эффекта нужного вида
		/// </summary>
		/// <typeparam name="T">Наследуемый от эффекта тип</typeparam>
		/// <param name="target">Цель</param>
		/// <param name="crit">Критический эффект</param>
		/// <returns>Критический эффект нужного типа</returns>
		public static T CreateCritEffect<T>(Creature target, CreaturePart aimedPart, Condition crit) where T : Effect
		{
			var name = "Witcher.Core.Entities.Effects." + Enum.GetName(crit) + "CritEffect";

			Type type = Type.GetType(name);

			MethodInfo methodInfo = type.GetMethod("Create");

			ParameterExpression paramTarget = Expression.Parameter(typeof(Creature), "target");
			ParameterExpression paramAimedPart = Expression.Parameter(typeof(CreaturePart), "aimedPart");
			ParameterExpression paramName = Expression.Parameter(typeof(string), "name");
			MethodCallExpression methodCall = Expression.Call(
				null, methodInfo, paramTarget, paramAimedPart, paramName);

			Expression<Func<Creature, CreaturePart, string, T>> lambda =
				Expression.Lambda<Func<Creature, CreaturePart, string, T>>(
					methodCall, paramTarget, paramAimedPart, paramName);

			Func<Creature, CreaturePart, string, T> func = lambda.Compile();

			return func(target, aimedPart, CritNames.GetConditionFullName(crit));
		}

		/// <summary>
		/// Применение общего пенальти
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="newCrit">Критический эффект с общим пенальти</param>
		public static void ApplySharedPenalty(Creature creature, ISharedPenaltyCrit newCrit)
		{
			ISharedPenaltyCrit existingCrit = creature.Effects.FirstOrDefault(x =>
				x is ISharedPenaltyCrit crit && crit.BodyPartLocation == newCrit.BodyPartLocation && crit.PenaltyApplied) as ISharedPenaltyCrit;

			if (existingCrit is not null && existingCrit.Severity >= newCrit.Severity)
				return;

			if (existingCrit != null)
				existingCrit.RevertStatChanges(creature);

			newCrit.ApplyStatChanges(creature);
		}

		/// <summary>
		/// Перенос общего пенальти на другой критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="updatingCrit">Критический эффект с общим пенальти</param>
		/// <returns>Общее пенальти перенесено</returns>
		public static void SharedPenaltyMovedToAnotherCrit(Creature creature, ISharedPenaltyCrit updatingCrit)
		{
			var severiestCrit = creature.Effects.Where(x => x.Id != updatingCrit.Id
			&& x is ISharedPenaltyCrit crit
			&& crit.BodyPartLocation == updatingCrit.BodyPartLocation
			&& crit.Severity > updatingCrit.Severity
			&& !crit.PenaltyApplied).Cast<ISharedPenaltyCrit>().OrderByDescending(x => x.Severity).FirstOrDefault();

			if (severiestCrit is null)
				return;

			updatingCrit.RevertStatChanges(creature);
			severiestCrit.ApplyStatChanges(creature);
		}

		/// <summary>
		/// Ппроверка наличия такого же эффекта и удаление стабилизированной версии эффекта при ее наличии
		/// </summary>
		/// <typeparam name="T">Тип Критического эффекта</typeparam>
		/// <param name="creature">Существо</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Возможность создания эффекта</returns>
		public static bool CheckExistingEffectAndRemoveStabilizedEffect<T>(Creature creature, CreaturePart aimedPart) where T : CritEffect
		{
			if (creature.Effects.FirstOrDefault(x => x is T crit && crit.CreaturePartId == aimedPart.Id) is not ICrit existingCrit)
				return true;

			if (!IsStabile(existingCrit.Severity))
				return false;

			if (existingCrit is not ISharedPenaltyCrit sharedPenaltyCrit || sharedPenaltyCrit.PenaltyApplied)
				existingCrit.RevertStatChanges(creature);

			creature.Effects.Remove((CritEffect)existingCrit);

			return true;
		}
	}
}
