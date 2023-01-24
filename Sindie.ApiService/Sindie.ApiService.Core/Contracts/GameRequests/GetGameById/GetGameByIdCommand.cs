using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sindie.ApiService.Core.Contracts.GameRequests.GetGameById
{
	public sealed class GetGameByIdCommand : IRequest<GetGameByIdResponse>
	{
		/// <summary>
		/// Айди
		/// </summary>
		[Required]
		public Guid Id { get; set; }
	}
}
