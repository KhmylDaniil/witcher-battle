using Sindie.ApiService.Core.Abstractions;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Состояние
	/// </summary>
	public class Condition : EntityBase
	{
		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		#region navigation properties

		/// <summary>
		/// Применяемые условия
		/// </summary>
		public List<AppliedCondition> AppliedConditions { get; set; }

		/// <summary>
		/// Существа
		/// </summary>
		public List<Creature> Creatures { get; set; }

		#endregion navigation properties


	}
}
