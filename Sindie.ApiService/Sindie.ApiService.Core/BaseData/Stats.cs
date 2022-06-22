using Sindie.ApiService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.BaseData
{
	/// <summary>
	/// Список возможных корреспондирующих характеристик для параметров
	/// </summary>
	public static class Stats
	{
		public static readonly List<string> StatsList = new List<string>
		{
			"Int", "Ref", "Dex", "Body", "Emp", "Cra", "Will"
		};
	}
}
