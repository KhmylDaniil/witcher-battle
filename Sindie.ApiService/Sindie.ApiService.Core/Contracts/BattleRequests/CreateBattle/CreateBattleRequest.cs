using MediatR;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.BattleRequests.CreateBattle
{
	/// <summary>
	/// Запрос создания боя
	/// </summary>
	public class CreateBattleRequest: IRequest
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Айди графического файла
		/// </summary>
		public Guid? ImgFileId { get; set; }

		/// <summary>
		/// Название боя
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание боя
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Список существ
		/// </summary>
		public List<CreateBattleRequestItem> Creatures { get; set; } 
	}
}
