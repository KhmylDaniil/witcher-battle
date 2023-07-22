using System;
using Witcher.Core.Contracts.BaseRequests;
using MediatR;

namespace Witcher.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Команда изменения модификатора урона по типу 
	/// </summary>
	public class ChangeDamageTypeModifierForCreatureTemplateCommand : ChangeDamageTypeModifierCommandBase, IRequest
	{
		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; set; }
	}
}
