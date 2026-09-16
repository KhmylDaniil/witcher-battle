using System.Linq.Expressions;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Models.Filters
{
	public class BodyTemplateFilter : BaseFilter<BodyTemplate>
	{
		public long? GameId { get; set; }

		public string? Name { get; set; }

		public override Expression<Func<BodyTemplate, bool>> GetFilterExpression()
		{
			return entity =>
			(Id == null || entity.Id == Id) &&
			(GameId == null || entity.GameId == GameId) &&
			(Name == null || entity.Name.Contains(Name));
		}
	}
}
