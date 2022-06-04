using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.ParameterRequests
{
	/// <summary>
	/// Запрос на изменение параметра
	/// </summary>
	public class ChangeParameterRequest: IRequest
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Название параметра
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание параметра
		/// </summary>
		public string Description { get; set; }

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
