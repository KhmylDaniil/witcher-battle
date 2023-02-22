using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.UserRequests.LoginUser;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.UserRequests.LoginUser
{
	/// <summary>
	/// Обработчик команды аутентификации пользователя
	/// </summary>
	public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserCommandResponse>
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Хеширование пароля
		/// </summary>
		private readonly IPasswordHasher _passwordHasher;

		/// <summary>
		/// Генератор токенов
		/// </summary>
		private readonly IJwtGenerator _jwtGenerator;

		/// <summary>
		/// Конструктор обработчика команды аутентификации пользователя
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="passwordHasher">Хеширование пароля</param>
		/// <param name="jwtGenerator">Генератор токенов</param>
		public LoginUserCommandHandler(IAppDbContext appDbContext, IPasswordHasher passwordHasher,
			IJwtGenerator jwtGenerator)
		{
			_appDbContext = appDbContext;
			_passwordHasher = passwordHasher;
			_jwtGenerator = jwtGenerator;
		}

		/// <summary>
		/// Обработать запрос аутентификации пользователя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns>ответ</returns>
		public async Task<LoginUserCommandResponse> Handle
			(LoginUserCommand request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException($"Пришел пустой запрос {typeof(LoginUserCommand)}");
			if (string.IsNullOrWhiteSpace(request.Password))
				throw new ExceptionRequestFieldIncorrectData<LoginUserCommand>(nameof(request.Password));
			if (string.IsNullOrWhiteSpace(request.Login))
				throw new ExceptionRequestFieldIncorrectData<LoginUserCommand>(nameof(request.Login));

			var existingUserAccount = await _appDbContext.UserAccounts
				.Include(x => x.User)
					.ThenInclude(x => x.UserRoles)
					.ThenInclude(x => x.SystemRole)
				.FirstOrDefaultAsync(x => x.Login == request.Login, cancellationToken);

			if (existingUserAccount == null)
				throw new AuthenticationException("Не верный логин");

			bool isPasswordCorrect = _passwordHasher.VerifyHash
				(request.Password, existingUserAccount.PasswordHash);

			if (!isPasswordCorrect)
				throw new AuthenticationException("Не верный пароль");

			string authToken = _jwtGenerator.CreateToken(
				existingUserAccount.UserId,
				existingUserAccount.User.UserRoles.FirstOrDefault().SystemRole.Name
				?? throw new ExceptionEntityNotFound<SystemRole>());
			if (authToken == null)
				throw new ExceptionUnauthorizedBase(request.Login); //TODO to do подобрать или сделать ошибку получше

			return new LoginUserCommandResponse
			{
				UserId = existingUserAccount.UserId,
				AuthenticationToken = authToken
			};
		}
	}
}
