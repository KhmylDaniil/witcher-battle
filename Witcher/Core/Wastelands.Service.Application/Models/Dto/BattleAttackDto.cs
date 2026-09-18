using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Dto
{
	public class BattleAttackDto : BaseDto
	{
		public ParticipantKind AttackerKind { get; set; }

		public long AttackerId { get; set; }

		public string AttackerName { get; set; }

		public long AbilityId { get; set; }

		public string AbilityName { get; set; }

		/// <summary>Справочное значение характеристика+навык атакующего для этой способности — менять нельзя.</summary>
		public int AttackerSkillValue { get; set; }

		/// <summary>Сколько к6 бросает способность — для подсказки допустимого диапазона броска урона на фронте.</summary>
		public int AbilityDamageDiceCount { get; set; }

		public int AttacksAllowed { get; set; }

		public int AttacksUsed { get; set; }

		public ParticipantKind DefenderKind { get; set; }

		public long DefenderId { get; set; }

		public string DefenderName { get; set; }

		/// <summary>Заполнено, только если защитник — существо.</summary>
		public List<CreaturePartOptionDto>? AvailableCreatureParts { get; set; }

		public long? TargetedCreaturePartId { get; set; }

		public int? AttackRoll { get; set; }

		public bool AttackerConfirmed { get; set; }

		public List<Skill> AvailableDefensiveSkills { get; set; } = [];

		/// <summary>Справочное значение характеристика+навык защитника для каждого доступного защитного навыка.</summary>
		public Dictionary<Skill, int> DefensiveSkillValues { get; set; } = [];

		public Skill? DefensiveSkill { get; set; }

		public int? DefenseRoll { get; set; }

		public bool DefenderConfirmed { get; set; }

		public BattleAttackPhase Phase { get; set; }

		public bool? LastHitSucceeded { get; set; }

		public long? ResolvedCreaturePartId { get; set; }

		public int? DamageRoll { get; set; }
	}
}
