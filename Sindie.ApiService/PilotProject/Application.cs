using PilotProject.Controllers;
using Sindie.ApiService.Core.Abstractions;
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

		public Application(IAppDbContext appDbContext, IAuthorizationService authorizationService, IDateTimeProvider dateTimeProvider)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
			_dateTimeProvider = dateTimeProvider;
		}

		public async void Run()
		{
			Console.WriteLine("Welcome to witcher battle helper. Would you manage creature templates (press 1) or go to battle simulation (press 2)?");

			CreatureTemplateController newCTController = new(_appDbContext, _authorizationService, _dateTimeProvider);

			int input = 0;

			while (input != 1 && input != 2)
				int.TryParse(Console.ReadLine(), out input);

			if (input == 1) await newCTController.GetAsync();
		}
	}
}
