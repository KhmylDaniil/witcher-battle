using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class ItemArmorPartConfiguration : EntityConfiguration<ItemArmorPart>
	{
		public override void Configure(EntityTypeBuilder<ItemArmorPart> builder)
		{
			base.Configure(builder);

			builder.ToTable("ItemArmorPart");

			builder.Property(x => x.ItemId)
				.HasColumnName("ItemId")
				.IsRequired();

			builder.Property(x => x.Part)
				.HasColumnName("Part")
				.IsRequired();

			builder.Property(x => x.ArmorValue)
				.HasColumnName("ArmorValue")
				.IsRequired();

			builder.Property(x => x.MaxDurability)
				.HasColumnName("MaxDurability")
				.IsRequired();

			builder.Property(x => x.CurrentDurability)
				.HasColumnName("CurrentDurability")
				.IsRequired();
		}
	}
}
