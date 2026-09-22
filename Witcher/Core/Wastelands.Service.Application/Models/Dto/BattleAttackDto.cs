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

		/// <summary>Заполнено, только если защитник — персонаж (фиксированный набор — см. HUMAN_BODY_PARTS на фронте).</summary>
		public HumanBodyPart? TargetedHumanBodyPart { get; set; }

		public int? AttackRoll { get; set; }

		public bool AttackerConfirmed { get; set; }

		public List<Skill> AvailableDefensiveSkills { get; set; } = [];

		/// <summary>Справочное значение характеристика+навык защитника для каждого доступного защитного навыка.</summary>
		public Dictionary<Skill, int> DefensiveSkillValues { get; set; } = [];

		public Skill? DefensiveSkill { get; set; }

		public int? DefenseRoll { get; set; }

		/// <summary>Доступно ли защитнику парирование вместо обычного защитного навыка — есть ли экипированное оружие ближнего боя.</summary>
		public bool CanParry { get; set; }

		/// <summary>Навык, которым будет парировать защитник (навык атаки его оружия ближнего боя) — заполнено, только если CanParry.</summary>
		public Skill? ParrySkill { get; set; }

		/// <summary>Справочное значение характеристика+навык парирования до вычета ParryRules.RollPenalty — заполнено, только если CanParry.</summary>
		public int? ParrySkillValue { get; set; }

		/// <summary>true — защитник выбрал парирование (см. BattleAttack.IsParry).</summary>
		public bool IsParry { get; set; }

		public bool DefenderConfirmed { get; set; }

		/// <summary>Оглушённый защитник не выбирает навык/не бросает защиту — фронт должен скрыть этот шаг и сразу предложить подтвердить.</summary>
		public bool DefenderIsStunned { get; set; }

		public BattleAttackPhase Phase { get; set; }

		public bool? LastHitSucceeded { get; set; }

		public long? ResolvedCreaturePartId { get; set; }

		public int? DamageRoll { get; set; }

		/// <summary>Заполнено, только когда Phase == AwaitingStunSave или уже пройдена — ручной ввод д10 для stun save защитника.</summary>
		public int? StunSaveRoll { get; set; }

		/// <summary>true — Оглушение наложено этой атакой, false — не наложено. Null, пока проверка не пройдена.</summary>
		public bool? StunSaveSucceeded { get; set; }

		/// <summary>true — это дополнительное действие персонажа за выносливость, со штрафом к атаке (см. BattleAttack.IsBonusAction).</summary>
		public bool IsBonusAction { get; set; }
	}
}
