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
			}
		}

		#endregion navigation properties
	}
}
