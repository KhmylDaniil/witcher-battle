using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;

namespace Witcher.MVC
{
	/// <summary>
	/// Получение контекста пользователя из веба
	/// </summary>
	public class UserContext : IUserContext
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		/// <summary>
		/// Конструктор класса получение контекста пользователя из веба
		/// </summary>
		/// <param name="httpContextAccessor">HTTP контекст акессор</param>
		public UserContext(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// Текущий айди пользователя
		/// </summary>
		public Guid CurrentUserId
		{
			get
			{
				if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
					return SystemUsers.SystemUserId;

				var value = _httpContextAccessor.HttpContext.User.Claims
					.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
					?.Value;
				if (value == null)
					throw new InvalidOperationException("Айди текущего пользователя не определено");
				return new Guid(value);
			}
		}

		/// <summary>
		/// Текущая роль пользователя
		/// </summary>
		public string Role
		{
			get
			{
				if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
					return SystemRoles.AndminRoleName;

				var value = _httpContextAccessor.HttpContext.User.Claims
					.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")
					?.Value;
				if (value == null)
					throw new InvalidOperationException("Роль текущего пользователя не определена");
				return new string(value);
			}
		}
	}
}
