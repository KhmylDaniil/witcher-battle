using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Abstractions
{
	/// <summary>
	/// Контекст базы данных
	/// </summary>
	public interface IAppDbContext
	{
		/// <summary>
		/// Способности
		/// </summary>
		DbSet<Ability> Abilities { get; }

		/// <summary>
		/// Действия
		/// </summary>
		DbSet<Core.Entities.Action> Actions { get; }

		/// <summary>
		/// Деятельности
		/// </summary>
		DbSet<Activity> Activities { get; }

		/// <summary>
		/// Действия деятельности
		/// </summary>
		DbSet<ActivityAction> ActivityActions { get; }

		/// <summary>
		/// Области применения
		/// </summary>
		DbSet<ApplicationArea> ApplicationAreas { get; }

		/// <summary>
		/// Применяемые состояния
		/// </summary>
		DbSet<AppliedCondition> AppliedConditions { get; }

		/// <summary>
		/// Статьи
		/// </summary>
		DbSet<Article> Articles { get; }

		/// <summary>
		/// Сумки
		/// </summary>
		DbSet<Bag> Bags { get; }

		/// <summary>
		/// Предметы в сумках
		/// </summary>
		DbSet<BagItem> BagItems { get; }

		/// <summary>
		/// Тела
		/// </summary>
		DbSet<Body> Bodies { get; }

		/// <summary>
		/// Части тела
		/// </summary>
		DbSet<BodyPart> BodyParts { get; }

		/// <summary>
		/// Типы частей тела
		/// </summary>
		DbSet<BodyPartType> BodyPartTypes { get; }

		/// <summary>
		/// Шаблоны тел
		/// </summary>
		DbSet<BodyTemplate> BodyTemplates { get; }

		/// <summary>
		/// Части шаблона тела
		/// </summary>
		DbSet<BodyTemplatePart> BodyTemplateParts { get; }

		/// <summary>
		/// Пресонажи
		/// </summary>
		DbSet<Character> Characters { get; }

		/// <summary>
		/// Характеристики
		/// </summary>
		DbSet<Characteristic> Characteristics { get; }

		/// <summary>
		/// Модификаторы персонажа
		/// </summary>
		DbSet<CharacterModifier> CharacterModifiers { get; }

		/// <summary>
		/// Параметры персонажей
		/// </summary>
		DbSet<CharacterParameter> CharacterParameters { get; }

		/// <summary>
		/// Шаблоны персонажа
		/// </summary>
		DbSet<CharacterTemplate> CharacterTemplates { get; }

		/// <summary>
		/// Модификаторы шаблона персонажа
		/// </summary>
		DbSet<CharacterTemplateModifier> CharacterTemplateModifiers { get; }

		/// <summary>
		/// Слоты шаблона пресонажа
		/// </summary>
		DbSet<CharacterTemplateSlot> CharacterTemplateSlots { get; }

		/// <summary>
		/// Состояния
		/// </summary>
		DbSet<Condition> Conditions { get; }

		/// <summary>
		/// Существа
		/// </summary>
		DbSet<Creature> Creatures { get; }

		/// <summary>
		/// Параметры существ
		/// </summary>
		DbSet<CreatureParameter> CreatureParameters { get; }

		/// <summary>
		/// Части тел существ
		/// </summary>
		DbSet<CreaturePart> CreatureParts { get; }

		/// <summary>
		/// Шаблоны существ
		/// </summary>
		DbSet<CreatureTemplate> CreatureTemplates { get; }

		/// <summary>
		/// Части тел шаблонов существ
		/// </summary>
		DbSet<CreatureTemplatePart> CreatureTemplateParts { get; }

		/// <summary>
		/// Параметры шаблонов существ
		/// </summary>
		DbSet<CreatureTemplateParameter> CreatureTemplateParameters { get; }

		/// <summary>
		/// События
		/// </summary>
		DbSet<Event> Events { get; }

		/// <summary>
		/// Игры
		/// </summary>
		DbSet<Game> Games { get; }

		/// <summary>
		/// Роли в игре
		/// </summary>
		DbSet<GameRole> GameRoles { get; }

		/// <summary>
		/// Графические файлы
		/// </summary>
		DbSet<ImgFile> ImgFiles { get; }

		/// <summary>
		/// Экземпляры
		/// </summary>
		DbSet<Instance> Instances { get; }

		/// <summary>
		/// Взаимодействия
		/// </summary>
		DbSet<Interaction> Interactions { get; }

		/// <summary>
		/// Предметы во взаимодействии
		/// </summary>
		DbSet<InteractionItem> InteractionItems { get; }

		/// <summary>
		/// Роли во взаимодействии
		/// </summary>
		DbSet<InteractionsRole> InteractionsRoles { get; }

		/// <summary>
		/// Деятельности роли во взаимодействии
		/// </summary>
		DbSet<InteractionsRoleActivity> InteractionsRoleActivities { get; }

		/// <summary>
		/// Интерфейсы
		/// </summary>
		DbSet<Interface> Interfaces { get; }

		/// <summary>
		/// Предметы
		/// </summary>
		DbSet<Item> Items { get; }

		/// <summary>
		/// Шаблоны предметов
		/// </summary>
		DbSet<ItemTemplate> ItemTemplates { get; }

		/// <summary>
		/// Модификаторы шаблона предмета
		/// </summary>
		DbSet<ItemTemplateModifier> ItemTemplateModifiers { get; }

		/// <summary>
		/// Модификаторы игры
		/// </summary>
		DbSet<Modifier> Modifiers { get; }

		/// <summary>
		/// Скрипты модификатора
		/// </summary>
		DbSet<ModifierScript> ModifierScripts { get; }

		/// <summary>
		/// Уведомления
		/// </summary>
		DbSet<Notification> Notifications { get; }

		/// <summary>
		/// Уведомления об удалении предметов
		/// </summary>
		DbSet<NotificationDeletedItem> NotificationsDeletedItems { get; }

		/// <summary>
		/// Уведомления о предложении передать предметы
		/// </summary>
		DbSet<NotificationTradeRequest> NotificationsTradeRequest { get; }
		/// <summary>
		/// Параметры
		/// </summary>
		DbSet<Parameter> Parameters { get; }

		/// <summary>
		/// Стороны
		/// </summary>
		DbSet<Party> Parties { get; }

		/// <summary>
		/// Роли стороны во взаимодействии
		/// </summary>
		DbSet<PartyInteractionsRole> PartyInteractionsRoles { get; }

		/// <summary>
		/// Пререквизиты
		/// </summary>
		DbSet<Prerequisite> Prerequisites { get; }

		/// <summary>
		/// Cкрипты
		/// </summary>
		DbSet<Script> Scripts { get; }

		/// <summary>
		/// Пререквезиты скрипта
		/// </summary>
		DbSet<ScriptPrerequisites> ScriptPrerequisites { get; }

		/// <summary>
		/// Слоты
		/// </summary>
		DbSet<Slot> Slots { get; }

		/// <summary>
		/// Роли в системе
		/// </summary>
		DbSet<SystemRole> SystemRoles { get; }

		/// <summary>
		/// Текстовые файлы
		/// </summary>
		DbSet<TextFile> TextFiles { get; }

		/// <summary>
		/// Пользователи
		/// </summary>
		DbSet<User> Users { get; }

		/// <summary>
		/// Аккаунты пользователей
		/// </summary>
		DbSet<UserAccount> UserAccounts { get; }

		/// <summary>
		/// Пользователи игры
		/// </summary>
		DbSet<UserGame> UserGames { get; }

		/// <summary>
		/// Персонажи пользователя игры
		/// </summary>
		DbSet<UserGameCharacter> UserGameCharacters { get; }

		/// <summary>
		/// Роли пользователей
		/// </summary>
		DbSet<UserRole> UserRoles { get; }

		/// <summary>
		/// Сохранить изменения
		/// </summary>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns>Токен</returns>
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
