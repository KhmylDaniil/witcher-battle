using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sindie.ApiService.Core.Contracts.GameRequests.ChangeGame
{
	public sealed class ChangeGameCommand : IRequest<Unit>
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		[Required]
		public Guid Id { get; set; }

		/// <summary>
		/// Айди аватара игры
		/// </summary>
		public Guid? AvatarId { get; set; }

		/// <summary>
		/// Название игры
		/// </summary>
		[Required]
		public string Name { get; set; }

		/// <summary>
		/// Описание игры
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Текстовые файлы игры
		/// </summary>
		public List<Guid> TextFiles { get; set; }

		/// <summary>
		/// Графические файлы игры
		/// </summary>
		public List<Guid> ImgFiles { get; set; }
	}
}
