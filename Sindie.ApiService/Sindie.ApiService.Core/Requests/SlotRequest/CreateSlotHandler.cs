namespace Sindie.ApiService.Core.Requests.SlotRequest
{/*
	/// <summary>
	/// Обработчик команды создания слота
	/// </summary>
	public class CreateSlotHandler : IRequestHandler<CreateSlotCommand, CreateSlotResponse>
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Интерфейс получения данных пользователя из веба
		/// </summary>
		private readonly IUserContext _userContext;

		/// <summary>
		/// Конструктор обработчика команды регистрации пользователя
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="userContext">Получение контекста пользователя из веба</param>
		public CreateSlotHandler(
			IAppDbContext appDbContext,
			IUserContext userContext)
		{
			_appDbContext = appDbContext;
			_userContext = userContext;
		}

		/// <summary>
		/// Обработать запрос создания слота
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns>Ответ</returns>
		public async Task<CreateSlotResponse> Handle
			(CreateSlotCommand request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException($"Пришел пустой запрос {typeof(CreateSlotCommand)}");
			if (request.GameId == null)
				throw new ExceptionRequestFieldNull<CreateSlotCommand>(nameof(request.GameId));
			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ExceptionRequestFieldIncorrectData<CreateSlotCommand>(nameof(request.Name));

			var existingGame = await _appDbContext.Games
				.Include(x => x.Slots)
				.ThenInclude(x => x.Items)
				.FirstOrDefaultAsync(x => x.Id == request.GameId, cancellationToken);
			
			Slot existingSlot = Slot.CreateSlot(
			name: request.Name,
			description: request.Description,
			game: existingGame);
			
			var createdItems = new List<(
			string Name,
			string Description,
			int? MaxQuantityItem,
			bool? IsRemovable,
			bool? AutoWear,
			double? Weight,
			Guid CreatedByUserId)>();

			foreach (var x in request.Items)
			{
				createdItems.Add((x.Name,
						x.Description,
						x.MaxQuantityItem,
						x.IsRemovable,
						x.AutoWear,
						x.Weight,
						_userContext.CurrentUserId));
			}

			existingSlot.CreateItemsInSlot(createdItems, existingGame);

			existingGame.Slots.Add(existingSlot);
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return new CreateSlotResponse()
			{ SlotId = existingSlot.Id };
		}
	}*/
}
