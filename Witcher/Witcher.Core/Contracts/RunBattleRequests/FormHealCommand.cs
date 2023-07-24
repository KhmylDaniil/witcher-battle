using System;
using MediatR;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	public sealed class FormHealCommand : IRequest<FormHealResponse>
	{
		public Guid TargetCreatureId { get; set; }
	}
}
