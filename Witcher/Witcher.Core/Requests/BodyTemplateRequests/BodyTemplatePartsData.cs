using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BodyTemplateRequests;
using Witcher.Core.Entities;
using System.Collections.Generic;

namespace Witcher.Core.Requests.BodyTemplateRequests
{
	/// <summary>
	/// Данные для обновления частей шаблона тела
	/// </summary>
	public class BodyTemplatePartsData : UpdateBodyTemplateRequestItem
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="data">Данные о части шаблона тела</param>
		private BodyTemplatePartsData(IBodyTemplatePartData data)
		{
			Name = data.Name;
			BodyPartType = data.BodyPartType;
			DamageModifier = data.DamageModifier;
			HitPenalty = data.HitPenalty;
			MaxToHit = data.MaxToHit;
			MinToHit = data.MinToHit;
		}

		/// <summary>
		/// Пустой конструктор для драфта
		/// </summary>
		public BodyTemplatePartsData()
		{
		}

		/// <summary>
		/// Создание данных для списка шаблонов частей тела для метода внесения изменений в список частей шаблона тела
		/// </summary>
		/// <param name="bodyTemplateParts">Имеющиеся части шаблона тела</param>
		/// <param name="request">Запрос на изменение части шаблона тела, затрагивающий несколько частей шаблона тела</param>
		/// <returns>Данные для списка шаблонов частей тела</returns>
		public static List<BodyTemplatePartsData> CreateBodyTemplatePartsData(List<BodyTemplatePart> bodyTemplateParts, ChangeBodyTemplatePartCommand request)
		{
			var result = new List<BodyTemplatePartsData>() { new BodyTemplatePartsData(request) };

			foreach(var part in bodyTemplateParts)

				if (part.MinToHit < request.MinToHit && part.MaxToHit > request.MaxToHit)
					{
						result.Add(new BodyTemplatePartsData(part) { MaxToHit = request.MinToHit - 1 });
						result.Add(new BodyTemplatePartsData(part) { MinToHit = request.MaxToHit + 1 });
					}

				else if (part.MaxToHit > request.MaxToHit && part.MinToHit <= request.MaxToHit)
					result.Add(new BodyTemplatePartsData(part) { MinToHit = request.MaxToHit + 1 });

				else if (part.MinToHit < request.MinToHit && part.MaxToHit >= request.MinToHit)

					result.Add(new BodyTemplatePartsData(part) { MaxToHit = request.MinToHit - 1 });

				else if (part.MinToHit > request.MaxToHit || part.MaxToHit < request.MinToHit)
					result.Add(new BodyTemplatePartsData(part));

			return result;
		}
	}
}
