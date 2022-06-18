using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.BattleRequests
{
	/// <summary>
	/// Тест для <see cref="MonsterAttackHandler"/>
	/// </summary>
	[TestClass]
	public class MonsterAttackHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly BodyTemplate _target;
		private readonly BodyTemplatePart _torso;
		private readonly Condition _condition;
		private readonly Parameter _parameter;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly CreatureTemplateParameter _creatureTemplateParameter;
		private readonly Ability _ability;

		/// <summary>
		/// Конструктор для теста <see cref="MonsterAttackHandler"/>
		/// </summary>
		//public MonsterAttackHandlerTest() : base()
		//{
		//	_game = Game.CreateForTest();
		//	_target = BodyTemplate.CreateForTest(game: _game);
		//	_torso = BodyTemplatePart.CreateForTest(
		//		bodyTemplate: _bodyTemplate,
		//		name: "torso",
		//		hitPenalty: 1,
		//		damageModifier: 1,
		//		minToHit: 1,
		//		maxToHit: 10);
		//	_condition = Condition.CreateForTest(game: _game);
		//	_parameter = Parameter.CreateForTest(game: _game);

		//	_creatureTemplate = CreatureTemplate.CreateForTest(
		//		game: _game,
		//		bodyTemplate: _bodyTemplate,
		//		bodyParts: new List<BodyPart>
		//		{
		//			new BodyPart(
		//				name: "torso",
		//				hitPenalty: 1,
		//				damageModifier: 1,
		//				minToHit: 1,
		//				maxToHit: 10,
		//				startingArmor: 5,
		//				currentArmor: 5)
		//		});
		//	_ability = Ability.CreateForTest(
		//		creatureTemplate: _creatureTemplate,
		//		name: "attack",
		//		attackDiceQuantity: 1,
		//		damageModifier: 1,
		//		attackSpeed: 1,
		//		accuracy: 1,
		//		attackParameter: _parameter1);

		//	_creatureTemplateParameter = CreatureTemplateParameter.CreateForTest(
		//		creatureTemplate: _creatureTemplate,
		//		parameter: _parameter1,
		//		value: 6);

		//	_dbContext = CreateInMemoryContext(x => x.AddRange(
		//		_game,
		//		_imgFile,
		//		_parameter1,
		//		_parameter2,
		//		_bodyTemplate,
		//		_torso,
		//		_ability,
		//		_creatureTemplate,
		//		_creatureTemplateParameter,
		//		_condition));
		//}
	}
}
