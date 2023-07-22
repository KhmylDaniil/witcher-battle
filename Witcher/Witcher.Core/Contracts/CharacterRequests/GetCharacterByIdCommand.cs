using MediatR;
using System;

namespace Witcher.Core.Contracts.CharacterRequests
{
	public sealed class GetCharacterByIdCommand : IRequest<GetCharacterByIdResponse>
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }
	}
}
