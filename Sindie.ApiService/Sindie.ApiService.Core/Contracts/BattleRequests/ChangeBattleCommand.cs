using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;

namespace Sindie.ApiService.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Запрос изменения боя
	/// </summary>
	public sealed class ChangeBattleCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди боя
		/// </summary>
		public Guid Id { get; set; }

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
				throw new RequestFieldNullException<ChangeBattleCommand>(nameof(Name));
		}
	}
}
