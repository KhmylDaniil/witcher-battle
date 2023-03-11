using AutoMapper;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests;
using Sindie.ApiService.Core.Contracts.RunBattleRequests;
using Witcher.MVC.ViewModels.CreatureTemplate;
using Witcher.MVC.ViewModels.RunBattle;

namespace Witcher.MVC
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<MakeTurnResponse, MakeTurnViewModel>();
			CreateMap<AttackCommand, MakeAttackViewModel>();
			CreateMap<HealEffectCommand, MakeHealViewModel>();

			CreateMap<CreateCreatureTemplateCommand, CreateCreatureTemplateCommandViewModel>();
			CreateMap<ChangeCreatureTemplateCommand, ChangeCreatureTemplateCommandViewModel>();
		}
	}
}
