using AutoMapper;
using Sindie.ApiService.Core.Contracts.BattleRequests;
using Sindie.ApiService.Core.Contracts.RunBattleRequests;
using Witcher.MVC.ViewModels.RunBattle;

namespace Witcher.MVC
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<MakeTurnResponse, MakeTurnViewModel>();
		}
	}
}
