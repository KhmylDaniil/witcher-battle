﻿using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;

namespace Witcher.Core.Contracts.GameRequests
{
	public sealed class ChangeGameCommand : IValidatableCommand
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

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			if (string.IsNullOrEmpty(Name))
				throw new RequestFieldNullException<ChangeGameCommand>(nameof(Name));
		}
	}
}
