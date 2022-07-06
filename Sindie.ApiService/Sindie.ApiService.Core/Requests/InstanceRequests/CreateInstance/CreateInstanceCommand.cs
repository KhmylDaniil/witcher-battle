using Sindie.ApiService.Core.Contracts.InstanceRequests.CreateInstance;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Requests.InstanceRequests.CreateInstance
{
	/// <summary>
	/// Команда создания инстанса
	/// </summary>
	public class CreateInstanceCommand: CreateInstanceRequest
	{
		public CreateInstanceCommand(
			Guid gameId,
			Guid? imgFileId,
			string name,
			string description,
			List<CreateInstanceRequestItem> creatures)
		{
			GameId = gameId;
			ImgFileId = imgFileId;
			Name = string.IsNullOrWhiteSpace(name)
				? throw new ExceptionRequestFieldNull<CreateInstanceRequest>(nameof(Name))
				: name;
			Description = description;
			Creatures = creatures;
		}
	}
}
