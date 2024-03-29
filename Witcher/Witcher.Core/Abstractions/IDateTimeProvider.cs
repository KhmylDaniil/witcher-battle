﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Witcher.Core.Abstractions
{
	/// <summary>
	/// Провайдер дат
	/// </summary>
	public interface IDateTimeProvider
	{
		/// <summary>
		/// Провайдер дат
		/// </summary>
		public DateTime TimeProvider { get; }
	}
}
