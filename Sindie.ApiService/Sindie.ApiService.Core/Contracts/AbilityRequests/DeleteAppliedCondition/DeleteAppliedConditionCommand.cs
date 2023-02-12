using MediatR;
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
	public class DeleteAppliedCondionCommand :  IRequest
	{
		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid AbilityId { get; set; }

		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }
	}
}
