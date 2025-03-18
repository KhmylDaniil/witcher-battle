using AutoMapper;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Models.Dto;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.Infrastructure.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<RegisterUserRequest, User>()
				.ForMember(dst => dst.Password, opt => opt.Ignore());

			CreateMap<Character, CharacterDto>()
				.ReverseMap();

			CreateMap<CreateCharacterRequest, Character>()
				.ForMember(dst => dst.UserId, opt => opt.Ignore());

			CreateMap<UpdateCharacterRequest, Character>()
				.ForMember(dst => dst.Id, opt => opt.Ignore())
				.ForMember(dst => dst.UserId, opt => opt.Ignore());
		}
	}
}
