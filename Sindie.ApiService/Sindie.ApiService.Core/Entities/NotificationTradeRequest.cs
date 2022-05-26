using Sindie.ApiService.Core.Requests.BagRequests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Уведомление о предложении передать предметы
	/// </summary>
	public class NotificationTradeRequest : Notification
	{
		/// <summary>
		/// Поле для <see cref="_sourceBag"/>
		/// </summary>
		public const string SourceBagField = nameof(_sourceBag);

		/// <summary>
		/// Поле для <see cref="_receiveCharacter"/>
		/// </summary>
		public const string ReceiveCharacterField = nameof(_receiveCharacter);


		private Bag _sourceBag;
		private Character _receiveCharacter;

		/// <summary>
		/// Пустой конструтор
		/// </summary>
		protected NotificationTradeRequest()
		{
		}

		/// <summary>
		/// Конструктор уведомления о предложении передать предметы
		/// </summary>
		/// <param name="sourceBag">Сумка-источник</param>
		/// <param name="bagItems">Список предметов</param>
		/// <param name="receiveCharacter">Персонаж-получатель</param>
		public NotificationTradeRequest(Bag sourceBag, IEnumerable<BagItemData> bagItems, Character receiveCharacter)
		{
			Name = sourceBag.Character != null
				? $"{ sourceBag.Character.Name} предлагает вам предметы"
				: "Получить предметы";
			BagItems = new List<NotificationTradeRequestItem>();

			var message = new StringBuilder("Вам предлагают предметы: ");
			
			foreach (var item in bagItems)
			{
				message.AppendLine($"{item.Item.Name} в количестве {item.Blocked}. ");
				BagItems.Add(new NotificationTradeRequestItem(item.Item, item.Blocked, item.Stack.Value));
			}
			message.Append("Получить предметы?");
			
			Message = message.ToString();
			SourceBag = sourceBag;
			ReceiveCharacter = receiveCharacter;
			Duration = 60;
			Receivers = new List<User>();
		}

		/// <summary>
		/// Айди сумки-получателя
		/// </summary>
		public Guid ReceiveBagId { get; protected set; }

		/// <summary>
		/// Айди персонажа-получателя
		/// </summary>
		public Guid? ReceiveCharacterId { get; protected set; }

		/// <summary>
		/// Айди сумки-источника
		/// </summary>
		public Guid SourceBagId { get; protected set; }

		/// <summary>
		/// Список элементов уведомления о предложении передать предметы
		/// </summary>
		public List<NotificationTradeRequestItem> BagItems { get; set; }

		#region navigation properties
		/// <summary>
		/// Сумка-источник
		/// </summary>
		public Bag SourceBag
		{
			get => _sourceBag;
			set
			{
				_sourceBag = value ?? throw new ApplicationException("Необходимо передать сумку");
				SourceBagId = value.Id;
			}
		}

		/// <summary>
		/// Персонаж-получатель
		/// </summary>
		public Character ReceiveCharacter
		{
			get => _receiveCharacter;
			set
			{
				_receiveCharacter = value ?? throw new ApplicationException("Необходимо передать персонажа");
				ReceiveCharacterId = value.Id;
				ReceiveBagId = value.BagId ?? throw new ApplicationException("Персонаж-получатель не имеет сумки");
			}
		}
		#endregion navigation properties

		/// <summary>
		/// Создание тестовой сущности с заполненными полями
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="name">Название</param>
		/// <param name="message">Сообщение</param>
		/// <param name="duration">Продолжительность</param>
		/// <param name="sourceBag">Сумка-источник</param>
		/// <param name="receiveCharacter">Персонаж-получатель</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата модификации</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns>Уведомление о предложении передать предметы</returns>
		[Obsolete("Только для тестов")]
		public static NotificationTradeRequest CreateForTest(
			Guid? id = default,
			string name = default,
			string message = default,
			int duration = default,
			Bag sourceBag = default,
			Character receiveCharacter = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new NotificationTradeRequest()
		{
			Id = id ?? Guid.NewGuid(),
			Name = name ?? "Notification",
			Message = message ?? "default",
			Duration = duration,
			SourceBag = sourceBag,
			ReceiveCharacter = receiveCharacter,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId,
			BagItems = new List<NotificationTradeRequestItem>()
		};
	}
}
