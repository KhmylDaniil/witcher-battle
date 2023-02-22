using Sindie.ApiService.Core.Abstractions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sindie.ApiService.Core.Contracts.GameRequests.GetGameById
{
	public sealed class GetGameByIdCommand : IValidatableCommand<GetGameByIdResponse>
	{
		/// <summary>
		/// Айди
		/// </summary>
		[Required]
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
