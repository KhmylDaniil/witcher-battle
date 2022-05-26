using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.ParameterRequests
{
	/// <summary>
	/// Команда создания параметра
	/// </summary>
	public class CreateParameterCommand : IRequest<CreateParameterResponse>
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
		/// Минимальное значение параметра
		/// </summary>
		public double MinValueParameter { get; set; }

		/// <summary>
		/// Максимальное значение параметра
		/// </summary>
		public double MaxValueParameter { get; set; }
	}
}