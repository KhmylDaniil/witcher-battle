using System;

namespace Witcher.Core.Abstractions
{
	/// <summary>
	/// Интерфейс айди игры для авторизации
	/// </summary>
	public interface IGameIdService
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; }

		/// <summary>
		/// Установить айди
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		public void Set(Guid gameId);

		/// <summary>
		/// Сбросить айди
		/// </summary>
		public void Reset();
	}
}
