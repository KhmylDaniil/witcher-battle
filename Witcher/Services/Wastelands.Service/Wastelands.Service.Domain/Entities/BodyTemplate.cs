using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Drafts;

namespace Wastelands.Service.Domain.Entities
{
	public class BodyTemplate : Entity
	{
		public long GameId { get; private set; }

		public string Name { get; private set; }

		public string? Description { get; private set; }

		public List<BodyTemplatePart> Parts { get; private set; } = [];

		private BodyTemplate()
		{
		}

		public BodyTemplate(long gameId, string name, string? description)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(gameId, nameof(gameId));
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

			GameId = gameId;
			Name = name;
			Description = description;

			Parts = DefaultHumanBodyTemplatePartsDraft.Create()
				.Select(draft => new BodyTemplatePart(draft))
				.ToList();
		}

		public void UpdateBodyTemplate(string name, string? description)
		{
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

			Name = name;
			Description = description;
		}
	}
}
