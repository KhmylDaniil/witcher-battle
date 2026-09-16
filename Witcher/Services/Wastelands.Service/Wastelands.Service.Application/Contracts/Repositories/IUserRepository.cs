using Wastelands.Core.EfDataAccess.Contracts;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Contracts.Repositories
{
	public interface IUserRepository : IBaseRepository<User>
	{
		Task<User> GetByLoginAsync(string login);
	}
}
