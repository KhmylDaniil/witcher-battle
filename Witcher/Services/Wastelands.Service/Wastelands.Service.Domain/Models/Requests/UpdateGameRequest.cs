namespace Wastelands.Service.Domain.Models.Requests
{
	public class UpdateGameRequest : BaseRequest
	{
		public long Id { get; set; }

		public string Name { get; set; }
	}
}
