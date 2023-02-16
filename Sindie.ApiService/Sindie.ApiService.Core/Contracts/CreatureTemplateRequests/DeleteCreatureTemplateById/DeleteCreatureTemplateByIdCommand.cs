using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.AbilityRequests.DeleteAbilitybyId;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.DeleteCreatureTemplateById
{
	/// <summary>
	/// Команда на удаление шаблона существа по айди
	/// </summary>
	public sealed class DeleteCreatureTemplateByIdCommand: IValidatableCommand
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

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
			if (string.IsNullOrEmpty(Name))
				throw new RequestFieldNullException<DeleteAbilityByIdCommand>(nameof(Name));
		}
	}
}
