using PilotProject.DbContext;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.ChangeCreatureTemplate;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.CreateCreatureTemplate;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.DeleteCreatureTemplateById;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplate;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplateById;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.ChangeCreatureTemplate;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.CreateCreatureTemplate;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.GetCreatureTemplate;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace PilotProject.CommandBuilders
{
	/// <summary>
	/// Создание команд для работы с шаблонами существ
	/// </summary>
	internal class CTCommandBuilder
	{
		internal static CreateCreatureTemplateCommand FormCreateCTCommand(IAppDbContext appDbContext)
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

			Guid abilityId = ChooseAbility(appDbContext);

			List<CreateCreatureTemplateRequestArmorList> armorList = CreateArmorList(appDbContext);

			List<CreateCreatureTemplateRequestSkill> createCreatureTemplateRequestSkills = CreateCreatureTemplateSkills(appDbContext);

			CreateCreatureTemplateRequest request = new()
			{
				GameId = TestDbContext.GameId,
				ImgFileId = null,
				BodyTemplateId = TestDbContext.BodyTemplateId,
				CreatureType = CreatureType.Human,
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
			return CreateCommandFromQuery(request);

			static List<CreateCreatureTemplateRequestSkill> CreateCreatureTemplateSkills(IAppDbContext appDbContext)
			{
				var skills = appDbContext.Skills.ToList();
				var createCreatureTemplateRequestSkills = new List<CreateCreatureTemplateRequestSkill>();

				Console.WriteLine("You need to add some skills.");

				foreach (var skill in skills)
				{
					Console.WriteLine($"Enter value to {skill.Name} skill. Please do not enter more then ten.");
					int value = 0;
					while (!int.TryParse(Console.ReadLine(), out value) || value < 0) ;

					createCreatureTemplateRequestSkills.Add(new CreateCreatureTemplateRequestSkill { SkillId = skill.Id, Value = value });
				}

				return createCreatureTemplateRequestSkills;
			}

			static List<CreateCreatureTemplateRequestArmorList> CreateArmorList(IAppDbContext appDbContext)
			{
				var bodyTemplateParts = appDbContext.BodyTemplateParts.Where(x => x.BodyTemplateId == TestDbContext.BodyTemplateId).ToList();

				var armorList = new List<CreateCreatureTemplateRequestArmorList>();

				Console.WriteLine("Time to add armor.");
				foreach (var part in bodyTemplateParts)
				{
					Console.WriteLine($"Add some armor to {part.Name}. Please do not add more then ten armor per body part.");
					int armor = 0;
					while (!int.TryParse(Console.ReadLine(), out armor) || armor < 0) ;

					armorList.Add(new CreateCreatureTemplateRequestArmorList { Armor = armor, BodyTemplatePartId = part.Id });
				}

				return armorList;
			}

			static Guid ChooseAbility(IAppDbContext appDbContext)
			{
				Console.WriteLine("Pick up one ability: sword (press 1) or bow (press 2) or claws (press 3).");

				int input = 0;

				while (int.TryParse(Console.ReadLine(), out input) && (input < 1 || input > 3)) ;

				var abilityId = input switch
				{
					1 => appDbContext.Abilities.FirstOrDefault(x => x.Name.Equals("SwordAttack")).Id,
					2 => appDbContext.Abilities.FirstOrDefault(x => x.Name.Equals("ArcheryAttack")).Id,
					3 => appDbContext.Abilities.FirstOrDefault(x => x.Name.Equals("ClawsAttack")).Id
				};
				return abilityId;
			}

			CreateCreatureTemplateCommand CreateCommandFromQuery(CreateCreatureTemplateRequest request)
			{
				return request == null
					? throw new ArgumentNullException(nameof(request))
					: new CreateCreatureTemplateCommand(
						gameId: request.GameId,
						imgFileId: request.ImgFileId,
						bodyTemplateId: request.BodyTemplateId,
						creatureType: request.CreatureType,
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

		internal static GetCreatureTemplateCommand FormGetCTCommand()
		{
			GetCreatureTemplateQuery query = new GetCreatureTemplateQuery();
			query.GameId = TestDbContext.GameId;

			return GetCommandFromQuery(query);

			GetCreatureTemplateCommand GetCommandFromQuery(GetCreatureTemplateQuery request)
			{
				return request == null
						? throw new ArgumentNullException(nameof(request))
						: new GetCreatureTemplateCommand(
							gameId: request.GameId,
							name: request.Name,
							creatureType: request.CreatureType,
							userName: request.UserName,
							creationMaxTime: request.CreationMaxTime,
							creationMinTime: request.CreationMinTime,
							modificationMaxTime: request.ModificationMaxTime,
							modificationMinTime: request.ModificationMinTime,
							bodyTemplateName: request.BodyTemplateName,
							bodyPartType: request.BodyPartType,
							conditionName: request.ConditionName,
							pageNumber: request.PageNumber,
							pageSize: request.PageSize,
							orderBy: request.OrderBy,
							isAscending: request.IsAscending);
			}
		}

		internal static GetCreatureTemplateByIdQuery FormGetCTByIdQuery(Guid id)
		{
			GetCreatureTemplateByIdQuery query = new GetCreatureTemplateByIdQuery() { GameId = TestDbContext.GameId, Id = id };

			return query;
		}

		internal static ChangeCreatureTemplateCommand FormChangeCTCommand(Guid id, IAppDbContext appDbContext)
		{
			var creatureTemplate = appDbContext.CreatureTemplates.FirstOrDefault(x => x.Id == id)
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

			Guid abilityId = ChooseAbility(appDbContext);

			List<ChangeCreatureTemplateRequestArmorList> armorList = FormArmorList(appDbContext);

			List<ChangeCreatureTemplateRequestSkill> changeCreatureTemplateRequestSkills = FormSkillData(appDbContext, creatureTemplate);

			ChangeCreatureTemplateRequest request = new()
			{
				Id = id,
				GameId = TestDbContext.GameId,
				ImgFileId = null,
				BodyTemplateId = TestDbContext.BodyTemplateId,
				CreatureType = CreatureType.Human,
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

			return CreateCommandFromQuery(request);

			ChangeCreatureTemplateCommand CreateCommandFromQuery(ChangeCreatureTemplateRequest request)
			{
				return request == null
					? throw new ArgumentNullException(nameof(request))
					: new ChangeCreatureTemplateCommand(
						id: id,
						gameId: request.GameId,
						imgFileId: request.ImgFileId,
						bodyTemplateId: request.BodyTemplateId,
						creatureType: request.CreatureType,
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

			static List<ChangeCreatureTemplateRequestSkill> FormSkillData(IAppDbContext appDbContext, CreatureTemplate creatureTemplate)
			{
				var skills = appDbContext.Skills.ToList();
				var changeCreatureTemplateRequestSkills = new List<ChangeCreatureTemplateRequestSkill>();

				Console.WriteLine("You need to add some skills.");

				foreach (var skill in skills)
				{
					var existingCTSkill = creatureTemplate.CreatureTemplateSkills.FirstOrDefault(x => x.SkillId == skill.Id);

					Console.WriteLine($"Enter value to {skill.Name} skill. Please do not enter more then ten.");
					int value = 0;
					while (!int.TryParse(Console.ReadLine(), out value) || value < 0) ;

					changeCreatureTemplateRequestSkills.Add(new ChangeCreatureTemplateRequestSkill
					{
						Id = existingCTSkill?.Id,
						SkillId = skill.Id,
						Value = value
					});
				}

				return changeCreatureTemplateRequestSkills;
			}

			static List<ChangeCreatureTemplateRequestArmorList> FormArmorList(IAppDbContext appDbContext)
			{
				var bodyTemplateParts = appDbContext.BodyTemplateParts.Where(x => x.BodyTemplateId == TestDbContext.BodyTemplateId).ToList();

				var armorList = new List<ChangeCreatureTemplateRequestArmorList>();

				Console.WriteLine("Time to add armor.");
				foreach (var part in bodyTemplateParts)
				{
					Console.WriteLine($"Add some armor to {part.Name}. Please do not add more then ten armor per body part.");
					int armor = 0;
					while (!int.TryParse(Console.ReadLine(), out armor) || armor < 0) ;

					armorList.Add(new ChangeCreatureTemplateRequestArmorList { Armor = armor, BodyTemplatePartId = part.Id });
				}

				return armorList;
			}

			static Guid ChooseAbility(IAppDbContext appDbContext)
			{
				Console.WriteLine("Pick up one ability: sword (press 1) or bow (press 2) or claws (press 3).");

				int input = 0;

				while (int.TryParse(Console.ReadLine(), out input) && (input < 1 || input > 3)) ;

				var abilityId = input switch
				{
					1 => appDbContext.Abilities.FirstOrDefault(x => x.Name.Equals("SwordAttack")).Id,
					2 => appDbContext.Abilities.FirstOrDefault(x => x.Name.Equals("ArcheryAttack")).Id,
					3 => appDbContext.Abilities.FirstOrDefault(x => x.Name.Equals("ClawsAttack")).Id
				};
				return abilityId;
			}
		}

		internal static DeleteCreatureTemplateByIdCommand FormDeleteCTCommand(Guid id)
		{
			return new DeleteCreatureTemplateByIdCommand { GameId = TestDbContext.GameId, Id = id };
		}
	}
}
