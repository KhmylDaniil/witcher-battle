using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	public class CreatureTemplate : Entity
	{
		public const int MinSkillValue = 1;
		public const int MaxSkillValue = 10;

		public long GameId { get; private set; }

		public long BodyTemplateId { get; private set; }

		public CreatureType CreatureType { get; private set; }

		public string Name { get; private set; }

		public string? Description { get; private set; }

		public int HP { get; private set; }

		public int Sta { get; private set; }

		public int Int { get; private set; }

		public int Ref { get; private set; }

		public int Dex { get; private set; }

		public int Body { get; private set; }

		public int Emp { get; private set; }

		public int Cra { get; private set; }

		public int Will { get; private set; }

		public int Speed { get; private set; }

		public int Luck { get; private set; }

		public List<CreatureTemplatePart> Parts { get; private set; } = [];

		public Dictionary<Skill, int> Skills { get; private set; } = [];

		public Dictionary<DamageType, DamageTypeModifier> DamageTypeModifiers { get; private set; } = [];

		public List<Ability> Abilities { get; private set; } = [];

		private CreatureTemplate()
		{
		}

		public CreatureTemplate(
			long gameId,
			BodyTemplate bodyTemplate,
			CreatureType creatureType,
			string name,
			string? description,
			int hp,
			int sta,
			int @int,
			int @ref,
			int dex,
			int body,
			int emp,
			int cra,
			int will,
			int speed,
			int luck)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(gameId, nameof(gameId));
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(hp, nameof(hp));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(sta, nameof(sta));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(@int, nameof(@int));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(@ref, nameof(@ref));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(dex, nameof(dex));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(body, nameof(body));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(emp, nameof(emp));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(cra, nameof(cra));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(will, nameof(will));
			InvalidArgumentException.ThrowIfLessThanZero(speed, nameof(speed));
			InvalidArgumentException.ThrowIfLessThanZero(luck, nameof(luck));

			GameId = gameId;
			BodyTemplateId = bodyTemplate.Id;
			CreatureType = creatureType;
			Name = name;
			Description = description;
			HP = hp;
			Sta = sta;
			Int = @int;
			Ref = @ref;
			Dex = dex;
			Body = body;
			Emp = emp;
			Cra = cra;
			Will = will;
			Speed = speed;
			Luck = luck;

			Parts = bodyTemplate.Parts.Select(part => new CreatureTemplatePart(part)).ToList();
		}

		public void UpdateCreatureTemplate(
			CreatureType creatureType,
			string name,
			string? description,
			int hp,
			int sta,
			int @int,
			int @ref,
			int dex,
			int body,
			int emp,
			int cra,
			int will,
			int speed,
			int luck)
		{
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(hp, nameof(hp));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(sta, nameof(sta));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(@int, nameof(@int));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(@ref, nameof(@ref));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(dex, nameof(dex));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(body, nameof(body));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(emp, nameof(emp));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(cra, nameof(cra));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(will, nameof(will));
			InvalidArgumentException.ThrowIfLessThanZero(speed, nameof(speed));
			InvalidArgumentException.ThrowIfLessThanZero(luck, nameof(luck));

			CreatureType = creatureType;
			Name = name;
			Description = description;
			HP = hp;
			Sta = sta;
			Int = @int;
			Ref = @ref;
			Dex = dex;
			Body = body;
			Emp = emp;
			Cra = cra;
			Will = will;
			Speed = speed;
			Luck = luck;
		}
	}
}
