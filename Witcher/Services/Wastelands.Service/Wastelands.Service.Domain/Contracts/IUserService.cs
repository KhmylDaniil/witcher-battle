using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.Domain.Contracts
{
	public interface IUserService
	{
		Task LoginUserAsync(LoginUserRequest request);

		Task RegisterUserAsync(RegisterUserRequest request);
	}
}
