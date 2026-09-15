using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Models.Dto
{
	public class CharacterDto : BaseDto
	{
		public long UserId { get; set; }

		public string Name { get; set; }

		public int Int { get; set; }

		public int Str { get; set; }

		public int Rea { get; set; }

		public int Dex { get; set; }

		public int Cra { get; set; }

		public int Emp { get; set; }

		public int Wil { get; set; }

		public Dictionary<Skill, int> Skills { get; set; }
	}
}
