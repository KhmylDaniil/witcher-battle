using Sindie.ApiService.Core.Contracts.BodyTemplatePartsRequests;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.ChangeBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.CreateBodyTemplate;
using Sindie.ApiService.Core.Entities;
using System;
using System.Collections.Generic;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Requests.BodyTemplateRequests
{
	/// <summary>
	/// Данные для обновления частей шаблона тела
	/// </summary>
	public class BodyTemplatePartsData
	{
		/// <summary>
		/// Название части тела
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Тип части тела
		/// </summary>
		public BodyPartType BodyPartType { get; set; }

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
			CreateBodyTemplatePartsData(CreateBodyTemplateRequest request)
		{
			if (request == null) return Drafts.BodyTemplateDrafts.CreateBodyTemplatePartsDraft.CreateBodyPartsDraft();

			var result = new List<BodyTemplatePartsData>();

			foreach (var part in request.BodyTemplateParts)
				result.Add(new BodyTemplatePartsData()
				{
					Name = part.Name,
					BodyPartType = part.BodyPartType,
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
			CreateBodyTemplatePartsData(ChangeBodyTemplateRequest request)
		{
			if (request == null) return null;
			
			var result = new List<BodyTemplatePartsData>();

			foreach (var part in request.BodyTemplateParts)
				result.Add(new BodyTemplatePartsData()
				{
					Name = part.Name,
					BodyPartType = part.BodyPartType,
					DamageModifier = part.DamageModifier,
					HitPenalty = part.HitPenalty,
					MinToHit = part.MinToHit,
					MaxToHit = part.MaxToHit
				});
			return result;
		}

		/// <summary>
		/// Создание данных для списка шаблонов частей тела для метода внесения изменений в список частей шаблона тела
		/// </summary>
		/// <param name="bodyTemplateParts">Имеющиеся части шаблона тела</param>
		/// <param name="request">Запрос на изменение части шаблона тела, затрагивающий несколько частей шаблона тела</param>
		/// <returns>Данные для списка шаблонов частей тела</returns>
		public static List<BodyTemplatePartsData> CreateBodyTemplatePartsData(List<BodyTemplatePart> bodyTemplateParts, ChangeBodyTemplatePartCommand request)
		{
			throw new NotImplementedException();
		}
	}
}
