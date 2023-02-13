using MediatR;
using Sindie.ApiService.Core.Entities;
using System;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbilityById
{
	public class GetAbilityByIdQuery : IRequest<Ability>
	{
		public Guid Id { get; set; }

		public Guid GameId { get; set; }
	}
}
