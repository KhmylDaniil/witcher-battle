using Sindie.ApiService.Core.Abstractions;
using System;

namespace Sindie.ApiService.Core.Services.Authorization
{
	/// <summary>
	/// Интерфейс айди игры для авторизации
	/// </summary>
	public class GameIdService : IGameIdService
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; private set; }

		/// <summary>
		/// Установить айди
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		public void Set(Guid gameId)
		{
			GameId = gameId;
		}

		/// <summary>
		/// Сбросить айди
		/// </summary>
		public void Reset()
		{
			GameId = Guid.Empty;
		}
	}
}
