using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Witcher.Core.Abstractions
{
	/// <summary>
	/// Критический эффект с общим пенальти
	/// </summary>
	public interface ISharedPenaltyCrit : ICrit
	{
		/// <summary>
		/// Пенальти применено
		/// </summary>
		public bool PenaltyApplied { get; }
	}
}
