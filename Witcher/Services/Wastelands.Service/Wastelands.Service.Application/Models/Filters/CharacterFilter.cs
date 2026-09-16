using System.Linq.Expressions;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Models.Filters
{
	public class CharacterFilter : BaseFilter<Character>
	{
		public string Name { get; set; }
		public long? GameId { get; set; }
		public override Expression<Func<Character, bool>> GetFilterExpression()
		{
			return entity =>
			(Id == null || entity.Id == Id) &&
			(Name == null || entity.Name.Contains(Name)) &&
			(GameId == null || entity.GameId == GameId);
		}
	}
}
