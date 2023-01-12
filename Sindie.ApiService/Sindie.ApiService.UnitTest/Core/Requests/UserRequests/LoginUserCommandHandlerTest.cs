using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.UserRequests.LoginUser;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.UserRequests.LoginUser;
using System;
using System.Linq;
using System.Security.Claims;
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

			var httpContextMock = new Mock<IHttpContextAccessor>();
			httpContextMock.Setup(x => x.HttpContext.SignInAsync(It.IsAny<string>(), It.IsAny<ClaimsPrincipal>())).Returns(Task.CompletedTask);

			//Arrange
			var loginUserCommandHandler = new LoginUserCommandHandler
				(_dbContext, passwordHasherMock.Object, httpContextMock.Object);

			//Act
			var result = await loginUserCommandHandler.Handle(request, default);

			//Assert
			Assert.AreEqual(_user.Id, result?.UserId);
			var userAcc = _user.UserAccounts.First();
			Assert.IsNotNull(userAcc);
			Assert.AreEqual(userAcc.Login, request.Login);
			Assert.AreEqual(userAcc.PasswordHash, request.Password);

		}
	}
}