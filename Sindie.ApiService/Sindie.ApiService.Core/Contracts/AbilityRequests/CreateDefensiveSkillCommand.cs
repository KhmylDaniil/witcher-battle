using MediatR;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Команда добавления защитного навыка для способности
	/// </summary>
	public class CreateDefensiveSkillCommand : IValidatableCommand
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

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			if (!Enum.IsDefined(Skill))
				throw new RequestFieldIncorrectDataException<CreateDefensiveSkillCommand>(nameof(Skill));
		}
	}
}
