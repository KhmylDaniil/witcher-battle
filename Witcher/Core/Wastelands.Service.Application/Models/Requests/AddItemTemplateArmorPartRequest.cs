using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class AddItemTemplateArmorPartRequest : BaseRequest
	{
		public long ItemTemplateId { get; set; }

		public HumanBodyPart Part { get; set; }

		public int Armor { get; set; }
	}
}
