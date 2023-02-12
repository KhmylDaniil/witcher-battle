using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Команда удаления защитного навыка для способности
	/// </summary>
	public class DeleteDefensiveSkillCommand : IRequest
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
	}
}
