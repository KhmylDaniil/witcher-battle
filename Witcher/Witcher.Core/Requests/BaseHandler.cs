using MediatR;
using Witcher.Core.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests
{
	/// <summary>
	/// Базовый класс для обработчика
	/// </summary>
	public abstract class BaseHandler<TRequest, Tout> : IRequestHandler<TRequest, Tout> where TRequest: IRequest<Tout>
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

		public abstract Task<Tout> Handle(TRequest request, CancellationToken cancellationToken);
	}
}
