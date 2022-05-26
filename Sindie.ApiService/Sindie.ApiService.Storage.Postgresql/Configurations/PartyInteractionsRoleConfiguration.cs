using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для роли стороны
	/// </summary>
	public class PartyInteractionsRoleConfiguration : EntityBaseConfiguration<PartyInteractionsRole>
	{
		/// <summary>
		/// Конфигурация для роли стороны
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<PartyInteractionsRole> builder)
		{
			builder.ToTable("PartyInteractionsRoles", "InteractionRules")
				.HasComment("Роли стороны");

			builder.Property(x => x.InteractionsRoleId)
				.HasColumnName("InteractionsRoleId")
				.HasComment("Айди роли взаимодействия")
				.IsRequired();

			builder.Property(x => x.PartyId)
				.HasColumnName("PartyId")
				.HasComment("Айди стороны")
				.IsRequired();

			builder.Property(x => x.MinQuantityCharacters)
				.HasColumnName("MinQuantityCharacters")
				.HasComment("Минимальное количество персонажей");

			builder.Property(x => x.MaxQuantityCharacters)
				.HasColumnName("MaxQuantityCharacters")
				.HasComment("Максимальное количество персонажей");

			builder.HasOne(x => x.InteractionsRole)
				.WithMany(x => x.PartyInteractionsRoles)
				.HasForeignKey(x => x.InteractionsRoleId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Party)
				.WithMany(x => x.PartyInteractionsRoles)
				.HasForeignKey(x => x.PartyId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var roleNavigation = builder.Metadata.FindNavigation(nameof(PartyInteractionsRole.InteractionsRole));
			roleNavigation.SetField(PartyInteractionsRole.InteractionsRoleField);
			roleNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var partyNavigation = builder.Metadata.FindNavigation(nameof(PartyInteractionsRole.Party));
			partyNavigation.SetField(PartyInteractionsRole.PartyField);
			partyNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}

