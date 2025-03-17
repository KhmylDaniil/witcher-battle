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
	}
}