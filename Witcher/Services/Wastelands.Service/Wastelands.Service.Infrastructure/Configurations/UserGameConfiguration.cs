using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class UserGameConfiguration : EntityConfiguration<UserGame>
	{
		public override void Configure(EntityTypeBuilder<UserGame> builder)
		{
			base.Configure(builder);

			builder.ToTable("UserGame");

			builder.Property(x => x.UserId)
				.HasColumnName("UserId")
				.IsRequired();

			builder.Property(x => x.GameId)
				.HasColumnName("GameId")
				.IsRequired();
		}
	}
}
