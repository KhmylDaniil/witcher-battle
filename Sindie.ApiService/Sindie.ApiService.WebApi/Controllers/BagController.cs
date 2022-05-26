using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.BagRequests.ChangeBag;
using Sindie.ApiService.Core.Contracts.BagRequests.GetBag;
using Sindie.ApiService.Core.Contracts.BagRequests.GiveItems;
using Sindie.ApiService.Core.Contracts.BagRequests.TakeItems;
using Sindie.ApiService.Core.Requests.BagRequests.ChangeBag;
using Sindie.ApiService.Core.Requests.BagRequests.GetBag;
using Sindie.ApiService.Core.Requests.BagRequests.GiveItems;
using Sindie.ApiService.Core.Requests.BagRequests.TakeItems;
using Sindie.ApiService.WebApi.Versioning;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.WebApi.Controllers
{
	/// <summary>
	/// Контроллер сумки
	/// </summary>
	[ApiVersion(ApiVersions.V1)]
	[Authorize]
	public class BagController: ApiControllerBase
	{
		/// <summary>
		/// Медиатор
		/// </summary>
		private readonly IMediator _mediator;

		/// <summary>
		/// Конструктор для контроллера сумки
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		public BagController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Обработчик изменения сумки
		/// </summary>
		/// <param name="request">Запрос на изменение сумки</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPut("ChangeBag")]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task ChangeBagAsync([FromBody] ChangeBagRequest request, CancellationToken cancellationToken)
		{
			await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new ChangeBagCommand(
					gameId: request.GameId,
					instanceId: request.InstanceId,
					id: request.Id,
					bagItems: request.BagItems), cancellationToken);
		}

		/// <summary>
		/// Обработчик получения предметов
		/// </summary>
		/// <param name="request">Запрос на получение предметов</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPut("TakeItems")]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task TakeItemsAsync([FromBody] TakeItemsRequest request, CancellationToken cancellationToken)
		{
			await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new TakeItemsCommand(
					gameId: request.GameId,
					instanceId: request.InstanceId,
					sourceBagId: request.SourceBagId,
					receiveBagId: request.ReceiveBagId,
					bagItems: request.BagItems), cancellationToken);
		}

		/// <summary>
		/// Обработчик получения сумки и списка предметов в сумке
		/// </summary>
		/// <param name="request">Запрос на получение предметов</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpGet("GetBag")]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<GetBagResponse> GetBagAsync([FromQuery] GetBagQuery request, CancellationToken cancellationToken)
		{
			return await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new GetBagCommand(
					gameId: request.GameId,
					instanceId: request.InstanceId,
					bagId: request.BagId,
					itemName: request.ItemName,
					itemTemplateName: request.ItemTemplateName,
					slotName: request.SlotName,
					pageSize: request.PageSize,
					pageNumber: request.PageNumber,
					orderBy: request.OrderBy,
					isAscending: request.IsAscending), cancellationToken);
		}

		/// <summary>
		/// Обработчик получения сумки и списка предметов в сумке
		/// </summary>
		/// <param name="request">Запрос на получение предметов</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpGet("GetGivenItems")]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<GetGivenItemsResponse> GetGivenItemsAsync([FromQuery] GetGivenItemsQuery request, CancellationToken cancellationToken)
		{
			return await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new GetGivenItemsCommand(
					notificationId: request.NotificationId), cancellationToken);
		}

		/// <summary>
		/// Обработчик передачи предметов
		/// </summary>
		/// <param name="request">Запрос на передачу предметов</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPut("GiveItems")]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task GiveItemsAsync([FromBody] GiveItemsRequest request, CancellationToken cancellationToken)
		{
			await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new GiveItemsCommand(
					gameId: request.GameId,
					instanceId: request.InstanceId,
					sourceBagId: request.SourceBagId,
					receiveCharacterId: request.ReceiveCharacterId,
					bagItems: request.BagItems), cancellationToken);
		}

		/// <summary>
		/// Обработчик получения предметов
		/// </summary>
		/// <param name="request">Запрос на передачу предметов</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPut("ReceiveItems")]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task ReceiveItemsAsync([FromBody] ReceiveItemsRequest request, CancellationToken cancellationToken)
		{
			await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new ReceiveItemsCommand(
					notificationId: request.NotificationId,
					consent: request.Consent), cancellationToken);
		}
	}
}
