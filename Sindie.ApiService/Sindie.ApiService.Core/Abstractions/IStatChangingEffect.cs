using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Abstractions
{
	/// <summary>
	/// Интерфейс изменяющего характеристики эффекта 
	/// </summary>
	public interface IStatChangingEffect
	{
		/// <summary>
		/// Изменение интеллекта
		/// </summary>
		int Int { get; }

		/// <summary>
		/// Изменение рефлексов
		/// </summary>
		int Ref { get; }

		/// <summary>
		/// Изменение ловкости
		/// </summary>
		int Dex { get; }

		/// <summary>
		/// Изменение телосложения
		/// </summary>
		int Body { get; }

		/// <summary>
		/// Изменение эмпатии
		/// </summary>
		int Emp { get; }

		/// <summary>
		/// Изменение ремесла
		/// </summary>
		int Cra { get; }

		/// <summary>
		/// Изменение воли
		/// </summary>
		int Will { get; }

		/// <summary>
		/// Изменение скорости
		/// </summary>
		int Speed { get; }
	}
}
