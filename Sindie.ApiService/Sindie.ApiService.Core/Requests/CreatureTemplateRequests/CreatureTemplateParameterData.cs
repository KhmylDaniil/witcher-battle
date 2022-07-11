using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.ChangeCreatureTemplate;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.CreateCreatureTemplate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sindie.ApiService.Core.Requests.CreatureTemplateRequests
{
	/// <summary>
	/// Данные для обновления параетров шаблона существа
	/// </summary>
	public class CreatureTemplateParameterData
	{
		/// <summary>
		/// Айди
		/// </summary>
		internal Guid? Id { get; set; }

		/// <summary>
		/// Параметр
		/// </summary>
		internal Parameter Parameter { get; set; }

		/// <summary>
		/// Значение параметра
		/// </summary>
		internal int Value { get; set; }

		/// <summary>
		/// Создание данных для <see cref="CreatureTemplateParameter"/>
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		/// <returns>Данные</returns>
		internal static List<CreatureTemplateParameterData> CreateCreatureTemplateParameterData(ChangeCreatureTemplateCommand request, Game game)
		{
			if (request?.CreatureTemplateParameters == null)
				throw new ExceptionEntityNotIncluded<CreatureTemplateParameter>(nameof(Game));
			if (game?.Parameters == null)
				throw new ExceptionEntityNotIncluded<Parameter>(nameof(Game));

			var result = new List<CreatureTemplateParameterData>();

			foreach (var item in request.CreatureTemplateParameters)
				result.Add(new CreatureTemplateParameterData()
				{
					Id = item.Id,
					Parameter = game.Parameters.FirstOrDefault(x => x.Id == item.ParameterId),
					Value = item.Value
				});
			return result;
		}

		/// <summary>
		/// Создание данных для <see cref="CreatureTemplateParameter"/>
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		/// <returns>Данные</returns>
		internal static List<CreatureTemplateParameterData> CreateCreatureTemplateParameterData(CreateCreatureTemplateCommand request, Game game)
		{
			if (request?.CreatureTemplateParameters == null)
				throw new ExceptionEntityNotIncluded<CreatureTemplateParameter>(nameof(Game));
			if (game?.Parameters == null)
				throw new ExceptionEntityNotIncluded<Parameter>(nameof(Game));

			var result = new List<CreatureTemplateParameterData>();

			foreach (var item in request.CreatureTemplateParameters)
				result.Add(new CreatureTemplateParameterData()
				{
					Id = null,
					Parameter = game.Parameters.FirstOrDefault(x => x.Id == item.ParameterId),
					Value = item.Value
				});
			return result;
		}
	}
}
