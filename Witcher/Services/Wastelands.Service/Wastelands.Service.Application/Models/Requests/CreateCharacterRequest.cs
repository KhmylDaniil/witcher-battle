namespace Wastelands.Service.Application.Models.Requests
{
	public class CreateCharacterRequest : BaseRequest
	{
		public long GameId { get; set; }

		public string Name { get; set; }

		public string? ImageUrl { get; set; }

		public int HP { get; set; }

		public int Sta { get; set; }

		public int Int { get; set; }

		public int Str { get; set; }

		public int Rea { get; set; }

		public int Dex { get; set; }

		public int Cra { get; set; }

		public int Emp { get; set; }

		public int Wil { get; set; }
	}
}
