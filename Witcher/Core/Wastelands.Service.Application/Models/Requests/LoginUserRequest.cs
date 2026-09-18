namespace Wastelands.Service.Application.Models.Requests
{
	public class LoginUserRequest : BaseRequest
	{
		public string Login { get; set; }

		public string Password { get; set; }
	}
}
