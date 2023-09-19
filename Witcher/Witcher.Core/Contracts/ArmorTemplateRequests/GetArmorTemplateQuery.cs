using MediatR;
using System.Collections.Generic;
using Witcher.Core.Contracts.BaseRequests;

namespace Witcher.Core.Contracts.ArmorTemplateRequests
{
	public sealed class GetArmorTemplateQuery : GetBaseQuery, IRequest<IEnumerable<GetArmorTemplateResponse>>
	{
		/// <summary>
		/// Фильтр по названию
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Фильтр по типу закрываемой броней части тела
		/// </summary>
		public string BodyPartType { get; set; }

		/// <summary>
		/// Фильтр по типу урона
		/// </summary>
		public string DamageTypeModifier { get; set; }

		/// <summary>
		/// Минимальное количество брони
		/// </summary>
		public int MinArmor { get; set; }

		/// <summary>
		/// Максимальное количество брони
		/// </summary>
		public int MaxArmor { get; set; } = int.MaxValue;

		/// <summary>
		/// Фильтр по автору
		/// </summary>
		public string UserName { get; set; }
	}
}
