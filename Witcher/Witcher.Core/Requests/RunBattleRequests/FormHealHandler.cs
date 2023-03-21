using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.RunBattleRequests
{
	public class FormHealHandler : BaseHandler<FormHealCommand, FormHealResponse>
	{
		public FormHealHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<FormHealResponse> Handle(FormHealCommand request, CancellationToken cancellationToken)
		{
			var targetCreature = await _appDbContext.Creatures
				.Include(c => c.Effects)
				.FirstOrDefaultAsync(x => x.Id == request.TargetCreatureId, cancellationToken)
					?? throw new EntityNotFoundException<Creature>(request.TargetCreatureId);

			return new FormHealResponse
			{
				EffectsOnTarget = targetCreature.Effects.ToDictionary(x => x.Id, x => x.Name)
			};
		}
	}
}
