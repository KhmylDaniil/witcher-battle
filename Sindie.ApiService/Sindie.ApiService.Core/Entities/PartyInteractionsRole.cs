using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Роль стороны
	/// </summary>
	public class PartyInteractionsRole : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_interactionsRole"/>
		/// </summary>
		public const string InteractionsRoleField = nameof(_interactionsRole);

		private InteractionsRole _interactionsRole;

		/// <summary>
		/// Поле для <see cref="_party"/>
		/// </summary>
		public const string PartyField = nameof(_party);

		private Party _party;

		/// <summary>
		/// Айди роли взаимодействия
		/// </summary>
		public Guid InteractionsRoleId { get; protected set; }

		/// <summary>
		/// Айди стороны
		/// </summary>
		public Guid PartyId { get; protected set; }

		/// <summary>
		/// Минимальное количество персонажей
		/// </summary>
		public int? MinQuantityCharacters { get; set; }

		/// <summary>
		/// Максимальное количество персонажей
		/// </summary>
		public int? MaxQuantityCharacters { get; set; }

		#region navigation properties

		/// <summary>
		/// Роль во взаимодействии
		/// </summary>
		public InteractionsRole InteractionsRole
		{
			get => _interactionsRole;
			set
			{
				_interactionsRole = value;
				InteractionsRoleId = value.Id;
			}
		}

		/// <summary>
		/// Сторона
		/// </summary>
		public Party Party
		{
			get => _party;
			set
			{
				_party = value;
				PartyId = value.Id;
			}
		}
		#endregion navigation properties
	}
}
