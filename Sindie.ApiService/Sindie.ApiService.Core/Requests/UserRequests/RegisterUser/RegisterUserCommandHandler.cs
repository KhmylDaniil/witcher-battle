using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.UserRequests.RegisterUser;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.UserRequests.RegisterUser
{
	/// <summary>
	/// Обработчик команды регистрации пользователя
	/// </summary>
	public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserCommandResponse>
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
		/// Хеширование пароля
		/// </summary>
		private readonly UserAccount.HasUsersWithLogin _hasUsersWithLogin;

		/// <summary>
		/// Конструктор обработчика команды регистрации пользователя
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="passwordHasher">Хеширование пароля</param>
		public RegisterUserCommandHandler(
			IAppDbContext appDbContext, 
			IPasswordHasher passwordHasher, 
			UserAccount.HasUsersWithLogin hasUsersWithLogin)
		{
			_appDbContext = appDbContext;
			_passwordHasher = passwordHasher;
			_hasUsersWithLogin = hasUsersWithLogin;
		}

		/// <summary>
		/// Обработать запрос регистрации пользователя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns>Ответ</returns>
		public async Task<RegisterUserCommandResponse> Handle
			(RegisterUserCommand request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException($"Пришел пустой запрос {typeof(RegisterUserCommand)}");
			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ExceptionRequestFieldIncorrectData<RegisterUserCommand>(nameof(request.Name));
			if (string.IsNullOrWhiteSpace(request.Password))
				throw new ExceptionRequestFieldIncorrectData<RegisterUserCommand>(nameof(request.Password));
			if (string.IsNullOrWhiteSpace(request.Login))
				throw new ExceptionRequestFieldIncorrectData<RegisterUserCommand>(nameof(request.Login));

			var systemInterface = await _appDbContext.Interfaces
				.FirstOrDefaultAsync(x => x.Id == SystemInterfaces.SystemLightId, cancellationToken)
					?? throw new ExceptionEntityNotFound<Interface>(SystemInterfaces.SystemLightId);
			var role = await _appDbContext.SystemRoles
				.FirstOrDefaultAsync(x => x.Id == SystemRoles.UserRoleId, cancellationToken)
					?? throw new ExceptionEntityNotFound<SystemRole>(SystemRoles.UserRoleId);

			var user = new User(
				name: request.Name,
				email: request.Email,
				phone: request.Phone,
				@interface: systemInterface);

			user.UserAccounts.Add(new UserAccount(
				login: request.Login,
				passwordHash: _passwordHasher.Hash(request.Password),
				user: user,
				_hasUsersWithLogin));

			var acc = new UserAccount(
				login: request.Login,
				passwordHash: _passwordHasher.Hash(request.Password),
				user: user,
				_hasUsersWithLogin);

			user.UserRoles.Add(new UserRole(
				user: user,
				role: role));

			await _appDbContext.Users.AddAsync(user, cancellationToken);
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return new RegisterUserCommandResponse
			{
				UserId = user.Id
			};
		}
	}
}
