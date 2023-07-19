using System;
using MediatR;

namespace Witcher.Core.Contracts.GameRequests
{
	public sealed class DeleteGameCommand : IRequest
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
