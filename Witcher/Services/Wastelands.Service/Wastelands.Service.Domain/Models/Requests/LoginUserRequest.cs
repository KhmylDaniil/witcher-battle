namespace Wastelands.Service.Domain.Models.Requests
{
	public class LoginUserRequest : BaseRequest
	{
		public string Login { get; set; }

		public string Password { get; set; }
	}
}
