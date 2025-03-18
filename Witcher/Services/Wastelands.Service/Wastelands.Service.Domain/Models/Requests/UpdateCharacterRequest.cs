namespace Wastelands.Service.Domain.Models.Requests
{
	public class UpdateCharacterRequest : BaseRequest
	{
		public long Id { get; set; }

		public string Name { get; set; }
	}
}
