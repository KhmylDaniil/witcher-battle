using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Параметры существа
	/// </summary>
	public class CreatureTemplateParameter : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_creatureTemplate"/>
		/// </summary>
		public const string CreatureTemplateField = nameof(_creatureTemplate);

		/// <summary>
		/// Поле для <see cref="_parameter"/>
		/// </summary>
		public const string ParameterField = nameof(_parameter);

		private CreatureTemplate _creatureTemplate;
		private Parameter _parameter;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected CreatureTemplateParameter()
		{
		}

		/// <summary>
		/// Параметр швблона существа
		/// </summary>
		/// <param name="parameterValue">Значение параметра шаблона существа</param>
		/// <param name="creatureTemplate">Шаблон существа</param>
		/// <param name="parameter">Параметр</param>
		public CreatureTemplateParameter(
			double parameterValue,
			CreatureTemplate creatureTemplate,
			Parameter parameter)
		{
			CreatureTemplate = creatureTemplate;
			Parameter = parameter;
			ParameterValue = parameterValue;
		}

		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; protected set; }

		/// <summary>
		/// Айди параметра
		/// </summary>
		public Guid ParameterId { get; protected set; }

		/// <summary>
		/// значение параметра у шаблона существа
		/// </summary>
		public double ParameterValue { get; set; }

		#region navigation properties

		/// <summary>
		/// Шаблон существа
		/// </summary>
		public CreatureTemplate CreatureTemplate
		{
			get => _creatureTemplate;
			set
			{
				_creatureTemplate = value ?? throw new ApplicationException("Необходимо передать шаблон существа");
				CreatureTemplateId = value.Id;
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
