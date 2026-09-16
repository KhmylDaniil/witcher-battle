using Wastelands.Core.EfDataAccess.Entities;

namespace Wastelands.Service.Domain.Entities
{
	public class User : Entity
	{
		protected User()
		{
		}

		public string Name { get; set; }

		public string Email { get; set; }

		public string Login { get; set; }

		public string Password { get; set; }

		public List<Character> Characters { get; set; }

		/// <summary>EF-навигация — нужна только для конфигурации FK Game.CreatedByUserId.</summary>
		public List<Game> CreatedGames { get; set; } = [];

		/// <summary>EF-навигация — нужна только для конфигурации FK UserGame.UserId.</summary>
		public List<UserGame> UserGames { get; set; } = [];

		/// <summary>EF-навигация — нужна только для конфигурации FK GameJoinRequest.UserId.</summary>
		public List<GameJoinRequest> GameJoinRequests { get; set; } = [];
	}
}