using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.Postgresql.Configurations
{
	public class ArmorConfiguration : HierarchyConfiguration<Armor>
	{
		public override void ConfigureChild(EntityTypeBuilder<Armor> builder)
		{
			builder.ToTable("Armors", "Items").
				HasComment("Броня");

			builder.HasMany(r => r.ArmorParts)
				.WithOne(x => x.Armor)
				.HasForeignKey(x => x.ArmorId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
