using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.SlotRequests;
using Sindie.ApiService.WebApi.Versioning;
using System.Threading.Tasks;

namespace Sindie.ApiService.WebApi.Controllers
{
	/// <summary>
	/// Контроллер слота
	/// </summary>
	[ApiVersion(ApiVersions.V1)]
	public class SlotController: ApiControllerBase
	{
		private readonly IMediator _mediator;

		/// <summary>
		/// Конструктор контроллера слота
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		public SlotController(IMediator mediator)
		{
			_mediator = mediator;
		}
		/*
		/// <summary>
		/// Создать слот
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Ответ</returns>
		[Authorize]
		[HttpPost]
		public async Task<CreateSlotResponse> Post([FromBody] CreateSlotCommand request)
		{
			return await _mediator.Send(request);
		}
		*/
	}
}
