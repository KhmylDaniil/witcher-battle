using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.ChangeCreatureTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.ChangeCreatureTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.UnitTest.Core.Requests.CreatureTemplatesRequests
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
		private readonly BodyTemplate _bodyTemplate;
		private readonly BodyTemplatePart _torso;
		private readonly Condition _condition;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly CreatureTemplatePart _creatureTemplatePart;
		private readonly CreatureTemplateSkill _creatureTemplateSkill;
		private readonly Ability _ability1;
		private readonly Ability _ability2;
		private readonly DamageType _damageType;

		/// <summary>
		/// Конструктор для теста <see cref="CreateCreatureTemlplateHandler"/>
		/// </summary>
		public ChangeCreatureTemplateHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_damageType = DamageType.CreateForTest();
			_imgFile = ImgFile.CreateForTest();
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

			_condition = Condition.CreateForTest();

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

			_ability1 = Ability.CreateForTest(game: _game, attackSkill: Skill.Melee, damageType: _damageType);
			_creatureTemplate.Abilities.Add(_ability1);

			_ability2 = Ability.CreateForTest(game: _game, attackSkill: Skill.Staff, damageType: _damageType);

			_creatureTemplateSkill = CreatureTemplateSkill.CreateForTest(
				creatureTemplate: _creatureTemplate,
				skill: Skill.Melee,
				value: 6);

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_imgFile,
				_bodyTemplate,
				_torso,
				_ability1,
				_ability2,
				_creatureTemplate,
				_creatureTemplateSkill,
				_condition));
		}

		/// <summary>
		/// Тест метода Handle - изменение шаблона существа
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ChangeCreatureTemplate_ShouldReturnUnit()
		{
			var request = new ChangeCreatureTemplateCommand(
				id: _creatureTemplate.Id,
				gameId: _game.Id,
				imgFileId: _imgFile.Id,
				bodyTemplateId: _bodyTemplate.Id,
				name: "newCT",
				description: "description",
				creatureType: CreatureType.Cursed,
				hp: 10,
				sta: 10,
				@int: 6,
				@ref: 7,
				dex: 8,
				body: 8,
				emp: 1,
				cra: 2,
				will: 5,
				speed: 7,
				luck: 1,
				armorList: new List<ChangeCreatureTemplateRequestArmorList>
				{
					new ChangeCreatureTemplateRequestArmorList()
					{
						BodyTemplatePartId = _torso.Id,
						Armor = 4
					}
				},
				abilities: new List<Guid> { _ability2.Id },
				creatureTemplateSkills: new List<ChangeCreatureTemplateRequestSkill>
				{
					new ChangeCreatureTemplateRequestSkill()
					{
						Id = _creatureTemplateSkill.Id,
						Skill = Skill.Melee,
						Value = 9
					},
					new ChangeCreatureTemplateRequestSkill()
					{
						Skill = Skill.Staff,
						Value = 3
					}
				});

			var newHandler = new ChangeCreatureTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var creatureTemplate = _dbContext.CreatureTemplates.FirstOrDefault(x => x.Id == request.Id);
			Assert.IsNotNull(creatureTemplate);
			Assert.AreEqual(_dbContext.CreatureTemplates.Count(), 1);

			Assert.AreEqual(request.GameId, creatureTemplate.GameId);
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
			Assert.AreEqual(creatureTemplate.Abilities.Count(), 1);
			var ability = creatureTemplate.Abilities.FirstOrDefault(x => x.Id == _ability2.Id);
			Assert.IsNotNull(ability);

			Assert.IsNotNull(creatureTemplate.CreatureTemplateSkills);
			Assert.AreEqual(creatureTemplate.CreatureTemplateSkills.Count(), 2);
			var creatureTemplateParameter1 = creatureTemplate.CreatureTemplateSkills
				.FirstOrDefault(x => x.Skill == Skill.Melee);
			Assert.IsTrue(creatureTemplateParameter1.SkillValue == 9);

			var creatureTemplateParameter2 = creatureTemplate.CreatureTemplateSkills
				.FirstOrDefault(x => x.Skill == Skill.Staff);
			Assert.IsTrue(creatureTemplateParameter2.SkillValue == 3);
		}
	}
}
