namespace Wastelands.Service.Domain.Models.Requests
{
	public class CreateBodyTemplateRequest : BaseRequest
	{
		public long GameId { get; set; }

		public string Name { get; set; }

		public string? Description { get; set; }
	}
}
