using System;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BaseRequests;

namespace Witcher.Core.Contracts.ArmorTemplateRequests
{
	public class ChangeDamageTypeModifierForArmorTemplateCommand: ChangeDamageTypeModifierCommandBase, IValidatableCommand
	{
		/// <summary>
		/// Айди шаблона брони
		/// </summary>
		public Guid ArmorTemplateId { get; set; }
	}
}
