using AutoMapper;
using Witcher.Core.Contracts.ArmorTemplateRequests;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Contracts.UserGameRequests;
using Witcher.MVC.ViewModels.ArmorTemplate;
using Witcher.MVC.ViewModels.Battle;
using Witcher.MVC.ViewModels.CreatureTemplate;
using Witcher.MVC.ViewModels.Game;
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

			CreateMap<CreateArmorTemplateCommand, CreateArmorTemplateViewModel>();

			CreateMap<CreateCreatureCommand, CreateCreatureCommandViewModel>();
			CreateMap<AddCharacterToBattleCommand, AddCharacterToBattleViewModel>();
			CreateMap<CreateUserGameCommand, CreateUserGameCommandViewModel>();
		}
	}
}
