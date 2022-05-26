using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.UserGameRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.UserGameRequests;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.UserGameRequests
{
	/// <summary>
	/// Тест для <see cref="ChangeUserGameHandler"/>
	/// </summary>
	[TestClass]
	public class ChangeUserGameHandlerTest: UnitTestBase
	{
		private readonly Interface _currentInterface;
		private readonly Interface _newInterface;
		private readonly User _user;
		private readonly UserGame _userGame;
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Конструктор для <see cref="ChangeUserGameHandler"/>
		/// </summary>
		public ChangeUserGameHandlerTest()
		{
			_user = User.CreateForTest(id: UserId);
			_currentInterface = Interface.CreateForTest(id: SystemInterfaces.GameDarkId);
			_newInterface = Interface.CreateForTest(id: SystemInterfaces.GameLightId);
			_userGame = UserGame.CreateForTest(user: _user, @interface: _currentInterface);
			_appDbContext = CreateInMemoryContext(x => x.AddRange(
				_currentInterface,
				_newInterface,
				_user,
				_userGame));
		}

		/// <summary>
		/// Тест метода Handle для изменения интерфейса пользователя игры
		/// </summary>
		/// <returns>Юнит</returns>
		[TestMethod]
		public async Task Handle_ChangeUserGame_ShouldReturnUnit()
		{
			var request = new ChangeUserGameCommand()
			{
				UserGameId = _userGame.Id,
				InterfaceId = _newInterface.Id,
			};

			var newHandler = new ChangeUserGameHandler(_appDbContext, UserContext.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			var userGame = _appDbContext.UserGames.FirstOrDefault(x => x.Id == _userGame.Id);
			Assert.IsNotNull(userGame);
			Assert.IsTrue(userGame.InterfaceId == _newInterface.Id);
		}
	}
}
