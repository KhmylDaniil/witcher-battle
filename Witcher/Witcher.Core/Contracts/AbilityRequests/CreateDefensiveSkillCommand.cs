using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions.RequestExceptions;
using System;
using static Witcher.Core.BaseData.Enums;
using MediatR;

namespace Witcher.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Команда добавления защитного навыка для способности
	/// </summary>
	public sealed class CreateDefensiveSkillCommand : IRequest
	{
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
