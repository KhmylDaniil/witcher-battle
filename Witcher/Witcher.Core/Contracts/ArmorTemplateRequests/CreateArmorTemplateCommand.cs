using System;
using System.Collections.Generic;
using Witcher.Core.Contracts.ItemTemplateBase;
using static Witcher.Core.BaseData.Enums;
using MediatR;

namespace Witcher.Core.Contracts.ArmorTemplateRequests
{
	public class CreateArmorTemplateCommand : CreateOrUpdateItemTemplateCommandBase, IRequest<Guid>
	{
		/// <summary>
		/// Айди шаблона тела
		/// </summary>
		public Guid BodyTemplateId { get; set; }

		/// <summary>
		/// Броня
		/// </summary>
		public int Armor { get; set; }

		/// <summary>
		/// Скованность движений
		/// </summary>
		public int EncumbranceValue { get; set; }

		/// <summary>
		/// Части тела, закрываемые броней
		/// </summary>
		public List<BodyPartType> BodyPartTypes { get; set; } = new();
	}
}
