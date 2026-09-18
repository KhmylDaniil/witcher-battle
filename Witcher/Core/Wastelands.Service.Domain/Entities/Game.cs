using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;

namespace Wastelands.Service.Domain.Entities
{
	public class Game : Entity
	{
		public string Name { get; private set; }

		public long CreatedByUserId { get; private set; }

		/// <summary>EF-навигация (как <see cref="User.Characters"/>) — нужна только для конфигурации FK Character.GameId.</summary>
		public List<Character> Characters { get; set; } = [];

		/// <summary>EF-навигация — нужна только для конфигурации FK UserGame.GameId.</summary>
		public List<UserGame> UserGames { get; set; } = [];

		/// <summary>EF-навигация — нужна только для конфигурации FK GameJoinRequest.GameId.</summary>
		public List<GameJoinRequest> GameJoinRequests { get; set; } = [];

		/// <summary>EF-навигация — нужна только для конфигурации FK BodyTemplate.GameId.</summary>
		public List<BodyTemplate> BodyTemplates { get; set; } = [];

		/// <summary>EF-навигация — нужна только для конфигурации FK CreatureTemplate.GameId.</summary>
		public List<CreatureTemplate> CreatureTemplates { get; set; } = [];

		/// <summary>EF-навигация — нужна только для конфигурации FK Battle.GameId.</summary>
		public List<Battle> Battles { get; set; } = [];

		private Game()
		{
		}

		public Game(string name, long createdByUserId)
		{
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(createdByUserId, nameof(createdByUserId));

			Name = name;
			CreatedByUserId = createdByUserId;
		}

		public void UpdateGame(string name)
		{
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

			Name = name;
		}
	}
}
