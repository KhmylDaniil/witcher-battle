using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using System.Security.Claims;
using Witcher.Core.Exceptions;

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
					.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
					?? throw new ApplicationSystemBaseException("Айди текущего пользователя не определено");

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
					return SystemRoles.AdminRoleName;

				var value = _httpContextAccessor.HttpContext.User.Claims
					.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
					?? throw new ApplicationSystemBaseException("Роль текущего пользователя не определена");

				return new string(value);
			}
		}
	}
}
