using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Деятельность роли взаимодействия
	/// </summary>
	public class InteractionsRoleActivity : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_interactionsRole"/>
		/// </summary>
		public const string InteractionsRoleField = nameof(_interactionsRole);

		private InteractionsRole _interactionsRole;

		/// <summary>
		/// Поле для <see cref="_activity"/>
		/// </summary>
		public const string ActivityField = nameof(_activity);

		private Activity _activity;

		/// <summary>
		/// Айди роли взаимодействия
		/// </summary>
		public Guid InteractionsRoleId { get; protected set; }

		/// <summary>
		/// Айди деятельности
		/// </summary>
		public Guid ActivityId { get; protected set; }

		/// <summary>
		/// Порядок деятельности
		/// </summary>
		public int? Order { get; set; }

		#region navigation properties

		/// <summary>
		/// Роль во взаимодействии
		/// </summary>
		public InteractionsRole InteractionsRole
		{
			get => _interactionsRole;
			set
			{
				_interactionsRole = value ?? throw new ApplicationException("Необходимо передать взаимодействие");
				InteractionsRoleId = value.Id;
			}
		}

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
		#endregion navigation properties
	}
}
