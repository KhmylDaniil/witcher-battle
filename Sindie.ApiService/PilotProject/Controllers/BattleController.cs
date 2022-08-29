using PilotProject.DbContext;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests.CreateBattle;
using Sindie.ApiService.Core.Requests.BattleRequests.CreateBattle;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.ChangeCreatureTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PilotProject.Controllers
{
	internal class BattleController
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
		/// Бросок параметра
		/// </summary>
		private readonly IRollService _rollService;

		/// <summary>
		/// Словарь для сбора данных о бое
		/// </summary>
		public readonly Dictionary<Guid, string> PickedCreatureTemplates = new();

		/// <summary>
		/// Конструктор обработчика атаки существа
		/// </summary>
		/// <param name="appDbContext"></param>
		/// <param name="authorizationService"></param>
		/// <param name="rollService">Сервис бросков</param>
		public BattleController(IAppDbContext appDbContext, IAuthorizationService authorizationService, IRollService rollService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
			_rollService = rollService;
		}

		public static void PickUpToBattle(Guid id)
		{
			Console.WriteLine($"Enter unique name for this creature");
			string name = string.Empty;
			while (string.IsNullOrEmpty(name))
				name = Console.ReadLine();

			PickedCreatureTemplates.Add(id, name);

			if (PickedCreatureTemplates.Count != 2)
				return;

			var creatures = new List<CreateBattleRequestItem>();

			foreach (var pickedCreature in PickedCreatureTemplates)
				creatures.Add(new CreateBattleRequestItem
				{
					CreatureTemplateId = pickedCreature.Key,
					Name = pickedCreature.Value
				});
			
			CreateBattleRequest request = new()
			{
				GameId = TestDbContext.GameId,
				ImgFileId = null,
				Name = "TestName",
				Description = null,
				Creatures = creatures
			};

			Application.CreateBattle(CreateBattleCommandFromQuery(request));

			static CreateBattleCommand CreateBattleCommandFromQuery(CreateBattleRequest request)
				=> request == null
					? throw new ArgumentNullException(nameof(request))
					: new CreateBattleCommand(
						gameId: request.GameId,
						imgFileId: request.ImgFileId,
						name: request.Name,
						description: request.Description,
						creatures: request.Creatures);
		}
	}
}
