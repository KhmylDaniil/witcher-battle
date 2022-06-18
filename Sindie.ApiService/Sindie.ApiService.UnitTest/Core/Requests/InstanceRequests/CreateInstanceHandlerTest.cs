using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.InstanceRequests.CreateInstance;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.InstanceRequests.CreateInstance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.InstanceRequests
{
	/// <summary>
	/// Тест для <see cref="CreateInstanceHandler"/>
	/// </summary>
	[TestClass]
	public class CreateInstanceHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly ImgFile _imgFile;
		private readonly BodyTemplate _bodyTemplate;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly Ability _ability;
		private readonly Condition _condition;
		private readonly Parameter _parameter;

		/// <summary>
		/// Конструктор для теста <see cref="CreateInstanceHandler"/>
		/// </summary>
		public CreateInstanceHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_imgFile = ImgFile.CreateForTest();
			_condition = Condition.CreateForTest(game: _game);
			_parameter = Parameter.CreateForTest(game: _game);
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_creatureTemplate = CreatureTemplate.CreateForTest(game: _game, bodyTemplate: _bodyTemplate);
			_creatureTemplate.CreatureTemplateParameters
				.Add(CreatureTemplateParameter.CreateForTest(
					creatureTemplate: _creatureTemplate,
					parameter: _parameter,
					value: 5));
			_ability = Ability.CreateForTest(
				creatureTemplate: _creatureTemplate,
				name: "attack",
				description: "bite",
				attackDiceQuantity: 2,
				damageModifier: 4,
				accuracy: -1,
				attackSpeed: 1,
				attackParameter: _parameter);
			_ability.AppliedConditions.Add(new AppliedCondition(_ability, _condition, 50));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_imgFile,
				_condition,
				_parameter,
				_bodyTemplate,
				_creatureTemplate,
				_ability));
		}

		/// <summary>
		/// Тест метода Handle - создание инстанса
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_CreateInstance_ShouldReturnUnit()
		{
			var request = new CreateInstanceCommand(
				gameId: _game.Id,
				imgFileId: _imgFile.Id,
				name: "instance",
				description: "description",
				creatures: new List<CreateInstanceRequestItem>()
				{
					new CreateInstanceRequestItem()
					{
						CreatureTemplateId = _creatureTemplate.Id,
						Name = "monster1",
						Description = "newMonster"
					}
				});

			var newHandler = new CreateInstanceHandler(_dbContext, AuthorizationService.Object);

			var result = newHandler.Handle(request, default);
			
			Assert.IsNotNull(result);
			var instance = _dbContext.Instances.FirstOrDefault();
			Assert.IsNotNull(instance);
			Assert.IsTrue(_dbContext.Instances.Count() == 1);

			Assert.AreEqual(instance.Name, "instance");
			Assert.AreEqual(instance.Description, "description");
			Assert.AreEqual(instance.ImgFileId, _imgFile.Id);
			Assert.AreEqual(instance.GameId, _game.Id);

			Assert.IsNotNull(instance.Creatures);
			Assert.IsTrue(instance.Creatures.Count() == 1);
			var creature = instance.Creatures.FirstOrDefault();
			
			Assert.IsNotNull(creature);
			Assert.IsNotNull(creature.Name);
			Assert.AreEqual(creature.Name, "monster1");
			Assert.AreEqual(creature.Description, "newMonster");
			Assert.AreEqual(_creatureTemplate.ImgFileId, creature.ImgFileId);
			Assert.AreEqual(_creatureTemplate.BodyTemplateId, creature.BodyTemplateId);
			Assert.IsNotNull(creature.Type);
			Assert.AreEqual(_creatureTemplate.Type, creature.Type);
			Assert.AreEqual(_creatureTemplate.HP, creature.HP);
			Assert.AreEqual(_creatureTemplate.Sta, creature.Sta);
			Assert.AreEqual(_creatureTemplate.Int, creature.Int);
			Assert.AreEqual(_creatureTemplate.Ref, creature.Ref);
			Assert.AreEqual(_creatureTemplate.Dex, creature.Dex);
			Assert.AreEqual(_creatureTemplate.Body, creature.Body);
			Assert.AreEqual(_creatureTemplate.Emp, creature.Emp);
			Assert.AreEqual(_creatureTemplate.Cra, creature.Cra);
			Assert.AreEqual(_creatureTemplate.Will, creature.Will);
			Assert.AreEqual(_creatureTemplate.Speed, creature.Speed);
			Assert.AreEqual(_creatureTemplate.Luck, creature.Luck);

			Assert.IsNotNull(creature.BodyParts);
			var bodyPart = creature.BodyParts.FirstOrDefault();
			Assert.IsNotNull(bodyPart);
			Assert.AreEqual(creature.BodyParts.Count, 1);
			Assert.AreEqual(bodyPart.Name, "torso");
			Assert.AreEqual(bodyPart.HitPenalty, 1);
			Assert.AreEqual(bodyPart.DamageModifier, 1);
			Assert.AreEqual(bodyPart.MaxToHit, 10);
			Assert.AreEqual(bodyPart.MinToHit, 1);
			Assert.AreEqual(bodyPart.CurrentArmor, 5);
			Assert.AreEqual(bodyPart.StartingArmor, 5);

			Assert.IsNotNull(creature.Abilities);
			Assert.AreEqual(creature.Abilities.Count(), 1);
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

			Assert.IsNotNull(creature.CreatureParameters);
			Assert.AreEqual(creature.CreatureParameters.Count(), 1);
			var creatureParameter = _dbContext.CreatureParameters
				.FirstOrDefault(x => x.CreatureId == creature.Id);

			Assert.IsTrue(creatureParameter.ParameterId == _parameter.Id);
			Assert.IsTrue(creatureParameter.ParameterValue == 5);
		}
	}
}
