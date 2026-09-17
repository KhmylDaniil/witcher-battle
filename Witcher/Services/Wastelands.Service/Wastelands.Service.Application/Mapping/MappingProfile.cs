using AutoMapper;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<RegisterUserRequest, User>()
				.ForMember(dst => dst.Password, opt => opt.Ignore());

			CreateMap<Character, CharacterDto>();

			CreateMap<Game, GameDto>();
			CreateMap<GameJoinRequest, GameJoinRequestDto>();

			CreateMap<BodyTemplatePart, BodyTemplatePartDto>();
			CreateMap<BodyTemplate, BodyTemplateDto>();
			CreateMap<CreatureTemplatePart, CreatureTemplatePartDto>();
			CreateMap<CreatureTemplate, CreatureTemplateDto>();
			CreateMap<Ability, AbilityDto>();
			CreateMap<AbilityAppliedCondition, AbilityAppliedConditionDto>();
			CreateMap<AbilityDefensiveSkill, AbilityDefensiveSkillDto>();

			CreateMap<Battle, BattleDto>();
			CreateMap<Creature, BattleCreatureDto>();
			CreateMap<BattleCharacter, BattleCharacterDto>()
				.ForMember(dst => dst.CharacterName, opt => opt.MapFrom(src => src.Character.Name))
				.ForMember(dst => dst.CharacterUserId, opt => opt.MapFrom(src => src.Character.UserId));
			// Attacker/Ability/Defender имена, справочное значение навыка и списки доступных выборов —
			// не поля сущности, а вычисляются сервисом (BattleService.EnrichAttackDtoAsync) после маппинга,
			// т.к. требуют подгрузки CreatureTemplate/Character/Ability, которых у BattleAttack нет.
			CreateMap<BattleAttack, BattleAttackDto>();
			CreateMap<BattleLogEntry, BattleLogEntryDto>();
		}
	}
}
