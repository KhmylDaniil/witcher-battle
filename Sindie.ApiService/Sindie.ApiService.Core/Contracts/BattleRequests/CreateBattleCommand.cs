using Witcher.Core.Abstractions;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.RequestExceptions;
using System;

namespace Witcher.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Команда создания боя
	/// </summary>
	public class CreateBattleCommand : IValidatableCommand<Battle>
	{
		/// <summary>
		/// Айди графического файла
		/// </summary>
		public Guid? ImgFileId { get; set; }

		/// <summary>
		/// Название боя
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание боя
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			if (string.IsNullOrEmpty(Name))
				throw new RequestFieldNullException<CreateBattleCommand>(nameof(Name));
		}
	}
}
