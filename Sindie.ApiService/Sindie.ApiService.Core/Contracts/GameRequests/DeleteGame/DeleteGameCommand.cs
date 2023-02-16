using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sindie.ApiService.Core.Contracts.GameRequests.DeleteGame
{
	public class DeleteGameCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди
		/// </summary>
		[Required]
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
				throw new RequestFieldNullException<DeleteGameCommand>(nameof(Name));
		}
	}
}
