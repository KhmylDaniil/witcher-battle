﻿using Sindie.ApiService.Core.Abstractions;
using System;
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

		/// <summary>
		/// Поле для <see cref="_creature"/>
		/// </summary>
		public const string CreatureField = nameof(_creature);

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
		protected Effect(Creature creature, string name)
		{
			Creature = creature;
			Name = name;
		}

		/// <summary>
		/// Название эффекта
		/// </summary>
		public string Name { get; protected set; }

		/// <summary>
		/// Айди существа
		/// </summary>
		public Guid CreatureId { get; protected set; }

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
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public abstract void Run(Creature creature, ref StringBuilder message);

		/// <summary>
		/// Автоматически прекратить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public abstract void AutoEnd(Creature creature, ref StringBuilder message);

		/// <summary>
		/// Попробовать снять эффект
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public abstract void Treat(IRollService rollService, Creature creature, ref StringBuilder message);

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
			ParameterExpression paramName = Expression.Parameter(typeof(string), "name");
			MethodCallExpression methodCall = Expression.Call(
				null, methodInfo, paramRollService, paramAttacker, paramTarget, paramName);

			Expression<Func<IRollService, Creature, Creature, string, T>> lambda =
				Expression.Lambda<Func<IRollService, Creature, Creature, string, T>>(
					methodCall,
					new ParameterExpression[] { paramRollService, paramAttacker, paramTarget, paramName }
					);

			Func<IRollService, Creature, Creature, string, T> func = lambda.Compile();

			return func(rollService, attacker, target, condition.Name);
		}
	}
}