using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Contracts.BattleRequests.TurnBeginning
{
	/// <summary>
	/// Отчет о начале хода существа
	/// </summary>
	public sealed class TurnBeginningResponse
	{
		/// <summary>
		/// Сообщение
		/// </summary>
		public string Message { get; set; }
	}
}
