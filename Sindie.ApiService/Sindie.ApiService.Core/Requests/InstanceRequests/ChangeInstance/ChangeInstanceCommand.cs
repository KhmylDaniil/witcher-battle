using Sindie.ApiService.Core.Contracts.InstanceRequests.ChangeInstance;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Requests.InstanceRequests.ChangeInstance
{
	/// <summary>
	/// Команда изменения инстанса
	/// </summary>
	public class ChangeInstanceCommand: ChangeInstanceRequest
	{
		/// <summary>
		/// Конструктор команды изменения инстанса
		/// </summary>
		/// <param name="gameId">Ади игры</param>
		/// <param name="id">Айди</param>
		/// <param name="imgFileId">Айди графического файла</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="creatures">Существа</param>
		public ChangeInstanceCommand(
			Guid gameId,
			Guid id,
			Guid? imgFileId,
			string name,
			string description,
			List<ChangeInstanceRequestItem> creatures)
		{
			GameId = gameId;
			Id = id;
			ImgFileId = imgFileId;
			Name = string.IsNullOrWhiteSpace(name)
				? throw new ExceptionRequestFieldNull<ChangeInstanceRequest>(nameof(Name))
				: name;
			Description = description;
			Creatures = creatures;
		}
	}
}
