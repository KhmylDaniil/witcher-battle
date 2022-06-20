using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.ChangeCreatureTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.ChangeCreatureTemplate;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
		private readonly Parameter _parameter1;
		private readonly Parameter _parameter2;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly CreatureTemplatePart _creatureTemplatePart;
		private readonly CreatureTemplateParameter _creatureTemplateParameter;
		private readonly Ability _ability;
		private readonly BodyPartType _torsoType;

		/// <summary>
		/// Конструктор для теста <see cref="CreateCreatureTemlplateHandler"/>
		/// </summary>
		public ChangeCreatureTemplateHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_torsoType = BodyPartType.CreateForTest(BodyPartTypes.TorsoId, BodyPartTypes.TorsoName);
			_imgFile = ImgFile.CreateForTest();
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);

			_torso = BodyTemplatePart.CreateForTest(
				bodyTemplate: _bodyTemplate,
				bodyPartType: _torsoType,
				name: "torso1",
				hitPenalty: 2,
				damageModifier: 2,
				minToHit: 1,
				maxToHit: 10);
			_bodyTemplate.BodyTemplateParts = new List<BodyTemplatePart> { _torso };

			_condition = Condition.CreateForTest(game: _game);
			_parameter1 = Parameter.CreateForTest(game: _game);
			_parameter2 = Parameter.CreateForTest(game: _game);

			_creatureTemplate = CreatureTemplate.CreateForTest(
				game: _game,
				bodyTemplate: _bodyTemplate);

			_creatureTemplatePart = CreatureTemplatePart.CreateForTest(
				creatureTemplate: _creatureTemplate,
				bodyPartType: _torsoType,
				name: "torso",
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 1,
				maxToHit: 10,
				armor: 0);
			_creatureTemplate.CreatureTemplateParts.Add(_creatureTemplatePart);

			_ability = Ability.CreateForTest(
				creatureTemplate: _creatureTemplate,
				name: "attack",
				attackDiceQuantity: 1,
				damageModifier: 1,
				attackSpeed: 1,
				accuracy: 1,
				attackParameter: _parameter1);

			_creatureTemplateParameter = CreatureTemplateParameter.CreateForTest(
				creatureTemplate: _creatureTemplate,
				parameter: _parameter1,
				value: 6);

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_imgFile,
				_parameter1,
				_parameter2,
				_bodyTemplate,
				_torso,
				_torsoType,
				_ability,
				_creatureTemplate,
				_creatureTemplateParameter,
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
				type: "newType",
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
				abilities: new List<ChangeCreatureTemplateRequestAbility>
				{
					new ChangeCreatureTemplateRequestAbility()
					{
						Id = _ability.Id,
						Name = "attack2",
						Description = "bite",
						AttackParameterId = _parameter2.Id,
						AttackDiceQuantity = 2,
						DamageModifier = 4,
						AttackSpeed = 1,
						Accuracy = -1,
						AppliedConditions = new List<ChangeCreatureTemplateRequestAppliedCondition>
						{
							new ChangeCreatureTemplateRequestAppliedCondition()
							{
								ConditionId = _condition.Id,
								ApplyChance = 50
							}
						}
					}
				},
				creatureTemplateParameters: new List<ChangeCreatureTemplateRequestParameter>
				{
					new ChangeCreatureTemplateRequestParameter()
					{
						Id = _creatureTemplateParameter.Id,
						ParameterId = _parameter1.Id,
						Value = 9
					},
					new ChangeCreatureTemplateRequestParameter()
					{
						ParameterId = _parameter2.Id,
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
			Assert.IsNotNull(creatureTemplate.Type);
			Assert.AreEqual(request.Type, creatureTemplate.Type);
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
			var ability = creatureTemplate.Abilities.FirstOrDefault(x => x.Id == _ability.Id);
			Assert.IsNotNull(ability);

			Assert.AreEqual(ability.Name, "attack2");
			Assert.AreEqual(ability.Description, "bite");
			Assert.AreEqual(ability.Accuracy, -1);
			Assert.AreEqual(ability.AttackSpeed, 1);
			Assert.AreEqual(ability.AttackDiceQuantity, 2);
			Assert.AreEqual(ability.AttackParameterId, _parameter2.Id);
			Assert.AreEqual(ability.DamageModifier, 4);

			Assert.IsNotNull(ability.AppliedConditions);
			Assert.AreEqual(ability.AppliedConditions.Count(), 1);
			var appliedCondition = ability.AppliedConditions.FirstOrDefault();

			Assert.AreEqual(appliedCondition.ApplyChance, 50);
			Assert.AreEqual(appliedCondition.ConditionId, _condition.Id);

			Assert.IsNotNull(creatureTemplate.CreatureTemplateParameters);
			Assert.AreEqual(creatureTemplate.CreatureTemplateParameters.Count(), 2);
			var creatureTemplateParameter1 = creatureTemplate.CreatureTemplateParameters
				.FirstOrDefault(x => x.ParameterId == _parameter1.Id);
			Assert.IsTrue(creatureTemplateParameter1.ParameterValue == 9);

			var creatureTemplateParameter2 = creatureTemplate.CreatureTemplateParameters
				.FirstOrDefault(x => x.ParameterId == _parameter2.Id);
			Assert.IsTrue(creatureTemplateParameter2.ParameterValue == 3);
		}
	}
}
