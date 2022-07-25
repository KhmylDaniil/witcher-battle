using Sindie.ApiService.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
		/// Создание эффекта нужного вида
		/// </summary>
		/// <typeparam name="T">Наследуемый от эффекта тип</typeparam>
		/// <param name="creature">Существо</param>
		/// <param name="condition">Состояние</param>
		/// <returns>Эффект нужного типа</returns>
		public static T CreateEffect<T>(Creature creature, Condition condition) where T : Effect
		{
			var name = "Sindie.ApiService.Core.Entities." + condition.Name + "Effect";

			Type type = Type.GetType(name);

			MethodInfo methodInfo = type.GetMethod("Create");

			ParameterExpression paramCreature = Expression.Parameter(typeof(Creature), "creature");
			ParameterExpression paramCondition = Expression.Parameter(typeof(Condition), "condition");

			MethodCallExpression methodCall = Expression.Call(
				null, methodInfo, paramCreature, paramCondition);

			Expression<Func<Creature, Condition, T>> lambda =
				Expression.Lambda<Func<Creature, Condition, T>>(
					methodCall,
					new ParameterExpression[] { paramCreature, paramCondition }
					);

			Func<Creature, Condition, T> func = lambda.Compile();

			return func(creature, condition);
		}
	}
}
