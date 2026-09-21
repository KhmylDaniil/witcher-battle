using AutoMapper;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Services
{
	/// <summary>Способности шаблона существа и их наложенные эффекты/защитные навыки — см. ICreatureTemplateAbilityService.</summary>
	public class CreatureTemplateAbilityService : ICreatureTemplateAbilityService
	{
		private readonly ICreatureTemplateRepository _creatureTemplateRepository;
		private readonly IMapper _mapper;

		public CreatureTemplateAbilityService(ICreatureTemplateRepository creatureTemplateRepository, IMapper mapper)
		{
			_creatureTemplateRepository = creatureTemplateRepository;
			_mapper = mapper;
		}

		public async Task<CreatureTemplateDto> AddAbilityAsync(CreateAbilityRequest request)
		{
			var creatureTemplate = await GetByIdAsync(request.CreatureTemplateId);

			var ability = Ability.ForCreatureTemplate(
				creatureTemplate.Id, request.Name, request.AttackSkill, request.AttacksPerTurn,
				request.DamageDiceCount, request.AttackModifier, request.DamageModifier, request.DamageType);

			creatureTemplate.Abilities.Add(ability);
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task<CreatureTemplateDto> UpdateAbilityAsync(UpdateAbilityRequest request)
		{
			var creatureTemplate = await GetByIdAsync(request.CreatureTemplateId);
			var ability = AbilityHelpers.GetAbility(creatureTemplate.Abilities, request.AbilityId);

			ability.ChangeAbility(
				request.Name, request.AttackSkill, request.AttacksPerTurn,
				request.DamageDiceCount, request.AttackModifier, request.DamageModifier, request.DamageType);

			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task<CreatureTemplateDto> RemoveAbilityAsync(long creatureTemplateId, long abilityId)
		{
			var creatureTemplate = await GetByIdAsync(creatureTemplateId);
			var ability = AbilityHelpers.GetAbility(creatureTemplate.Abilities, abilityId);

			creatureTemplate.Abilities.Remove(ability);
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task<CreatureTemplateDto> AddAbilityConditionAsync(AddAbilityConditionRequest request)
		{
			var creatureTemplate = await GetByIdAsync(request.CreatureTemplateId);
			var ability = AbilityHelpers.GetAbility(creatureTemplate.Abilities, request.AbilityId);

			AbilityHelpers.ThrowIfConditionDuplicate(ability, request.Condition, excludeConditionId: null);

			ability.AppliedConditions.Add(new AbilityAppliedCondition(ability.Id, request.Condition, request.ApplyChance));
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task<CreatureTemplateDto> UpdateAbilityConditionAsync(UpdateAbilityConditionRequest request)
		{
			var creatureTemplate = await GetByIdAsync(request.CreatureTemplateId);
			var ability = AbilityHelpers.GetAbility(creatureTemplate.Abilities, request.AbilityId);
			var condition = ability.AppliedConditions.FirstOrDefault(x => x.Id == request.ConditionId);

			NotFoundException.ThrowIfNull(
				condition, ErrorCode.AbilityConditionNotFound, nameof(AbilityAppliedCondition), nameof(AbilityAppliedCondition.Id), request.ConditionId.ToString());

			AbilityHelpers.ThrowIfConditionDuplicate(ability, request.Condition, excludeConditionId: request.ConditionId);

			condition.ChangeCondition(request.Condition, request.ApplyChance);
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task<CreatureTemplateDto> RemoveAbilityConditionAsync(long creatureTemplateId, long abilityId, long conditionId)
		{
			var creatureTemplate = await GetByIdAsync(creatureTemplateId);
			var ability = AbilityHelpers.GetAbility(creatureTemplate.Abilities, abilityId);
			var condition = ability.AppliedConditions.FirstOrDefault(x => x.Id == conditionId);

			NotFoundException.ThrowIfNull(
				condition, ErrorCode.AbilityConditionNotFound, nameof(AbilityAppliedCondition), nameof(AbilityAppliedCondition.Id), conditionId.ToString());

			ability.AppliedConditions.Remove(condition);
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task<CreatureTemplateDto> AddAbilityDefensiveSkillAsync(AddAbilityDefensiveSkillRequest request)
		{
			var creatureTemplate = await GetByIdAsync(request.CreatureTemplateId);
			var ability = AbilityHelpers.GetAbility(creatureTemplate.Abilities, request.AbilityId);

			if (ability.DefensiveSkills.Any(x => x.Skill == request.Skill))
			{
				throw new InvalidArgumentException(
					ErrorCode.AbilityDefensiveSkillAlreadyExisted,
					string.Format(ExceptionMessages.ValueMustBeUnique, nameof(request.Skill)));
			}

			ability.DefensiveSkills.Add(new AbilityDefensiveSkill(ability.Id, request.Skill));
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task<CreatureTemplateDto> RemoveAbilityDefensiveSkillAsync(long creatureTemplateId, long abilityId, long defensiveSkillId)
		{
			var creatureTemplate = await GetByIdAsync(creatureTemplateId);
			var ability = AbilityHelpers.GetAbility(creatureTemplate.Abilities, abilityId);
			var defensiveSkill = ability.DefensiveSkills.FirstOrDefault(x => x.Id == defensiveSkillId);

			NotFoundException.ThrowIfNull(
				defensiveSkill, ErrorCode.AbilityDefensiveSkillNotFound, nameof(AbilityDefensiveSkill), nameof(AbilityDefensiveSkill.Id), defensiveSkillId.ToString());

			ability.DefensiveSkills.Remove(defensiveSkill);
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		private async Task<CreatureTemplate> GetByIdAsync(long id)
		{
			var creatureTemplate = await _creatureTemplateRepository.GetByIdAsync(id);

			NotFoundException.ThrowIfNull(
				creatureTemplate,
				ErrorCode.CreatureTemplateNotFound,
				nameof(CreatureTemplate),
				nameof(CreatureTemplate.Id),
				id.ToString());

			return creatureTemplate;
		}
	}
}
