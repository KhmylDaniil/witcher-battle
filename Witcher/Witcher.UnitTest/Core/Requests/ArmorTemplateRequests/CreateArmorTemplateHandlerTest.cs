using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.ArmorTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.ArmorTemplateRequests;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.UnitTest.Core.Requests.ArmorTemplateRequests
{
	[TestClass]
	public class CreateArmorTemplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly BodyTemplate _bodyTemplate;
		private readonly BodyTemplatePart _bodyTemplatePartHead;
		private readonly BodyTemplatePart _bodyTemplatePartTorso;
		private readonly BodyTemplatePart _bodyTemplatePartLarm;
		private readonly BodyTemplatePart _bodyTemplatePartRarm;
		private readonly BodyTemplatePart _bodyTemplatePartLleg;
		private readonly BodyTemplatePart _bodyTemplatePartRleg;

		public CreateArmorTemplateHandlerTest() : base()
		{
			_game = Game.CreateForTest();

			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);

			_bodyTemplatePartHead = BodyTemplatePart.CreateForTest(
				bodyTemplate: _bodyTemplate,
				bodyPartType: BodyPartType.Head,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 1,
				maxToHit: 1);

			_bodyTemplatePartTorso = BodyTemplatePart.CreateForTest(
				bodyTemplate: _bodyTemplate,
				bodyPartType: BodyPartType.Torso,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 2,
				maxToHit: 4);

			_bodyTemplatePartLarm = BodyTemplatePart.CreateForTest(
				bodyTemplate: _bodyTemplate,
				bodyPartType: BodyPartType.Arm,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 5,
				maxToHit: 5);

			_bodyTemplatePartRarm = BodyTemplatePart.CreateForTest(
				bodyTemplate: _bodyTemplate,
				bodyPartType: BodyPartType.Arm,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 6,
				maxToHit: 6);

			_bodyTemplatePartLleg = BodyTemplatePart.CreateForTest(
				bodyTemplate: _bodyTemplate,
				bodyPartType: BodyPartType.Leg,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 7,
				maxToHit: 8);

			_bodyTemplatePartRleg = BodyTemplatePart.CreateForTest(
				bodyTemplate: _bodyTemplate,
				bodyPartType: BodyPartType.Leg,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 9,
				maxToHit: 10);

			_bodyTemplate.BodyTemplateParts.AddRange( new List<BodyTemplatePart>
			{
				_bodyTemplatePartHead, _bodyTemplatePartLarm, _bodyTemplatePartRarm, _bodyTemplatePartLleg, _bodyTemplatePartRleg, _bodyTemplatePartTorso 
			});

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_bodyTemplate,
				_bodyTemplatePartHead,
				_bodyTemplatePartLarm,
				_bodyTemplatePartRarm,
				_bodyTemplatePartLleg,
				_bodyTemplatePartRleg,
				_bodyTemplatePartTorso));
		}

		[TestMethod]
		public async Task Handle_CreateArmorTemplate_ShouldReturnUnit()
		{
			var request = new CreateArmorTemplateCommand
			{
				Name = "name",
				Description = "description",
				Price = 100,
				Weight = 5,
				Armor = 5,
				EncumbranceValue = 1,
				BodyTemplateId = _bodyTemplate.Id,
				BodyPartTypes = new List<BodyPartType> { BodyPartType.Head, BodyPartType.Torso, BodyPartType.Arm},
			};

			var newHandler = new CreateArmorTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.ItemTemplates.Count());
			var armorTemplate = _dbContext.ItemTemplates.FirstOrDefault(x => x.Id == result) as ArmorTemplate;

			Assert.AreEqual(_game.Id, armorTemplate.GameId);
			Assert.IsNotNull(armorTemplate.Name);
			Assert.AreEqual(request.Name, armorTemplate.Name);
			Assert.AreEqual(request.Description, armorTemplate.Description);
			Assert.AreEqual(request.Price, armorTemplate.Price);
			Assert.AreEqual(request.Weight, armorTemplate.Weight);
			Assert.AreEqual(request.Armor, armorTemplate.Armor);
			Assert.AreEqual(request.EncumbranceValue, armorTemplate.EncumbranceValue);

			Assert.AreEqual(request.BodyTemplateId, armorTemplate.BodyTemplateId);
			var bodyTemplateParts = armorTemplate.BodyTemplateParts;
			Assert.IsNotNull(bodyTemplateParts);
			Assert.AreEqual(4, bodyTemplateParts.Count);
			Assert.IsTrue(bodyTemplateParts.All(x => request.BodyPartTypes.Contains(x.BodyPartType)));
		}
	}
}
