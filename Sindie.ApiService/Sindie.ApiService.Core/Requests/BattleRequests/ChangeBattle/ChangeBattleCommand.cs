using Sindie.ApiService.Core.Contracts.BattleRequests.ChangeBattle;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Requests.BattleRequests.ChangeBattle
{
	/// <summary>
	/// Команда изменения боя
	/// </summary>
	public class ChangeBattleCommand: ChangeBattleRequest
	{
		/// <summary>
		/// Конструктор команды изменения боя
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		/// <param name="id">Айди</param>
		/// <param name="imgFileId">Айди графического файла</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="creatures">Существа</param>
		public ChangeBattleCommand(
			Guid gameId,
			Guid id,
			Guid? imgFileId,
			string name,
			string description,
			List<ChangeBattleRequestItem> creatures)
		{
			GameId = gameId;
			Id = id;
			ImgFileId = imgFileId;
			Name = string.IsNullOrWhiteSpace(name)
				? throw new RequestFieldNullException<ChangeBattleRequest>(nameof(Name))
				: name;
			Description = description;
			Creatures = creatures;
		}
	}
}
