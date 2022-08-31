using Microsoft.EntityFrameworkCore;
using PilotProject.DbContext;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.BattleRequests.CreateBattle;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.ChangeCreatureTemplate;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.CreateCreatureTemplate;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.DeleteCreatureTemplateById;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplate;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplateById;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.ChangeCreatureTemplate;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.CreateCreatureTemplate;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.DeleteCreatureTemplateById;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.GetCreatureTemplate;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.GetCreatureTemplateById;
using System.Text.Json;

namespace PilotProject.Controllers
{
	/// <summary>
	/// Контроллер шаблона существа
	/// </summary>
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

		/// <summary>
		/// Бросок параметра
		/// </summary>
		private readonly IRollService _rollService;

		/// <summary>
		/// Словарь для сбора данных о бое
		/// </summary>
		public static readonly Dictionary<string, Guid> PickedCreatureTemplates = new();

		/// <summary>
		/// Конструктор контроллера шаблона существа
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		/// <param name="dateTimeProvider">Провайдер времени</param>
		public CreatureTemplateController(IAppDbContext appDbContext, IAuthorizationService authorizationService, IDateTimeProvider dateTimeProvider, IRollService rollService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
			_dateTimeProvider = dateTimeProvider;
			_rollService = rollService;
		}

		/// <summary>
		/// Метод вывода списка шаблонов существ 
		/// </summary>
		/// <returns></returns>
		public async Task<Task> GetAsync()
		{
			GetCreatureTemplateQuery query = new GetCreatureTemplateQuery();
			query.GameId = TestDbContext.GameId;

			var newHandler = new GetCreatureTemplateHandler(_appDbContext, _authorizationService, _dateTimeProvider);

			var result = await newHandler.Handle(GetCommandFromQuery(query), default);

			if (result == null) throw new ApplicationException("Get CT query is null");

			Console.WriteLine($"\nThere are {result.TotalCount} aviable creature templates:");
			for (int i = 0; i < result.TotalCount; i++)
			{
				Console.WriteLine($"Name: {result.CreatureTemplatesList[i].Name}. If you want to look closer, press {i + 1}.");
			}
			Console.WriteLine("If you want to create creature template, press 0");

			int input = 0;

			while (!int.TryParse(Console.ReadLine(), out input) || (input < 0 && input > result.TotalCount)) ;

			if (input == 0)
				return CreateAsync();

			else
				return GetByIdAsync(result.CreatureTemplatesList[input - 1].Id);

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

		/// <summary>
		/// Метод создания шаблона существа с набором данных через консоль
		/// </summary>
		/// <returns></returns>
		public async Task<Task> CreateAsync()
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

			while (int.TryParse(Console.ReadLine(), out input) && (input < 1 || input > 3 ));

			var abilityId = input switch
			{
				1 => _appDbContext.Abilities.FirstOrDefault(x => x.Name.Equals("SwordAttack")).Id,
				2 => _appDbContext.Abilities.FirstOrDefault(x => x.Name.Equals("ArcheryAttack")).Id,
				3 => _appDbContext.Abilities.FirstOrDefault(x => x.Name.Equals("ClawsAttack")).Id
			};

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
				Console.WriteLine($"Enter value to {skill.Name} skill. Please do not enter more then ten.");
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

			await newHandler.Handle(CreateCommandFromQuery(request), default);

			return GetAsync();

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

		/// <summary>
		/// Получение шаблона существа по айди
		/// </summary>
		/// <param name="id">айди</param>
		/// <returns></returns>
		public async Task<Task> GetByIdAsync(Guid id)
		{
			GetCreatureTemplateByIdQuery query = new GetCreatureTemplateByIdQuery() { GameId = TestDbContext.GameId, Id = id};

			var newHandler = new GetCreatureTemplateByIdHandler(_appDbContext, _authorizationService);

			var result = await newHandler.Handle(query, default);

			if (result == null) throw new ApplicationException($"Creature Template with id {id} is not found");

			Console.WriteLine($"Name: {result.Name}\n Description: {result.Description}\n HP: {result.HP}\n Stamina: {result.Sta}\n" +
				$" Int: {result.Int}\n Ref: {result.Ref}\n Dex {result.Dex}, Body: {result.Body}\n Emp: {result.Emp}\n Cra: {result.Cra}\n Will: {result.Will}\n" +
				$" Luck: {result.Luck}\n Speed: {result.Speed}");

			if (result.CreatureTemplateParts.Any())
				ViewCreatureTemplateParts(result);

			if (result.Abilities.Any())
				ViewAbilities(result);

			if (result.CreatureTemplateSkills.Any())
				ViewSkills(result);

			Console.WriteLine("\nYou can edit this creature template (press 1), delete it (press 2), pick it up to battle (press 3) or return to general view (press 0)");

			int input = 0;

			while (!int.TryParse(Console.ReadLine(), out input) || (input < 0 && input > 2)) ;


			switch (input)
			{
				case 0:
					return await GetAsync();
				case 1:
					return await ChangeAsync(id);
				case 2:
					return await DeleteAsync(id);
				case 3:
					return await GoToBattleAsync(id);
			}
			return await GetAsync();

			static void ViewCreatureTemplateParts(GetCreatureTemplateByIdResponse result)
			{
				Console.WriteLine($"This creature template has {result.CreatureTemplateParts.Count} body parts.");

				foreach (var part in result.CreatureTemplateParts)
					Console.WriteLine($"{part.Name} has {part.Armor} armor, has damage modifier {part.DamageModifier} " +
						$"and can be hit on {part.MinToHit}-{part.MaxToHit} or aimed with penalty {part.HitPenalty}");
			}

			static void ViewAbilities(GetCreatureTemplateByIdResponse result)
			{
				Console.WriteLine($"This creature template has {result.Abilities.Count} ability.");

				foreach (var ability in result.Abilities)
				{
					Console.WriteLine($"{ability.Name} brings {ability.AttackDiceQuantity}d6+{ability.DamageModifier} damage. Attacking skill is {ability.AttackParameterName}.");

					if (ability.AppliedConditions.Any())
					{
						Console.WriteLine("This ablity can apply:");
						foreach (var appliedCondition in ability.AppliedConditions)
							Console.WriteLine($"{appliedCondition.ConditionName} with {appliedCondition.ApplyChance}% chance.");
					}
				}
			}

			static void ViewSkills(GetCreatureTemplateByIdResponse result)
			{
				Console.WriteLine($"This creature template has {result.CreatureTemplateSkills.Count} skills");
				foreach (var skill in result.CreatureTemplateSkills)
					Console.WriteLine($"{skill.SkillName} value: {skill.SkillValue}.");
			}
		}

		/// <summary>
		/// Удаление шаблона существа по айди
		/// </summary>
		/// <param name="id">айди</param>
		/// <returns></returns>
		public async Task<Task> DeleteAsync(Guid id)
		{
			DeleteCreatureTemplateByIdCommand command = new DeleteCreatureTemplateByIdCommand { GameId = TestDbContext.GameId, Id = id};

			var newHandler = new DeleteCreatureTemplateByIdHandler(_appDbContext, _authorizationService);

			await newHandler.Handle(command, default);

			Console.WriteLine($"Creature template with id {id} is deleted.");

			return await GetAsync();
		}

		/// <summary>
		/// Изменение шаблона существа с набором данных через консоль
		/// </summary>
		/// <returns></returns>
		public async Task<Task> ChangeAsync(Guid id)
		{
			var creatureTemplate = _appDbContext.CreatureTemplates.FirstOrDefault(x => x.Id == id)
				?? throw new ExceptionEntityNotFound<CreatureTemplate>(id);

			Console.WriteLine("Enter creature template data");

			Console.WriteLine($"Enter name");
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

			while (int.TryParse(Console.ReadLine(), out input) && (input < 1 || input > 3)) ;

			var abilityId = input switch
			{
				1 => _appDbContext.Abilities.FirstOrDefault(x => x.Name.Equals("SwordAttack")).Id,
				2 => _appDbContext.Abilities.FirstOrDefault(x => x.Name.Equals("ArcheryAttack")).Id,
				3 => _appDbContext.Abilities.FirstOrDefault(x => x.Name.Equals("ClawsAttack")).Id
			};

			var bodyTemplateParts = _appDbContext.BodyTemplateParts.Where(x => x.BodyTemplateId == TestDbContext.BodyTemplateId).ToList();

			var armorList = new List<ChangeCreatureTemplateRequestArmorList>();

			Console.WriteLine("Time to add armor.");
			foreach (var part in bodyTemplateParts)
			{
				Console.WriteLine($"Add some armor to {part.Name}. Please do not add more then ten armor per body part.");
				int armor = 0;
				while (!int.TryParse(Console.ReadLine(), out armor) || armor < 0) ;

				armorList.Add(new ChangeCreatureTemplateRequestArmorList { Armor = armor, BodyTemplatePartId = part.Id });
			}

			var skills = _appDbContext.Skills.ToList();
			var changeCreatureTemplateRequestSkills = new List<ChangeCreatureTemplateRequestSkill>();

			Console.WriteLine("You need to add some skills.");

			foreach (var skill in skills)
			{
				var existingCTSkill = creatureTemplate.CreatureTemplateSkills.FirstOrDefault(x => x.SkillId == skill.Id);

				Console.WriteLine($"Enter value to {skill.Name} skill. Please do not enter more then ten.");
				int value = 0;
				while (!int.TryParse(Console.ReadLine(), out value) || value < 0);

				changeCreatureTemplateRequestSkills.Add(new ChangeCreatureTemplateRequestSkill
				{
					Id = existingCTSkill?.Id,
					SkillId = skill.Id,
					Value = value 
				});
			}

			ChangeCreatureTemplateRequest request = new()
			{
				Id = id,
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
				CreatureTemplateSkills = changeCreatureTemplateRequestSkills
			};

			var newHandler = new ChangeCreatureTemplateHandler(_appDbContext, _authorizationService);

			await newHandler.Handle(CreateCommandFromQuery(request), default);

			ChangeCreatureTemplateCommand CreateCommandFromQuery(ChangeCreatureTemplateRequest request)
			{
				return request == null
					? throw new ArgumentNullException(nameof(request))
					: new ChangeCreatureTemplateCommand(
						id: id,
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

			return GetAsync();
		}

		public async Task<Task> GoToBattleAsync(Guid id)
		{
			Console.WriteLine($"Enter unique name for this creature");
			string name = string.Empty;
			while (string.IsNullOrEmpty(name))
				name = Console.ReadLine();

			PickedCreatureTemplates.Add(name, id);

			if (PickedCreatureTemplates.Count != 2)
				return GetAsync();

			var creatures = new List<CreateBattleRequestItem>();

			foreach (var pickedCreature in PickedCreatureTemplates)
				creatures.Add(new CreateBattleRequestItem
				{
					CreatureTemplateId = pickedCreature.Value,
					Name = pickedCreature.Key
				});

			CreateBattleRequest request = new()
			{
				GameId = TestDbContext.GameId,
				ImgFileId = null,
				Name = "TestName",
				Description = null,
				Creatures = creatures
			};

			PickedCreatureTemplates.Clear();

			//using (FileStream fs = new FileStream(Constants.BattlePath, FileMode.OpenOrCreate))
			//	await JsonSerializer.SerializeAsync(fs, request);

			var newBattleController = new BattleController(_appDbContext, _authorizationService, _rollService);

			return newBattleController.CreateBattle();
		}
	}
}
