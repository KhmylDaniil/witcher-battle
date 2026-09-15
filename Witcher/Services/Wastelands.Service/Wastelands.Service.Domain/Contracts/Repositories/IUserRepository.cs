using Wastelands.Core.EfDataAccess.Contracts;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Domain.Contracts.Repositories
{
	public interface IUserRepository : IBaseRepository<User>
	{
		Task<User> GetByLoginAsync(string login);
	}
}
