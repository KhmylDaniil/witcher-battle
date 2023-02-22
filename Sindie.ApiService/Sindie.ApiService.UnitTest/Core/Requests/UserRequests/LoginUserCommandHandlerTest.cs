using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.UserRequests.LoginUser;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.UserRequests.LoginUser;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.UserRequests
{
	/// <summary>
	/// Тест для <see cref="LoginUserCommandHandler" >
	/// </summary>
	[TestClass]
	public class LoginUserCommandHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly User _user;
		private readonly User _user2;

		/// <summary>
		/// Конструктор
		/// </summary>
		public LoginUserCommandHandlerTest()
		{
			var role = SystemRole.CreateForTest(name: "User");
			_user = User.CreateForTest(id: new Guid(), login: "aaa", password: "aaa", role: role);
			_user2 = User.CreateForTest(id: new Guid(), login: "bbb", password: "bbb", role: role);

			_dbContext = CreateInMemoryContext(x => x.AddRange(_user, _user2));
		}

		/// <summary>
		/// Тест метода Handle( - аутентификации пользователя
		/// - должен возвращать айди пользователя равный ожидаемому айди
		/// </summary>
		/// /// <param name="login">Логин</param>
		/// <param name="password">Пароль</param>
		/// /// <param name="id">Айди</param>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ByLoginUserCommandHandler_ShouldLoginUser()
		{
			var request = new LoginUserCommand()
			{
				Login = _user.UserAccounts.First().Login,
				Password = _user.UserAccounts.First().PasswordHash
			};

			var passwordHasherMock = new Mock<IPasswordHasher>();

			passwordHasherMock.Setup(foo => foo.VerifyHash
				(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

			var jwtGeneratorMock = new Mock<IJwtGenerator>();
			jwtGeneratorMock.Setup(foo => foo.CreateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns("aaa");

			//Arrange
			var loginUserCommandHandler = new LoginUserCommandHandler
				(_dbContext, passwordHasherMock.Object, jwtGeneratorMock.Object);

			//Act
			var result = await loginUserCommandHandler.Handle(request, default);

			//Assert
			Assert.AreEqual(_user.Id, result?.UserId);
			var userAcc = _user.UserAccounts.First();
			Assert.IsNotNull(userAcc);
			Assert.AreEqual(userAcc.Login, request.Login);
			Assert.AreEqual(userAcc.PasswordHash, request.Password);
			Assert.IsNotNull(result?.AuthenticationToken);
			Assert.IsNotNull(result?.CreatedOn);
		}

		/// <summary>
		/// Тест метода Handle( - аутентификации пользователя
		/// - он должен генерировать исключение в случае если пришел пустой запрос
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ByLoginUserCommandHandler_ShouldThrowArgumentNullException()
		{
			LoginUserCommand request = null;

			//Arrange
			var loginUserCommandHandler = new LoginUserCommandHandler(default, default, default);

			//Assert
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
				 await loginUserCommandHandler.Handle(request, default));
		}
	}
}