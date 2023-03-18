using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для роли пользователя <see cref="UserRole"/>
	/// </summary>
	public class UserRoleConfiguration : EntityBaseConfiguration<UserRole>
	{
		/// <summary>
		/// Конфигурация модели
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<UserRole> builder)
		{
			builder.ToTable("UserRoles", "System")
				.HasComment("Роли пользователей");

			builder.Property(r => r.UserId)
				.IsRequired();

			builder.Property(r => r.SystemRoleId)
				.IsRequired();

			builder.HasOne(x => x.User)
				.WithMany(x => x.UserRoles)
				.HasForeignKey(x => x.UserId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.SystemRole)
				.WithMany(x => x.UserRoles)
				.HasForeignKey(x => x.SystemRoleId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
