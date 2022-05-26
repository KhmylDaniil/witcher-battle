using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Предмет во взаимодействии
	/// </summary>
	public class InteractionItem : EntityBase
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
		/// Поле для <see cref="_item"/>
		/// </summary>
		public const string ItemField = nameof(_item);

		private Item _item;

		/// <summary>
		/// Айди роли взаимодействия
		/// </summary>
		public Guid InteractionsRoleId { get; protected set; }

		/// <summary>
		/// Айди деятельности во взаимодействии
		/// </summary>
		public Guid ActivityId { get; protected set; }

		/// <summary>
		/// Айди предмета во взаимодействии
		/// </summary>
		public Guid ItemId { get; protected set; }

		/// <summary>
		/// Максимальное количество применений
		/// </summary>
		public int ExpendQuantity { get; set; }

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

		/// <summary>
		/// Предмет
		/// </summary>
		public Item Item
		{
			get => _item;
			set
			{
				_item = value ?? throw new ApplicationException("Необходимо передать предмет");
				ItemId = value.Id;
			}
		}
		#endregion navigation properties
	}
}
