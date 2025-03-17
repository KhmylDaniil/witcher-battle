using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.Infrastructure.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<RegisterUserRequest, User>()
		   .ForMember(dst => dst.Password, opt => opt.Ignore());
		}
	}
}
