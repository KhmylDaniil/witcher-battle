using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.ParameterRequests;
using Sindie.ApiService.WebApi.Versioning;
using System.Threading.Tasks;

namespace Sindie.ApiService.WebApi.Controllers
{
	/// <summary>
	/// Контроллер параметров
	/// </summary>
	[ApiVersion(ApiVersions.V1)]
	public class ParameterController: ApiControllerBase
	{
		private readonly IMediator _mediator;

		/// <summary>
		/// Конструктор контроллера параметров
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		public ParameterController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Создать параметр
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Ответ</returns>
		[Authorize]
		[HttpPost]
		public async Task<CreateParameterResponse> Post([FromBody] CreateParameterCommand request)
		{
			return await _mediator.Send(request);
		}
	}
}