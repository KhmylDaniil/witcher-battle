using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.GameRequests
{
	public sealed class GetGameByIdResponse
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Имя мастера игры
		/// </summary>
		public string GameMasterName { get; set; }

		/// <summary>
		/// Название игры
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Айди Аватара
		/// </summary>
		public Guid? AvatarId { get; set; }

		/// <summary>
		/// Описание игры
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Пользователи игры
		/// </summary>
		public Dictionary<Guid, (string, string)> Users { get; set; }

		/// <summary>
		/// Текстовые файлы
		/// </summary>
		public List<Guid> TextFiles { get; set; }

		/// <summary>
		/// Графические файлы
		/// </summary>
		public List<Guid> ImgFiles { get; set; }
	}
}
