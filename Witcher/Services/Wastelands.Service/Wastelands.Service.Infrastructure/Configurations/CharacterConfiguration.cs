using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class CharacterConfiguration : EntityConfiguration<Character>
	{
		public override void Configure(EntityTypeBuilder<Character> builder)
		{
			base.Configure(builder);

			builder.ToTable("Character");

			builder.Property(x => x.Name)
				.HasColumnName("Login")
				.HasColumnType("varchar(30)")
				.IsRequired();
		}
	}
}
