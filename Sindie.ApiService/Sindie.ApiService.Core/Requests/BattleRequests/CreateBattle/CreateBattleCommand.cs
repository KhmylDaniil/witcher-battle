using Sindie.ApiService.Core.Contracts.BattleRequests.CreateBattle;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Requests.BattleRequests.CreateBattle
{
	/// <summary>
	/// Команда создания боя
	/// </summary>
	public class CreateBattleCommand: CreateBattleRequest
	{
		public CreateBattleCommand(
			Guid gameId,
			Guid? imgFileId,
			string name,
			string description,
			List<CreateBattleRequestItem> creatures)
		{
			GameId = gameId;
			ImgFileId = imgFileId;
			Name = string.IsNullOrWhiteSpace(name)
				? throw new RequestFieldNullException<CreateBattleRequest>(nameof(Name))
				: name;
			Description = description;
			Creatures = creatures;
		}
	}
}
