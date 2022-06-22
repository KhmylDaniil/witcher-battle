using Sindie.ApiService.Core.Contracts.ParameterRequests;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;

namespace Sindie.ApiService.Core.Requests.ParameterRequests.ChangeParameter
{
	/// <summary>
	/// Команда изменения параметра
	/// </summary>
	public class ChangeParameterCommand: ChangeParameterRequest
	{

		/// <summary>
		/// Конструктор команды изменения параметра
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		/// <param name="id">Айди параметра</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="statName">Название корреспондиирующей характеристики</param>
		/// <param name="minValueParameter">Минимальное значение</param>
		/// <param name="maxValueParameter">Максимальное значение</param>
		public ChangeParameterCommand(
			Guid gameId,
			Guid id,
			string name,
			string description,
			string statName,
			int minValueParameter,
			int maxValueParameter)
		{
			GameId = gameId;
			Id = id;
			Name = string.IsNullOrEmpty(name)
				? throw new ExceptionRequestFieldNull<ChangeParameterRequest>(nameof(Name))
				: name;
			Description = description;
			StatName = BaseData.Stats.StatsList.Contains(statName)
	? statName
	: throw new ExceptionRequestFieldIncorrectData<ChangeParameterRequest>(nameof(StatName));
			MinValueParameter = minValueParameter;
			MaxValueParameter = maxValueParameter < minValueParameter
				? throw new ExceptionRequestFieldIncorrectData<ChangeParameterRequest>(nameof(MaxValueParameter))
				: maxValueParameter;
		}
	}
}
