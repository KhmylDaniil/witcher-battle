using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Действие
	/// </summary>
	public class Action : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_interaction"/>
		/// </summary>
		public const string InteractionField = nameof(_interaction);

		private Interaction _interaction;

		/// <summary>
		/// Поле для <see cref="_scenarioAction"/>
		/// </summary>
		public const string ScenarioActionField = nameof(_scenarioAction);

		private Script _scenarioAction;

		/// <summary>
		/// Айди взаимодействия
		/// </summary>
		public Guid InteractionId { get; protected set; }

		/// <summary>
		/// Айди сценария действия во взаимодействии
		/// </summary>
		public Guid ScenarioActionId { get; protected set; }

		/// <summary>
		/// Название действия
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание действия
		/// </summary>
		public string Description { get; set; }

		#region navigation properties

		/// <summary>
		/// Взаимодействие
		/// </summary>
		public Interaction Interaction
		{
			get => _interaction;
			set
			{
				_interaction = value ?? throw new ApplicationException("Необходимо передать взаимодействие");
				InteractionId = value.Id;
			}
		}

		/// <summary>
		/// Сценарий действия во взаимодействии
		/// </summary>
		public Script ScenarioAction
		{
			get => _scenarioAction;
			set
			{
				_scenarioAction = value ?? throw new ApplicationException("Необходимо передать взаимодействие");
				ScenarioActionId = value.Id;
			}
		}

		/// <summary>
		/// Действия деятельности
		/// </summary>
		public List<ActivityAction> ActivityActions { get; set; }

		#endregion navigation properties
	}
}
