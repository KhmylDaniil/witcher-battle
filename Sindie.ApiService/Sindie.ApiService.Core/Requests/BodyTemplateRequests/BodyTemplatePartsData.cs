using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BodyTemplateRequests.ChangeBodyTemplate;
using Sindie.ApiService.Core.Requests.BodyTemplateRequests.CreateBodyTemplate;
using System;
using System.Collections.Generic;
using System.Linq;

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
		/// <param name="bodyPartTypes">Типы частей тела</param>
		/// <returns>Данные для списка шаблонов частей тела</returns>
		public static List<BodyTemplatePartsData>
			CreateBodyTemplatePartsData(CreateBodyTemplateCommand request, List<BodyPartType> bodyPartTypes)
		{
			var result = new List<BodyTemplatePartsData>();

			foreach (var part in request.BodyTemplateParts)
				result.Add(new BodyTemplatePartsData()
				{
					Name = part.Name,
					BodyPartType = bodyPartTypes.First(x => x.Id == part.BodyPartTypeId),
					DamageModifier = part.DamageModifier,
					HitPenalty = part.HitPenalty,
					MinToHit = part.MinToHit,
					MaxToHit = part.MaxToHit
				});;
			return result;
		}

		/// <summary>
		/// Создание данных для списка шаблонов частей тела
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="bodyPartTypes">Типы частей тела</param>
		/// <returns>Данные для списка шаблонов частей тела</returns>
		public static List<BodyTemplatePartsData>
			CreateBodyTemplatePartsData(ChangeBodyTemplateCommand request, List<BodyPartType> bodyPartTypes)
		{
			var result = new List<BodyTemplatePartsData>();

			foreach (var part in request.BodyTemplateParts)
				result.Add(new BodyTemplatePartsData()
				{
					Name = part.Name,
					BodyPartType = bodyPartTypes.First(x => x.Id == part.BodyPartTypeId),
					DamageModifier = part.DamageModifier,
					HitPenalty = part.HitPenalty,
					MinToHit = part.MinToHit,
					MaxToHit = part.MaxToHit
				});
			return result;
		}
	}
}
