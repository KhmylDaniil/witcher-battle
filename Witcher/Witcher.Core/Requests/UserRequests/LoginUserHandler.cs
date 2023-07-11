using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Exceptions.SystemExceptions;
using System;
using Witcher.Core.Contracts.UserRequests;

namespace Witcher.Core.Requests.UserRequests
{
	/// <summary>
	/// Обработчик команды аутентификации пользователя
	/// </summary>
	public class LoginUserHandler : BaseHandler<LoginUserCommand, Guid>
	{

		/// <summary>
		/// Хеширование пароля
		/// </summary>
		private readonly IPasswordHasher _passwordHasher;

		/// <summary>
		/// Доступ к Http контексту
		/// </summary>
		protected readonly IHttpContextAccessor _httpContextAccessor;

		/// <summary>
		/// Конструктор обработчика команды аутентификации пользователя
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="passwordHasher">Хеширование пароля</param>
		/// <param name="httpContextAccessor">Доступ к Http контексту</param>
		public LoginUserHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IPasswordHasher passwordHasher, IHttpContextAccessor httpContextAccessor)
			: base(appDbContext, authorizationService)
		{
			_passwordHasher = passwordHasher;
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// Обработать запрос аутентификации пользователя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns>ответ</returns>
		public override async Task<Guid> Handle
			(LoginUserCommand request, CancellationToken cancellationToken)
		{
			var existingUserAccount = await _appDbContext.UserAccounts
				.Include(x => x.User)
					.ThenInclude(x => x.UserRoles)
						.ThenInclude(x => x.SystemRole)
				.FirstOrDefaultAsync(x => x.Login == request.Login, cancellationToken);

			if (existingUserAccount == null)
				throw new ApplicationSystemNullException<LoginUserHandler>(nameof(existingUserAccount));

			bool isPasswordCorrect = _passwordHasher.VerifyHash
				(request.Password, existingUserAccount.PasswordHash);

			if (!isPasswordCorrect)
				throw new ApplicationSystemNullException<LoginUserHandler>(nameof(isPasswordCorrect));

			var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, existingUserAccount.UserId.ToString()),
					new Claim(ClaimTypes.Role, existingUserAccount.User.UserRoles.FirstOrDefault().SystemRole.Name)
				};

			ClaimsIdentity claimsIdentity = new(claims, "Cookies");

			await SignCookiesAsync(_httpContextAccessor.HttpContext, new ClaimsPrincipal(claimsIdentity));

			return existingUserAccount.UserId;
		}

		/// <summary>
		/// Выделение метода расширения для возможности переопределения его поведения в тестах 
		/// </summary>
		/// <param name="httpContext"></param>
		/// <param name="claimsPrincipal"></param>
		/// <returns></returns>
		protected virtual async Task SignCookiesAsync(HttpContext httpContext, ClaimsPrincipal claimsPrincipal)
			=> await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
	}
}
