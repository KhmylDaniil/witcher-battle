using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace PilotProject.DbContext
{
	internal class FillDbContext : TestDbContext
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;

		private readonly BodyTemplate _bodyTemplate;

		private readonly Ability _swordAttack;
		private readonly Ability _archeryAttack;
		private readonly Ability _clawsAttack;

		public FillDbContext()
		{
			_game = Game.CreateForTest(id: GameId);

		_swordAttack = Ability.CreateForTest(
				id: AbilitySwordId,
				game: _game,
				name: "SwordAttack",
				attackDiceQuantity: 2,
				damageModifier: 2,
				attackSpeed: 1,
				accuracy: 1,
				attackSkill: Skill.Sword,
				defensiveSkills: new List<Skill> { Skill.Melee, Skill.Dodge, Skill.Athletics });
			_swordAttack.AppliedConditions.Add(AppliedCondition.CreateAppliedCondition(_swordAttack, Condition.Bleed, 50));

			_clawsAttack = Ability.CreateForTest(
				id: AbilityclawsId,
				game: _game,
				name: "ClawsAttack",
				attackDiceQuantity: 3,
				damageModifier: 0,
				attackSpeed: 1,
				accuracy: 0,
				attackSkill: Skill.Melee,
				defensiveSkills: new List<Skill> { Skill.Melee, Skill.Dodge, Skill.Athletics });
			_clawsAttack.AppliedConditions.Add(AppliedCondition.CreateAppliedCondition(_clawsAttack, Condition.Poison, 50));

			_archeryAttack = Ability.CreateForTest(
				id: AbilityBowId,
				game: _game,
				name: "ArcheryAttack",
				attackDiceQuantity: 4,
				damageModifier: 0,
				attackSpeed: 1,
				accuracy: 1,
				attackSkill: Skill.Archery,
				defensiveSkills: new List<Skill> { Skill.Dodge, Skill.Athletics });

			_bodyTemplate = BodyTemplate.CreateForTest(id: BodyTemplateId, name: "TestBodyTemplate", game: _game);
			_bodyTemplate.BodyTemplateParts.AddRange(new List<BodyTemplatePart>
			{
				BodyTemplatePart.CreateForTest(
					id: HeadId,
					bodyTemplate: _bodyTemplate,
					bodyPartType: BodyPartType.Head,
					damageModifier: 3,
					hitPenalty: 6,
					minToHit: 1,
					maxToHit: 1),

				BodyTemplatePart.CreateForTest(
					id: TorsoId,
					bodyTemplate: _bodyTemplate,
					bodyPartType: BodyPartType.Torso,
					damageModifier: 1,
					hitPenalty: 1,
					minToHit: 2,
					maxToHit: 4),

				BodyTemplatePart.CreateForTest(
					id: LeftArmId,
					bodyTemplate: _bodyTemplate,
					bodyPartType: BodyPartType.Arm,
					name: "Left Arm",
					damageModifier: 0.5,
					hitPenalty: 3,
					minToHit: 5,
					maxToHit: 5),

				BodyTemplatePart.CreateForTest(
					id: RightArmId,
					bodyTemplate: _bodyTemplate,
					bodyPartType: BodyPartType.Arm,
					name: "Right Arm",
					damageModifier: 0.5,
					hitPenalty: 3,
					minToHit: 6,
					maxToHit: 6),

				BodyTemplatePart.CreateForTest(
					id: RightLegId,
					bodyTemplate: _bodyTemplate,
					bodyPartType: BodyPartType.Leg,
					name: "Right Leg",
					damageModifier: 0.5,
					hitPenalty: 2,
					minToHit: 7,
					maxToHit: 8),

				BodyTemplatePart.CreateForTest(
					id: LeftLegId,
					bodyTemplate: _bodyTemplate,
					bodyPartType: BodyPartType.Leg,
					name: "Left Leg",
					damageModifier: 0.5,
					hitPenalty: 2,
					minToHit: 9,
				maxToHit: 10)
			});

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_bodyTemplate,
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
