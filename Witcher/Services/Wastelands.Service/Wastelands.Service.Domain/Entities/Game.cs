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
