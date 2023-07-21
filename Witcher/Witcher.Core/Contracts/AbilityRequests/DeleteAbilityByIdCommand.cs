using System;
using MediatR;

namespace Witcher.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Команда на удаление способности по айди
	/// </summary>
	public sealed class DeleteAbilityByIdCommand : IRequest
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
