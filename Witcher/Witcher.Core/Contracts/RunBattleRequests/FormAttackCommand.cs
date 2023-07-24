using System;
using static Witcher.Core.BaseData.Enums;
using MediatR;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	public sealed class FormAttackCommand : IRequest<FormAttackResponse>
	{
		public Guid AttackerId { get; set; }

		public Guid AttackFormulaId { get; set; }

		public Guid TargetId { get; set; }

		public AttackType AttackType { get; set; }
	}
}
