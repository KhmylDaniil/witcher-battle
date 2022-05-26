using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для сущности параметры у персонажей
	/// </summary>
	public class CharacterParameterConfiguration : EntityBaseConfiguration<CharacterParameter>
	{
		/// <summary>
		/// Конфигурация для сущности параметры у персонажей
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<CharacterParameter> builder)
		{
			builder.ToTable("CharacterParameters", "GameInstance")
				.HasComment("Параметры персонажа");

			builder.Property(r => r.CharacterId)
				.HasColumnName("CharacterId")
				.HasComment("Айди персонажа")
				.IsRequired();

			builder.Property(r => r.ParameterId)
				.HasColumnName("ParameterId")
				.HasComment("Айди параметра")
				.IsRequired();

			builder.Property(r => r.ParameterValue)
				.HasColumnName("ParametrValue")
				.HasComment("Значение параметра")
				.IsRequired();

			builder.HasOne(x => x.Character)
				.WithMany(x => x.CharacterParameters)
				.HasForeignKey(x => x.CharacterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
			 
			builder.HasOne(x => x.Parameter)
				.WithMany(x => x.CharacterParameters)
				.HasForeignKey(x => x.ParameterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var characterFileNavigation = builder.Metadata.FindNavigation(nameof(CharacterParameter.Character));
			characterFileNavigation.SetField(CharacterParameter.CharacterField);
			characterFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var parameterFileNavigation = builder.Metadata.FindNavigation(nameof(CharacterParameter.Parameter));
			parameterFileNavigation.SetField(CharacterParameter.ParameterField);
			parameterFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
