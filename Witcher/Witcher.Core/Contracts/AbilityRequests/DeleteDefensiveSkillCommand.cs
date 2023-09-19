using System;
using MediatR;

namespace Witcher.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Команда удаления защитного навыка для способности
	/// </summary>
	public sealed class DeleteDefensiveSkillCommand : IRequest
	{
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
