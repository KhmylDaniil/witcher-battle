using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.MySql.Configurations
{
	/// <summary>
	/// Конфигурация для аккаунта пользователя <see cref="UserAccount"/>
	/// </summary>
	public class UserAccountConfiguration : EntityBaseConfiguration<UserAccount>
	{
		/// <summary>
		/// Конфигурация модели
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<UserAccount> builder)
		{
			builder.ToTable("UserAccounts", "System")
				.HasComment("Аккаунты пользователей");

			builder.Property(r => r.UserId)
				.HasColumnName("Id")
				.HasComment("Айди")
				.IsRequired();

			builder.Property(x => x.Login)
				.HasColumnName("Login")
				.HasComment("Логин")
				.IsRequired();

			builder.HasIndex(x => x.Login)
				.IsUnique();

			builder.Property(r => r.PasswordHash)
				.HasColumnName("PasswordHash")
				.HasComment("Хэш пароля")
				.IsRequired();

			builder.HasOne(x => x.User)
				.WithMany(x => x.UserAccounts)
				.HasForeignKey(x => x.UserId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
