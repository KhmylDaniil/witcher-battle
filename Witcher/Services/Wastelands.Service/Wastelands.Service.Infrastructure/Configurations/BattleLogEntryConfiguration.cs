using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class BattleLogEntryConfiguration : EntityConfiguration<BattleLogEntry>
	{
		public override void Configure(EntityTypeBuilder<BattleLogEntry> builder)
		{
			base.Configure(builder);

			builder.ToTable("BattleLogEntry");

			builder.Property(x => x.BattleId).HasColumnName("BattleId").IsRequired();

			builder.Property(x => x.Message)
				.HasColumnName("Message")
				.HasColumnType("varchar(500)")
				.IsRequired();

			builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt").IsRequired();
		}
	}
}
