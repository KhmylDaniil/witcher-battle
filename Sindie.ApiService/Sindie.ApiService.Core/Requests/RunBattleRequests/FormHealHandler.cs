using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.RunBattleRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.RunBattleRequests
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
					?? throw new ExceptionEntityNotFound<Creature>(request.TargetCreatureId);

			return new FormHealResponse
			{
				EffectsOnTarget = targetCreature.Effects.ToDictionary(x => x.Id, x => x.Name)
			};
		}
	}
}
