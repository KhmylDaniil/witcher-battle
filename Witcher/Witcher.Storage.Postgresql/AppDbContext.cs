using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Policy;

namespace Witcher.Storage.Postgresql
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
		/// Битвы
		/// </summary>
		public DbSet<Battle> Battles { get; set; }

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
		/// Существа
		/// </summary>
		public DbSet<Creature> Creatures { get; set; }

		/// <summary>
		/// Параметры существ
		/// </summary>
		public DbSet<CreatureSkill> CreatureSkills { get; set; }

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
		public DbSet<CreatureTemplateSkill> CreatureTemplateSkills { get; set; }

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
		/// Интерфейсы
		/// </summary>
		public DbSet<Interface> Interfaces { get; set; }

		/// <summary>
		/// Предметы
		/// </summary>
		public DbSet<ItemTemplate> ItemTemplates { get; set; }

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
		/// Оружие
		/// </summary>
		public DbSet<Weapon> Weapons { get; set; }

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

			//разное поведение для удаления существ и персонажей
			var creatureEntries = ChangeTracker.Entries()
				.Where(e => e.Entity is Creature creature
				&& creature is not Character
				&& (e.State == EntityState.Added || e.State == EntityState.Modified)
				&& creature.Battle is null);

			foreach(var entityEntry in creatureEntries)
				entityEntry.State = EntityState.Deleted;

			return await base.SaveChangesAsync(cancellationToken);
		}
	}
}
