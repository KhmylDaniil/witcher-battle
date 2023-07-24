using System;
using System.Linq;

namespace Witcher.Core.ExtensionMethods
{
	public static class StringHelper
	{
		/// <summary>
		/// Метод для поиска возможных совпадений перечисления для строки поиска
		/// </summary>
		/// <typeparam name="TEnum">Тип перечисления</typeparam>
		/// <param name="source">строка поиска</param>
		/// <returns>массив подходящих значений перечисления</returns>
		public static int[] GetPossibleEnumNumbersFromSearchString<TEnum>(this string source) where TEnum : struct, Enum
		{
			if (source == null)
				return Array.Empty<int>();

			var values = Enum.GetValues<TEnum>().Where(x => Enum.GetName(x).Contains(source, StringComparison.OrdinalIgnoreCase)).Cast<int>().ToArray();

			return values;
		}
	}
}
