using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Параметры персонажей
	/// </summary>
	public class CharacterParameter : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_character"/>
		/// </summary>
		public const string CharacterField = nameof(_character);

		/// <summary>
		/// Поле для <see cref="_parameter"/>
		/// </summary>
		public const string ParameterField = nameof(_parameter);

		private Character _character;
		private Parameter _parameter;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected CharacterParameter()
		{
		}

		/// <summary>
		/// Параметр персонажа
		/// </summary>
		/// <param name="parameterValue">Значение параметра персонажа</param>
		/// <param name="character">Персонаж</param>
		/// <param name="parameter">Параметр</param>
		public CharacterParameter(
			double parameterValue,
			Character character,
			Parameter parameter)
		{
			Character = character;
			Parameter = parameter;
			ParameterValue = parameterValue;
		}

		/// <summary>
		/// Айди персонажа
		/// </summary>
		public Guid CharacterId { get; protected set; }

		/// <summary>
		/// Айди параметра
		/// </summary>
		public Guid ParameterId { get; protected set; }

		/// <summary>
		/// значение параметра у персонажа
		/// </summary>
		public double ParameterValue { get; set; }

		#region navigation properties

		/// <summary>
		/// Персонаж
		/// </summary>
		public Character Character
		{
			get => _character;
			set
			{
				_character = value ?? throw new ApplicationException("Необходимо передать персонажа");
				CharacterId = value.Id;
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