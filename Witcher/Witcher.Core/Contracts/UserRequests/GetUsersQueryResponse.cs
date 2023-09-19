using System;

namespace Witcher.Core.Contracts.UserRequests
{
	/// <summary>
	/// Шаблон пользователя для отправки списка пользователей
	/// </summary>
	public sealed class GetUsersQueryResponse
	{
		/// <summary>
		/// Имя пользователя
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Емайл пользователя
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Телефон пользователя
		/// </summary>
		public string Phone { get; set; }

		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Дата создания
		/// </summary>
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Дата изменения
		/// </summary>
		public DateTime ModifiedOn { get; set; }

		/// <summary>
		/// Айди создавшего пользователя
		/// </summary>
		public Guid CreatedByUserId { get; set; }

		/// <summary>
		/// Айди изменившего пользователя
		/// </summary>
		public Guid ModifiedByUserId { get; set; }
	}
}
