using Microsoft.EntityFrameworkCore;
using Witcher.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Abstractions
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
		/// Битвы
		/// </summary>
		DbSet<Battle> Battles { get; }

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
		/// Существа
		/// </summary>
		DbSet<Creature> Creatures { get; }

		/// <summary>
		/// Параметры существ
		/// </summary>
		DbSet<CreatureSkill> CreatureSkills { get; }

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
		DbSet<CreatureTemplateSkill> CreatureTemplateSkills { get; }

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
		/// Интерфейсы
		/// </summary>
		DbSet<Interface> Interfaces { get; }

		/// <summary>
		/// Предметы
		/// </summary>
		DbSet<ItemTemplate> ItemTemplates { get; }

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
		/// Оружие
		/// </summary>
		DbSet<Weapon> Weapons { get; }

		/// <summary>
		/// Сохранить изменения
		/// </summary>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns>Токен</returns>
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
