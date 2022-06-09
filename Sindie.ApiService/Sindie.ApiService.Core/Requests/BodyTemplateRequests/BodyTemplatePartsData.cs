﻿using Sindie.ApiService.Core.Requests.BodyTemplateRequests.ChangeBodyTemplate;
using Sindie.ApiService.Core.Requests.BodyTemplateRequests.CreateBodyTemplate;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Requests.BodyTemplateRequests
{
	public class BodyTemplatePartsData
	{
		/// <summary>
		/// Название части тела
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Модификатор урона
		/// </summary>
		public double DamageModifier { get; set; }

		/// <summary>
		/// Пенальти за прицеливание
		/// </summary>
		public int HitPenalty { get; set; }

		/// <summary>
		/// Минимум на попадание
		/// </summary>
		public int MinToHit { get; set; }

		/// <summary>
		/// Максимум на попадание
		/// </summary>
		public int MaxToHit { get; set; }

		/// <summary>
		/// Создание данных для списка шаблонов частей тела
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Данные для списка шаблонов частей тела</returns>
		public static List<BodyTemplatePartsData>
			CreateBodyTemplatePartsData(CreateBodyTemplateCommand request)
		{
			var result = new List<BodyTemplatePartsData>();

			foreach (var part in request.BodyTemplateParts)
				result.Add(new BodyTemplatePartsData()
				{
					Name = part.Name,
					DamageModifier = part.DamageModifier,
					HitPenalty = part.HitPenalty,
					MinToHit = part.MinToHit,
					MaxToHit = part.MaxToHit
				});
			return result;
		}

		/// <summary>
		/// Создание данных для списка шаблонов частей тела
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Данные для списка шаблонов частей тела</returns>
		public static List<BodyTemplatePartsData>
			CreateBodyTemplatePartsData(ChangeBodyTemplateCommand request)
		{
			var result = new List<BodyTemplatePartsData>();

			foreach (var part in request.BodyTemplateParts)
				result.Add(new BodyTemplatePartsData()
				{
					Name = part.Name,
					DamageModifier = part.DamageModifier,
					HitPenalty = part.HitPenalty,
					MinToHit = part.MinToHit,
					MaxToHit = part.MaxToHit
				});
			return result;
		}
	}
}