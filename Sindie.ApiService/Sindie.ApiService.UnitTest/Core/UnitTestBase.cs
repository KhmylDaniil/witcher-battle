using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Infrastructure;
using Sindie.ApiService.Storage.Postgresql;
using System;
using System.Collections.Generic;
using System.Linq;
using static Sindie.ApiService.Core.Entities.UserAccount;

namespace Sindie.ApiService.UnitTest.Core
{
	/// <summary>
	/// Базовый класс для модульных тестов
	/// </summary>
	public class UnitTestBase : IDisposable
	{
		/// <summary>
		/// Идентификатор пользователя
		/// </summary>
		protected Guid UserId = new Guid("8094e0d0-3148-4791-9053-9667cbe137d8");

		/// <summary>
		/// Идентификатор игры
		/// </summary>
		protected Guid GameId = new Guid("8094e0d0-3148-4791-9053-9667cbe137d9");

		/// <summary>
		/// Идентификатор роли
		/// </summary>
		protected Guid RoleId = new Guid("8094e0d0-3148-4791-9053-9667cbe137d7");

		/// <summary>
		/// Контекст текущего пользователя
		/// </summary>
		protected Mock<IUserContext> UserContext { get; private set; }

		/// <summary>
		/// Контекст текущего пользователя в качестве пользователя
		/// </summary>
		protected Mock<IUserContext> UserContextAsUser { get; private set; }

		/// <summary>
		/// Сервис проверки прав доступа
		/// </summary>
		protected Mock<IAuthorizationService> AuthorizationService { get; private set; }

		/// <summary>
		/// Сервис проверки прав доступа с указанием айди игры
		/// </summary>
		protected Mock<IAuthorizationService> AuthorizationServiceWithGameId { get; private set; }

		/// <summary>
		/// Провайдер даты и времени
		/// </summary>
		protected Mock<IDateTimeProvider> DateTimeProvider { get; private set; }

		/// <summary>
		/// Провайдер даты и времени
		/// </summary>
		protected Mock<IChangeListService> ChangeListServiceMock { get; private set; }

		/// <summary>
		/// Сервис бросков
		/// </summary>
		protected Mock<IRollService> RollService { get; private set; }

		/// <summary>
		/// Проверить, есть ли пользак с таким же логином
		/// </summary>
		protected HasUsersWithLogin HasUsersWithLogin => (x, y) => false;

		/// <summary>
		/// Конструктор
		/// </summary>
		public UnitTestBase()
		{
			ChangeListServiceMock = new Mock<IChangeListService>();
			ChangeListServiceMock
				.Setup(x => x.ChangeTextFilesList(
				It.IsAny<IEntityWithFiles>(), 
				It.IsAny<IEnumerable<TextFile>>()));
			ChangeListServiceMock
				.Setup(x => x.ChangeImgFilesList(
				It.IsAny<IEntityWithFiles>(),
				It.IsAny<IEnumerable<ImgFile>>()));
			
			DateTimeProvider = new Mock<IDateTimeProvider>();
			DateTimeProvider.Setup(x => x.TimeProvider).Returns(new DateTime(2021, 1, 10));

			UserContext = new Mock<IUserContext>();
			UserContext.Setup(x => x.CurrentUserId).Returns(UserId);
			UserContext.Setup(x => x.Role).Returns(SystemRoles.AndminRoleName);

			UserContextAsUser = new Mock<IUserContext>();
			UserContextAsUser.Setup(x => x.CurrentUserId).Returns(UserId);
			UserContextAsUser.Setup(x => x.Role).Returns(SystemRoles.UserRoleName);

			AuthorizationService = new Mock<IAuthorizationService>();
			AuthorizationService
				.Setup(x => x.RoleGameFilter(It.IsAny<IQueryable<Game>>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
				.Returns<IQueryable<Game>, Guid, Guid>((x, y, z) => x);
			AuthorizationService
				.Setup(x => x.UserGameFilter(It.IsAny<IQueryable<Game>>(), It.IsAny<Guid>()))
				.Returns<IQueryable<Game>, Guid>((x, y) => x);
			AuthorizationService
				.Setup(x => x.BagOwnerOrMasterFilter(It.IsAny<IQueryable<Game>>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
				.Returns<IQueryable<Game>, Guid, Guid>((x, y, z) => x);
			AuthorizationService
				.Setup(x => x.InstanceMasterFilter(It.IsAny<IQueryable<Game>>(), It.IsAny<Guid>()))
				.Returns<IQueryable<Game>, Guid>((x, y) => x);

			AuthorizationServiceWithGameId = new Mock<IAuthorizationService>();
			AuthorizationServiceWithGameId
				.Setup(x => x.RoleGameFilter(It.IsAny<IQueryable<Game>>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
				.Returns<IQueryable<Game>, Guid, Guid>((x, y, z) => x.Where(a => a.Id == y));

			RollService = new Mock<IRollService>();
			RollService.Setup(x => x.RollAttack(It.IsAny<int>(), It.IsAny<int>()))
				.Returns<int, int>((x, y) => x-y);
		}

		/// <summary>
		/// АвтоМаппер
		/// </summary>
		protected IMapper Mapper => new Mapper(new MapperConfiguration
			(cfd => cfd.AddProfile(new MappingProfile())));

		/// <summary>
		/// Создать контекст с БД в памяти
		/// </summary>
		/// <returns>Контекст AppDbContext</returns>
		protected AppDbContext CreateInMemoryContext(Action<AppDbContext> dbSeeder = null)
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: $"Test{Guid.NewGuid()}")
				.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
				.Options;

			using (var context = new AppDbContext(options, UserContext.Object, DateTimeProvider.Object))
			{
				dbSeeder?.Invoke(context);
				context.SaveChangesAsync().GetAwaiter().GetResult();
			}
			return new AppDbContext(options, UserContext.Object, DateTimeProvider.Object);
		}

		/// <inheritdoc/>
		public void Dispose() => GC.SuppressFinalize(this);
	}
}
