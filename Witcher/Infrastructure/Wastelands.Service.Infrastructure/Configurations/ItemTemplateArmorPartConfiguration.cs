using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class ItemTemplateArmorPartConfiguration : EntityConfiguration<ItemTemplateArmorPart>
	{
		public override void Configure(EntityTypeBuilder<ItemTemplateArmorPart> builder)
		{
			base.Configure(builder);

			builder.ToTable("ItemTemplateArmorPart");

			builder.Property(x => x.ItemTemplateId)
				.HasColumnName("ItemTemplateId")
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
		}
	}
}
