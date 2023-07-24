using System.Collections.Generic;
using Witcher.Core.Contracts.BaseRequests;
using Witcher.Core.Contracts.ItemTemplateBase;

namespace Witcher.Core.Contracts.ArmorTemplateRequests
{
	public sealed class GetArmorTemplateByIdResponse : GetItemByIdResponseBase
	{
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

		/// <summary>
		/// Названия защищаемых частей тела
		/// </summary>
		public List<string> BodyTemplatePartsNames { get; set; }

		/// <summary>
		/// Список модификаторов по типу урона
		/// </summary>
		public List<GetResponsePartDamageTypeModifier> DamageTypeModifiers { get; set; }
	}
}