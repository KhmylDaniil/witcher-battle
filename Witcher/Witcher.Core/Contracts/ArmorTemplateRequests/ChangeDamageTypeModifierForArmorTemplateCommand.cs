using MediatR;
using System;
using Witcher.Core.Contracts.BaseRequests;

namespace Witcher.Core.Contracts.ArmorTemplateRequests
{
	public sealed class ChangeDamageTypeModifierForArmorTemplateCommand: ChangeDamageTypeModifierCommandBase, IRequest<Unit>
	{
		/// <summary>
		/// Айди шаблона брони
		/// </summary>
		public Guid ArmorTemplateId { get; set; }
	}
}
