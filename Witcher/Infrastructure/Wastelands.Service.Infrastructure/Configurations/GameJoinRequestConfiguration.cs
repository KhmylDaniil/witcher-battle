using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class GameJoinRequestConfiguration : EntityConfiguration<GameJoinRequest>
	{
		public override void Configure(EntityTypeBuilder<GameJoinRequest> builder)
		{
			base.Configure(builder);

			builder.ToTable("GameJoinRequest");

			builder.Property(x => x.UserId)
				.HasColumnName("UserId")
				.IsRequired();

			builder.Property(x => x.GameId)
				.HasColumnName("GameId")
				.IsRequired();

			builder.Property(x => x.Status)
				.HasColumnName("Status")
				.IsRequired();
		}
	}
}
