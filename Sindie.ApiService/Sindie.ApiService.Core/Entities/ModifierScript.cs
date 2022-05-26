using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Скрипт модификатора
	/// </summary>
	public class ModifierScript : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_script"/>
		/// </summary>
		public const string ScriptField = nameof(_script);

		/// <summary>
		/// Поле для <see cref="_event"/>
		/// </summary>
		public const string EventField = nameof(_event);

		/// <summary>
		/// Поле для <see cref="_modifier"/>
		/// </summary>
		public const string ModifierField = nameof(_modifier);

		private Script _script;
		private Event _event;
		private Modifier _modifier;

		/// <summary>
		/// Конструктор для EF
		/// </summary>
		protected ModifierScript()
		{
		}

		/// <summary>
		/// Конструктор скрипта модификатора
		/// </summary>
		/// <param name="script">Скрипт</param>
		/// <param name="event">Событие</param>
		/// <param name="modifier">Модификатор</param>
		/// <param name="activationTime">Время активации модификатора</param>
		/// <param name="periodOfActivity">Период активации модификатора</param>
		/// <param name="periodOfInactivity">Период неактивности модификатора</param>
		/// <param name="numberOfRepetitions">Количество повторений активации модификатора</param>
		public ModifierScript(
			Script script,
			Event @event,
			Modifier modifier,
			DateTime activationTime,
			int periodOfActivity,
			int periodOfInactivity,
			int numberOfRepetitions)
		{
			Script = script;
			Event = @event;
			Modifier = modifier;
			ChangeActiveCyclesList(
				activationTime,
				periodOfActivity,
				periodOfInactivity,
				numberOfRepetitions);
		}

		/// <summary>
		/// Айди события
		/// </summary>
		public Guid? EventId { get; protected set; }

		/// <summary>
		/// Айди модификатора
		/// </summary>
		public Guid ModifierId { get; protected set; }

		/// <summary>
		/// Айди скрипта
		/// </summary>
		public Guid ScriptId { get; protected set; }


		#region navigation properties

		/// <summary>
		/// Скрипт
		/// </summary>
		public Script Script
		{
			get => _script;
			protected set
			{
				_script = value ?? throw new ApplicationException("Необходимо передать скрипт");
				ScriptId = value.Id;
			}
		}

		/// <summary>
		/// Событие
		/// </summary>
		public Event Event
		{
			get => _event;
			protected set
			{
				_event = value;
				EventId = value?.Id;
			}
		}

		/// <summary>
		/// Модификатор
		/// </summary>
		public Modifier Modifier
		{
			get => _modifier;
			protected set
			{
				_modifier = value ?? throw new ApplicationException("Необходимо передать игру");
				ModifierId = value.Id;
			}
		}

		/// <summary>
		/// Циклы активности скрипта модификатора
		/// </summary>
		public List<ActiveCycle> ActiveCycles { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="script">Скрипт</param>
		/// <param name="event">Событие</param>
		/// <param name="modifier">Модификатор</param>
		/// <param name="activationTime">Время активации</param>
		/// <param name="periodOfActivity">Период активности в минутах</param>
		/// <param name="periodOfInactivity">Период неактивности в минутах</param>
		/// <param name="numberOfRepetitions">Количество повторений активации</param>
		/// <returns>Скрипт модификатора</returns>
		[Obsolete("Только для тестов")]
		public static ModifierScript CreateForTest(
			Guid? id = default,
			Script script = default,
			Event @event = default,
			Modifier modifier = default,
			DateTime activationTime = default,
			int periodOfActivity = default,
			int periodOfInactivity = default,
			int numberOfRepetitions = default)
		{
			var result = new ModifierScript()
			{
				Id = id ?? Guid.NewGuid(),
				Script = script,
				Event = @event,
				Modifier = modifier};
			if (activationTime != default
				&& periodOfActivity != default
				&& periodOfInactivity != default
				&& numberOfRepetitions != default)
				result.ChangeActiveCyclesList(
				activationTime,
				periodOfActivity,
				periodOfInactivity,
				numberOfRepetitions);
			else
				result.ActiveCycles = new List<ActiveCycle>();

			return result;
		}
		
		/// <summary>
		/// Создать скрипт модификатора
		/// </summary>
		/// <param name="script">Скрипт</param>
		/// <param name="event">Событие</param>
		/// <param name="modifier">Модификатор</param>
		/// <param name="activationTime">Время активации</param>
		/// <param name="periodOfActivity">Период активности в минутах</param>
		/// <param name="periodOfInactivity">Период неактивности в минутах</param>
		/// <param name="numberOfRepetitions">Количество повторов</param>
		/// <returns>Скрипт модификатора</returns>
		public static ModifierScript CreateModifierScript(
			Script script,
			Event @event,
			Modifier modifier,
			DateTime activationTime,
			int periodOfActivity,
			int periodOfInactivity,
			int numberOfRepetitions)
		{
			var newModifierScript = new ModifierScript(
				script,
				@event,
				modifier,
				activationTime,
				periodOfActivity,
				periodOfInactivity,
				numberOfRepetitions);

			return newModifierScript;
		}

		/// <summary>
		/// Изменить скрипт модификатора
		/// </summary>
		/// <param name="script">Скрипт</param>
		/// <param name="event">Событие</param>
		/// <param name="activationTime">Время активации</param>
		/// <param name="periodOfActivity">Период активности в минутах</param>
		/// <param name="periodOfInactivity">Период неактивности в минутах</param>
		/// <param name="numberOfRepetitions">Количество повторений</param>
		public void ChangeModifierScript(
			Script script,
			Event @event,
			DateTime activationTime,
			int periodOfActivity,
			int periodOfInactivity,
			int numberOfRepetitions)
		{
			Script = script;
			Event = @event;
			ChangeActiveCyclesList(
				activationTime,
				periodOfActivity,
				periodOfInactivity,
				numberOfRepetitions);
		}

		/// <summary>
		/// Cоздани и изменение списка циклов активности скрипта модификатора
		/// </summary>
		/// <param name="activationTime">Начало первого цикла активности</param>
		/// <param name="periodOfActivity">Период активности в минутах</param>
		/// <param name="periodOfInactivity">Период неактивности в минутах</param>
		/// <param name="numberOfRepetitions">Количество повторений</param>
		/// <returns>Список циклов активности скрипта модификатора</returns>
		public void ChangeActiveCyclesList(
			DateTime activationTime,
			int periodOfActivity,
			int periodOfInactivity,
			int numberOfRepetitions)
		{
			if (periodOfActivity < 0)
				throw new Exception("Период активности должен быть больше нуля");
			if (periodOfInactivity < 0)
				throw new Exception("Период неактивности должен быть больше нуля");
			if (numberOfRepetitions < 0)
				throw new Exception("Количество повторений не может быть меньше нуля");

			ActiveCycles = new List<ActiveCycle>();

			if (periodOfActivity == 0)
				return;

			if (periodOfInactivity == 0)
			{
				ActiveCycles.Add(new ActiveCycle(
					activationBeginning: activationTime,
					activationEnd: activationTime.AddMinutes(periodOfActivity * (1 + numberOfRepetitions))
					));
				return;
			}

			for (int i = 0; i<= numberOfRepetitions; i++)
				ActiveCycles.Add(new ActiveCycle(
					activationBeginning: activationTime.AddMinutes((periodOfActivity + periodOfInactivity) * i),
					activationEnd: activationTime.AddMinutes((periodOfActivity + periodOfInactivity) * i + periodOfActivity)));
			return;
		}
	}
}
