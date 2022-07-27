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
		/// <param name="skills">Навыки</param>
		/// <returns>Данные</returns>
		internal static List<CreatureTemplateSkillData> CreateCreatureTemplateSkillData(ChangeCreatureTemplateCommand request, List<Skill> skills)
		{
			if (request?.CreatureTemplateSkills == null)
				throw new ExceptionEntityNotIncluded<CreatureTemplateSkill>(nameof(Game));
			
			var result = new List<CreatureTemplateSkillData>();

			foreach (var item in request.CreatureTemplateSkills)
				result.Add(new CreatureTemplateSkillData()
				{
					Id = item.Id,
					Skill = skills.FirstOrDefault(x => x.Id == item.SkillId),
					Value = item.Value
				});
			return result;
		}

		/// <summary>
		/// Создание данных для <see cref="CreatureTemplateSkill"/>
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="skills">Навыки</param>
		/// <returns>Данные</returns>
		internal static List<CreatureTemplateSkillData> CreateCreatureTemplateSkillData(CreateCreatureTemplateCommand request, List<Skill> skills)
		{
			if (request?.CreatureTemplateSkills == null)
				throw new ExceptionEntityNotIncluded<CreatureTemplateSkill>(nameof(Game));
			if (skills == null)
				throw new ExceptionEntityNotIncluded<Skill>(nameof(Game));

			var result = new List<CreatureTemplateSkillData>();

			foreach (var item in request.CreatureTemplateSkills)
				result.Add(new CreatureTemplateSkillData()
				{
					Id = null,
					Skill = skills.FirstOrDefault(x => x.Id == item.SkillId),
					Value = item.Value
				});
			return result;
		}
	}
}
