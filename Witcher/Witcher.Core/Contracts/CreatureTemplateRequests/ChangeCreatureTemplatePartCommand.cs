using System;
using MediatR;

namespace Witcher.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Команда изменения брони для шаблона существа или его части
	/// </summary>
	public sealed class ChangeCreatureTemplatePartCommand : IRequest
	{
		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; set; }

		/// <summary>
		/// Айди
		/// </summary>
		public Guid? Id { get; set; }

		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Значение брони
		/// </summary>
		public int ArmorValue { get; set; }
	}
}
