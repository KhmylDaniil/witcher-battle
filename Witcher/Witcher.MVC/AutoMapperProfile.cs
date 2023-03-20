using AutoMapper;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Contracts.RunBattleRequests;
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
