using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Entities;
using Witcher.Core.Contracts.WeaponTemplateRequests;
using Witcher.Core.Requests.WeaponTemplateRequests;

namespace Witcher.UnitTest.Core.Requests.WeaponTemplateRequests
{
	[TestClass]
	public class GetWeaponTemplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly User _user;
		private readonly Game _game;
		private readonly WeaponTemplate _weaponTemplate;

		public GetWeaponTemplateHandlerTest() : base()
		{
			_user = User.CreateForTest(
				id: UserContext.Object.CurrentUserId,
				name: "Author");

			_game = Game.CreateForTest();
			_game.UserGames.Add(
				UserGame.CreateForTest(
					user: _user,
					gameRole: GameRole.CreateForTest(GameRoles.MasterRoleId)));

			_weaponTemplate = WeaponTemplate.CreateForTest(
				game: _game,
				name: "test",
				attackDiceQuantity: 3,
				attackSkill: Skill.Melee,
				createdByUserId: _user.Id);

			_weaponTemplate.AppliedConditions.Add(new AppliedCondition(Condition.Bleed, 100));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_user,
				_game,
				_weaponTemplate));
		}

		[TestMethod]
		public async Task Handle_GetWeaponTemplate_ShouldReturn_GetWeaponTemplateResponse()
		{
			var request = new GetWeaponTemplateQuery()
			{
				Name = "test",
				AttackSkillName = "Melee",
				DamageType = "Slashing",
				ConditionName = "Кровотечение",
				MinAttackDiceQuantity = 2,
				MaxAttackDiceQuantity = 3,
				UserName = "Author",
			};

			var newHandler = new GetWeaponTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count());

			var resultItem = result.First();

			var weaponTemplate = _dbContext.ItemTemplates
				.FirstOrDefault(x => x.Id == resultItem.Id) as WeaponTemplate;
			Assert.IsNotNull(weaponTemplate);
			Assert.IsTrue(weaponTemplate.Name.Contains(request.Name));
			Assert.IsTrue(weaponTemplate.AttackDiceQuantity >= request.MinAttackDiceQuantity
				&& weaponTemplate.AttackDiceQuantity <= request.MaxAttackDiceQuantity);


			Assert.AreEqual(weaponTemplate.AttackSkill, Skill.Melee);
			Assert.AreEqual(weaponTemplate.DamageType, DamageType.Slashing);
			Assert.IsTrue(weaponTemplate.AppliedConditions.Any(x => x.Condition == Condition.Bleed));

			var user = _dbContext.Users.FirstOrDefault(x => x.Id == weaponTemplate.CreatedByUserId);
			Assert.IsNotNull(user);
			Assert.IsTrue(user.Name.Contains(request.UserName));
		}
	}
}
