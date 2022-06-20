using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BattleRequests.MonsterAttack;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.BattleRequests
{
	/// <summary>
	/// Тест для <see cref="MonsterAttackHandler"/>
	/// </summary>
	[TestClass]
	public class MonsterAttackHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Instance _instance;
		private readonly BodyPartType _torso;
		private readonly BodyPartType _head;
		private readonly BodyTemplate _target;
		private readonly BodyTemplatePart _headPart;
		private readonly BodyTemplatePart _torsoPart;
		private readonly Condition _condition;
		private readonly Parameter _parameter;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly Ability _ability;
		private readonly Creature _creature;

		/// <summary>
		/// Тест для <see cref="MonsterAttackHandler"/>
		/// </summary>
		public MonsterAttackHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_instance = Instance.CreateForTest(game: _game);
			_target = BodyTemplate.CreateForTest(game: _game);
			_torso = BodyPartType.CreateForTest(BodyPartTypes.TorsoId, BodyPartTypes.TorsoName);
			_head = BodyPartType.CreateForTest(BodyPartTypes.HeadId, BodyPartTypes.HeadName);
			_condition = Condition.CreateForTest(game: _game);
			_parameter = Parameter.CreateForTest(game: _game);

			_target = BodyTemplate.CreateForTest(
				game: _game,
				name: "target");

			_headPart = BodyTemplatePart.CreateForTest(
				bodyTemplate: _target,
				bodyPartType: _head,
				name: _head.Name,
				damageModifier: 3,
				hitPenalty: 6,
				minToHit: 1,
				maxToHit: 3);
			_torsoPart = BodyTemplatePart.CreateForTest(
				bodyTemplate: _target,
				bodyPartType: _torso,
				name: _torso.Name,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 4,
				maxToHit: 10);
			_target.BodyTemplateParts.AddRange(new List<BodyTemplatePart>{ _headPart, _torsoPart });

			_creatureTemplate = CreatureTemplate.CreateForTest(
				game: _game,
				bodyTemplate: _target);

			_ability = Ability.CreateForTest(
				creatureTemplate: _creatureTemplate,
				name: "attack",
				attackDiceQuantity: 1,
				damageModifier: 1,
				attackSpeed: 1,
				accuracy: 1,
				attackParameter: _parameter);
			_ability.AppliedConditions.Add(AppliedCondition.CreateAppliedCondition(_ability, _condition, 50));

			_creature = Creature.CreateForTest(
				instance: _instance,
				creatureTemlpate: _creatureTemplate,
				bodyTemplate: _target);
			_creature.Abilities.Add(_ability);
			_creature.CreatureParameters.Add(CreatureParameter.CreateForTest(
				creature: _creature,
				parameter: _parameter,
				value: 10));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_instance,
				_target,
				_torso,
				_head,
				_condition,
				_parameter,
				_target,
				_creatureTemplate,
				_ability,
				_creature));
		}

		/// <summary>
		/// Тест метода Handle - атака монстра
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_MonsterAttack_ShouldReturnUnit()
		{
			var request = new MonsterAttackCommand(
				instanceId: _instance.Id,
				id: _creature.Id,
				abilityId: _ability.Id,
				targetBodyTemplateId: _target.Id,
				bodyTemplatePartId: _torsoPart.Id,
				defenseValue: 5);

			var newHandler = new MonsterAttackHandler(_dbContext, AuthorizationService.Object, RollService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var message = result.Message;
			Assert.IsNotNull(message);
			Assert.IsTrue(message.Contains("Попадание"));
		}
	}
}
