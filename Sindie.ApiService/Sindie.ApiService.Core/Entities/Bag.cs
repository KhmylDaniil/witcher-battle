using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Requests.BagRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Сумка
	/// </summary>
	public class Bag : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_instance"/>
		/// </summary>
		public const string InstanceField = nameof(_instance);

		/// <summary>
		/// Поле для <see cref="_imgFile"/>
		/// </summary>
		public const string ImgFileField = nameof(_imgFile);

		/// <summary>
		/// Поле для <see cref="_character"/>
		/// </summary>
		public const string CharacterField = nameof(_character);

		private Instance _instance;
		private ImgFile _imgFile;
		private Character _character;
		private int? _maxBagSize;
		private double? _maxWeight;

		/// <summary>
		/// Конструктор
		/// </summary>
		protected Bag()
		{
		}

		/// <summary>
		/// Конструктор сумки
		/// </summary>
		/// <param name="instance">Экземпляр</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="character">Персонаж</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="maxBagSize">Максимальное количество ячеек для предметов</param>
		/// <param name="maxWeight">Максимальная грузоподъемность</param>
		public Bag(
			Instance instance,
			ImgFile? imgFile,
			Character? character,
			string name,
			string description,
			int? maxBagSize,
			double? maxWeight)
		{
			Instance = instance;
			ImgFile = imgFile;
			Character = character;
			Name = name;
			Description = description;
			MaxBagSize = maxBagSize;
			MaxWeight = maxWeight;
			BagItems = new List<BagItem>();
			NotificationDeletedItems = new List<NotificationDeletedItem>();
		}

		/// <summary>
		/// Айди экземпляра игры
		/// </summary>
		public Guid InstanceId { get; protected set; }

		/// <summary>
		/// Айди графического файла
		/// </summary>
		public Guid? ImgFileId { get; protected set; }

		/// <summary>
		/// Айди персонажа
		/// </summary>
		public Guid? CharacterId { get; protected set; }

		/// <summary>
		/// Название сумки
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание сумки
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Максимальное количество ячеек для предметов
		/// </summary>
		public int? MaxBagSize
		{
			get => _maxBagSize;
			set
			{
				if (value != null && value <= 0)
					throw new ExceptionFieldOutOfRange<Bag>(nameof(MaxBagSize));
				_maxBagSize = value;
			}
		}
		
		/// <summary>
		/// Максимальная грузоподъемность
		/// </summary>
		public double? MaxWeight
		{
			get => _maxWeight;
			set
			{
				if (value != null && value <= 0)
					throw new ExceptionFieldOutOfRange<Bag>(nameof(MaxWeight));
				_maxWeight = value;
			}
		}

		#region navigation properties

		/// <summary>
		/// Экземпляр
		/// </summary>
		public Instance Instance
		{
			get => _instance;
			set
			{
				_instance = value ?? throw new ApplicationException("Необходимо передать экземпляр игры");
				InstanceId = value.Id;
			}
		}

		/// <summary>
		/// Графический файл
		/// </summary>
		public ImgFile ImgFile
		{
			get => _imgFile;
			set
			{
				_imgFile = value;
				ImgFileId = value?.Id;
			}
		}

		/// <summary>
		/// Персонаж
		/// </summary>
		public Character Character
		{
			get => _character;
			set
			{
				_character = value;
				CharacterId = value?.Id;
			}
		}

		/// <summary>
		/// Предметы в сумке
		/// </summary>
		public List<BagItem> BagItems { get; protected set; }

		/// <summary>
		/// Уведомления об удаленных предметах
		/// </summary>
		public List<NotificationDeletedItem> NotificationDeletedItems { get; protected set; }

		/// <summary>
		/// Уведомления о предложении передать предметы-отправители
		/// </summary>
		public List<NotificationTradeRequest> NotificationsTradeRequestSource { get; protected set; }

		#endregion navigation properties

		/// <summary>
		/// Проверка возможности изменения сумки по весу и объему
		/// </summary>
		/// <param name="data">Данные</param>
		/// <param name="maxBagSize">Максимальный объем сумки</param>
		/// <param name="maxWeight">Максимальная грузоподъемность</param>
		internal static string CheckBagCapacity(
			IEnumerable<BagItemData> data,
				int? maxBagSize,
				double? maxWeight)
		{
			if (data is null)
				throw new ArgumentNullException(nameof(data));

			StringBuilder result = new StringBuilder();

			if (!data.Any())
				return result.ToString();

			if (maxWeight != null && CheckBagWeigth(data) > maxWeight)
				result.AppendLine("Суммарный вес предметов превышает грузоподъемность");

			if (maxBagSize is null)
				return result.ToString();

			var unspecificStackData = data.Where(x => x.Stack == null);
			var specificStackData = data.Where(x => x.Stack != null);
			if (specificStackData.Any())
			{
				maxBagSize -= specificStackData.Count();
				unspecificStackData = unspecificStackData.Concat(CheckSpecialStacks(specificStackData));
			}

			if ((int)maxBagSize < CheckRequiredStacks(unspecificStackData))
				result.AppendLine("Необходимое количество стаков превышает объем сумки");
			return result.ToString();
		}

		/// <summary>
		/// Распределение конкретных стаков
		/// </summary>
		/// <param name="data">Данные с указанными стаками</param>
		/// <returns>Остатки после заполнения указанных стаков</returns>
		private static IEnumerable<BagItemData> 
		CheckSpecialStacks(IEnumerable<BagItemData> data)
		{
			var result = new List<BagItemData>();

			foreach (var dataItem in data)
			{
				if (dataItem.QuantityItem > dataItem.Item.MaxQuantity)
					result.Add(new BagItemData()
					{
						Item = dataItem.Item,
						QuantityItem = dataItem.QuantityItem - dataItem.Item.MaxQuantity,
						Stack = null,
						Blocked = 0
					});
			}
			return result;
		}

		/// <summary>
		/// Проверяет количество нужных стаков
		/// </summary>
		/// <param name="data">Данные с неуказанными стаками</param>
		/// <returns>Количество нужных стаков</returns>
		private static double CheckRequiredStacks(IEnumerable<BagItemData> data)
		{
			double result = 0;
			foreach (var item in data.Select(x => x.Item).Distinct())
			{
				double total = 0;
				foreach (var dataItem in data.Where(x => x.Item.Id == item.Id))
					total += dataItem.QuantityItem;
				result += Math.Ceiling(total / item.MaxQuantity);
			}
			return result;
		}

		/// <summary>
		/// Проверка веса
		/// </summary>
		/// <param name="data">Данные</param>
		/// <returns>Общий вес</returns>
		private static double CheckBagWeigth(IEnumerable<BagItemData> data)
		{
			double weight = 0;
			foreach (var dataItem in data)
				weight += dataItem.Item.Weight * dataItem.QuantityItem;
			return weight;
		}

		/// <summary>
		/// Обновление сумки
		/// </summary>
		/// <param name="data">Данные о предметах</param>
		internal void UpdateBagItems(IEnumerable<BagItemData> data)
		{
			BagItems = new List<BagItem>();
			if (!data.Any())
				return;

			var usedStacks = MaxBagSize == null
				? new bool[999]
				: new bool[(int)MaxBagSize];

			var unspecificStackData = data.Where(x => x.Stack == null);
			var specificStackData = data.Where(x => x.Stack != null);

			if (specificStackData.Any())
				unspecificStackData = unspecificStackData
					.Concat(UpdateSpecificStacks(specificStackData, usedStacks));

			UpdateUnspecificStacks(unspecificStackData, usedStacks);
		}

		/// <summary>
		/// Записать указанные стаки
		/// </summary>
		/// <param name="data">Данные</param>
		/// <param name="usedStacks">Массив занятых стаков</param>
		/// <returns>Остатки после занятых стаков</returns>
		private List<BagItemData> UpdateSpecificStacks(IEnumerable<BagItemData> data,
				bool[] usedStacks)
		{
			var result = new List<BagItemData>();

			foreach (var dataItem in data)
			{
				usedStacks[(int)dataItem.Stack] = true;
				if (dataItem.QuantityItem > dataItem.Item.MaxQuantity)
				{
					BagItems.Add(new BagItem(
						bag: this,
						item: dataItem.Item,
						quantityItem: dataItem.Item.MaxQuantity,
						maxQuantityItem: dataItem.Item.MaxQuantity,
						stack: dataItem.Stack.Value,
						blocked: dataItem.Blocked < dataItem.Item.MaxQuantity
							? dataItem.Blocked
							: dataItem.Item.MaxQuantity));
					result.Add(new BagItemData()
					{
						Item = dataItem.Item,
						QuantityItem = dataItem.QuantityItem - dataItem.Item.MaxQuantity,
						Stack = null,
						Blocked = dataItem.Blocked <= dataItem.Item.MaxQuantity
							? 0 
							: dataItem.Blocked - dataItem.Item.MaxQuantity,
					});
				}
				else
					BagItems.Add(new BagItem(
						bag: this,
						item: dataItem.Item,
						quantityItem: dataItem.QuantityItem,
						maxQuantityItem: dataItem.Item.MaxQuantity,
						stack: dataItem.Stack.Value,
						blocked: dataItem.Blocked));
			}
			return result;
		}

		/// <summary>
		/// Изменяет неуказанные стаки
		/// </summary>
		/// <param name="data">Данные с неуказанными стаками</param>
		/// <returns>Количество нужных стаков</returns>
		private void UpdateUnspecificStacks(IEnumerable<BagItemData> data,
			bool[] usedStacks)
		{
			foreach (var item in data.Select(x => x.Item).Distinct())
			{
				var totalQuantity = 0;
				var totalBlocked = 0;
				foreach (var dataItem in data.Where(x => x.Item.Id == item.Id))
				{
					totalQuantity += dataItem.QuantityItem;
					totalBlocked += dataItem.Blocked;
				}

				do
				{
					BagItems.Add(new BagItem(
						bag: this,
						item: item,
						quantityItem: totalQuantity > item.MaxQuantity
							? item.MaxQuantity 
							: totalQuantity,
						maxQuantityItem: item.MaxQuantity,
						stack: Array.IndexOf(usedStacks, false),
						blocked: totalBlocked > item.MaxQuantity
							? item.MaxQuantity
							: totalBlocked));
					usedStacks[Array.IndexOf(usedStacks, false)] = true;
					totalQuantity -= item.MaxQuantity;
					totalBlocked = totalBlocked > item.MaxQuantity
						? totalBlocked -= item.MaxQuantity
						: 0;
				}
				while (totalQuantity > item.MaxQuantity);
			}
		}

		/// <summary>
		/// Запись уведомления при удалении вещей из сумки
		/// </summary>
		/// <param name="data">Данные для обновления предметов в сумке</param>
		internal void NotifyDeleteItems(IEnumerable<BagItemData> data)
        {
			var message = new StringBuilder();

			foreach (var item in BagItems.Select(x => x.Item).Distinct())
            {
				var oldQuantity = 0;
				foreach (var oldItem in BagItems.Where(x => x.ItemId == item.Id))
					oldQuantity += oldItem.QuantityItem;

				var newQuantity = 0;
				foreach (var newItem in data.Where(x => x.Item.Id == item.Id))
					newQuantity += newItem.QuantityItem;

				if (oldQuantity > newQuantity)
					message.AppendLine($"Пользователь {CreatedByUserId} удалил из сумки {this.Id} предмет(ы) {item.Name} в количестве {oldQuantity - newQuantity} штук");
			}
			if (message.Length > 0)
				NotificationDeletedItems.Add(new NotificationDeletedItem(this, message.ToString()));
		}

		/// <summary>
		/// Создание тестовой сущности с заполненными полями
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="instance">Экземпляр</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="character">Персонаж</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="maxBagSize">Максимальная вместимость</param>
		/// <param name="maxWeight">Максиммальная грузоподъеммность</param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static Bag CreateForTest(
			Guid? id = default,
			Instance instance = default,
			ImgFile imgFile = default,
			Character character = default,
			string name = default,
			string description = default,
			int? maxBagSize = default,
			double? maxWeight = default)
		=> new Bag()
		{
			Id = id ?? Guid.NewGuid(),
			Instance = instance,
			Character = character,
			ImgFile = imgFile,
			Name = name ?? "Bag",
			Description = description,
			MaxBagSize = maxBagSize,
			MaxWeight = maxWeight,
			BagItems = new List<BagItem>(),
			NotificationDeletedItems = new List<NotificationDeletedItem>()
		};
	}
}
