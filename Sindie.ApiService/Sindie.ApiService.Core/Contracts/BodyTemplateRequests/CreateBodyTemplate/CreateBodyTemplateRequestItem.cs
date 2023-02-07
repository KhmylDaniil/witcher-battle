﻿using System.ComponentModel.DataAnnotations;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Contracts.BodyTemplateRequests.CreateBodyTemplate
{
	/// <summary>
	/// Элемент запроса создания шаблона тела
	/// </summary>
	public class CreateBodyTemplateRequestItem
	{
		/// <summary>
		/// Название части тела
		/// </summary>
		[Required]
		public string Name { get; set; }

		/// <summary>
		/// Тип части тела
		/// </summary>
		[Required]
		public BodyPartType BodyPartType { get; set; }

		/// <summary>
		/// Модификатор урона
		/// </summary>
		[Required]
		public double DamageModifier { get; set; }

		/// <summary>
		/// Пенальти за прицеливание
		/// </summary>
		[Range(0, int.MaxValue)]
		public int HitPenalty { get; set; }

		/// <summary>
		/// Минимум на попадание
		/// </summary>
		[Range(1, BaseData.DiceValue.Value)]
		public int MinToHit { get; set; }

		/// <summary>
		/// Максимум на попадание
		/// </summary>
		[Range(1, BaseData.DiceValue.Value)]
		public int MaxToHit { get; set; }
	}
}
