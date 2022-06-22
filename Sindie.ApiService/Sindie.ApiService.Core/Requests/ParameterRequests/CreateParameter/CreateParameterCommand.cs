using Sindie.ApiService.Core.Contracts.ParameterRequests;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;

namespace Sindie.ApiService.Core.Requests.ParameterRequests.CreateParameter
{
	/// <summary>
	/// Команда создания параметра
	/// </summary>
	public class CreateParameterCommand : CreateParameterRequest
	{
		/// <summary>
		/// Конструктор команды создания параметра
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="statName">Название корреспондиирующей характеристики</param>
		/// <param name="minValueParameter">Минимальное значение</param>
		/// <param name="maxValueParameter">Максимальное значение</param>
		public CreateParameterCommand(
			Guid gameId,
			string name,
			string description,
			string statName,
			int minValueParameter,
			int maxValueParameter)
		{
			GameId = gameId;
			Name = string.IsNullOrEmpty(name)
				? throw new ExceptionRequestFieldNull<CreateParameterRequest>(nameof(Name))
				: name;
			Description = description;
			StatName = BaseData.Stats.StatsList.Contains(statName)
				? statName 
				: throw new ExceptionRequestFieldIncorrectData<CreateParameterRequest>(nameof(StatName));
			MinValueParameter = minValueParameter;
			MaxValueParameter = maxValueParameter < minValueParameter
				? throw new ExceptionRequestFieldIncorrectData<CreateParameterRequest>(nameof(MaxValueParameter))
				: maxValueParameter;
		}
	}
}
