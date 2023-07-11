using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Entities;
using Witcher.Storage.Postgresql;
using System;
using System.Threading.Tasks;
using Witcher.Core.Requests.UserRequests;
using Witcher.Core.Contracts.UserRequests;

namespace Witcher.UnitTest.Core.Requests.UserRequests
{
	/// <summary>
	/// Тест для <see cref="RegisterUserHandler" >
	/// </summary>
	[TestClass]
	public class RegisterUserCommandHandlerTest : UnitTestBase
	{
		/// <summary>
		/// Тест метода Handle( - регистрации пользователя
		/// - должен возвращать айди пользователя, 
		/// - имя пользователя должно находиться в базе по этому айди
		/// </summary>
		/// /// <param name="name">Имя</param>
		/// <param name="password">Пароль</param>
		/// /// <param name="login">Логин</param>
		/// <returns></returns>
		[DataRow("name1", "password1", "login1")]
		[DataRow("name2", "password2", "login2")]
		[TestMethod]
		public async Task Handle_RegisterUserCommandHandler_ShouldRegisterUser
			(string name, string login, string password)
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: $"Test{Guid.NewGuid()}")
				.Options;
			using var context = new AppDbContext(options, UserContext.Object, DateTimeProvider.Object);
			context.Interfaces.Add(Interface.CreateForTest());
			context.Interfaces.Add(Interface.CreateForTest(SystemInterfaces.SystemDarkName, InterfaceType.System, SystemInterfaces.SystemDarkId));
			context.SystemRoles.Add(SystemRole.CreateForTest());
			context.SaveChanges();

			var request = new RegisterUserCommand()
			{
				Name = name,
				Login = login,
				Password = password
			};

			var passwordHasherMock = new Mock<IPasswordHasher>();
			passwordHasherMock.Setup(foo => foo.Hash
				(
				It.IsAny<string>()
				)
			).Returns("AAA");

			//Arrange
			var registerUserCommandHandler = new RegisterUserHandler
				(context, AuthorizationService.Object, passwordHasherMock.Object, HasUsersWithLogin);

			//Act
			var result = await registerUserCommandHandler.Handle(request, default);

			//Assert
			Assert.IsNotNull(result);

			var userId = await context.UserAccounts
				?.Include(x => x.User)
				.FirstOrDefaultAsync(x => x.Login == login);
			Assert.IsNotNull(userId?.User?.Id);
			Assert.AreEqual(userId.User.Id, result);

			var userName = await context.UserAccounts
				?.Include(x => x.User)
				.FirstOrDefaultAsync(x => x.Login == login);
			Assert.IsNotNull(userName?.User?.Name);
			Assert.AreEqual(name, userName.User.Name);
		}
	}
}
