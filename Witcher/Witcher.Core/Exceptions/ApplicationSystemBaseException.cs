using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Witcher.Core.Exceptions
{
	/// <summary>
	/// Предвиденная ошибка вне бизнес-логики приложения 
	/// </summary>
	public class ApplicationSystemBaseException : Exception
	{
		public ApplicationSystemBaseException(string message) : base(message)
		{
		}
	}
}
