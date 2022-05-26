using MediatR;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.BagRequests.ChangeBag
{
	/// <summary>
	/// Запрос на изменение сумки
	/// </summary>
	public class ChangeBagRequest: IRequest
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }
		
		/// <summary>
		/// Айди экземпляра игры
		/// </summary>
		public Guid InstanceId { get; set; }

		/// <summary>
		/// Айди сумки
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Подкласссы запроса изменения сумки
		/// </summary>
		public List<ChangeBagRequestItem> BagItems { get; set; }
	}
}
