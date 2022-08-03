using Sindie.ApiService.Core.Abstractions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Эффект
	/// </summary>
	public abstract class Effect : EntityBase
	{
		private Creature _creature;
		private Condition _condition;

		/// <summary>
		/// Поле для <see cref="_creature"/>
		/// </summary>
		public const string CreatureField = nameof(_creature);

		/// <summary>
		/// Поле для <see cref="_condition"/>
		/// </summary>
		public const string ConditionField = nameof(_condition);

		/// <summary>
		/// Пустой конструктор для EF
		/// </summary>
		protected Effect()
		{
		}

		/// <summary>
		/// Базовый конструктор для эффекта
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="condition">Состояние</param>
		public Effect(Creature creature, Condition condition)
		{
			Creature = creature;
			Condition = condition;
		}

		public string Name { get; protected set; }

		/// <summary>
		/// Айди существа
		/// </summary>
		public Guid CreatureId { get; protected set; }

		/// <summary>
		/// Айди состояния
		/// </summary>
		public Guid EffectId { get; protected set; }

		/// <summary>
		/// Существо
		/// </summary>
		public Creature Creature
		{
			get => _creature;
			set
			{
				_creature = value ?? throw new ApplicationException("Необходимо передать существо");
				CreatureId = value.Id;
			}
		}

		/// <summary>
		/// Состояние
		/// </summary>
		public Condition Condition
		{
			get => _condition;
			set
			{
				_condition = value ?? throw new ApplicationException("Необходимо передать состояние");
				EffectId = value.Id;
				Name = value.Name;
			}
		}

		/// <summary>
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public abstract void Run(ref Creature creature, ref StringBuilder message);

		/// <summary>
		/// Автоматически прекратить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public abstract void AutoEnd(ref Creature creature, ref StringBuilder message);

		/// <summary>
		/// Попробовать снять эффект
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public abstract void Treat(IRollService rollService, ref Creature creature, ref StringBuilder message);

		/// <summary>
		/// Создание эффекта нужного вида
		/// </summary>
		/// <typeparam name="T">Наследуемый от эффекта тип</typeparam>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="attacker">Атакующий</param>
		/// <param name="target">Цель</param>
		/// <param name="condition">Состояние</param>
		/// <returns>Эффект нужного типа</returns>
		public static T CreateEffect<T>(IRollService rollService, Creature attacker, Creature target, Condition condition) where T : Effect
		{
			var name = "Sindie.ApiService.Core.Entities.Effects." + condition.Name + "Effect";

			Type type = Type.GetType(name);

			MethodInfo methodInfo = type.GetMethod("Create");

			ParameterExpression paramRollService = Expression.Parameter(typeof(IRollService), "rollService");
			ParameterExpression paramAttacker = Expression.Parameter(typeof(Creature), "attacker");
			ParameterExpression paramTarget = Expression.Parameter(typeof(Creature), "target");
			ParameterExpression paramCondition = Expression.Parameter(typeof(Condition), "condition");

			MethodCallExpression methodCall = Expression.Call(
				null, methodInfo, paramRollService, paramAttacker, paramTarget, paramCondition);

			Expression<Func<IRollService, Creature, Creature, Condition, T>> lambda =
				Expression.Lambda<Func<IRollService, Creature, Creature, Condition, T>>(
					methodCall,
					new ParameterExpression[] { paramRollService, paramAttacker, paramTarget, paramCondition }
					);

			Func<IRollService, Creature, Creature, Condition, T> func = lambda.Compile();

			return func(rollService, attacker, target, condition);
		}

		/// <summary>
		/// Создание критического эффекта нужного вида
		/// </summary>
		/// <typeparam name="T">Наследуемый от эффекта тип</typeparam>
		/// <param name="target">Цель</param>
		/// <param name="crit">Критический эффект</param>
		/// <returns>Критический эффект нужного типа</returns>
		public static T CreateCritEffect<T>(Creature target, Condition crit) where T : Effect
		{
			var name = "Sindie.ApiService.Core.Entities.Effects." + crit.Name + "Effect";

			Type type = Type.GetType(name);

			MethodInfo methodInfo = type.GetMethod("Create");

			ParameterExpression paramTarget = Expression.Parameter(typeof(Creature), "target");
			ParameterExpression paramCondition = Expression.Parameter(typeof(Condition), "crit");

			MethodCallExpression methodCall = Expression.Call(
				null, methodInfo, paramTarget, paramCondition);

			Expression<Func<Creature, Condition, T>> lambda =
				Expression.Lambda<Func<Creature, Condition, T>>(
					methodCall,
					new ParameterExpression[] { paramTarget, paramCondition }
					);

			Func<Creature, Condition, T> func = lambda.Compile();

			return func(target, crit);
		}
	}
}
