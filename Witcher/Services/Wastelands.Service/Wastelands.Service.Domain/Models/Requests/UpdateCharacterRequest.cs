namespace Wastelands.Service.Domain.Models.Requests
{
	public class UpdateCharacterRequest : BaseRequest
	{
		public long Id { get; set; }

		public string Name { get; set; }

		public int Int { get; set; }

		public int Str { get; set; }

		public int Rea { get; set; }

		public int Dex { get; set; }

		public int Cra { get; set; }

		public int Emp { get; set; }

		public int Wil { get; set; }
	}
}
