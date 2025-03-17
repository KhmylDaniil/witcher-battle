using Wastelands.Core.EfDataAccess.Entities;

namespace Wastelands.Service.Domain.Entities
{
	public class Character : Entity
	{
		public long UserId { get; set; }

		public string Name { get; set; }
	}
}
