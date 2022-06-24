using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Параметр существа
	/// </summary>
	public class CreatureParameter: EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_creature"/>
		/// </summary>
		public const string CreatureField = nameof(_creature);

		/// <summary>
		/// Поле для <see cref="_parameter"/>
		/// </summary>
		public const string ParameterField = nameof(_parameter);

		private Creature _creature;
		private Parameter _parameter;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected CreatureParameter()
		{
		}

		/// <summary>
		/// Параметр существа
		/// </summary>
		/// <param name="parameterValue">Значение параметра существа</param>
		/// <param name="creature">Cущество</param>
		/// <param name="parameter">Параметр</param>
		public CreatureParameter(
			int parameterValue,
			Creature creature,
			Parameter parameter)
		{
			Creature = creature;
			Parameter = parameter;
			ParameterValue = parameterValue;
		}

		/// <summary>
		/// Айди существа
		/// </summary>
		public Guid CreatureId { get; protected set; }

		/// <summary>
		/// Айди параметра
		/// </summary>
		public Guid ParameterId { get; protected set; }

		/// <summary>
		/// Название корреспондирующей характеристики
		/// </summary>
		public string StatName { get; protected set; }

		/// <summary>
		/// значение параметра у существа
		/// </summary>
		public int ParameterValue { get; set; }

		#region navigation properties

		/// <summary>
		/// Шаблон существа
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
		/// Параметр
		/// </summary>
		public Parameter Parameter
		{
			get => _parameter;
			set
			{
				_parameter = value ?? throw new ApplicationException("Необходимо передать параметр");
				ParameterId = value.Id;
				StatName = value.StatName;
			}
		}

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="creature">Существо</param>
		/// <param name="parameter">Параметр</param>
		/// <param name="value">Значение параметра</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns>Параметр существа</returns>
		[Obsolete("Только для тестов")]
		public static CreatureParameter CreateForTest(
			Guid? id = default,
			Creature creature = default,
			Parameter parameter = default,
			int value = default,
			string statName = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new CreatureParameter()
		{
			Id = id ?? Guid.NewGuid(),
			Creature = creature,
			Parameter = parameter,
			ParameterValue = value == 0 ? 1 : value,
			StatName = statName ?? "Ref",
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId
		};
	}
}
