﻿using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;

namespace Witcher.Core.Contracts.GameRequests
{
	/// <summary>
	/// Команда создания игры
	/// </summary>
	public sealed class CreateGameCommand : IValidatableCommand
	{
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
				throw new RequestFieldNullException<CreateGameCommand>(nameof(Name));

			if (Name.Length > 20)
				throw new RequestFieldIncorrectDataException<JoinGameRequest>(nameof(Name), "Длина названия превышает 20 символов.");
		}
	}
}