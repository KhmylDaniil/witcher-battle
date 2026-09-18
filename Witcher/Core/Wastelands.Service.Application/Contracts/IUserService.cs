using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Contracts
{
	public interface IUserService
	{
		Task LoginUserAsync(LoginUserRequest request);

		Task RegisterUserAsync(RegisterUserRequest request);
	}
}
