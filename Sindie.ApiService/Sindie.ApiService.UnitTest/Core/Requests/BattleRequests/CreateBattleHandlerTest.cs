using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests.CreateBattle;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BattleRequests.CreateBattle;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.BattleRequests
{
	/// <summary>
	/// Тест для <see cref="CreateBattleHandler"/>
	/// </summary>
	[TestClass]
	public class CreateBattleHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly ImgFile _imgFile;
		private readonly BodyTemplate _bodyTemplate;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly CreatureTemplatePart _creatureTemplatePart;
		private readonly BodyPartType _bodyPartType;
		private readonly Ability _ability;
		private readonly Condition _condition;
		private readonly Skill _parameter;
		private readonly CreatureType _creatureType;

		/// <summary>
		/// Конструктор для теста <see cref="CreateBattleHandler"/>
		/// </summary>
		public CreateBattleHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_imgFile = ImgFile.CreateForTest();
			_bodyPartType = BodyPartType.CreateForTest();
			_creatureType = CreatureType.CreateForTest();
			_condition = Condition.CreateForTest();
			_parameter = Skill.CreateForTest();
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_creatureTemplate = CreatureTemplate.CreateForTest(game: _game, bodyTemplate: _bodyTemplate, creatureType: _creatureType);
			_creatureTemplatePart = CreatureTemplatePart.CreateForTest(
				creatureTemplate: _creatureTemplate,
				bodyPartType: _bodyPartType,
				name: "torso",
				armor: 5);
			_creatureTemplate.CreatureTemplateParts.Add(_creatureTemplatePart);

			_creatureTemplate.CreatureTemplateSkills
				.Add(CreatureTemplateSkill.CreateForTest(
					creatureTemplate: _creatureTemplate,
					skill: _parameter,
					value: 5));
			_ability = Ability.CreateForTest(
				game: _game,
				name: "attack",
				description: "bite",
				attackDiceQuantity: 2,
				damageModifier: 4,
				accuracy: -1,
				attackSpeed: 1,
				attackSkill: _parameter);
			_ability.AppliedConditions.Add(new AppliedCondition(_ability, _condition, 50));
			_creatureTemplate.Abilities.Add(_ability);

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_imgFile,
				_condition,
				_parameter,
				_bodyTemplate,
				_creatureTemplate,
				_creatureType,
				_ability));
		}

		/// <summary>
		/// Тест метода Handle - создание боя
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_CreateInstance_ShouldReturnUnit()
		{
			var request = new CreateBattleCommand(
				gameId: _game.Id,
				imgFileId: _imgFile.Id,
				name: "instance",
				description: "description",
				creatures: new List<CreateBattleRequestItem>()
				{
					new CreateBattleRequestItem()
					{
						CreatureTemplateId = _creatureTemplate.Id,
						Name = "monster1",
						Description = "newMonster"
					}
				});

			var newHandler = new CreateBattleHandler(_dbContext, AuthorizationService.Object);

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
			Assert.AreEqual(_creatureTemplate.CreatureTypeId, creature.CreatureTypeId);
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

			Assert.IsNotNull(creature.CreatureParts);
			var creaturePart = creature.CreatureParts.FirstOrDefault();
			Assert.IsNotNull(creaturePart);
			Assert.AreEqual(creature.CreatureParts.Count, 1);
			Assert.AreEqual(creaturePart.Name, "torso");
			Assert.AreEqual(creaturePart.HitPenalty, 1);
			Assert.AreEqual(creaturePart.DamageModifier, 1);
			Assert.AreEqual(creaturePart.MaxToHit, 10);
			Assert.AreEqual(creaturePart.MinToHit, 1);
			Assert.AreEqual(creaturePart.CurrentArmor, 5);
			Assert.AreEqual(creaturePart.StartingArmor, 5);

			Assert.IsNotNull(creature.Abilities);
			Assert.AreEqual(creature.Abilities.Count(), 1);
			var ability = _dbContext.Abilities.FirstOrDefault();

			Assert.AreEqual(ability.Name, "attack");
			Assert.AreEqual(ability.Description, "bite");
			Assert.AreEqual(ability.Accuracy, -1);
			Assert.AreEqual(ability.AttackSpeed, 1);
			Assert.AreEqual(ability.AttackDiceQuantity, 2);
			Assert.AreEqual(ability.AttackSkillId, _parameter.Id);
			Assert.AreEqual(ability.DamageModifier, 4);

			Assert.IsNotNull(ability.AppliedConditions);
			Assert.AreEqual(ability.AppliedConditions.Count(), 1);
			var appliedCondition = ability.AppliedConditions.FirstOrDefault();

			Assert.AreEqual(appliedCondition.ApplyChance, 50);
			Assert.AreEqual(appliedCondition.ConditionId, _condition.Id);

			Assert.IsNotNull(creature.CreatureSkills);
			Assert.AreEqual(creature.CreatureSkills.Count(), 1);
			var creatureParameter = _dbContext.CreatureParameters
				.FirstOrDefault(x => x.CreatureId == creature.Id);

			Assert.IsTrue(creatureParameter.SkillId == _parameter.Id);
			Assert.IsTrue(creatureParameter.SkillValue == 5);
		}
	}
}
