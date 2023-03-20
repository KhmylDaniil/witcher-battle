using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.CreatureTemplateRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.UnitTest.Core.Requests.CreatureTemplatesRequests
{
	/// <summary>
	/// Тест для <see cref="CreateCreatureTemlplateHandler"/>
	/// </summary>
	[TestClass]
	public class CreateCreatureTemplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly ImgFile _imgFile;
		private readonly BodyTemplate _bodyTemplate;
		private readonly BodyTemplatePart _bodyTemplatePart;
		private readonly Ability _ability;

		/// <summary>
		/// Конструктор для теста <see cref="CreateCreatureTemlplateHandler"/>
		/// </summary>
		public CreateCreatureTemplateHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_imgFile = ImgFile.CreateForTest();
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_bodyTemplatePart = BodyTemplatePart.CreateForTest(
				bodyTemplate: _bodyTemplate,
				bodyPartType: BodyPartType.Void,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 1,
				maxToHit: 10);
			_bodyTemplate.BodyTemplateParts = new List<BodyTemplatePart> { _bodyTemplatePart};

			_ability = Ability.CreateForTest(game: _game, attackSkill: Skill.Melee);
			
			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _imgFile, _bodyTemplate, _bodyTemplatePart, _ability));
		}

		/// <summary>
		/// Тест метода Handle - создание шаблона существа
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_CreateCreatureTemplate_ShouldReturnUnit()
		{
			var request = new CreateCreatureTemplateCommand()
			{
				ImgFileId = _imgFile.Id,
				BodyTemplateId = _bodyTemplate.Id,
				Name = "name",
				Description = "description",
				CreatureType = CreatureType.Human,
				HP = 10,
				Sta = 10,
				Int = 6,
				Ref = 7,
				Dex = 8,
				Body = 8,
				Emp = 1,
				Cra = 2,
				Will = 5,
				Speed = 7,
				Luck = 1,
				ArmorList = new List<UpdateCreatureTemplateRequestArmorList>
				{
					new UpdateCreatureTemplateRequestArmorList()
					{
						BodyTemplatePartId = _bodyTemplatePart.Id,
						Armor = 4
					}
				},
				Abilities = new List<Guid> { _ability.Id },
				CreatureTemplateSkills = new List<UpdateCreatureTemplateRequestSkill>
				{
					new UpdateCreatureTemplateRequestSkill()
					{
						Skill = Skill.Melee,
						Value = 5
					}
				}
			};

			var newHandler = new CreateCreatureTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			var creatureTemplate = _dbContext.CreatureTemplates.FirstOrDefault();
			Assert.IsNotNull(creatureTemplate);
			Assert.AreEqual(1, _dbContext.CreatureTemplates.Count());

			Assert.AreEqual(_game.Id, creatureTemplate.GameId);
			Assert.IsNotNull(creatureTemplate.Name);
			Assert.AreEqual(request.Name, creatureTemplate.Name);
			Assert.AreEqual(request.Description, creatureTemplate.Description);
			Assert.AreEqual(request.ImgFileId, creatureTemplate.ImgFileId);
			Assert.AreEqual(request.BodyTemplateId, creatureTemplate.BodyTemplateId);
			Assert.AreEqual(request.CreatureType, creatureTemplate.CreatureType);
			Assert.AreEqual(request.HP, creatureTemplate.HP);
			Assert.AreEqual(request.Sta, creatureTemplate.Sta);
			Assert.AreEqual(request.Int, creatureTemplate.Int);
			Assert.AreEqual(request.Ref, creatureTemplate.Ref);
			Assert.AreEqual(request.Dex, creatureTemplate.Dex);
			Assert.AreEqual(request.Body, creatureTemplate.Body);
			Assert.AreEqual(request.Emp, creatureTemplate.Emp);
			Assert.AreEqual(request.Cra, creatureTemplate.Cra);
			Assert.AreEqual(request.Will, creatureTemplate.Will);
			Assert.AreEqual(request.Speed, creatureTemplate.Speed);
			Assert.AreEqual(request.Luck, creatureTemplate.Luck);

			Assert.IsNotNull(creatureTemplate.CreatureTemplateParts);
			var creatureTemplatePart = creatureTemplate.CreatureTemplateParts.FirstOrDefault();
			Assert.IsNotNull(creatureTemplatePart);
			Assert.AreEqual(1, creatureTemplate.CreatureTemplateParts.Count);
			Assert.AreEqual("Void", creatureTemplatePart.Name);
			Assert.AreEqual(1, creatureTemplatePart.HitPenalty);
			Assert.AreEqual(1, creatureTemplatePart.DamageModifier);
			Assert.AreEqual(10, creatureTemplatePart.MaxToHit);
			Assert.AreEqual(1, creatureTemplatePart.MinToHit);
			Assert.AreEqual(4, creatureTemplatePart.Armor);

			Assert.IsNotNull(creatureTemplate.Abilities);
			Assert.AreEqual(1, creatureTemplate.Abilities.Count);
			var ability = _dbContext.Abilities.FirstOrDefault();
			Assert.AreEqual(ability.Id, _ability.Id);

			Assert.IsNotNull(creatureTemplate.CreatureTemplateSkills);
			Assert.AreEqual(1, creatureTemplate.CreatureTemplateSkills.Count);
			var creatureTemplateSkill = _dbContext.CreatureTemplateSkills
				.FirstOrDefault(x => x.CreatureTemplateId == creatureTemplate.Id);

			Assert.IsTrue(creatureTemplateSkill.Skill == Skill.Melee);
			Assert.IsTrue(creatureTemplateSkill.SkillValue == 5);
		}
	}
}
