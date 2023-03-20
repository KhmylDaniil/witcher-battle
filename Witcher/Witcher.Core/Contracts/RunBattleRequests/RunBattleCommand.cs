using Witcher.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	/// <summary>
	/// Команда запуска битвы
	/// </summary>
	public class RunBattleCommand : IValidatableCommand<RunBattleResponse>
	{
		/// <summary>
		/// Айди битвы
		/// </summary>
		public Guid BattleId { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
