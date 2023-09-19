using System;
using System.Collections.Generic;
using Witcher.Core.Contracts.ItemTemplateBase;
using static Witcher.Core.BaseData.Enums;
using MediatR;

namespace Witcher.Core.Contracts.ArmorTemplateRequests
{
	public sealed class ChangeArmorTemplateCommand : CreateOrUpdateItemTemplateCommandBase, IRequest
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

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
		public List<BodyPartType> BodyPartTypes { get; set; }
	}
}
