using Witcher.Core.Abstractions;
using System;


namespace Witcher.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Команда удаления битвы
	/// </summary>
	public class DeleteBattleCommand : IValidatableCommand
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
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
