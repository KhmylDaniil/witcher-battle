namespace Wastelands.Service.Domain.Models.Requests
{
	public class BaseDeleteRequest : BaseRequest
	{
		public string Name {  get; set; }

		public long Id { get; set; }
	}
}
