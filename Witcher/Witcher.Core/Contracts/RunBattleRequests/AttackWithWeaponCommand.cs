using System;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	public class AttackWithWeaponCommand : AttackBaseCommand, IValidatableCommand
	{
		public Guid WeaponId { get; set; }

		public bool? IsStrongAttack { get; set; }
	}
}
