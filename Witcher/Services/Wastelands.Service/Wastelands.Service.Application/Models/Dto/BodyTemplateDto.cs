namespace Wastelands.Service.Application.Models.Dto
{
	public class BodyTemplateDto : BaseDto
	{
		public long GameId { get; set; }

		public string Name { get; set; }

		public string? Description { get; set; }

		public List<BodyTemplatePartDto> Parts { get; set; } = [];
	}
}
