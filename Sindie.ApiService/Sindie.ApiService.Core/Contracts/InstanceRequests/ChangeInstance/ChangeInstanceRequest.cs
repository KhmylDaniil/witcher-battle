﻿using MediatR;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.InstanceRequests.ChangeInstance
{
	/// <summary>
	/// Запрос изменения инстанса
	/// </summary>
	public class ChangeInstanceRequest : IRequest
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Айди инстанса
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Айди графического файла
		/// </summary>
		public Guid? ImgFileId { get; set; }

		/// <summary>
		/// Название инстанса
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание инстанса
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Список существ
		/// </summary>
		public List<ChangeInstanceRequestItem> Creatures { get; set; }
	}
}