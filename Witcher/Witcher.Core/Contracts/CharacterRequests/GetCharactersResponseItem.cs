using System;

namespace Witcher.Core.Contracts.CharacterRequests
{
	public sealed class GetCharactersResponseItem
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Имя владющего пользователя
		/// </summary>
		public string OwnerName { get; set; }
	}
}