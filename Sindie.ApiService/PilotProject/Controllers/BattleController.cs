using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests.CreateBattle;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Requests.BattleRequests.CreateBattle;
using System.IO;
using System.Text;
using System.Text.Json;

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

		

		public async Task<Task> CreateBattle()
		{
			//CreateBattleRequest request = new();

			//using (FileStream fstream = File.OpenRead(Constants.BattlePath))
			//{
			//	byte[] buffer = new byte[fstream.Length];
	
			//	await fstream.ReadAsync(buffer, 0, buffer.Length);

			//	string json = Encoding.Default.GetString(buffer);
			//	request = JsonSerializer.Deserialize<CreateBattleRequest>(json);
			//}


			//using (FileStream fs = new FileStream(Constants.BattlePath, FileMode.))
			//{
			//	var aaa = await
			//		JsonSerializer.DeserializeAsync<CreateBattleRequest>(fs);
			//}

			//var newHandler = new CreateBattleHandler(_appDbContext, _authorizationService);

			//await newHandler.Handle(CreateBattleCommandFromQuery(request), default);

			return BattleOrder();

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

		public async Task BattleOrder()
		{
			var battle = _appDbContext.Battles.FirstOrDefault() ?? throw new ExceptionEntityNotFound<Battle>();

			
			foreach (var creature in battle.Creatures)
			{
				Console.WriteLine($"Creature name is {creature.Name}");
			}


		}
	}
}
