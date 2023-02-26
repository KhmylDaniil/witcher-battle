using System.Collections.Generic;
using System;

namespace Sindie.ApiService.Core.Contracts.GameRequests
{
	public class GetGameResponseItem
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

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
		public Dictionary<Guid, string> Users { get; set; }

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