using System.Linq.Expressions;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Models.Filters
{
	public class GameFilter : BaseFilter<Game>
	{
		public string Name { get; set; }

		public override Expression<Func<Game, bool>> GetFilterExpression()
		{
			return entity =>
			(Id == null || entity.Id == Id) &&
			(Name == null || entity.Name.Contains(Name));
		}
	}
}
