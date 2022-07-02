using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Services.Authorization;
using Sindie.ApiService.Core.Services.ChangeListService;
using Sindie.ApiService.Core.Services.DateTimeProvider;
using Sindie.ApiService.Core.Services.Hasher;
using Sindie.ApiService.Core.Services.Roll;
using Sindie.ApiService.Core.Services.Token;
using System;
using System.Linq;

namespace Sindie.ApiService.Core
{
	/// <summary>
	/// Точка входа
	/// </summary>
	public static class Entry
	{
		/// <summary>
		/// Добавить зависимости проекта Core
		/// </summary>
		/// <param name="services">Сервисы IServiceCollection</param>
		/// <param name="hasherOptions">Параметры хеширования</param>
		public static void AddCore(this IServiceCollection services, HasherOptions hasherOptions,
			string authServer, string authClient, string authKey)
		{
			if (string.IsNullOrWhiteSpace(hasherOptions?.Salt))
				throw new ArgumentNullException(nameof(hasherOptions));
			if (string.IsNullOrWhiteSpace(authServer))
				throw new ArgumentNullException(nameof(authServer));
			if (string.IsNullOrWhiteSpace(authClient))
				throw new ArgumentNullException(nameof(authClient));
			if (string.IsNullOrWhiteSpace(authKey))
				throw new ArgumentNullException(nameof(authKey));

			services.AddSingleton(new AuthOptions(authServer, authClient, authKey));
			services.AddMediatR(typeof(Entry).Assembly);
			services.AddTransient<IPasswordHasher>
				(o => new PasswordHasher(hasherOptions));
			services.AddScoped<IJwtGenerator, JwtGenerator>();

			services.AddAutoMapper(typeof(Entry).Assembly);

			services.AddScoped<IAuthorizationService, AuthorizationService>();

			services.AddScoped<IDateTimeProvider, DateTimeProvider>();

			services.AddTransient<IChangeListService, ChangeListService>();

			services.AddTransient<IRollService, RollService>();

			services.AddScoped<UserAccount.HasUsersWithLogin>(
				sp =>
					(UserAccount account, string login) => sp.GetRequiredService<IAppDbContext>()
						.UserAccounts
						.Any(x => x.Login == login && x.Id != account.Id));
		}
	}
}
