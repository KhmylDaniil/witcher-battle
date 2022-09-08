using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;

namespace PilotProject.DbContext
{
	internal class FillDbContext : TestDbContext
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly CreatureType _creatureType;
		private readonly DamageType _damageType;

		private readonly BodyPartType _torso;
		private readonly BodyPartType _head;
		private readonly BodyPartType _leg;
		private readonly BodyPartType _arm;

		private readonly Condition _bleed;
		private readonly Condition _poison;
		private readonly Condition _stun;

		private readonly Condition _simpleLegCrit;
		private readonly Condition _simpleArmCrit;
		private readonly Condition _simpleHead1Crit;
		private readonly Condition _simpleHead2Crit;
		private readonly Condition _simpleTorso1Crit;
		private readonly Condition _simpleTorso2Crit;

		private readonly Condition _complexLegCrit;
		private readonly Condition _complexArmCrit;
		private readonly Condition _complexHead1Crit;
		private readonly Condition _complexHead2Crit;
		private readonly Condition _complexTorso1Crit;
		private readonly Condition _complexTorso2Crit;

		private readonly Condition _difficultLegCrit;
		private readonly Condition _difficultArmCrit;
		private readonly Condition _difficultHead1Crit;
		private readonly Condition _difficultHead2Crit;
		private readonly Condition _difficultTorso1Crit;
		private readonly Condition _difficultTorso2Crit;

		private readonly Condition _deadlyLegCrit;
		private readonly Condition _deadlyArmCrit;
		private readonly Condition _deadlyHead1Crit;
		private readonly Condition _deadlyHead2Crit;
		private readonly Condition _deadlyTorso1Crit;
		private readonly Condition _deadlyTorso2Crit;

		private readonly BodyTemplate _bodyTemplate;

		private readonly Skill _meleeSkill;
		private readonly Skill _dodgeSkill;
		private readonly Skill _athleticsSkill;
		private readonly Skill _swordmanship;
		private readonly Skill _archery;

		private readonly Ability _swordAttack;
		private readonly Ability _archeryAttack;
		private readonly Ability _clawsAttack;

