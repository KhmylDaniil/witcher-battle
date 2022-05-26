using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Действие деятельности
	/// </summary>
	public class ActivityAction : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_activity"/>
		/// </summary>
		public const string ActivityField = nameof(_activity);

		private Activity _activity;

		/// <summary>
		/// Поле для <see cref="_action"/>
		/// </summary>
		public const string ActionField = nameof(_action);

		private Action _action;

		/// <summary>
		/// Айди деятельности
		/// </summary>
		public Guid ActivityId { get; protected set; }

		/// <summary>
		/// Айди действия
		/// </summary>
		public Guid ActionId { get; protected set; }

		/// <summary>
		/// Порядок действий
		/// </summary>
		public int Order { get; set; }

		#region navigation properties

		/// <summary>
		/// Деятельность
		/// </summary>
		public Activity Activity
		{
			get => _activity;
			set
			{
				_activity = value ?? throw new ApplicationException("Необходимо передать деятельность");
				ActivityId = value.Id;
			}
		}

		/// <summary>
		/// Действие
		/// </summary>
		public Action Action
		{
			get => _action;
			set
			{
				_action = value ?? throw new ApplicationException("Необходимо передать действие");
				ActionId = value.Id;
			}
		}
		#endregion navigation properties
	}
}
