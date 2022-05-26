using Sindie.ApiService.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Services.DateTimeProvider
{
	/// <summary>
	/// Провайдер дат
	/// </summary>
	public class DateTimeProvider : IDateTimeProvider
	{
		/// <summary>
		/// Провайдер дат
		/// </summary>
		public DateTime TimeProvider { get; set; } = DateTime.UtcNow;
	}
}
