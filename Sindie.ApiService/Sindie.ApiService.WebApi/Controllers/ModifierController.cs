using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.ModifierRequests.ChangeModifier;
using Sindie.ApiService.Core.Contracts.ModifierRequests.CreateModifier;
using Sindie.ApiService.Core.Contracts.ModifierRequests.DeleteModifier;
using Sindie.ApiService.Core.Contracts.ModifierRequests.GetModifier;
using Sindie.ApiService.WebApi.Versioning;
using System;
using System.Threading.Tasks;

namespace Sindie.ApiService.WebApi.Controllers
{
	/// <summary>
	/// Контроллер модификатора
	/// </summary>
	[ApiVersion(ApiVersions.V1)]
	public class ModifierController: ApiControllerBase
	{
		/// <summary>
		/// Медиатор
		/// </summary>
		private readonly IMediator _mediator;

		/// <summary>
		/// Конструктор контроллера модификатора
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		public ModifierController(IMediator mediator)
		{
				_mediator = mediator;
		}
		

		/*
		/// <summary>
		/// Создание модификтора
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Юнит</returns>
		[Authorize]
		[HttpPost]
		public async Task<Unit> CreateModifier([FromBody] CreateModifierCommand request)
		{
			return await _mediator.Send(request);
		}

		/// <summary>
		/// Изменение модификатора
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Юнит</returns>
		[Authorize]
		[HttpPut]
		public async Task<Unit> ChangeModifier([FromBody] ChangeModifierCommand request)
		{
			return await _mediator.Send(request);
		}

		/// <summary>
		/// Получение списка модификаторов
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Ответ на запрос получения списка модификаторов</returns>
		[Authorize]
		[HttpGet]
		public async Task<GetModifierQueryResponse> GetModifier([FromQuery] GetModifierQuery request)
		{
			return await _mediator.Send(request);
		}

		/// <summary>
		/// Удаление модификатора
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Юнит</returns>
		[Authorize]
		[HttpDelete]
		public async Task<Unit> DeleteModifier([FromQuery] DeleteModifierCommand request)
		{
			return await _mediator.Send(request);
		}

		*/
	}
}
