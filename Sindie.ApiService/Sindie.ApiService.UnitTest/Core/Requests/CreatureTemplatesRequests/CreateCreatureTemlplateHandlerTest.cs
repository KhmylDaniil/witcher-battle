using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.CreateCreatureTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.CreateCreatureTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.CreatureTemplatesRequests
{
	/// <summary>
	/// Тест для <see cref="CreateCreatureTemlplateHandler"/>
	/// </summary>
	[TestClass]
	public class CreateCreatureTemlplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly ImgFile _imgFile;
		private readonly BodyTemplate _bodyTemplate;
		private readonly BodyTemplatePart _bodyTemplatePart;
		private readonly Condition _condition;
		private readonly Parameter _parameter;
		private readonly CreatureType _creatureType;

		/// <summary>
		/// Конструктор для теста <see cref="CreateCreatureTemlplateHandler"/>
		/// </summary>
		public CreateCreatureTemlplateHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_imgFile = ImgFile.CreateForTest();
			_creatureType = CreatureType.CreateForTest();
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_bodyTemplatePart = BodyTemplatePart.CreateForTest(
				bodyTemplate: _bodyTemplate,
				bodyPartType: BodyPartType.CreateForTest(),
				name: "void",
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 1,
				maxToHit: 10);
			_bodyTemplate.BodyTemplateParts = new List<BodyTemplatePart> { _bodyTemplatePart};

			_condition = Condition.CreateForTest(game: _game);
			_parameter = Parameter.CreateForTest(game: _game);
			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _imgFile, _parameter, _bodyTemplate, _bodyTemplatePart, _condition, _creatureType));
		}

		/// <summary>
		/// Тест метода Handle - создание шаблона существа
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_CreateCreatureTemplate_ShouldReturnUnit()
		{
			var request = new CreateCreatureTemplateCommand(
				gameId: _game.Id,
				imgFileId: _imgFile.Id,
				bodyTemplateId: _bodyTemplate.Id,
				name: "name",
				description: "description",
				creatureTypeId: _creatureType.Id,
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
				armorList: new List<CreateCreatureTemplateRequestArmorList>
				{
					new CreateCreatureTemplateRequestArmorList()
					{
						BodyTemplatePartId = _bodyTemplatePart.Id,
						Armor = 4
					}
				},
				abilities: new List<CreateCreatureTemplateRequestAbility>
				{
					new CreateCreatureTemplateRequestAbility()
					{
						Name = "attack",
						Description = "bite",
						AttackParameterId = _parameter.Id,
						AttackDiceQuantity = 2,
						DamageModifier = 4,
						AttackSpeed = 1,
						Accuracy = -1,
						AppliedConditions = new List<CreateCreatureTemplateRequestAppliedCondition>
						{
							new CreateCreatureTemplateRequestAppliedCondition()
							{
								ConditionId = _condition.Id,
								ApplyChance = 50
							}
						}
					}
				},
				creatureTemplateParameters: new List<CreateCreatureTemplateRequestParameter>
				{
					new CreateCreatureTemplateRequestParameter()
					{
						ParameterId = _parameter.Id,
						Value = 5
					}
				});

			var newHandler = new CreateCreatureTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			var creatureTemplate = _dbContext.CreatureTemplates.FirstOrDefault();
			Assert.IsNotNull(creatureTemplate);
			Assert.AreEqual(_dbContext.CreatureTemplates.Count(), 1);

			Assert.AreEqual(request.GameId, creatureTemplate.GameId);
			Assert.IsNotNull(creatureTemplate.Name);
			Assert.AreEqual(request.Name, creatureTemplate.Name);
			Assert.AreEqual(request.Description, creatureTemplate.Description);
			Assert.AreEqual(request.ImgFileId, creatureTemplate.ImgFileId);
			Assert.AreEqual(request.BodyTemplateId, creatureTemplate.BodyTemplateId);
			Assert.AreEqual(request.CreatureTypeId, creatureTemplate.CreatureTypeId);
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
			Assert.AreEqual(creatureTemplatePart.Name, "void");
			Assert.AreEqual(creatureTemplatePart.HitPenalty, 1);
			Assert.AreEqual(creatureTemplatePart.DamageModifier, 1);
			Assert.AreEqual(creatureTemplatePart.MaxToHit, 10);
			Assert.AreEqual(creatureTemplatePart.MinToHit, 1);
			Assert.AreEqual(creatureTemplatePart.Armor, 4);

			Assert.IsNotNull(creatureTemplate.Abilities);
			Assert.AreEqual(creatureTemplate.Abilities.Count(), 1);
			var ability = _dbContext.Abilities.FirstOrDefault();

			Assert.AreEqual(ability.Name, "attack");
			Assert.AreEqual(ability.Description, "bite");
			Assert.AreEqual(ability.Accuracy, -1);
			Assert.AreEqual(ability.AttackSpeed, 1);
			Assert.AreEqual(ability.AttackDiceQuantity, 2);
			Assert.AreEqual(ability.AttackParameterId, _parameter.Id);
			Assert.AreEqual(ability.DamageModifier, 4);

			Assert.IsNotNull(ability.AppliedConditions);
			Assert.AreEqual(ability.AppliedConditions.Count(), 1);
			var appliedCondition = ability.AppliedConditions.FirstOrDefault();

			Assert.AreEqual(appliedCondition.ApplyChance, 50);
			Assert.AreEqual(appliedCondition.ConditionId, _condition.Id);

			Assert.IsNotNull(creatureTemplate.CreatureTemplateParameters);
			Assert.AreEqual(creatureTemplate.CreatureTemplateParameters.Count(), 1);
			var creatureTemplateParameter = _dbContext.CreatureTemplateParameters
				.FirstOrDefault(x => x.CreatureTemplateId == creatureTemplate.Id);

			Assert.IsTrue(creatureTemplateParameter.ParameterId == _parameter.Id);
			Assert.IsTrue(creatureTemplateParameter.ParameterValue == 5);
		}
	}
}
