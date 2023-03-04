using Sindie.ApiService.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Contracts.RunBattleRequests
{
	/// <summary>
	/// Команда запуска битвы
	/// </summary>
	public class RunBattleCommand : IValidatableCommand<RunBattleResponse>
	{
		/// <summary>
		/// Айди битвы
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
