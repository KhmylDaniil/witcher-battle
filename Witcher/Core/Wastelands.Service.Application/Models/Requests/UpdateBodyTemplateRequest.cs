namespace Wastelands.Service.Application.Models.Requests
{
	public class UpdateBodyTemplateRequest : BaseRequest
	{
		public long Id { get; set; }

		public string Name { get; set; }

		public string? Description { get; set; }
	}
}
