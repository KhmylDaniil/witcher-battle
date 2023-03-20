using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.UserRequests.LoginUser;
using Witcher.Core.Entities;
using Witcher.Core.Requests.UserRequests.LoginUser;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Witcher.UnitTest.Core.Requests.UserRequests
{
	/// <summary>
	/// Тест для <see cref="LoginUserCommandHandler">
	/// </summary>
	[TestClass]
	public class LoginUserCommandHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly User _user;
		private readonly User _user2;

		public LoginUserCommandHandlerTest()
		{
			var role = SystemRole.CreateForTest(name: "User");
			_user = User.CreateForTest(id: new Guid(), login: "aaa", password: "aaa", role: role);
			_user2 = User.CreateForTest(id: new Guid(), login: "bbb", password: "bbb", role: role);

			_dbContext = CreateInMemoryContext(x => x.AddRange(_user, _user2));
		}

		/// <summary>
		/// Тест метода Handle(аутентификации пользователя
		/// должен возвращать айди пользователя равный ожидаемому айди
		/// </summary>
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

			var loginUserCommandHandler = new TestLoginUserCommandHandler
				(_dbContext, AuthorizationService.Object, passwordHasherMock.Object, httpContextMock.Object);

			var result = await loginUserCommandHandler.Handle(request, default);

			Assert.AreEqual(_user.Id, result?.UserId);
			var userAcc = _user.UserAccounts.First();
			Assert.IsNotNull(userAcc);
			Assert.AreEqual(userAcc.Login, request.Login);
			Assert.AreEqual(userAcc.PasswordHash, request.Password);
		}
	}

	/// <summary>
	/// Тестовый класс для обработчика
	/// </summary>
	class TestLoginUserCommandHandler : LoginUserCommandHandler
	{
		public TestLoginUserCommandHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IPasswordHasher passwordHasher, IHttpContextAccessor httpContextAccessor)
			: base(appDbContext, authorizationService, passwordHasher, httpContextAccessor)
		{
		}

		/// <summary>
		/// Переопределенный метод для исключения из тестирования метода расширения
		/// </summary>
		/// <param name="httpContext"></param>
		/// <param name="claimsPrincipal"></param>
		/// <returns></returns>
		protected override async Task SignCookiesAsync(HttpContext httpContext, ClaimsPrincipal claimsPrincipal)
		{
			await Task.CompletedTask;
		}
	}
}