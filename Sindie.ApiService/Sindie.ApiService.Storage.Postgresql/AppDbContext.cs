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
		/// Применяемые состояния
		/// </summary>
		public DbSet<AppliedCondition> AppliedConditions { get; set; }

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
		public DbSet<CreatureSkill> CreatureParameters { get; set; }

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
		public DbSet<CreatureTemplateSkill> CreatureTemplateParameters { get; set; }

		/// <summary>
		/// Типы существ
		/// </summary>
		public DbSet<CreatureType> CreatureTypes { get; set; }

		/// <summary>
		/// Типы урона
		/// </summary>
		public DbSet<DamageType> DamageTypes { get; set; }

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
		public DbSet<Battle> Battles { get; set; }

		/// <summary>
		/// Интерфейсы
		/// </summary>
		public DbSet<Interface> Interfaces { get; set; }

		/// <summary>
		/// Навыки
		/// </summary>
		public DbSet<Skill> Skills { get; set; }

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
