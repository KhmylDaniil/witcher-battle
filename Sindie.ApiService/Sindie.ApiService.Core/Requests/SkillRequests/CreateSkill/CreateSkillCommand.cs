using Sindie.ApiService.Core.Contracts.SkillRequests;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;

namespace Sindie.ApiService.Core.Requests.SkillRequests.CreateSkill
{
	/// <summary>
	/// Команда создания навыка
	/// </summary>
	public class CreateSkillCommand : CreateSkillRequest
	{
		/// <summary>
		/// Конструктор команды создания навыка
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="statName">Название корреспондиирующей характеристики</param>
		/// <param name="minValueSkill">Минимальное значение</param>
		/// <param name="maxValueSkill">Максимальное значение</param>
		public CreateSkillCommand(
			Guid gameId,
			string name,
			string description,
			string statName,
			int minValueSkill,
			int maxValueSkill)
		{
			GameId = gameId;
			Name = string.IsNullOrWhiteSpace(name)
				? throw new ExceptionRequestFieldNull<CreateSkillRequest>(nameof(Name))
				: name;
			Description = description;
			StatName = BaseData.Stats.StatsList.Contains(statName)
				? statName 
				: throw new ExceptionRequestFieldIncorrectData<CreateSkillRequest>(nameof(StatName));
			MinValueSkill = minValueSkill;
			MaxValueSkill = maxValueSkill < minValueSkill
				? throw new ExceptionRequestFieldIncorrectData<CreateSkillRequest>(nameof(MaxValueSkill))
				: maxValueSkill;
		}
	}
}
