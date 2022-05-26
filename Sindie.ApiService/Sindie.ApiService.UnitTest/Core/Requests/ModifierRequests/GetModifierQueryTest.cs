using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.ModifierRequests.GetModifier;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.ModifierRequests.GetModifier;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.ModifierRequests
{
	/// <summary>
	/// Тест для <see cref="GetModifierQuery"/>
	/// </summary>
	[TestClass]
	public class GetModifierQueryTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Modifier _modifier;
		private readonly User _user;
		private readonly Game _game;

		/// <summary>
		/// Конструктор теста получения модификатора
		/// </summary>
		public GetModifierQueryTest(): base()
		{
			_user = User.CreateForTest(
				id: UserContext.Object.CurrentUserId,
				name: "Author");

			_game = Game.CreateForTest();
			_game.UserGames.Add(
				UserGame.CreateForTest(
					user: _user,
					gameRole: GameRole.CreateForTest(GameRoles.MasterRoleId)));

			_modifier = Modifier.CreateForTest(
				name: "testName",
				game: _game,
				createdOn: DateTimeProvider.Object.TimeProvider,
				modifiedOn: DateTimeProvider.Object.TimeProvider,
				createdByUserId: _user.Id);
			_modifier.ModifierScripts.Add(
				ModifierScript.CreateForTest(
					modifier: _modifier,
					script: Script.CreateForTest(),
					@event: Event.CreateForTest(),
					activationTime: DateTimeProvider.Object.TimeProvider,
					periodOfActivity: 24*60,
					periodOfInactivity: 24*60,
					numberOfRepetitions: 1
					));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game, _user, _modifier));
		}

		/// <summary>
		/// Тест метода Handle получение списка модификаторов с фильтрами
		/// по названию, дате создания/изменения, активности, создавшему пользователю
		/// требует права мастера
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_GetModifier_ShouldReturn_GetModifierResponse()
		{
			var creationMinTime = DateTimeProvider.Object.TimeProvider.AddDays(-1);
			var creationMaxTime = DateTimeProvider.Object.TimeProvider.AddDays(1);
			var modificationMinTime = DateTimeProvider.Object.TimeProvider.AddDays(-1);
			var modificationMaxTime = DateTimeProvider.Object.TimeProvider.AddDays(1);
			
			var request = new GetModifierQuery()
			{
				GameId = _game.Id,
				SearchText = "test",
				UserName = "Author",
				CreationMaxTime = creationMaxTime,
				CreationMinTime = creationMinTime,
				ModificationMaxTime = modificationMaxTime,
				ModificationMinTime = modificationMinTime,
				IsActive = true,
				PageSize = 1,
				PageNumber = 1
			};

			var newHandler = new GetModifierHandler(_dbContext, DateTimeProvider.Object, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.TotalCount);
			
			var resultItem = result.ModifiersList.First();
			Assert.IsTrue(resultItem.Name.Contains("test"));
			Assert.IsTrue(resultItem.CreatedOn >= creationMinTime && resultItem.CreatedOn <= creationMaxTime);
			Assert.IsTrue(resultItem.ModifiedOn >= modificationMinTime && resultItem.ModifiedOn <= modificationMaxTime);

			var modifierScript = _dbContext.ModifierScripts
				.Include(x => x.ActiveCycles)	
				.First(x => x.ModifierId == resultItem.Id);
			Assert.IsNotNull(modifierScript);
			Assert.IsNotNull(modifierScript.ActiveCycles);
			

			Assert.IsTrue(modifierScript.ActiveCycles.Any(
				ac => ac.ActivationBeginning >= DateTimeProvider.Object.TimeProvider
				&& ac.ActivationEnd <= DateTimeProvider.Object.TimeProvider.AddDays(1)));

			var users = _dbContext.Users
				.Where(x => x.Name.Contains("Author"))
				.Select(y => y.Id).ToList();
			Assert.IsTrue(users.Contains(resultItem.CreatedByUserId));

			var userGame = _dbContext.UserGames.
				FirstOrDefault(x => x.GameId == resultItem.GameId);
			Assert.IsNotNull(userGame);
			Assert.IsTrue(userGame.UserId == _user.Id);
			Assert.IsTrue(userGame.GameRoleId == GameRoles.MasterRoleId);
		}
	}
}
