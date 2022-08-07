using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Abstractions
{
	/// <summary>
	/// Критический эффект в крыло
	/// </summary>
	public interface IWingCrit : ICrit
	{
		/// <summary>
		/// Пенальти применено
		/// </summary>
		public bool PenaltyApplied { get; }
	}
}
