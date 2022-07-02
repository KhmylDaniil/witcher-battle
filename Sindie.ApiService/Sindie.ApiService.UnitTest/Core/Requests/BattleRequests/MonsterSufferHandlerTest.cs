using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BattleRequests.MonsterSuffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.BattleRequests
{
	/// <summary>
	/// Тест для <see cref="MonsterSufferHandler"/>
	/// </summary>
	[TestClass]
	public class MonsterSufferHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Instance _instance;
		private readonly BodyPartType _torso;
		private readonly BodyPartType _head;
		private readonly BodyTemplate _bodyTemplate;
		private readonly CreaturePart _headPart;
		private readonly CreaturePart _torsoPart;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly Creature _creature;
		private readonly CreatureType _creatureType;

		/// <summary>
		/// Тест для <see cref="MonsterSufferHandler"/>
		/// </summary>
		public MonsterSufferHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_creatureType = CreatureType.CreateForTest(CreatureTypes.SpecterId, CreatureTypes.SpecterName);
			_instance = Instance.CreateForTest(game: _game);
			_torso = BodyPartType.CreateForTest(BodyPartTypes.TorsoId, BodyPartTypes.TorsoName);
			_head = BodyPartType.CreateForTest(BodyPartTypes.HeadId, BodyPartTypes.HeadName);
			
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);

			_creatureTemplate = CreatureTemplate.CreateForTest(
				game: _game,
				bodyTemplate: _bodyTemplate,
				creatureType: _creatureType);
			
			_creature = Creature.CreateForTest(
				instance: _instance,
				creatureTemlpate: _creatureTemplate,
				bodyTemplate: _bodyTemplate,
				creatureType: _creatureType,
				hp: 10);

			_headPart = CreaturePart.CreateForTest(
				creature: _creature,
				bodyPartType: _head,
				name: _head.Name,
				damageModifier: 3,
				hitPenalty: 6,
				minToHit: 1,
				maxToHit: 3);
			_torsoPart = CreaturePart.CreateForTest(
				creature: _creature,
				bodyPartType: _torso,
				name: _torso.Name,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 4,
				maxToHit: 10,
				startingArmor: 4,
				currentArmor: 4);
			_creature.CreatureParts.AddRange(new List<CreaturePart> { _headPart, _torsoPart });

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_instance,
				_bodyTemplate,
				_torso,
				_head,
				_creatureTemplate,
				_creature,
				_creatureType));
		}

		/// <summary>
		/// Тест метода Handle - получение монстром урона
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_MonsterSuffer_ShouldReturnUnit()
		{
			var request = new MonsterSufferCommand(
				instanceId: _instance.Id,
				monsterId: _creature.Id,
				damageValue: 10,
				successValue: 1,
				creaturePartId: _torsoPart.Id,
				isResistant: true,
				isVulnerable: false);

			var newHandler = new MonsterSufferHandler(_dbContext, AuthorizationService.Object, RollService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var message = result.Message;
			Assert.IsNotNull(message);
			Assert.IsTrue(message.Contains("повреждена"));
			Assert.IsTrue(message.Contains("7 хитов"));

			var monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);
			Assert.AreEqual(monster.HP, 7);
			var torsoPart = monster.CreatureParts.FirstOrDefault(x => x.BodyPartTypeId == _torso.Id);
			Assert.IsNotNull(torsoPart);
			Assert.AreEqual(torsoPart.CurrentArmor, 3);
		}


		/// <summary>
		/// Тест метода Handle - атака монстра
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_MonsterAttackСrit_ShouldReturnUnit()
		{
			var request = new MonsterSufferCommand(
				instanceId: _instance.Id,
				monsterId: _creature.Id,
				damageValue: 10,
				successValue: 1,
				creaturePartId: _headPart.Id,
				isResistant: false,
				isVulnerable: false);

			var newHandler = new MonsterSufferHandler(_dbContext, AuthorizationService.Object, RollService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var message = result.Message;
			Assert.IsNotNull(message);
			Assert.IsTrue(message.Contains("погибает"));
			var monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNull(monster);
		}
	}
}
