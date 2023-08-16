using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.UserRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Threading;
using System.Threading.Tasks;
using System;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.UserRequests
{
	/// <summary>
	/// Обработчик команды регистрации пользователя
	/// </summary>
	public class RegisterUserHandler : BaseHandler<RegisterUserCommand, Guid>
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
		public RegisterUserHandler(
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
		public override async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
		{
			var systemInterface = await _appDbContext.Interfaces
				.FirstOrDefaultAsync(x => x.Id == SystemInterfaces.SystemLightId, cancellationToken)
					?? throw new EntityNotFoundException<Interface>(SystemInterfaces.SystemLightId);
			var role = await _appDbContext.SystemRoles
				.FirstOrDefaultAsync(x => x.Id == SystemRoles.UserRoleId, cancellationToken)
					?? throw new EntityNotFoundException<SystemRole>(SystemRoles.UserRoleId);

			if (await _appDbContext.Users.AnyAsync(x => x.Name == request.Name, cancellationToken: cancellationToken))
				throw new RequestNameNotUniqException<User>(request.Name);

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

			return user.Id;
		}
	}
}
