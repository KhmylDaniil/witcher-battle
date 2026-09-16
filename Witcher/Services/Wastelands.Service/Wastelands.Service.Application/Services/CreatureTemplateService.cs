using AutoMapper;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Services
{
	public class CreatureTemplateService : ICreatureTemplateService
	{
		private readonly ICreatureTemplateRepository _creatureTemplateRepository;
		private readonly IBodyTemplateRepository _bodyTemplateRepository;
		private readonly IGameAccessGuard _gameAccessGuard;
		private readonly IMapper _mapper;

		public CreatureTemplateService(
			ICreatureTemplateRepository creatureTemplateRepository,
			IBodyTemplateRepository bodyTemplateRepository,
			IGameAccessGuard gameAccessGuard,
			IMapper mapper)
		{
			_creatureTemplateRepository = creatureTemplateRepository;
			_bodyTemplateRepository = bodyTemplateRepository;
			_gameAccessGuard = gameAccessGuard;
			_mapper = mapper;
		}

		public async Task<CreatureTemplateDto> GetCreatureTemplateByIdAsync(long id)
		{
			var creatureTemplate = await GetByIdAsync(id);
			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task<List<CreatureTemplateDto>> GetCreatureTemplatesAsync(CreatureTemplateFilter filter)
		{
			var creatureTemplates = await _creatureTemplateRepository.GetListByFilterAsync(filter);
			return _mapper.Map<List<CreatureTemplateDto>>(creatureTemplates);
		}

		public async Task<CreatureTemplateDto> CreateCreatureTemplateAsync(CreateCreatureTemplateRequest request)
		{
			var game = await _gameAccessGuard.GetGameOwnedByCurrentUserAsync(request.GameId);
			var bodyTemplate = await GetOwnBodyTemplateForGameAsync(request.BodyTemplateId, game.Id);

			var entity = new CreatureTemplate(
				gameId: game.Id,
				bodyTemplate: bodyTemplate,
				creatureType: request.CreatureType,
				name: request.Name,
				description: request.Description,
				hp: request.HP,
				sta: request.Sta,
				@int: request.Int,
				@ref: request.Ref,
				dex: request.Dex,
				body: request.Body,
				emp: request.Emp,
				cra: request.Cra,
				will: request.Will,
				speed: request.Speed,
				luck: request.Luck);

			await _creatureTemplateRepository.CreateAsync(entity);

			return _mapper.Map<CreatureTemplateDto>(entity);
		}

		public async Task<CreatureTemplateDto> UpdateCreatureTemplateAsync(UpdateCreatureTemplateRequest request)
		{
			var creatureTemplate = await GetByIdAsync(request.Id);

			creatureTemplate.UpdateCreatureTemplate(
				creatureType: request.CreatureType,
				name: request.Name,
				description: request.Description,
				hp: request.HP,
				sta: request.Sta,
				@int: request.Int,
				@ref: request.Ref,
				dex: request.Dex,
				body: request.Body,
				emp: request.Emp,
				cra: request.Cra,
				will: request.Will,
				speed: request.Speed,
				luck: request.Luck);

			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task<CreatureTemplateDto> UpdatePartArmorAsync(long creatureTemplateId, long partId, int armor)
		{
			var creatureTemplate = await GetByIdAsync(creatureTemplateId);
			var part = creatureTemplate.Parts.FirstOrDefault(x => x.Id == partId);

			NotFoundException.ThrowIfNull(
				part,
				ErrorCode.CreatureTemplatePartNotFound,
				nameof(CreatureTemplatePart),
				nameof(CreatureTemplatePart.Id),
				partId.ToString());

			part.UpdateArmor(armor);
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task DeleteCreatureTemplateAsync(long id)
		{
			var creatureTemplate = await GetByIdAsync(id);
			await _creatureTemplateRepository.DeleteAsync(creatureTemplate);
		}

		public async Task AddSkillAsync(AddOrUpdateCreatureTemplateSkillRequest request)
		{
			var creatureTemplate = await GetByIdAsync(request.CreatureTemplateId);

			if (creatureTemplate.Skills.ContainsKey(request.Skill))
			{
				throw new InvalidArgumentException(
					ErrorCode.CreatureTemplateSkillAlreadyExisted,
					string.Format(ExceptionMessages.ValueMustBeUnique, nameof(request.Skill)));
			}

			await AddOrUpdateSkillAsync(creatureTemplate, request.Skill, request.Value);
		}

		public async Task UpdateSkillAsync(AddOrUpdateCreatureTemplateSkillRequest request)
		{
			var creatureTemplate = await GetByIdAsync(request.CreatureTemplateId);
			await AddOrUpdateSkillAsync(creatureTemplate, request.Skill, request.Value);
		}

		public async Task DeleteSkillAsync(DeleteCreatureTemplateSkillRequest request)
		{
			var creatureTemplate = await GetByIdAsync(request.Id);
			creatureTemplate.Skills.Remove(request.Skill);
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);
		}

		public async Task SetDamageTypeModifierAsync(SetDamageTypeModifierRequest request)
		{
			var creatureTemplate = await GetByIdAsync(request.CreatureTemplateId);
			creatureTemplate.DamageTypeModifiers[request.DamageType] = request.Modifier;
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);
		}

		public async Task RemoveDamageTypeModifierAsync(long creatureTemplateId, DamageType damageType)
		{
			var creatureTemplate = await GetByIdAsync(creatureTemplateId);
			creatureTemplate.DamageTypeModifiers.Remove(damageType);
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);
		}

		public async Task<CreatureTemplateDto> AddAbilityAsync(CreateAbilityRequest request)
		{
			var creatureTemplate = await GetByIdAsync(request.CreatureTemplateId);

			var ability = new Ability(
				creatureTemplate.Id, request.Name, request.AttackSkill, request.AttacksPerTurn,
				request.DamageDiceCount, request.DamageModifier, request.DamageType);

			creatureTemplate.Abilities.Add(ability);
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task<CreatureTemplateDto> UpdateAbilityAsync(UpdateAbilityRequest request)
		{
			var creatureTemplate = await GetByIdAsync(request.CreatureTemplateId);
			var ability = GetAbility(creatureTemplate, request.AbilityId);

			ability.ChangeAbility(
				request.Name, request.AttackSkill, request.AttacksPerTurn,
				request.DamageDiceCount, request.DamageModifier, request.DamageType);

			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task<CreatureTemplateDto> RemoveAbilityAsync(long creatureTemplateId, long abilityId)
		{
			var creatureTemplate = await GetByIdAsync(creatureTemplateId);
			var ability = GetAbility(creatureTemplate, abilityId);

			creatureTemplate.Abilities.Remove(ability);
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task<CreatureTemplateDto> AddAbilityConditionAsync(AddAbilityConditionRequest request)
		{
			var creatureTemplate = await GetByIdAsync(request.CreatureTemplateId);
			var ability = GetAbility(creatureTemplate, request.AbilityId);

			ThrowIfConditionDuplicate(ability, request.Condition, excludeConditionId: null);

			ability.AppliedConditions.Add(new AbilityAppliedCondition(ability.Id, request.Condition, request.ApplyChance));
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task<CreatureTemplateDto> UpdateAbilityConditionAsync(UpdateAbilityConditionRequest request)
		{
			var creatureTemplate = await GetByIdAsync(request.CreatureTemplateId);
			var ability = GetAbility(creatureTemplate, request.AbilityId);
			var condition = ability.AppliedConditions.FirstOrDefault(x => x.Id == request.ConditionId);

			NotFoundException.ThrowIfNull(
				condition, ErrorCode.AbilityConditionNotFound, nameof(AbilityAppliedCondition), nameof(AbilityAppliedCondition.Id), request.ConditionId.ToString());

			ThrowIfConditionDuplicate(ability, request.Condition, excludeConditionId: request.ConditionId);

			condition.ChangeCondition(request.Condition, request.ApplyChance);
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task<CreatureTemplateDto> RemoveAbilityConditionAsync(long creatureTemplateId, long abilityId, long conditionId)
		{
			var creatureTemplate = await GetByIdAsync(creatureTemplateId);
			var ability = GetAbility(creatureTemplate, abilityId);
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
			var ability = GetAbility(creatureTemplate, request.AbilityId);

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
			var ability = GetAbility(creatureTemplate, abilityId);
			var defensiveSkill = ability.DefensiveSkills.FirstOrDefault(x => x.Id == defensiveSkillId);

			NotFoundException.ThrowIfNull(
				defensiveSkill, ErrorCode.AbilityDefensiveSkillNotFound, nameof(AbilityDefensiveSkill), nameof(AbilityDefensiveSkill.Id), defensiveSkillId.ToString());

			ability.DefensiveSkills.Remove(defensiveSkill);
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		private async Task AddOrUpdateSkillAsync(CreatureTemplate creatureTemplate, Skill skill, int value)
		{
			InvalidArgumentException.ThrowIfNotInRange(value, CreatureTemplate.MinSkillValue, CreatureTemplate.MaxSkillValue, nameof(value));
			creatureTemplate.Skills[skill] = value;
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);
		}

		private static Ability GetAbility(CreatureTemplate creatureTemplate, long abilityId)
		{
			var ability = creatureTemplate.Abilities.FirstOrDefault(x => x.Id == abilityId);

			NotFoundException.ThrowIfNull(
				ability, ErrorCode.AbilityNotFound, nameof(Ability), nameof(Ability.Id), abilityId.ToString());

			return ability;
		}

		private static void ThrowIfConditionDuplicate(Ability ability, Condition condition, long? excludeConditionId)
		{
			if (ability.AppliedConditions.Any(x => x.Condition == condition && x.Id != excludeConditionId))
			{
				throw new InvalidArgumentException(
					ErrorCode.AbilityConditionAlreadyExisted,
					string.Format(ExceptionMessages.ValueMustBeUnique, nameof(condition)));
			}
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

		private async Task<BodyTemplate> GetOwnBodyTemplateForGameAsync(long bodyTemplateId, long gameId)
		{
			// BodyTemplateRepository уже скоупит выдачу на шаблоны игр текущего пользователя — если чужой
			// (или несуществующий) Id, здесь придёт null независимо от переданного gameId.
			var bodyTemplate = await _bodyTemplateRepository.GetByIdAsync(bodyTemplateId);
			NotFoundException.ThrowIfNull(bodyTemplate, ErrorCode.BodyTemplateNotFound, nameof(BodyTemplate), nameof(BodyTemplate.Id), bodyTemplateId.ToString());

			if (bodyTemplate.GameId != gameId)
			{
				throw new InvalidArgumentException(
					ErrorCode.BodyTemplateBelongsToAnotherGame,
					"Выбранный шаблон тела принадлежит другой игре.");
			}

			return bodyTemplate;
		}
	}
}
