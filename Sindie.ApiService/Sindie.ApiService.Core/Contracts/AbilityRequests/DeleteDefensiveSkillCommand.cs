using MediatR;
using Sindie.ApiService.Core.Abstractions;
using System;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Команда удаления защитного навыка для способности
	/// </summary>
	public class DeleteDefensiveSkillCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid AbilityId { get; set; }

		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
