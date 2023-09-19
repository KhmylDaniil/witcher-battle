using MediatR;
using System.Collections.Generic;
using Witcher.Core.Contracts.BaseRequests;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Contracts.WeaponTemplateRequests
{
	public sealed class GetWeaponTemplateQuery : GetBaseQuery, IRequest<IEnumerable<GetWeaponTemplateResponse>>
	{
		/// <summary>
		/// Фильтр по названию
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Навык атаки
		/// </summary>
		public string AttackSkillName { get; set; }

		/// <summary>
		/// Фильтр по типу урона
		/// </summary>
		public string DamageType { get; set; }

		/// <summary>
		/// фильтр по названию накладываемого состояния
		/// </summary>
		public string ConditionName { get; set; }

		/// <summary>
		/// Минимальное количество кубов атаки
		/// </summary>
		public int MinAttackDiceQuantity { get; set; }

		/// <summary>
		/// Максимальное количество кубов атаки
		/// </summary>
		public int MaxAttackDiceQuantity { get; set; } = int.MaxValue;

		/// <summary>
		/// Фильтр по автору
		/// </summary>
		public string UserName { get; set; }
	}
}