		public FillDbContext()
		{
			_game = Game.CreateForTest(id: GameId);
			_creatureType = CreatureType.CreateForTest(CreatureTypes.HumanId, CreatureTypes.HumanName);
			_damageType = DamageType.CreateForTest(DamageTypes.SlashingId, DamageTypes.SlashingName);

			_torso = BodyPartType.CreateForTest(BodyPartTypes.TorsoId, BodyPartTypes.TorsoName);
			_head = BodyPartType.CreateForTest(BodyPartTypes.HeadId, BodyPartTypes.HeadName);
			_leg = BodyPartType.CreateForTest(BodyPartTypes.LegId, BodyPartTypes.LegName);
			_arm = BodyPartType.CreateForTest(BodyPartTypes.ArmId, BodyPartTypes.ArmName);

			_bleed = Condition.CreateForTest(id: Conditions.BleedId, Conditions.BleedName);
			_poison = Condition.CreateForTest(id: Conditions.PoisonId, Conditions.PoisonName);
			_stun = Condition.CreateForTest(id: Conditions.StunId, Conditions.StunName);

			_simpleLegCrit = Condition.CreateForTest(id: Crit.SimpleLegId, name: Crit.SimpleLeg);
			_simpleArmCrit = Condition.CreateForTest(id: Crit.SimpleArmId, name: Crit.SimpleArm);
			_simpleHead1Crit = Condition.CreateForTest(id: Crit.SimpleHead1Id, name: Crit.SimpleHead1);
			_simpleHead2Crit = Condition.CreateForTest(id: Crit.SimpleHead2Id, name: Crit.SimpleHead2);
			_simpleTorso1Crit = Condition.CreateForTest(id: Crit.SimpleTorso1Id, name: Crit.SimpleTorso1);
			_simpleTorso2Crit = Condition.CreateForTest(id: Crit.SimpleTorso2Id, name: Crit.SimpleTorso2);

			_complexLegCrit = Condition.CreateForTest(id: Crit.ComplexLegId, name: Crit.ComplexLeg);
			_complexArmCrit = Condition.CreateForTest(id: Crit.ComplexArmId, name: Crit.ComplexArm);
			_complexHead1Crit = Condition.CreateForTest(id: Crit.ComplexHead1Id, name: Crit.ComplexHead1);
			_complexHead2Crit = Condition.CreateForTest(id: Crit.ComplexHead2Id, name: Crit.ComplexHead2);
			_complexTorso1Crit = Condition.CreateForTest(id: Crit.ComplexTorso1Id, name: Crit.ComplexTorso1);
			_complexTorso2Crit = Condition.CreateForTest(id: Crit.ComplexTorso2Id, name: Crit.ComplexTorso2);

			_difficultLegCrit = Condition.CreateForTest(id: Crit.DifficultLegId, name: Crit.DifficultLeg);
			_difficultArmCrit = Condition.CreateForTest(id: Crit.DifficultArmId, name: Crit.DifficultArm);
			_difficultHead1Crit = Condition.CreateForTest(id: Crit.DifficultHead1Id, name: Crit.DifficultHead1);
			_difficultHead2Crit = Condition.CreateForTest(id: Crit.DifficultHead2Id, name: Crit.DifficultHead2);
			_difficultTorso1Crit = Condition.CreateForTest(id: Crit.DifficultTorso1Id, name: Crit.DifficultTorso1);
			_difficultTorso2Crit = Condition.CreateForTest(id: Crit.DifficultTorso2Id, name: Crit.DifficultTorso2);

			_deadlyLegCrit = Condition.CreateForTest(id: Crit.DeadlyLegId, name: Crit.DeadlyLeg);
			_deadlyArmCrit = Condition.CreateForTest(id: Crit.DeadlyArmId, name: Crit.DeadlyArm);
			_deadlyHead1Crit = Condition.CreateForTest(id: Crit.DeadlyHead1Id, name: Crit.DeadlyHead1);
			_deadlyHead2Crit = Condition.CreateForTest(id: Crit.DeadlyHead2Id, name: Crit.DeadlyHead2);
			_deadlyTorso1Crit = Condition.CreateForTest(id: Crit.DeadlyTorso1Id, name: Crit.DeadlyTorso1);
			_deadlyTorso2Crit = Condition.CreateForTest(id: Crit.DeadlyTorso2Id, name: Crit.DeadlyTorso2);

			_meleeSkill = Skill.CreateForTest(id: Skills.MeleeId, name: Skills.MeleeName, statName: Enums.Stats.Ref);
			_dodgeSkill = Skill.CreateForTest(id: Skills.DodgeId, name: Skills.DodgeName, statName: Enums.Stats.Ref);
			_athleticsSkill = Skill.CreateForTest(id: Skills.AthleticsId, name: Skills.AthleticsName, statName: Enums.Stats.Dex);
			_swordmanship = Skill.CreateForTest(id: Skills.SwordId, name: Skills.SwordName, statName: Enums.Stats.Ref);
			_archery = Skill.CreateForTest(id: Skills.ArcheryId, name: Skills.ArcheryName, statName: Enums.Stats.Dex);

		_swordAttack = Ability.CreateForTest(
				id: AbilitySwordId,
				game: _game,
				name: "SwordAttack",
				attackDiceQuantity: 2,
				damageModifier: 2,
				attackSpeed: 1,
				accuracy: 1,
				attackSkill: _swordmanship,
				damageType: _damageType,
				defensiveSkills: new List<Skill> { _meleeSkill, _dodgeSkill, _athleticsSkill });
			_swordAttack.AppliedConditions.Add(AppliedCondition.CreateAppliedCondition(_swordAttack, _bleed, 50));

			_clawsAttack = Ability.CreateForTest(
				id: AbilityclawsId,
				game: _game,
				name: "ClawsAttack",
				attackDiceQuantity: 3,
				damageModifier: 0,
				attackSpeed: 1,
				accuracy: 0,
				attackSkill: _meleeSkill,
				damageType: _damageType,
				defensiveSkills: new List<Skill> { _meleeSkill, _dodgeSkill, _athleticsSkill });
			_clawsAttack.AppliedConditions.Add(AppliedCondition.CreateAppliedCondition(_clawsAttack, _poison, 50));

			_archeryAttack = Ability.CreateForTest(
				id: AbilityBowId,
				game: _game,
				name: "ArcheryAttack",
				attackDiceQuantity: 4,
				damageModifier: 0,
				attackSpeed: 1,
				accuracy: 1,
				attackSkill: _archery,
				damageType: _damageType,
				defensiveSkills: new List<Skill> { _dodgeSkill, _athleticsSkill });

			_bodyTemplate = BodyTemplate.CreateForTest(id: BodyTemplateId, name: "TestBodyTemplate", game: _game);
			_bodyTemplate.BodyTemplateParts.AddRange(new List<BodyTemplatePart>
			{
				BodyTemplatePart.CreateForTest(
					id: HeadId,
					bodyTemplate: _bodyTemplate,
					bodyPartType: _head,
					name: "Head",
					damageModifier: 3,
					hitPenalty: 6,
					minToHit: 1,
					maxToHit: 1),

				BodyTemplatePart.CreateForTest(
					id: TorsoId,
					bodyTemplate: _bodyTemplate,
					bodyPartType: _torso,
					name: "Torso",
					damageModifier: 1,
					hitPenalty: 1,
					minToHit: 2,
					maxToHit: 4),

				BodyTemplatePart.CreateForTest(
					id: LeftArmId,
					bodyTemplate: _bodyTemplate,
					bodyPartType: _arm,
					name: "Left Arm",
					damageModifier: 0.5,
					hitPenalty: 3,
					minToHit: 5,
					maxToHit: 5),

				BodyTemplatePart.CreateForTest(
					id: RightArmId,
					bodyTemplate: _bodyTemplate,
					bodyPartType: _arm,
					name: "Right Arm",
					damageModifier: 0.5,
					hitPenalty: 3,
					minToHit: 6,
					maxToHit: 6),

				BodyTemplatePart.CreateForTest(
					id: RightLegId,
					bodyTemplate: _bodyTemplate,
					bodyPartType: _leg,
					name: "Right Leg",
					damageModifier: 0.5,
					hitPenalty: 2,
					minToHit: 7,
					maxToHit: 8),

				BodyTemplatePart.CreateForTest(
					id: LeftLegId,
					bodyTemplate: _bodyTemplate,
					bodyPartType: _leg,
					name: "Left Leg",
					damageModifier: 0.5,
					hitPenalty: 2,
					minToHit: 9,
				maxToHit: 10)
			});

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_creatureType,
				_damageType,
				_torso,
				_head,
				_leg,
				_arm,
				_bleed,
				_poison,
				_stun,

				_simpleLegCrit,
				_simpleArmCrit,
				_simpleHead1Crit,
				_simpleHead2Crit,
				_simpleTorso1Crit,
				_simpleTorso2Crit,
				_complexLegCrit,
				_complexArmCrit,
				_complexHead1Crit,
				_complexHead2Crit,
				_complexTorso1Crit,
				_complexTorso2Crit,
				_difficultLegCrit,
				_difficultArmCrit,
				_difficultHead1Crit,
				_difficultHead2Crit,
				_difficultTorso1Crit,
				_difficultTorso2Crit,
				_deadlyLegCrit,
				_deadlyArmCrit,
				_deadlyHead1Crit,
				_deadlyHead2Crit,
				_deadlyTorso1Crit,
				_deadlyTorso2Crit,

				_bodyTemplate,

				_meleeSkill,
				_dodgeSkill,
				_athleticsSkill,
				_swordmanship,
				_archery,

				_swordAttack,
				_archeryAttack,
				_clawsAttack

				));
		}
		public IAppDbContext ReturnContext()
			=> _dbContext;

		public IAuthorizationService ReturnAuthorizationService()
			=> AuthorizationService.Object;

		public IDateTimeProvider ReturnDateTimeProvider()
			=> DateTimeProvider.Object;
	}
}
