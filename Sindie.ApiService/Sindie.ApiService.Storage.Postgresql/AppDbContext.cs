using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Storage.Postgresql
{
	/// <summary>
	/// Контекст базы данных
	/// </summary>
	public class AppDbContext : DbContext, IAppDbContext

	{
		/// <summary>
		/// Способности
		/// </summary>
		public DbSet<Ability> Abilities { get; set; }
		
		/// <summary>
		/// Действия
		/// </summary>
		public DbSet<Core.Entities.Action> Actions { get; set; }

		/// <summary>
		/// Деятельности
		/// </summary>
		public DbSet<Activity> Activities { get; set; }

		/// <summary>
		/// Действия деятельности
		/// </summary>
		public DbSet<ActivityAction> ActivityActions { get; set; }

		/// <summary>
		/// Области применения
		/// </summary>
		public DbSet<ApplicationArea> ApplicationAreas { get; set; }

		/// <summary>
		/// Применяемые состояния
		/// </summary>
		public DbSet<AppliedCondition> AppliedConditions { get; set; }

		/// <summary>
		/// Статьи
		/// </summary>
		public DbSet<Article> Articles { get; set; }

		/// <summary>
		/// Сумки
		/// </summary>
		public DbSet<Bag> Bags { get; set; }

		/// <summary>
		/// Предметы в сумках
		/// </summary>
		public DbSet<BagItem> BagItems { get; set; }

		/// <summary>
		/// Тела
		/// </summary>
		public DbSet<Body> Bodies { get; set; }

		/// <summary>
		/// Части тела
		/// </summary>
		public DbSet<BodyPart> BodyParts { get; set; }

		/// <summary>
		/// Типы частей тела
		/// </summary>
		public DbSet<BodyPartType> BodyPartTypes { get; set; }

		/// <summary>
		/// Шаблоны тел
		/// </summary>
		public DbSet<BodyTemplate> BodyTemplates { get; set; }

		/// <summary>
		/// Части шаблона тела
		/// </summary>
		public DbSet<BodyTemplatePart> BodyTemplateParts { get; set; }

		/// <summary>
		/// Пресонажи
		/// </summary>
		public DbSet<Character> Characters { get; set; }

		/// <summary>
		/// Характеристики
		/// </summary>
		public DbSet<Characteristic> Characteristics { get; set; }

		/// <summary>
		/// Модификаторы персонажа
		/// </summary>
		public DbSet<CharacterModifier> CharacterModifiers { get; set; }

		/// <summary>
		/// Параметры персонажей
		/// </summary>
		public DbSet<CharacterParameter> CharacterParameters { get; set; }

		/// <summary>
		/// Шаблоны персонажа
		/// </summary>
		public DbSet<CharacterTemplate> CharacterTemplates { get; set; }

		/// <summary>
		/// Модификаторы шаблона персонажа
		/// </summary>
		public DbSet<CharacterTemplateModifier> CharacterTemplateModifiers { get; set; }

		/// <summary>
		/// Слоты шаблона пресонажа
		/// </summary>
		public DbSet<CharacterTemplateSlot> CharacterTemplateSlots { get; set; }

		/// <summary>
		/// Состояния
		/// </summary>
		public DbSet<Condition> Conditions { get; set; }

		/// <summary>
		/// Существа
		/// </summary>
		public DbSet<Creature> Creatures { get; set; }

		/// <summary>
		/// Параметры существ
		/// </summary>
		public DbSet<CreatureParameter> CreatureParameters { get; set; }

		/// <summary>
		/// Части тел существ
		/// </summary>
		public DbSet<CreaturePart> CreatureParts { get; set; }

		/// <summary>
		/// Шаблоны существ
		/// </summary>
		public DbSet<CreatureTemplate> CreatureTemplates { get; set; }

		/// <summary>
		/// Части тел шаблонов существ
		/// </summary>
		public DbSet<CreatureTemplatePart> CreatureTemplateParts { get; set; }

		/// <summary>
		/// Параметры шаблонов существ
		/// </summary>
		public DbSet<CreatureTemplateParameter> CreatureTemplateParameters { get; set; }

		/// <summary>
		/// События
		/// </summary>
		public DbSet<Event> Events { get; set; }

		/// <summary>
		/// Игры
		/// </summary>
		public DbSet<Game> Games { get; set; }

		/// <summary>
		/// Роли в игре
		/// </summary>
		public DbSet<GameRole> GameRoles { get; set; }

		/// <summary>
		/// Графические файлы
		/// </summary>
		public DbSet<ImgFile> ImgFiles { get; set; }

		/// <summary>
		/// Экземпляры
		/// </summary>
		public DbSet<Instance> Instances { get; set; }

		/// <summary>
		/// Взаимодействия
		/// </summary>
		public DbSet<Interaction> Interactions { get; set; }

		/// <summary>
		/// Предметы во взаимодействии
		/// </summary>
		public DbSet<InteractionItem> InteractionItems { get; set; }

		/// <summary>
		/// Роли во взаимодействии
		/// </summary>
		public DbSet<InteractionsRole> InteractionsRoles { get; set; }

		/// <summary>
		/// Деятельности роли во взаимодействии
		/// </summary>
		public DbSet<InteractionsRoleActivity> InteractionsRoleActivities { get; set; }

		/// <summary>
		/// Интерфейсы
		/// </summary>
		public DbSet<Interface> Interfaces { get; set; }

		/// <summary>
		/// Предметы
		/// </summary>
		public DbSet<Item> Items { get; set; }

		/// <summary>
		/// Шаблоны предметов
		/// </summary>
		public DbSet<ItemTemplate> ItemTemplates { get; set; }

		/// <summary>
		/// Модификаторы шаблона предмета
		/// </summary>
		public DbSet<ItemTemplateModifier> ItemTemplateModifiers { get; set; }

		/// <summary>
		/// Модификаторы игры
		/// </summary>
		public DbSet<Modifier> Modifiers { get; set; }

		/// <summary>
		/// Скрипты модификатора
		/// </summary>
		public DbSet<ModifierScript> ModifierScripts { get; set; }

		/// <summary>
		/// Уведомления
		/// </summary>
		public DbSet<Notification> Notifications { get; set; }

		/// <summary>
		/// Уведомления об удалении предметов
		/// </summary>
		public DbSet<NotificationDeletedItem> NotificationsDeletedItems { get; set; }

		/// <summary>
		/// Уведомления о предложении передать предметы
		/// </summary>
		public DbSet<NotificationTradeRequest> NotificationsTradeRequest { get; set; }

		/// <summary>
		/// Параметры
		/// </summary>
		public DbSet<Parameter> Parameters { get; set; }

		/// <summary>
		/// Стороны
		/// </summary>
		public DbSet<Party> Parties { get; set; }

		/// <summary>
		/// Роли стороны во взаимодействии
		/// </summary>
		public DbSet<PartyInteractionsRole> PartyInteractionsRoles { get; set; }

		/// <summary>
		/// Пререквизиты
		/// </summary>
		public DbSet<Prerequisite> Prerequisites { get; set; }

		/// <summary>
		/// Cкрипты
		/// </summary>
		public DbSet<Script> Scripts { get; set; }

		/// <summary>
		/// Пререквезиты скрипта
		/// </summary>
		public DbSet<ScriptPrerequisites> ScriptPrerequisites { get; set; }

		/// <summary>
		/// Слоты
		/// </summary>
		public DbSet<Slot> Slots { get; set; }

		/// <summary>
		/// Роли в системе
		/// </summary>
		public DbSet<SystemRole> SystemRoles { get; set; }

		/// <summary>
		/// Текстовые файлы
		/// </summary>
		public DbSet<TextFile> TextFiles { get; set; }

		/// <summary>
		/// Пользователи
		/// </summary>
		public DbSet<User> Users { get; set; }

		/// <summary>
		/// Аккаунты пользователей
		/// </summary>
		public DbSet<UserAccount> UserAccounts { get; set; }

		/// <summary>
		/// Пользователи игры
		/// </summary>
		public DbSet<UserGame> UserGames { get; set; }

		/// <summary>
		/// Персонажи пользователя игры
		/// </summary>
		public DbSet<UserGameCharacter> UserGameCharacters { get; set; }

		/// <summary>
		/// Роли пользователей
		/// </summary>
		public DbSet<UserRole> UserRoles { get; set; }


		/// <summary>
		/// Интерфейс получения данных пользователя из веба
		/// </summary>
		private readonly IUserContext _userContext;

		/// <summary>
		///  Провайдер дат
		/// </summary>
		private readonly IDateTimeProvider _dateTimeProvider;

		/// <summary>
		/// Конструктор контекста базы данных
		/// </summary>
		/// <param name="dbContextOptions">параметры контекста</param>
		public AppDbContext(
			DbContextOptions<AppDbContext> dbContextOptions, 
			IUserContext userContext,
			IDateTimeProvider dateTimeProvider)
			: base(dbContextOptions)
		{
			_userContext = userContext;
			_dateTimeProvider = dateTimeProvider;
		}

		/// <summary>
		/// Конструктор контекста базы данных
		/// </summary>
		protected AppDbContext()
		{
		}

		/// <summary>
		/// Создать модели
		/// </summary>
		/// <param name="modelBuilder">Строитель моделей</param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		}

		/// <summary>
		/// Переопределенный метод сохранить изменения в бд
		/// </summary>
		/// <returns></returns>
		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var entries = ChangeTracker
				.Entries()
				.Where(e => e.Entity is EntityBase && (
						e.State == EntityState.Added
						|| e.State == EntityState.Modified));

			foreach (var entityEntry in entries)
			{
				if (entityEntry.Entity is EntityBase entityBase)
				{
					if (entityEntry.State == EntityState.Added)
					{
						entityBase.CreatedByUserId = _userContext.CurrentUserId;
						entityBase.RoleCreatedUser = _userContext.Role;
						entityBase.ModifiedOn = _dateTimeProvider.TimeProvider;
						entityBase.ModifiedByUserId = _userContext.CurrentUserId;
						entityBase.RoleModifiedUser = _userContext.Role;
					}

					if (entityEntry.State == EntityState.Modified)
					{
						entityBase.ModifiedOn = _dateTimeProvider.TimeProvider;
						entityBase.ModifiedByUserId = _userContext.CurrentUserId;
						entityBase.RoleModifiedUser = _userContext.Role;
					}
				}
			}
			return await base.SaveChangesAsync();
		}
	}
}
