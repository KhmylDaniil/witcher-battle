using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class UserConfiguration : EntityConfiguration<User>
	{
		public override void Configure(EntityTypeBuilder<User> builder)
		{
			base.Configure(builder);

			builder.ToTable("User");

			builder.Property(x => x.Login)
				.HasColumnName("Login")
				.HasColumnType("varchar(30)")
				.IsRequired();

			builder.Property(x => x.Password)
				.HasColumnName("Password")
				.HasColumnType("varchar(255)")
				.IsRequired();

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasColumnType("varchar(20)")
				.IsRequired();

			builder.Property(x => x.Email)
				.HasColumnName("Email")
				.HasColumnType("varchar(30)")
				.IsRequired(false);

			builder.HasMany(x => x.Characters)
				.WithOne()
				.HasForeignKey(x => x.UserId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
