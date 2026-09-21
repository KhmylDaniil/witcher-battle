namespace Wastelands.Service.Application.Models.Requests
{
	public class UpdateItemTemplateArmorPartRequest : BaseRequest
	{
		public long ItemTemplateId { get; set; }

		public long ArmorPartId { get; set; }

		public int Armor { get; set; }
	}
}
