namespace Wastelands.Service.Application.Models.Requests
{
	public class UpdateItemTemplateArmorPartRequest : BaseRequest
	{
		public long ItemTemplateId { get; set; }

		public long ArmorPartId { get; set; }

		public int ArmorValue { get; set; }

		public int MaxDurability { get; set; }
	}
}
