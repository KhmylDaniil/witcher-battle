using System.Security.Claims;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.WebExceptions;

namespace Wastelands.Service.MVC.Services
{
	public class UserContext : IUserContext
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserContext(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public long CurrentUserId
		{
			get
			{
				if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
					throw new UnauthorizedException(ErrorCode.UserNotAuthorized, ExceptionMessages.UserNotAuthorized);

				var value = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

				if (!long.TryParse(value, out long result))
				{
					throw new UnauthorizedException(ErrorCode.UserNotAuthorized, ExceptionMessages.UserNotAuthorized);
				}

				return result;
			}
		}
	}
}
