using PilotProject.Controllers;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Requests.BattleRequests.CreateBattle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		/// <summary>
		/// Контроллер боя
		/// </summary>
		private BattleController _battleController;

		/// <summary>
		/// Контроллер шаблона существа
		/// </summary>
		private CreatureTemplateController _creatureTemplateController;


		public Application(IAppDbContext appDbContext, IAuthorizationService authorizationService, IDateTimeProvider dateTimeProvider, IRollService rollService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
			_dateTimeProvider = dateTimeProvider;
			_rollService = rollService;
			_battleController = new(_appDbContext, _authorizationService, _rollService);
			_creatureTemplateController = new(_appDbContext, _authorizationService, _dateTimeProvider);
		}

		public async void Run()
		{
			Console.WriteLine("Welcome to Witcher battle helper.");
			await _creatureTemplateController.GetAsync();
		}

		public void StartBattle(CreateBattleCommand command)
		{
			
		}
	}
}
