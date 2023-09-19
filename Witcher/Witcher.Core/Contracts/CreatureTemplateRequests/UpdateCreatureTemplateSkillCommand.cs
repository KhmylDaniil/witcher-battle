using System;
using MediatR;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Команда на создание/изменение навыка шаблона персонажа
	/// </summary>
	public class UpdateCreatureTemplateSkillCommand : IRequest
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid? Id { get; set; }

		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; set; }

		/// <summary>
		/// Навык
		/// </summary>
		public Skill Skill { get; set; }

		/// <summary>
		/// Значение навыка
		/// </summary>
		public int Value { get; set; } = 1;
	}
}
