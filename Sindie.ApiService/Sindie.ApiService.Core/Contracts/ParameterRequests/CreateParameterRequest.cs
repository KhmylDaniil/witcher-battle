using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.ParameterRequests
{
	/// <summary>
	/// Запрос на создания параметра
	/// </summary>
	public class CreateParameterRequest : IRequest
	{
		/// <summary>
		/// АйДи игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Название параметра
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание параметра
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Название корреспондиирующей характеристики
		/// </summary>
		public string StatName { get; set; }

		/// <summary>
		/// Минимальное значение параметра
		/// </summary>
		public int MinValueParameter { get; set; }

		/// <summary>
		/// Максимальное значение параметра
		/// </summary>
		public int MaxValueParameter { get; set; }
	}
}