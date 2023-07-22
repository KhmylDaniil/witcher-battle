using MediatR;
using System;
using Witcher.Core.Entities;

namespace Witcher.Core.Contracts.CharacterRequests
{
	public sealed class CreateCharacterCommand : IRequest<Character>
	{
		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; set; }

		/// <summary>
		/// Название существа
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание существа
		/// </summary>
		public string Description { get; set; }
	}
}
