using System;
using MediatR;

namespace Witcher.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Команда удаления навыка шаблона существа
	/// </summary>
	public sealed class DeleteCreatureTemplateSkillCommand : IRequest
	{
		/// <summary>
		/// Айди шаблона существ
		/// </summary>
		public Guid CreatureTemplateId { get; set; }

		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }
	}
}
