using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Dto
{
	public class ItemTemplateArmorPartDto : BaseDto
	{
		public HumanBodyPart Part { get; set; }

		public int Armor { get; set; }
	}
}
