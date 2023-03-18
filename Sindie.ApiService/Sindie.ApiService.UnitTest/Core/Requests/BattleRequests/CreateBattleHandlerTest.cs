using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.BattleRequests;
using System.Linq;
using System.Threading.Tasks;

namespace Witcher.UnitTest.Core.Requests.BattleRequests
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

		/// <summary>
		/// Конструктор для теста <see cref="CreateBattleHandler"/>
		/// </summary>
		public CreateBattleHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_imgFile = ImgFile.CreateForTest();
	
			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_imgFile));
		}

		/// <summary>
		/// Тест метода Handle - создание боя
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_CreateBattle_ShouldReturnUnit()
		{
			var request = new CreateBattleCommand()
			{
				ImgFileId = _imgFile.Id,
				Name = "battle",
				Description = "description"
			};

			var newHandler = new CreateBattleHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			var instance = _dbContext.Battles.FirstOrDefault();
			Assert.IsNotNull(instance);
			Assert.IsTrue(_dbContext.Battles.Count() == 1);

			Assert.AreEqual("battle", instance.Name);
			Assert.AreEqual("description", instance.Description);
			Assert.AreEqual(instance.ImgFileId, _imgFile.Id);
			Assert.AreEqual(instance.GameId, _game.Id);

			//Assert.IsNotNull(instance.Creatures);
			//Assert.IsTrue(instance.Creatures.Count == 1);
			//var creature = instance.Creatures.FirstOrDefault();

			//Assert.IsNotNull(creature);
			//Assert.IsNotNull(creature.Name);
			//Assert.AreEqual(creature.Name, "monster1");
			//Assert.AreEqual(creature.Description, "newMonster");
			//Assert.AreEqual(_creatureTemplate.ImgFileId, creature.ImgFileId);
			//Assert.AreEqual(_creatureTemplate.CreatureType, creature.CreatureType);
			//Assert.AreEqual(_creatureTemplate.HP, creature.HP);
			//Assert.AreEqual(_creatureTemplate.Sta, creature.Sta);
			//Assert.AreEqual(_creatureTemplate.Int, creature.Int);
			//Assert.AreEqual(_creatureTemplate.Ref, creature.Ref);
			//Assert.AreEqual(_creatureTemplate.Dex, creature.Dex);
			//Assert.AreEqual(_creatureTemplate.Body, creature.Body);
			//Assert.AreEqual(_creatureTemplate.Emp, creature.Emp);
			//Assert.AreEqual(_creatureTemplate.Cra, creature.Cra);
			//Assert.AreEqual(_creatureTemplate.Will, creature.Will);
			//Assert.AreEqual(_creatureTemplate.Speed, creature.Speed);
			//Assert.AreEqual(_creatureTemplate.Luck, creature.Luck);

			//Assert.IsNotNull(creature.CreatureParts);
			//var creaturePart = creature.CreatureParts.FirstOrDefault();
			//Assert.IsNotNull(creaturePart);
			//Assert.AreEqual(creature.CreatureParts.Count, 1);
			//Assert.AreEqual(creaturePart.Name, "arm");
			//Assert.AreEqual(creaturePart.HitPenalty, 1);
			//Assert.AreEqual(creaturePart.DamageModifier, 1);
			//Assert.AreEqual(creaturePart.MaxToHit, 10);
			//Assert.AreEqual(creaturePart.MinToHit, 1);
			//Assert.AreEqual(creaturePart.CurrentArmor, 5);
			//Assert.AreEqual(creaturePart.StartingArmor, 5);

			//Assert.IsTrue(creature.LeadingArmId != default);
			//Assert.IsTrue(creature.LeadingArmId == creaturePart.Id);


			//Assert.IsNotNull(creature.Abilities);
			//Assert.AreEqual(creature.Abilities.Count, 1);
			//var ability = _dbContext.Abilities.FirstOrDefault();

			//Assert.AreEqual(ability.Name, "attack");
			//Assert.AreEqual(ability.Description, "bite");
			//Assert.AreEqual(ability.Accuracy, -1);
			//Assert.AreEqual(ability.AttackSpeed, 1);
			//Assert.AreEqual(ability.AttackDiceQuantity, 2);
			//Assert.AreEqual(ability.AttackSkill, Skill.Melee);
			//Assert.AreEqual(ability.DamageModifier, 4);

			//Assert.IsNotNull(ability.AppliedConditions);
			//Assert.AreEqual(ability.AppliedConditions.Count, 1);
			//var appliedCondition = ability.AppliedConditions.FirstOrDefault();

			//Assert.AreEqual(appliedCondition.ApplyChance, 50);
			//Assert.AreEqual(appliedCondition.Condition, Condition.Bleed);

			//Assert.IsNotNull(creature.CreatureSkills);
			//Assert.AreEqual(creature.CreatureSkills.Count, 1);
			//var creatureSkill = _dbContext.CreatureSkills
			//	.FirstOrDefault(x => x.CreatureId == creature.Id);

			//Assert.IsTrue(creatureSkill.Skill == Skill.Melee);
			//Assert.IsTrue(creatureSkill.SkillValue == 5);
		}
	}
}
