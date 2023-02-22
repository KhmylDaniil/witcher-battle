using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Команда изменения брони для шаблона существа или его части
	/// </summary>
	public class ChangeCreatureTemplatePartCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; set; }

		/// <summary>
		/// Айди
		/// </summary>
		public Guid? Id { get; set; }

		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Значение брони
		/// </summary>
		public int ArmorValue { get; set; }
		
		public void Validate()
		{
			if (ArmorValue < 0)
				throw new RequestFieldIncorrectDataException<ChangeCreatureTemplatePartCommand>(nameof(ArmorValue),
					"Значение брони не может быть отрицательным.");
		}
	}
}
