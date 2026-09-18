namespace Wastelands.Service.Application.Models.Requests
{
	public class AddItemRequest : BaseRequest
	{
		public long CharacterId { get; set; }

		public long ItemTemplateId { get; set; }
	}
}
