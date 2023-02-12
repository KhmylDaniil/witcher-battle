using MediatR;
using System;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Команда добавления защитного навыка для способности
	/// </summary>
	public class CreateDefensiveSkillCommand : IRequest
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
		/// Навык
		/// </summary>
		public Skill Skill { get; set; }
	}
}
