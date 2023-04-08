﻿using AutoMapper;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Contracts.UserGameRequests;
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
			CreateMap<AttackWithAbilityCommand, MakeAttackViewModel>();
			CreateMap<HealEffectCommand, MakeHealViewModel>();

			CreateMap<CreateCreatureTemplateCommand, CreateCreatureTemplateCommandViewModel>();
			CreateMap<ChangeCreatureTemplateCommand, ChangeCreatureTemplateCommandViewModel>();

			CreateMap<CreateCreatureCommand, CreateCreatureCommandViewModel>();
			CreateMap<AddCharacterToBattleCommand, AddCharacterToBattleViewModel>();
			CreateMap<CreateUserGameCommand, CreateUserGameCommandViewModel>();
		}
	}
}
