using Sindie.ApiService.Core.Contracts.SkillRequests;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;

namespace Sindie.ApiService.Core.Requests.SkillRequests.ChangeSkill
{
	/// <summary>
	/// Команда изменения навыка
	/// </summary>
	public class ChangeSkillCommand: ChangeSkillRequest
	{

		/// <summary>
		/// Конструктор команды изменения навыка
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		/// <param name="id">Айди навыка</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="statName">Название корреспондиирующей характеристики</param>
		/// <param name="minValueSkill">Минимальное значение</param>
		/// <param name="maxValueSkill">Максимальное значение</param>
		public ChangeSkillCommand(
			Guid gameId,
			Guid id,
			string name,
			string description,
			string statName,
			int minValueSkill,
			int maxValueSkill)
		{
			GameId = gameId;
			Id = id;
			Name = string.IsNullOrWhiteSpace(name)
				? throw new ExceptionRequestFieldNull<ChangeSkillRequest>(nameof(Name))
				: name;
			Description = description;
			StatName = BaseData.Stats.StatsList.Contains(statName)
				? statName
				: throw new ExceptionRequestFieldIncorrectData<ChangeSkillRequest>(nameof(StatName));
			MinValueSkill = minValueSkill;
			MaxValueSkill = maxValueSkill < minValueSkill
				? throw new ExceptionRequestFieldIncorrectData<ChangeSkillRequest>(nameof(MaxValueSkill))
				: maxValueSkill;
		}
	}
}
