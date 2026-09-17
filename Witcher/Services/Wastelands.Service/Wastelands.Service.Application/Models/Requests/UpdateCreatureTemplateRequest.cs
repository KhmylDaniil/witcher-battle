using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class UpdateCreatureTemplateRequest : BaseRequest
	{
		public long Id { get; set; }

		public CreatureType CreatureType { get; set; }

		public string Name { get; set; }

		public string? Description { get; set; }

		public int HP { get; set; }

		public int Sta { get; set; }

		public int Int { get; set; }

		public int Ref { get; set; }

		public int Dex { get; set; }

		public int Body { get; set; }

		public int Emp { get; set; }

		public int Cra { get; set; }

		public int Will { get; set; }

		public int Speed { get; set; }

		public int Luck { get; set; }
	}
}
