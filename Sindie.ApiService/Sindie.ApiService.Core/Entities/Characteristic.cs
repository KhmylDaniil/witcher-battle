using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Характеристика
	/// </summary>
	public class Characteristic : Prerequisite
	{
		/// <summary>
		/// Поле для <see cref="_interaction"/>
		/// </summary>
		public const string InteractionField = nameof(_interaction);

		private Interaction _interaction;

		/// <summary>
		/// Айди взаимодействия
		/// </summary>
		public Guid InteractionId { get; protected set; }

		/// <summary>
		/// Минимальное значение
		/// </summary>
		public int MinCondition { get; set; }

		/// <summary>
		/// Максимальное значение
		/// </summary>
		public int MaxCondition { get; set; }

		#region navigation properties

		/// <summary>
		/// Взаимодействие
		/// </summary>
		public Interaction Interaction
		{
			get => _interaction;
			set
			{
				_interaction = value;
				InteractionId = value.Id;
			}
		}
		#endregion navigation properties
	}
}
