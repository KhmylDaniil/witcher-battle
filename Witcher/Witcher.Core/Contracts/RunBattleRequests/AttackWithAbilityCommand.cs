using Witcher.Core.Abstractions;
using System;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	// <summary>
	/// Команда атаки существа способностью
	/// </summary>
	public class AttackWithAbilityCommand : AttackBaseCommand, IValidatableCommand
	{
		/// <summary>
		/// Айди способности атаки
		/// </summary>
		public Guid? AbilityId { get; set; }
	}
}
