using MediatR;
using System;

namespace Witcher.Core.Contracts.CharacterRequests
{
	public sealed class DeleteCharacterCommand : IRequest<Unit>
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }
	}
}
