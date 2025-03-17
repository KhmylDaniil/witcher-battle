using System.Linq.Expressions;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Domain.Models.Filters
{
	public class UserFilter : BaseFilter<User>
	{
		public string? Name { get; set; }

		public override Expression<Func<User, bool>> GetFilterExpression()
		{
			return entity =>
			(Id == null || entity.Id == Id) &&
			(Name == null || entity.Name.Contains(Name));
		}
	}
}
