using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Contracts.BodyTemplateRequests;
using Witcher.Core.Contracts.CharacterRequests;

namespace Witcher.MVC.Controllers.Api
{
	/// <summary>
	/// Read-only списки для заполнения выпадающих списков в формах CreatureTemplate (шаблон тела, способности) —
	/// аналог приватных GetBodyTemplateListForViewModel/GetAbilityListToViewModel в CreatureTemplateController.cs.
	/// Полный CRUD для BodyTemplate/Ability в эту итерацию не входит (см. план), только список для выбора.
	/// </summary>
	[Route("api/games/{gameId:guid}/reference")]
	public class ReferenceDataApiController : ApiControllerBase
	{
		public ReferenceDataApiController(IMediator mediator, IGameIdService gameIdService)
			: base(mediator, gameIdService)
		{
		}

		[HttpGet("body-templates")]
		public async Task<IEnumerable<GetBodyTemplateResponseItem>> BodyTemplates(Guid gameId, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			return await _mediator.Send(new GetBodyTemplateQuery { PageSize = 50 }, cancellationToken);
		}

		[HttpGet("abilities")]
		public async Task<IEnumerable<GetAbilityResponseItem>> Abilities(Guid gameId, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			return await _mediator.Send(new GetAbilityQuery { PageSize = 50 }, cancellationToken);
		}

		[HttpGet("characters")]
		public async Task<IEnumerable<GetCharactersResponseItem>> Characters(Guid gameId, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			return await _mediator.Send(new GetCharactersCommand { PageSize = 50 }, cancellationToken);
		}
	}
}
