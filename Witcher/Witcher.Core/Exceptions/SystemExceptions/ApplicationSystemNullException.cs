using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Witcher.Core.Exceptions.SystemExceptions
{
	internal class ApplicationSystemNullException: ApplicationSystemBaseException
	{
		public ApplicationSystemNullException(string classType, string argument)
			: base($"При работе класса {classType} отсутствует необходимый параметр {argument}.")
		{
		}
	}
}
