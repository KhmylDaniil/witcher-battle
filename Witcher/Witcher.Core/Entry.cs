using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Witcher.Core.Abstractions;
using Witcher.Core.Entities;
using Witcher.Core.Services.Authorization;
using Witcher.Core.Services.ChangeListService;
using Witcher.Core.Services.DateTimeProvider;
using Witcher.Core.Services.Hasher;
using Witcher.Core.Services.Roll;
using System;
using System.Linq;
using Witcher.Core.Exceptions;
using Witcher.Core.Exceptions.SystemExceptions;

namespace Witcher.Core
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
		public static void AddCore(this IServiceCollection services, HasherOptions hasherOptions)
		{
			if (string.IsNullOrWhiteSpace(hasherOptions?.Salt))
				throw new ApplicationSystemNullException(nameof(Entry), nameof(hasherOptions));

			services.AddMediatR(typeof(Entry).Assembly);
			services.AddTransient<IPasswordHasher>
				(o => new PasswordHasher(hasherOptions));

			services.AddSingleton<IGameIdService, GameIdService>();

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
