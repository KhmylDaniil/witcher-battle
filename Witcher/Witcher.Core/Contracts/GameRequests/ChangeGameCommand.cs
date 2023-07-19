using Witcher.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using MediatR;

namespace Witcher.Core.Contracts.GameRequests
{
	public sealed class ChangeGameCommand : IRequest
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Айди аватара игры
		/// </summary>
		public Guid? AvatarId { get; set; }

		/// <summary>
		/// Название игры
		/// </summary>
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
