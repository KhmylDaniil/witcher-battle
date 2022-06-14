using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.InterfaceRequests.GetInterfaces;
using Sindie.ApiService.WebApi.Versioning;
using System.Threading.Tasks;

namespace Sindie.ApiService.WebApi.Controllers
{
	/// <summary>
	/// Контроллер интерфейса
	/// </summary>
	[ApiVersion(ApiVersions.V1)]
	public class InterfaceController: ApiControllerBase
	{
		private readonly IMediator _mediator;

		/// <summary>
		/// Конструктор контроллера интерфейса
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		public InterfaceController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/*
		/// <summary>
		/// Запрос списка интерфейсов
		/// </summary>
		/// <param name="request">запрос</param>
		/// <returns></returns>
		[HttpGet]
		public async Task<GetInterfacesQueryResponse> Get([FromQuery] GetInterfacesQuery request)
		{
			return await _mediator.Send(request);
		}

		*/
	}
}
