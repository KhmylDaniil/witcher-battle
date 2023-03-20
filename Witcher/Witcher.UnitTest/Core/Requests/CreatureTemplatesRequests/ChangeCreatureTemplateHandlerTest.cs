using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Drafts.BodyTemplateDrafts;
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
	/// Тест для <see cref="ChangeCreatureTemplateHandler"/>
	/// </summary>
	[TestClass]
	public class ChangeCreatureTemplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly ImgFile _imgFile;
		private readonly BodyTemplate _humanBodyTemplate;
		private readonly BodyTemplate _bodyTemplate;
		private readonly BodyTemplatePart _torso;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly CreatureTemplatePart _creatureTemplatePart;
		private readonly CreatureTemplateSkill _creatureTemplateSkill;
		private readonly Ability _ability1;
		private readonly Ability _ability2;

		/// <summary>
		/// Конструктор для теста <see cref="CreateCreatureTemlplateHandler"/>
		/// </summary>
		public ChangeCreatureTemplateHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_imgFile = ImgFile.CreateForTest();

			_humanBodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_humanBodyTemplate.CreateBodyTemplateParts(CreateBodyTemplatePartsDraft.CreateBodyPartsDraft());
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);

			_torso = BodyTemplatePart.CreateForTest(
				bodyTemplate: _bodyTemplate,
				bodyPartType: BodyPartType.Torso,
				name: "torso1",
				hitPenalty: 2,
				damageModifier: 2,
				minToHit: 1,
				maxToHit: 10);
			_bodyTemplate.BodyTemplateParts = new List<BodyTemplatePart> { _torso };

			_creatureTemplate = CreatureTemplate.CreateForTest(
				game: _game,
				bodyTemplate: _bodyTemplate,
				creatureType: CreatureType.Beast);

			_creatureTemplatePart = CreatureTemplatePart.CreateForTest(
				creatureTemplate: _creatureTemplate,
				bodyPartType: BodyPartType.Torso,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 1,
				maxToHit: 10,
				armor: 0);
			_creatureTemplate.CreatureTemplateParts.Add(_creatureTemplatePart);

			_ability1 = Ability.CreateForTest(game: _game, attackSkill: Skill.Melee);
			_creatureTemplate.Abilities.Add(_ability1);

			_ability2 = Ability.CreateForTest(game: _game, attackSkill: Skill.Staff, damageType: DamageType.Piercing);

			_creatureTemplateSkill = CreatureTemplateSkill.CreateForTest(
				creatureTemplate: _creatureTemplate,
				skill: Skill.Melee,
				value: 6);

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_imgFile,
				_humanBodyTemplate,
				_bodyTemplate,
				_torso,
				_ability1,
				_ability2,
				_creatureTemplate,
				_creatureTemplateSkill));
		}

		/// <summary>
		/// Тест метода Handle - изменение шаблона существа
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ChangeCreatureTemplate_ShouldReturnUnit()
		{
			var request = new ChangeCreatureTemplateCommand()
			{
				Id = _creatureTemplate.Id,
				ImgFileId = _imgFile.Id,
				BodyTemplateId = _bodyTemplate.Id,
				Name = "newCT",
				Description = "description",
				CreatureType = CreatureType.Cursed,
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
						BodyTemplatePartId = _torso.Id,
						Armor = 4
					}
				},
				Abilities = new List<Guid> { _ability2.Id },
				CreatureTemplateSkills = new List<UpdateCreatureTemplateRequestSkill>
				{
					new UpdateCreatureTemplateRequestSkill()
					{
						Id = _creatureTemplateSkill.Id,
						Skill = Skill.Melee,
						Value = 9
					},
					new UpdateCreatureTemplateRequestSkill()
					{
						Skill = Skill.Staff,
						Value = 3
					}
				}
		};
			
			var newHandler = new ChangeCreatureTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var creatureTemplate = _dbContext.CreatureTemplates.FirstOrDefault(x => x.Id == request.Id);
			Assert.IsNotNull(creatureTemplate);
			Assert.IsTrue(_dbContext.CreatureTemplates.Count() == 1);

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
			Assert.AreEqual(creatureTemplate.CreatureTemplateParts.Count, 1);
			Assert.AreEqual(creatureTemplatePart.Name, "torso1");
			Assert.AreEqual(creatureTemplatePart.HitPenalty, 2);
			Assert.AreEqual(creatureTemplatePart.DamageModifier, 2);
			Assert.AreEqual(creatureTemplatePart.MaxToHit, 10);
			Assert.AreEqual(creatureTemplatePart.MinToHit, 1);
			Assert.AreEqual(creatureTemplatePart.Armor, 4);

			Assert.IsNotNull(creatureTemplate.Abilities);
			Assert.IsTrue(creatureTemplate.Abilities.Count == 1);
			var ability = creatureTemplate.Abilities.FirstOrDefault(x => x.Id == _ability2.Id);
			Assert.IsNotNull(ability);

			Assert.IsNotNull(creatureTemplate.CreatureTemplateSkills);
			Assert.IsTrue(creatureTemplate.CreatureTemplateSkills.Count == 2);
			var creatureTemplateParameter1 = creatureTemplate.CreatureTemplateSkills
				.FirstOrDefault(x => x.Skill == Skill.Melee);
			Assert.IsTrue(creatureTemplateParameter1.SkillValue == 9);

			var creatureTemplateParameter2 = creatureTemplate.CreatureTemplateSkills
				.FirstOrDefault(x => x.Skill == Skill.Staff);
			Assert.IsTrue(creatureTemplateParameter2.SkillValue == 3);
		}

		/// <summary>
		/// Тест метода Handle - изменение шаблона тела меняет части шаблона существа и обнуляет броню
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ChangeCreatureTemplateWithBodyTemplateChanging_ShouldReturnUnit()
		{
			var request = new ChangeCreatureTemplateCommand()
			{
				Id = _creatureTemplate.Id,
				BodyTemplateId = _humanBodyTemplate.Id,
				Name = "newCT",
				Description = "description",
				CreatureType = CreatureType.Cursed,
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
			};

			var newHandler = new ChangeCreatureTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var creatureTemplate = _dbContext.CreatureTemplates.FirstOrDefault(x => x.Id == request.Id);
			Assert.IsNotNull(creatureTemplate);
			Assert.IsTrue(_dbContext.CreatureTemplates.Count() == 1);

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
			Assert.AreEqual(6, creatureTemplate.CreatureTemplateParts.Count);

			var head = creatureTemplate.CreatureTemplateParts.FirstOrDefault(x => x.BodyPartType == BodyPartType.Head);
			Assert.IsNotNull(head);
			Assert.AreEqual("Head", head.Name);
			Assert.AreEqual(4, head.HitPenalty);
			Assert.AreEqual(3, head.DamageModifier);
			Assert.AreEqual(1, head.MaxToHit);
			Assert.AreEqual(1, head.MinToHit);
			Assert.AreEqual(0, head.Armor);
		}
	}
}
