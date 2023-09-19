using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Entities;
using Witcher.Core.Contracts.ArmorTemplateRequests;
using Witcher.Core.Requests.ArmorTemplateRequests;

namespace Witcher.UnitTest.Core.Requests.ArmorTemplateRequests
{
	[TestClass]
	public sealed class GetArmorTemplateByIdHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly User _user;
		private readonly Game _game;
		private readonly BodyTemplate _bodyTemplate;
		private readonly BodyTemplatePart _bodyTemplatePart;
		private readonly ArmorTemplate _armorTemplate;

		public GetArmorTemplateByIdHandlerTest() : base()
		{
			_user = User.CreateForTest(
				id: UserContext.Object.CurrentUserId,
				name: "Author");

			_game = Game.CreateForTest();
			_game.UserGames.Add(
				UserGame.CreateForTest(
					user: _user,
					gameRole: GameRole.CreateForTest(GameRoles.MasterRoleId)));

			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);

			_bodyTemplatePart = BodyTemplatePart.CreateForTest(
				bodyTemplate: _bodyTemplate,
				bodyPartType: BodyPartType.Torso,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 1,
				maxToHit: 10);

			_bodyTemplate.BodyTemplateParts.Add(_bodyTemplatePart);

			_armorTemplate = ArmorTemplate.CreateForTest(
				game: _game,
				bodyTemplate: _bodyTemplate,
				name: "test",
				armor: 3,
				createdByUserId: _user.Id);

			_armorTemplate.BodyTemplateParts.Add(_bodyTemplatePart);
			_armorTemplate.DamageTypeModifiers.Add(new EntityDamageTypeModifier(_armorTemplate.Id, DamageType.Slashing, DamageTypeModifier.Resistance));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_user,
				_game,
				_bodyTemplate,
				_armorTemplate,
				_bodyTemplatePart));
		}

		[TestMethod]
		public async Task Handle_GetArmorTemplateById_ShouldReturn_GetArmorTemplateByIdResponse()
		{
			var request = new GetArmorTemplateByIdQuery() { Id = _armorTemplate.Id };

			var newHandler = new GetArmorTemplateByIdHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(result.Name, _armorTemplate.Name);
			Assert.AreEqual(result.Description, _armorTemplate.Description);
			Assert.IsFalse(result.IsStackable);
			Assert.AreEqual(result.Price, _armorTemplate.Price);
			Assert.AreEqual(result.Weight, _armorTemplate.Weight);
			Assert.AreEqual(result.Armor, _armorTemplate.Armor);
			Assert.AreEqual(result.EncumbranceValue, _armorTemplate.EncumbranceValue);
			Assert.AreEqual(result.BodyTemplateName, _bodyTemplate.Name);

			Assert.IsNotNull(result.BodyTemplatePartsNames);
			Assert.IsTrue(result.BodyTemplatePartsNames.Count == 1);
			Assert.AreEqual("Torso", result.BodyTemplatePartsNames[0]);

			Assert.IsNotNull(result.DamageTypeModifiers);
			var damageTypeModifier = result.DamageTypeModifiers[0];
			Assert.AreEqual(DamageType.Slashing, damageTypeModifier.DamageType);
		}
	}
}
