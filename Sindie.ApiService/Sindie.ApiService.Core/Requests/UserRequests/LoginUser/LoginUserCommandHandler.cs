using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.UserRequests.LoginUser;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
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
		/// Доступ к Http контексту
		/// </summary>
		private readonly IHttpContextAccessor _httpContextAccessor;

		/// <summary>
		/// Конструктор обработчика команды аутентификации пользователя
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="passwordHasher">Хеширование пароля</param>
		/// <param name="httpContextAccessor">Доступ к Http контексту</param>
		public LoginUserCommandHandler(IAppDbContext appDbContext, IPasswordHasher passwordHasher, IHttpContextAccessor httpContextAccessor)
		{
			_appDbContext = appDbContext;
			_passwordHasher = passwordHasher;
			_httpContextAccessor = httpContextAccessor;
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
			var existingUserAccount = await _appDbContext.UserAccounts
				.Include(x => x.User)
					.ThenInclude(x => x.UserRoles)
						.ThenInclude(x => x.SystemRole)
				.FirstOrDefaultAsync(x => x.Login == request.Login, cancellationToken);

			if (existingUserAccount == null)
				throw new AuthenticationException("Неверный логин");

			bool isPasswordCorrect = _passwordHasher.VerifyHash
				(request.Password, existingUserAccount.PasswordHash);

			if (!isPasswordCorrect)
				throw new AuthenticationException("Неверный пароль");


			var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, existingUserAccount.UserId.ToString()),
					new Claim(ClaimTypes.Role, existingUserAccount.User.UserRoles.FirstOrDefault().SystemRole.Name)
				};

			ClaimsIdentity claimsIdentity = new (claims, "Cookies");

			await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

			return new LoginUserCommandResponse { UserId = existingUserAccount.UserId };
		}
	}
}
