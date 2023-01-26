using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sindie.ApiService.Core.Contracts.GameRequests.DeleteGame
{
	public class DeleteGameCommand : IRequest<Unit>
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
	}
}
