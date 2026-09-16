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
				.ForMember(dst => dst.CharacterName, opt => opt.MapFrom(src => src.Character.Name));
		}
	}
}
