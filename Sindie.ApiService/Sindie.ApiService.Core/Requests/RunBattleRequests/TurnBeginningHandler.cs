using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.RunBattleRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Logic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.RunBattleRequests
{
	/// <summary>
	/// Обработчик начала хода существа
	/// </summary>
	public class TurnBeginningHandler : BaseHandler<TurnBeginningCommand, TurnBeginningResponse>
	{
		/// <summary>
		/// Бросок параметра
		/// </summary>
		private readonly IRollService _rollService;

		/// <summary>
		/// Конструктор обработчика начала хода существа
		/// </summary>
		/// <param name="appDbContext"></param>
		/// <param name="authorizationService"></param>
		public TurnBeginningHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IRollService rollService)
			: base (appDbContext, authorizationService)
		{
			_rollService = rollService;
		}

		/// <summary>
		/// Начало хода существа
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public override async Task<TurnBeginningResponse> Handle(TurnBeginningCommand request, CancellationToken cancellationToken)
		{
			var battle = await _authorizationService.BattleMasterFilter(_appDbContext.Battles, request.BattleId)
				.Include(i => i.Creatures.Where(c => c.Id == request.CreatureId))
					.ThenInclude(c => c.CreatureSkills)
				.Include(i => i.Creatures.Where(c => c.Id == request.CreatureId))
					.ThenInclude(c => c.CreatureParts)
				.Include(i => i.Creatures.Where(c => c.Id == request.CreatureId))
					.ThenInclude(c => c.DamageTypeModifiers)
				.Include(i => i.Creatures.Where(c => c.Id == request.CreatureId))
					.ThenInclude(c => c.Effects)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Battle>();

			var creature = battle.Creatures.FirstOrDefault(x => x.Id == request.CreatureId)
				?? throw new ExceptionEntityNotFound<Creature>();

			StringBuilder message = new();

			foreach (var effect in creature.Effects)
			{
				effect.Run(creature, ref message);
				effect.AutoEnd(creature, ref message);
			}

			Attack.DisposeCorpses(battle);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return new TurnBeginningResponse() { Message = message.ToString() };
		}
	}
}
