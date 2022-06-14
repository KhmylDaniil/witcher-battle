using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.CharacterTemplateRequests.CreateCharacterTemplate;
using Sindie.ApiService.Core.Contracts.CharacterTemplateRequests.ChangeCharacterTemplate;
using Sindie.ApiService.Core.Contracts.CharacterTemplateRequests.DeleteCharacterTemplate;
using Sindie.ApiService.Core.Contracts.CharacterTemplateRequests.GetCharacterTemplate;
using Sindie.ApiService.WebApi.Versioning;
using System.Threading.Tasks;

namespace Sindie.ApiService.WebApi.Controllers
{
	/// <summary>
	/// Контроллер шаблона персонажа
	/// </summary>
	[ApiVersion(ApiVersions.V1)]
	public class CharacterTemplateController: ApiControllerBase
	{
		/// <summary>
		/// Медиатор
		/// </summary>
		private readonly IMediator _mediator;

		/// <summary>
		/// Конструктор контроллера шаблона персонажа
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		public CharacterTemplateController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/*

		/// <summary>
		/// Создание шаблона персонажа
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Юнит</returns>
		[Authorize]
		[HttpPost]
		public async Task<Unit> CreateCharacterTemplate([FromBody] CreateCharacterTemplateCommand request)
		{
			return await _mediator.Send(request);
		}

		/// <summary>
		/// Изменение шаблона персонажа
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Юнит</returns>
		[Authorize]
		[HttpPut]
		public async Task<Unit> ChangeCharacterTemplate([FromBody] ChangeCharacterTemplateCommand request)
		{
			return await _mediator.Send(request);
		}

		/// <summary>
		/// Получение списка шаблонов персонажа
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Ответ на запрос получения списка шаблонов персонажа</returns>
		[Authorize]
		[HttpGet]
		public async Task<GetCharacterTemplateQueryResponse> GetCharacterTemplate([FromQuery] GetCharacterTemplateQuery request)
		{
			return await _mediator.Send(request);
		}

		/// <summary>
		/// Удаление шаблона персонажа
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Юнит</returns>
		[Authorize]
		[HttpDelete]
		public async Task<Unit> DeleteCharacterTemplate([FromQuery] DeleteCharacterTemplateCommand request)
		{
			return await _mediator.Send(request);
		}

		*/
	}
}
