using static Witcher.Core.BaseData.Enums;
using System;
using System.Collections.Generic;

namespace Witcher.Core.Contracts.ArmorTemplateRequests
{
	public class GetArmorTemplateResponse
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Наазвание
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Броня
		/// </summary>
		public int Armor { get; set; }

		/// <summary>
		/// Скованность движений
		/// </summary>
		public int EncumbranceValue { get; set; }

		/// <summary>
		/// Название шаблона тела
		/// </summary>
		public string BodyTemplateName { get; set; }
	}
}