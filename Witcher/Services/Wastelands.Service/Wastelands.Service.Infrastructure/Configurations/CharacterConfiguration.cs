using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;
using Newtonsoft.Json;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class CharacterConfiguration : EntityConfiguration<Character>
	{
		private static readonly string _defaultSkillsValue = JsonConvert.SerializeObject(new Dictionary<Skill, int>());

		public override void Configure(EntityTypeBuilder<Character> builder)
		{
			base.Configure(builder);

			builder.ToTable("Character");

			builder.Property(x => x.Name)
			.HasColumnName("Name")
			.HasColumnType("varchar(30)")
			.IsRequired();

			builder.Property(r => r.Int)
			.HasColumnName("Int")
			.HasComment("Intellect")
			.IsRequired();

			builder.Property(r => r.Rea)
			.HasColumnName("Rea")
			.HasComment("Reaction")
			.IsRequired();

			builder.Property(r => r.Dex)
			.HasColumnName("Dex")
			.HasComment("Dexterity")
			.IsRequired();

			builder.Property(r => r.Str)
			.HasColumnName("Str")
			.HasComment("Strength")
			.IsRequired();

			builder.Property(r => r.Emp)
			.HasColumnName("Emp")
			.HasComment("Empathy")
			.IsRequired();

			builder.Property(r => r.Cra)
			.HasColumnName("Cra")
			.HasComment("Craft")
			.IsRequired();

			builder.Property(r => r.Wil)
			.HasColumnName("Wil")
			.HasComment("Willpower")
			.IsRequired();

			builder.Property(x => x.Skills)
			.HasColumnType("jsonb")
			.HasColumnName("Skills")
			.HasComment("Skills")
			.IsRequired();
		}
	}
}
