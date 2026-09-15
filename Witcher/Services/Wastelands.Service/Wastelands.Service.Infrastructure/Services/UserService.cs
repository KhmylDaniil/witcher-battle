using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.WebExceptions;
using Wastelands.Service.Domain.Contracts;
using Wastelands.Service.Domain.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.Infrastructure.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IPasswordService _passwordService;
		private readonly IMapper _mapper;

		public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IPasswordService passwordService, IMapper mapper)
		{
			_userRepository = userRepository;
			_httpContextAccessor = httpContextAccessor;
			_passwordService = passwordService;
			_mapper = mapper;
		}

		public async Task LoginUserAsync(LoginUserRequest request)
		{
			var user = await _userRepository.GetByLoginAsync(request.Login);

			if (!_passwordService.VerifyHash(request.Password, user.Password))
			{
				throw new BadRequestException(ErrorCode.InvalidPassword, ExceptionMessages.PasswordIsIncorrect);
			}

			var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.Id.ToString()),
				};

			ClaimsIdentity claimsIdentity = new(claims, "Cookies");

			await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
		}

		public async Task RegisterUserAsync(RegisterUserRequest request)
		{
			if (await _userRepository.AnyAsync(x => x.Login == request.Login))
			{
				throw new BadRequestException(ErrorCode.InvalidArgument, ExceptionMessages.ValueMustBeUnique);
			}

			var user = _mapper.Map<User>(request);
			user.Password = _passwordService.GetPasswordHash(request.Password);

			await _userRepository.CreateAsync(user);
		}
	}
}
