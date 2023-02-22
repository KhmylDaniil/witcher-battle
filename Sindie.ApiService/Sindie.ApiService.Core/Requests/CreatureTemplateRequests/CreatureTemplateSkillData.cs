using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.ChangeCreatureTemplate;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.CreateCreatureTemplate;
using System;
using System.Collections.Generic;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Requests.CreatureTemplateRequests
{
	/// <summary>
	/// Данные для обновления навыков шаблона существа
	/// </summary>
	public class CreatureTemplateSkillData
	{
		/// <summary>
		/// Айди
		/// </summary>
		internal Guid? Id { get; set; }

		/// <summary>
		/// Навык
		/// </summary>
		internal Skill Skill { get; set; }

		/// <summary>
		/// Значение навыка
		/// </summary>
		internal int Value { get; set; }

		/// <summary>
		/// Создание данных для <see cref="CreatureTemplateSkill"/>
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Данные</returns>
		internal static List<CreatureTemplateSkillData> CreateCreatureTemplateSkillData(ChangeCreatureTemplateCommand request)
		{
			if (request?.CreatureTemplateSkills == null)
				throw new ExceptionEntityNotIncluded<CreatureTemplateSkill>(nameof(Game));
			
			var result = new List<CreatureTemplateSkillData>();

			foreach (var item in request.CreatureTemplateSkills)
				result.Add(new CreatureTemplateSkillData()
				{
					Id = item.Id,
					Skill = item.Skill,
					Value = item.Value
				});
			return result;
		}

		/// <summary>
		/// Создание данных для <see cref="CreatureTemplateSkill"/>
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Данные</returns>
		internal static List<CreatureTemplateSkillData> CreateCreatureTemplateSkillData(CreateCreatureTemplateCommand request)
		{
			if (request?.CreatureTemplateSkills == null)
				throw new ExceptionEntityNotIncluded<CreatureTemplateSkill>(nameof(Game));

			var result = new List<CreatureTemplateSkillData>();

			foreach (var item in request.CreatureTemplateSkills)
				result.Add(new CreatureTemplateSkillData()
				{
					Id = null,
					Skill = item.Skill,
					Value = item.Value
				});
			return result;
		}
	}
}
