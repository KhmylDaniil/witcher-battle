using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Entities;
using Witcher.Storage.Postgresql;
using System;
using System.Collections.Generic;
using System.Linq;
using static Witcher.Core.Entities.UserAccount;

namespace Witcher.UnitTest.Core
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
		/// Сервис айди игры
		/// </summary>
		protected Mock<IGameIdService> GameIdService { get; private set; }

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
		/// Делегат для мока провала атакующего в броске атаки
		/// </summary>
		/// <param name="attackBase">База атаки</param>
		/// <param name="defenseBase">База защиты</param>
		/// <param name="fumble">Значение провала</param>
		private delegate void AttackerFumble(int attackBase, int defenseBase, out int fumble);

		/// <summary>
		/// Делегат для мока провала в контестном броске
		/// </summary>
		/// <param name="attackBase">База атаки</param>
		/// <param name="defenseBase">База защиты</param>
		/// <param name="attackerFumble">Провал атакующего</param>
		/// <param name="defenderFumble">Провал защитника</param>
		private delegate void ContestedFumble(
			int attackBase,
			int defenseBase,
			int? attackValue,
			int? defenseValue,
			out int attackerFumble,
			out int defenderFumble);

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
			UserContext.Setup(x => x.Role).Returns(SystemRoles.AdminRoleName);

			UserContextAsUser = new Mock<IUserContext>();
			UserContextAsUser.Setup(x => x.CurrentUserId).Returns(UserId);
			UserContextAsUser.Setup(x => x.Role).Returns(SystemRoles.UserRoleName);

			GameIdService = new Mock<IGameIdService>();
			GameIdService.Setup(x => x.GameId).Returns(GameId);

			AuthorizationService = new Mock<IAuthorizationService>();
			AuthorizationService
				.Setup(x => x.UserGameFilter(It.IsAny<IQueryable<Game>>()))
				.Returns<IQueryable<Game>>(x => x);
			AuthorizationService
				.Setup(x => x.BattleMasterFilter(It.IsAny<IQueryable<Battle>>(), It.IsAny<Guid>()))
				.Returns<IQueryable<Battle>, Guid>((x, y) => x);

			AuthorizationService
				.Setup(x => x.AuthorizedGameFilter(It.IsAny<IQueryable<Game>>(), It.IsAny<Guid>()))
				.Returns<IQueryable<Game>, Guid>((x, y) => x);

			AuthorizationServiceWithGameId = new Mock<IAuthorizationService>();

			RollService = new Mock<IRollService>();
			int attackerFumble = 0;
			int defenderFumble = 0;
			int roll = 0;
			RollService.Setup(x => x.BeatDifficultyWithFumble(It.IsAny<int>(), It.IsAny<int>(), out attackerFumble))
				.Callback(new AttackerFumble((int a, int d, out int attackerFumble)
					=> attackerFumble = 0 ))
				.Returns<int, int, int>((attackBase, defenseBase, attackerFumble) => attackBase - defenseBase);

			RollService.Setup(x => x.BeatDifficulty(It.IsAny<int>(), It.IsAny<int>()))
				.Returns<int, int>((attackBase, defenseBase) => attackBase > defenseBase);

			RollService.Setup(x => x.BeatDifficulty(It.IsAny<int>(), It.IsAny<int>(), out roll))
				.Callback(new AttackerFumble((int a, int d, out int roll)
					=> roll = a))
				.Returns<int, int, int>((attackBase, defenseBase, attackerFumble) => attackBase > defenseBase);

			RollService.Setup(x => x.ContestRollWithFumble(
				It.IsAny<int>(), 
				It.IsAny<int>(), 
				It.IsAny<int?>(), 
				It.IsAny<int?>(), 
				out attackerFumble, 
				out defenderFumble))
				.Callback(new ContestedFumble((int a, int d, int? av, int? dv, out int attackerFumble, out int defenderFumble)
				=>
				{
					attackerFumble = 0;
					defenderFumble = 0;
				}))
				.Returns<int, int, int?, int?, int, int>(
				(attackBase, defenseBase, attackValue, defenseValue, attackerFumble, defenderFumble) => attackBase - defenseBase);
		}

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
