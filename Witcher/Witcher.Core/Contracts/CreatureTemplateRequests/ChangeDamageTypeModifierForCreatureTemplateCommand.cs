using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions.RequestExceptions;
using System;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Contracts.BaseRequests;

namespace Witcher.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Команда изменения модификатора урона по типу 
	/// </summary>
	public class ChangeDamageTypeModifierForCreatureTemplateCommand : ChangeDamageTypeModifierCommandBase, IValidatableCommand
	{
		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; set; }
	}
}
