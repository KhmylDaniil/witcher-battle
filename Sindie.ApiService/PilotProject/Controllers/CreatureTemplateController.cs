using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PilotProject.DbContext;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.CreateCreatureTemplate;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.CreateCreatureTemplate;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.GetCreatureTemplate;
using Sindie.ApiService.Core.Services.Authorization;
using Sindie.ApiService.Core.Services.DateTimeProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PilotProject.Controllers
{
	internal class CreatureTemplateController
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Сервис авторизации
		/// </summary>
		private readonly IAuthorizationService _authorizationService;

		/// <summary>
		/// Провайдер времени
		/// </summary>
		private readonly IDateTimeProvider _dateTimeProvider;

		public CreatureTemplateController(IAppDbContext appDbContext, IAuthorizationService authorizationService, IDateTimeProvider dateTimeProvider)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
			_dateTimeProvider = dateTimeProvider;
		}

		public async Task GetAsync()
		{
			GetCreatureTemplateQuery query = new GetCreatureTemplateQuery();
			query.GameId = TestDbContext.GameId;

			var newHandler = new GetCreatureTemplateHandler(_appDbContext, _authorizationService, _dateTimeProvider);

			var result = await newHandler.Handle(GetCommandFromQuery(query), default);

			if (result == null) throw new ApplicationException("Get CT query is null");

			Console.WriteLine($"There are {result.TotalCount} aviable creature templates:");
			for (int i = 0; i < result.TotalCount; i++)
			{
				Console.WriteLine($"Name: {result.CreatureTemplatesList[i].Name}. If you want to look closer, press {i + 1}.");
			}
			Console.WriteLine("If you want to create creature template, press 0");

			int input = 0;

			while (!int.TryParse(Console.ReadLine(), out input) || (input < 0 && input > result.TotalCount)) ;

			if (input == 0)
				await CreateAsync();




			Console.ReadLine();

			GetCreatureTemplateCommand GetCommandFromQuery(GetCreatureTemplateQuery request)
			{
				return request == null
						? throw new ArgumentNullException(nameof(request))
						: new GetCreatureTemplateCommand(
							gameId: request.GameId,
							name: request.Name,
							creatureTypeId: request.CreatureTypeId,
							userName: request.UserName,
							creationMaxTime: request.CreationMaxTime,
							creationMinTime: request.CreationMinTime,
							modificationMaxTime: request.ModificationMaxTime,
							modificationMinTime: request.ModificationMinTime,
							bodyTemplateName: request.BodyTemplateName,
							bodyPartTypeId: request.BodyPartTypeId,
							conditionName: request.ConditionName,
							pageNumber: request.PageNumber,
							pageSize: request.PageSize,
							orderBy: request.OrderBy,
							isAscending: request.IsAscending);
			}
		}

		public async Task CreateAsync()
		{
			Console.WriteLine("Enter creature template data");

			Console.WriteLine("Enter name");
			string name = string.Empty;
			while (string.IsNullOrEmpty(name))
				name = Console.ReadLine();

			Console.WriteLine("Enter description");
			string description = string.Empty;
			description = Console.ReadLine();

			Console.WriteLine("Enter HP");
			int hp = 0;
			while (!int.TryParse(Console.ReadLine(), out hp) || hp < 1) ;

			int sta = hp;

			Console.WriteLine("Enter Intellect");
			int @int = 0;
			while (!int.TryParse(Console.ReadLine(), out @int) || @int < 1) ;

			Console.WriteLine("Enter Reflexes");
			int @ref = 0;
			while (!int.TryParse(Console.ReadLine(), out @ref) || @ref < 1) ;

			Console.WriteLine("Enter Dexterity");
			int dex = 0;
			while (!int.TryParse(Console.ReadLine(), out dex) || @dex < 1) ;

			Console.WriteLine("Enter Body");
			int body = 0;
			while (!int.TryParse(Console.ReadLine(), out body) || body < 1) ;

			Console.WriteLine("Enter Empathy");
			int emp = 0;
			while (!int.TryParse(Console.ReadLine(), out emp) || emp < 1) ;

			Console.WriteLine("Enter Craft");
			int cra = 0;
			while (!int.TryParse(Console.ReadLine(), out cra) || cra < 1) ;

			Console.WriteLine("Enter Will");
			int will = 0;
			while (!int.TryParse(Console.ReadLine(), out will) || will < 1) ;

			Console.WriteLine("Enter Speed");
			int speed = 0;
			while (!int.TryParse(Console.ReadLine(), out speed) || speed < 1) ;

			Console.WriteLine("Enter Luck");
			int luck = 0;
			while (!int.TryParse(Console.ReadLine(), out luck) || luck < 1) ;

			Console.WriteLine("Pick up one ability: sword (press 1) or bow (press 2) or claws (press 3).");

			int input = 0;

			while (input < 1 && input > 3 )
				int.TryParse(Console.ReadLine(), out input);

			var abillityId = input switch
			{
				case 1:
				{
				_appDbContext.Abilities.FirstOrDefault(x => x.Name.Equals("SwordAttack")).Id;
				}
			}
			
			//	case 1: 
			//	{
			//	_appDbContext.Abilities.FirstOrDefault(x => x.Name.Equals("SwordAttack")).Id;
			//	break;
			
			//	_ => throw new NotImplementedException(),
			//};
			//var abilityId = input == 1
			//	? _appDbContext.Abilities.FirstOrDefault(x => x.Name.Equals("SwordAttack")).Id
			//	: _appDbContext.Abilities.FirstOrDefault(x => x.Name.Equals("ArcheryAttack")).Id;


			var bodyTemplateParts = _appDbContext.BodyTemplateParts.Where(x => x.BodyTemplateId == TestDbContext.BodyTemplateId).ToList();

			var armorList = new List<CreateCreatureTemplateRequestArmorList>();

			Console.WriteLine("Time to add armor.");
			foreach (var part in bodyTemplateParts)
			{
				Console.WriteLine($"Add some armor to {part.Name}. Please do not add more then ten armor per body part.");
				int armor = 0;
				while (!int.TryParse(Console.ReadLine(), out armor) || armor < 0) ;

				armorList.Add(new CreateCreatureTemplateRequestArmorList { Armor = armor, BodyTemplatePartId = part.Id });
			}

			var skills = _appDbContext.Skills.ToList();
			var createCreatureTemplateRequestSkills = new List<CreateCreatureTemplateRequestSkill>();

			Console.WriteLine("You need to add some skills.");

			foreach (var skill in skills)
			{
				Console.WriteLine($"Enter value  to {skill.Name} skill. Please do not enter more then ten.");
				int value = 0;
				while (!int.TryParse(Console.ReadLine(), out value) || value < 0) ;

				createCreatureTemplateRequestSkills.Add(new CreateCreatureTemplateRequestSkill { SkillId = skill.Id, Value = value });
			}

			CreateCreatureTemplateRequest request = new()
			{
				GameId = TestDbContext.GameId,
				ImgFileId = null,
				BodyTemplateId = TestDbContext.BodyTemplateId,
				CreatureTypeId = CreatureTypes.HumanId,
				Name = name,
				Description = description,
				HP = hp,
				Sta = hp,
				Int = @int,
				Ref = @ref,
				Dex = dex,
				Body = body,
				Emp = emp,
				Cra = cra,
				Will = will,
				Speed = speed,
				Luck = luck,
				Abilities = new List<Guid> { abilityId },
				ArmorList = armorList,
				CreatureTemplateSkills = createCreatureTemplateRequestSkills
			};

			var newHandler = new CreateCreatureTemplateHandler(_appDbContext, _authorizationService);

			var result = newHandler.Handle(CreateCommandFromQuery(request), default);

			GetAsync();

			CreateCreatureTemplateCommand CreateCommandFromQuery(CreateCreatureTemplateRequest request)
			{
				return request == null
					? throw new ArgumentNullException(nameof(request))
					: new CreateCreatureTemplateCommand(
						gameId: request.GameId,
						imgFileId: request.ImgFileId,
						bodyTemplateId: request.BodyTemplateId,
						creatureTypeId: request.CreatureTypeId,
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
						luck: request.Luck,
						armorList: request.ArmorList,
						abilities: request.Abilities,
						creatureTemplateSkills: request.CreatureTemplateSkills);
			}
		}
	}
}
