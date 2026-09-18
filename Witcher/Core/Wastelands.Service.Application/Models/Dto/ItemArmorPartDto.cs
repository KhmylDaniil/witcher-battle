using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Dto
{
	public class ItemArmorPartDto : BaseDto
	{
		public HumanBodyPart Part { get; set; }

		public int ArmorValue { get; set; }

		public int MaxDurability { get; set; }

		public int CurrentDurability { get; set; }
	}
}
