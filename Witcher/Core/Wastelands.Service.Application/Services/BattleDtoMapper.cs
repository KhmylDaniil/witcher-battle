using AutoMapper;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Services
{
	public class BattleDtoMapper : IBattleDtoMapper
	{
		private readonly IMapper _mapper;
		private readonly IBattleCombatContextProvider _contextProvider;

		public BattleDtoMapper(IMapper mapper, IBattleCombatContextProvider contextProvider)
		{
			_mapper = mapper;
			_contextProvider = contextProvider;
		}

		public async Task<BattleDto> MapAsync(Battle battle)
		{
			var dto = _mapper.Map<BattleDto>(battle);
			if (dto.Attack is not null)
			{
				await EnrichAttackAsync(dto.Attack, battle);
			}

			return dto;
		}

		// AutoMapper мапит только поля самой сущности BattleAttack (Id/AttackerId/Phase/...) —
		// имена участников, название и справочное значение способности, доступные части тела/защитные
		// навыки требуют подгрузки CreatureTemplate/Character, которых у BattleAttack нет.
		private async Task EnrichAttackAsync(BattleAttackDto dto, Battle battle)
		{
			dto.AttackerName = BattleParticipants.GetName(battle, dto.AttackerKind, dto.AttackerId);
			dto.DefenderName = BattleParticipants.GetName(battle, dto.DefenderKind, dto.DefenderId);

			var attackerContext = await _contextProvider.GetContextAsync(battle, dto.AttackerKind, dto.AttackerId);
			var ability = attackerContext.Abilities.First(a => a.Id == dto.AbilityId);
			dto.AbilityName = ability.Name;
			dto.AttackerSkillValue = attackerContext.GetSkillValue(ability.AttackSkill);
			dto.AbilityDamageDiceCount = ability.DamageDiceCount;

			var defenderContext = await _contextProvider.GetContextAsync(battle, dto.DefenderKind, dto.DefenderId);
			var parrySkill = BattleParticipants.GetEquippedMeleeWeaponSkill(dto.DefenderKind, defenderContext);

			// Способность со своим списком защитных навыков ограничивает выбор только им; иначе —
			// стандартный набор Dodge/Acrobatics плюс (для персонажа с экипированным оружием ближнего
			// боя) навык блокирования этим оружием — см. BattleHitResolver.GetAvailableDefensiveSkillsAsync.
			dto.AvailableDefensiveSkills = ability.DefensiveSkills.Count > 0
				? ability.DefensiveSkills.Select(x => x.Skill).ToList()
				: parrySkill is { } blockSkill
					? [Skill.Dodge, Skill.Acrobatics, blockSkill]
					: [Skill.Dodge, Skill.Acrobatics];

			dto.DefensiveSkillValues = dto.AvailableDefensiveSkills.ToDictionary(s => s, defenderContext.GetSkillValue);
			dto.DefenderIsStunned = BattleParticipants.HasCondition(battle, dto.DefenderKind, dto.DefenderId, Condition.Stun);

			dto.CanParry = parrySkill is not null;
			dto.ParrySkill = parrySkill;
			dto.ParrySkillValue = parrySkill is { } skill ? defenderContext.GetSkillValue(skill) : null;

			if (dto.DefenderKind == ParticipantKind.Creature)
			{
				dto.AvailableCreatureParts = defenderContext.Template!.Parts
					.Select(p => new CreaturePartOptionDto { Id = p.Id, Name = p.Name })
					.ToList();
			}
		}
	}
}
