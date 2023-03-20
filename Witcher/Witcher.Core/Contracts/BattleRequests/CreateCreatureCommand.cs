using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions.RequestExceptions;
using System;

namespace Witcher.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Команда добавлени существа в битву
	/// </summary>
	public class CreateCreatureCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди боя
		/// </summary>
		public Guid BattleId { get; set; }

		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; set; }

		/// <summary>
		/// Название существа
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание существа
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			if (string.IsNullOrEmpty(Name))
				throw new RequestFieldNullException<CreateCreatureCommand>(nameof(Name));
		}
	}
}
