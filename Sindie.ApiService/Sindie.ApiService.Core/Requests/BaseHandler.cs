using Sindie.ApiService.Core.Abstractions;

namespace Sindie.ApiService.Core.Requests
{
	/// <summary>
	/// Базовый класс для обработчика
	/// </summary>
	public abstract class BaseHandler
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		protected readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Сервис авторизации
		/// </summary>
		protected readonly IAuthorizationService _authorizationService;

		/// <summary>
		/// Базовый конструктор обработчика
		/// </summary>
		/// <param name="appDbContext"></param>
		/// <param name="authorizationService"></param>
		protected BaseHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}
	}
}
