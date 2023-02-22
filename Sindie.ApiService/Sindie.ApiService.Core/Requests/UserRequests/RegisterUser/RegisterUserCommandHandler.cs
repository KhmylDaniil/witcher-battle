using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.UserRequests.RegisterUser;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.UserRequests.RegisterUser
{
	/// <summary>
	/// Обработчик команды регистрации пользователя
	/// </summary>
	public class RegisterUserCommandHandler : BaseHandler<RegisterUserCommand, RegisterUserCommandResponse>
	{
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
			IAuthorizationService authorizationService,
			IPasswordHasher passwordHasher, 
			UserAccount.HasUsersWithLogin hasUsersWithLogin)
			: base(appDbContext, authorizationService)
		{
			_passwordHasher = passwordHasher;
			_hasUsersWithLogin = hasUsersWithLogin;
		}

		/// <summary>
		/// Обработать запрос регистрации пользователя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns>Ответ</returns>
		public override async Task<RegisterUserCommandResponse> Handle (RegisterUserCommand request, CancellationToken cancellationToken)
		{
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

			user.UserRoles.Add(new UserRole(
				user: user,
				role: role));

			await _appDbContext.Users.AddAsync(user, cancellationToken);
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return new RegisterUserCommandResponse { UserId = user.Id };
		}
	}
}
