using System.Linq.Expressions;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Models.Filters
{
	public class CreatureTemplateFilter : BaseFilter<CreatureTemplate>
	{
		public long? GameId { get; set; }

		public long? BodyTemplateId { get; set; }

		public string? Name { get; set; }

		public override Expression<Func<CreatureTemplate, bool>> GetFilterExpression()
		{
			return entity =>
			(Id == null || entity.Id == Id) &&
			(GameId == null || entity.GameId == GameId) &&
			(BodyTemplateId == null || entity.BodyTemplateId == BodyTemplateId) &&
			(Name == null || entity.Name.Contains(Name));
		}
	}
}
