using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.WeaponTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.WeaponTemplateRequests;
using Witcher.Core.Contracts.ArmorTemplateRequests;
using Witcher.Core.Requests.ArmorTemplateRequests;

namespace Witcher.UnitTest.Core.Requests.ArmorTemplateRequests
{
	[TestClass]
	public class GetArmorTemplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly User _user;
		private readonly Game _game;
		private readonly BodyTemplate _bodyTemplate;
		private readonly BodyTemplatePart _bodyTemplatePart;
		private readonly ArmorTemplate _armorTemplate;

		public GetArmorTemplateHandlerTest() : base()
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
		public async Task Handle_GetArmorTemplate_ShouldReturn_GetArmorTemplateResponse()
		{
			var request = new GetArmorTemplateQuery()
			{
				Name = "test",
				MinArmor = 3,
				MaxArmor = 3,
				BodyPartType = "orso",
				DamageTypeModifier = "Slash",
				UserName = "Author",
			};

			var newHandler = new GetArmorTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count());

			var resultItem = result.First();

			var armorTemplate = _dbContext.ItemTemplates.Where(x => x.ItemType == ItemType.Armor)
				.FirstOrDefault(x => x.Id == resultItem.Id) as ArmorTemplate;
			Assert.IsNotNull(armorTemplate);
			Assert.IsTrue(armorTemplate.Name.Contains(request.Name));
			Assert.IsTrue(armorTemplate.Armor >= request.MinArmor
				&& armorTemplate.Armor <= request.MaxArmor);

			var bodyTemplatePart = _dbContext.BodyTemplateParts.FirstOrDefault(x => x.BodyTemplateId == armorTemplate.BodyTemplateId
			&& x.BodyPartType == BodyPartType.Torso);
			Assert.IsNotNull(bodyTemplatePart);

			var damageTypeModifier = armorTemplate.DamageTypeModifiers.FirstOrDefault(x => x.DamageType == DamageType.Slashing);
			Assert.IsNotNull(damageTypeModifier);

			var user = _dbContext.Users.FirstOrDefault(x => x.Id == armorTemplate.CreatedByUserId);
			Assert.IsNotNull(user);
			Assert.IsTrue(user.Name.Contains(request.UserName));
		}
	}
}
