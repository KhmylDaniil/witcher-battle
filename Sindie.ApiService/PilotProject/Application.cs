using PilotProject.CommandBuilders;
using PilotProject.Files;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.BattleRequests.CreatureAttack;
using Sindie.ApiService.Core.Contracts.BattleRequests.TurnBeginning;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.DeleteCreatureTemplateById;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplate;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplateById;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Requests.BattleRequests;
using Sindie.ApiService.Core.Requests.BattleRequests.CreateBattle;
using Sindie.ApiService.Core.Requests.BattleRequests.CreatureAttack;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.ChangeCreatureTemplate;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.CreateCreatureTemplate;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.DeleteCreatureTemplateById;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.GetCreatureTemplate;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.GetCreatureTemplateById;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace PilotProject
{
	internal class Application
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

		public Application(IAppDbContext appDbContext, IAuthorizationService authorizationService, IDateTimeProvider dateTimeProvider, IRollService rollService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
			_dateTimeProvider = dateTimeProvider;
			_rollService = rollService;
		}

		/// <summary>
		/// Запуск приложения
		/// </summary>
		public async void Run()
		{
			Console.WriteLine("Welcome to Witcher battle helper.");

			await LoadCT();

			await GetAsync();
		}

		/// <summary>
		/// Метод вывода списка шаблонов существ 
		/// </summary>
		/// <returns></returns>
		public async Task<Task> GetAsync()
		{
			var command = CTCommandBuilder.FormGetCTCommand();

			var newHandler = new GetCreatureTemplateHandler(_appDbContext, _authorizationService, _dateTimeProvider);

			var result = await newHandler.Handle(command, default);

			if (result == null) throw new ApplicationException("Get CT query is null");

			ViewResult(result);

			int input;
			while (!int.TryParse(Console.ReadLine(), out input) || input < 0 || input > result.TotalCount) ;

			if (input == 0)
				return CreateAsync();

			else
				return GetByIdAsync(result.CreatureTemplatesList[input - 1].Id);

			static void ViewResult(GetCreatureTemplateResponse result)
			{
				Console.WriteLine($"\nThere are {result.TotalCount} aviable creature templates:");
				for (int i = 0; i < result.TotalCount; i++)
				{
					Console.WriteLine($"Name: {result.CreatureTemplatesList[i].Name}. If you want to look closer, press {i + 1}.");
				}
				Console.WriteLine("If you want to create creature template, press 0");
			}
		}

		/// <summary>
		/// Метод создания шаблона существа с набором данных через консоль
		/// </summary>
		/// <returns></returns>
		public async Task<Task> CreateAsync()
		{
			var command = CTCommandBuilder.FormCreateCTCommand(_appDbContext);

			var newHandler = new CreateCreatureTemplateHandler(_appDbContext, _authorizationService);

			await newHandler.Handle(command, default);

			return GetAsync();
		}

		/// <summary>
		/// Получение шаблона существа по айди
		/// </summary>
		/// <param name="id">айди</param>
		/// <returns></returns>
		public async Task<Task> GetByIdAsync(Guid id)
		{
			var query = CTCommandBuilder.FormGetCTByIdQuery(id);

			var newHandler = new GetCreatureTemplateByIdHandler(_appDbContext, _authorizationService);

			var result = await newHandler.Handle(query, default);

			if (result == null) throw new ApplicationException($"Creature Template with id {id} is not found");

			ViewResult(result);

			Console.WriteLine("\nYou can edit this creature template (press 1), delete it (press 2), save it (press 3), pick it up to battle (press 4) or return to general view (press 0)");

			int input;
			while (!int.TryParse(Console.ReadLine(), out input) || input < 0 || input > 4) ;

			switch (input)
			{
				case 0:
					return await GetAsync();
				case 1:
					return await ChangeAsync(id);
				case 2:
					return await DeleteAsync(id);
				case 3:
					FilesManager.SaveCT(result, _appDbContext);
					return await GetAsync();
				case 4:
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
					Console.WriteLine($"{ability.Name} brings {ability.AttackDiceQuantity}d6+{ability.DamageModifier} damage. Attacking skill is {Enum.GetName(ability.AttackSkill)}.");

					if (ability.AppliedConditions.Any())
					{
						Console.WriteLine("This ablity can apply:");
						foreach (var appliedCondition in ability.AppliedConditions)
							Console.WriteLine($"{CritNames.GetConditionFullName(appliedCondition.Condition)} with {appliedCondition.ApplyChance}% chance.");
					}
				}
			}

			static void ViewSkills(GetCreatureTemplateByIdResponse result)
			{
				Console.WriteLine($"This creature template has {result.CreatureTemplateSkills.Count} skills");
				foreach (var skill in result.CreatureTemplateSkills)
					Console.WriteLine($"{Enum.GetName(skill.Skill)} value: {skill.SkillValue}.");
			}

			static void ViewResult(GetCreatureTemplateByIdResponse result)
			{
				Console.WriteLine($"Name: {result.Name}\n Description: {result.Description}\n HP: {result.HP}\n Stamina: {result.Sta}\n" +
								$" Int: {result.Int}\n Ref: {result.Ref}\n Dex {result.Dex}, Body: {result.Body}\n Emp: {result.Emp}\n Cra: {result.Cra}\n Will: {result.Will}\n" +
								$" Luck: {result.Luck}\n Speed: {result.Speed}");

				if (result.CreatureTemplateParts.Any())
					ViewCreatureTemplateParts(result);

				if (result.Abilities.Any())
					ViewAbilities(result);

				if (result.CreatureTemplateSkills.Any())
					ViewSkills(result);
			}
		}

		/// <summary>
		/// Удаление шаблона существа по айди
		/// </summary>
		/// <param name="id">айди</param>
		/// <returns></returns>
		public async Task<Task> DeleteAsync(Guid id)
		{
			DeleteCreatureTemplateByIdCommand command = CTCommandBuilder.FormDeleteCTCommand(id);

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
			ChangeCreatureTemplateCommand command = CTCommandBuilder.FormChangeCTCommand(id, _appDbContext);

			var newHandler = new ChangeCreatureTemplateHandler(_appDbContext, _authorizationService);

			await newHandler.Handle(command, default);

			return GetAsync();
		}

		/// <summary>
		/// Запись существа для битвы
		/// </summary>
		/// <param name="id">Айди</param>
		/// <returns></returns>
		public async Task<Task> GoToBattleAsync(Guid id)
		{
			BattleCommandBuilder.PickCreature(id);

			if (BattleCommandBuilder.PickedCreatureTemplates.Count != 2)
				return GetAsync();

			var command = BattleCommandBuilder.FormCreateBattleCommand();

			var newHandler = new CreateBattleHandler(_appDbContext, _authorizationService);

			await newHandler.Handle(command, default);

			return RunBattle();
		}

		/// <summary>
		/// Проведение битвы
		/// </summary>
		/// <returns></returns>
		public async Task<Task> RunBattle()
		{
			var battle = _appDbContext.Battles.FirstOrDefault() ?? throw new ExceptionEntityNotFound<Battle>();

			CreatureAttackRequest firstCreatureAttackRequest = new()
			{
				BattleId = battle.Id,
				AttackerId = battle.Creatures[0].Id,
				TargetCreatureId = battle.Creatures[1].Id,
			};

			CreatureAttackRequest secondCreatureAttackRequest = new()
			{
				BattleId = battle.Id,
				AttackerId = battle.Creatures[1].Id,
				TargetCreatureId = battle.Creatures[0].Id,
			};

			TurnBeginningCommand firstCreatureTurnBeginningRequest = new()
			{
				BattleId = battle.Id,
				CreatureId = battle.Creatures[0].Id
			};

			TurnBeginningCommand secondCreatureTurnBeginningRequest = new()
			{
				BattleId = battle.Id,
				CreatureId = battle.Creatures[1].Id
			};

			CreatureAttackHandler creatureAttackHandler = new(_appDbContext, _authorizationService, _rollService);
			TurnBeginningHandler turnBeginningHandler = new(_appDbContext, _authorizationService, _rollService);

			while (true)
			{
				Console.WriteLine((await turnBeginningHandler.Handle(firstCreatureTurnBeginningRequest, default)).Message);

				if (IsBattleOver(battle)) break;

				Console.WriteLine((await creatureAttackHandler.Handle(CreatureAttackCommandFromRequest(firstCreatureAttackRequest), default)).Message);

				if (IsBattleOver(battle)) break;

				Console.ReadKey(true);

				Console.WriteLine((await turnBeginningHandler.Handle(secondCreatureTurnBeginningRequest, default)).Message);

				if (IsBattleOver(battle)) break;

				Console.WriteLine((await creatureAttackHandler.Handle(CreatureAttackCommandFromRequest(secondCreatureAttackRequest), default)).Message);

				if (IsBattleOver(battle)) break;

				Console.ReadKey(true);
			}

			_appDbContext.Battles.Remove(battle);
			await _appDbContext.SaveChangesAsync();
			return GetAsync();

			static bool IsBattleOver(Battle battle)
				=> battle.Creatures.Count != 2;

			static CreatureAttackCommand CreatureAttackCommandFromRequest(CreatureAttackRequest request)
				=> request == null
					? throw new ArgumentNullException(nameof(request))
					: new CreatureAttackCommand(
						battleId: request.BattleId,
						attackerId: request.AttackerId,
						abilityId: request.AbilityId,
						targetCreatureId: request.TargetCreatureId,
						creaturePartId: request.CreaturePartId,
						defensiveSkill: request.DefensiveSkill,
						specialToHit: request.SpecialToHit,
						specialToDamage: request.SpecialToDamage);
		}

		/// <summary>
		/// Загрузка сохраненной базы данных
		/// </summary>
		/// <returns></returns>
		private async Task LoadCT()
		{
			var commands = FilesManager.LoadCT();

			var handler = new CreateCreatureTemplateHandler(_appDbContext, _authorizationService);

			foreach (var command in commands)
				await handler.Handle(command, default);
		}
	}
}
