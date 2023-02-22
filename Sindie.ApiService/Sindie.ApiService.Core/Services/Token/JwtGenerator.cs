using Microsoft.IdentityModel.Tokens;
using Sindie.ApiService.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Sindie.ApiService.Core.Services.Token
{
	/// <summary>
	/// Генератор токенов
	/// </summary>
	public class JwtGenerator : IJwtGenerator
	{
		/// <summary>
		/// Настройки авторизации
		// </summary>
		private readonly AuthOptions _authOptions;

		/// <summary>
		/// Конструктор контроллера пользователя
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		public JwtGenerator(AuthOptions authOptions)
		{
			_authOptions = authOptions;
		}

		/// <summary>
		/// Создание токена
		/// </summary>
		/// <param name="id">Айди</param>
		/// <returns>Токен</returns>
		public string CreateToken(Guid id, string Role)
		{
			var claims = new List<Claim>
				{
				new Claim(JwtRegisteredClaimNames.NameId, Convert.ToString(id)),
				new Claim(JwtRegisteredClaimNames.GivenName, Convert.ToString(Role))
				};
			var now = DateTime.UtcNow;

			var jwt = new JwtSecurityToken(
					issuer: _authOptions.AuthServer,
					audience: _authOptions.AuthClient,
					notBefore: now,
					claims: claims,
					expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
					signingCredentials: new SigningCredentials
						(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
			var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

			return encodedJwt;
		}
	}
}
