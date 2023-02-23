using MediatR;
using Sindie.ApiService.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests.DeleteAppliedCondition
{
	/// <summary>
	/// Команда удаления накладываемого состояния
	/// </summary>
	public class DeleteAppliedCondionCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid AbilityId { get; set; }

		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
