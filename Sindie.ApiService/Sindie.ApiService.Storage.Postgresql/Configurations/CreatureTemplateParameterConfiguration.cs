using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="CreatureTemplateParameter"/>
	/// </summary>
	public class CreatureTemplateParameterConfiguration : EntityBaseConfiguration<CreatureTemplateParameter>
	{
		/// <summary>
		/// Конфигурация для <see cref="CreatureTemplateParameter"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<CreatureTemplateParameter> builder)
		{
			builder.ToTable("CreatureTemplateParameters", "GameInstance")
				.HasComment("Параметры шаблона существа");

			builder.Property(r => r.CreatureTemplateId)
				.HasColumnName("CreatureId")
				.HasComment("Айди шаблона существа")
				.IsRequired();

			builder.Property(r => r.ParameterId)
				.HasColumnName("ParameterId")
				.HasComment("Айди параметра")
				.IsRequired();

			builder.Property(r => r.ParameterValue)
				.HasColumnName("ParametrValue")
				.HasComment("Значение параметра")
				.IsRequired();

			builder.Property(r => r.StatName)
				.HasColumnName("StatName")
				.HasComment("Название корреспондирующей характеристики");

			builder.HasOne(x => x.CreatureTemplate)
				.WithMany(x => x.CreatureTemplateParameters)
				.HasForeignKey(x => x.CreatureTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Parameter)
				.WithMany(x => x.CreatureTemplateParameters)
				.HasForeignKey(x => x.ParameterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var creatureTemplateNavigation = builder.Metadata.FindNavigation(nameof(CreatureTemplateParameter.CreatureTemplate));
			creatureTemplateNavigation.SetField(CreatureTemplateParameter.CreatureTemplateField);
			creatureTemplateNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var parameterNavigation = builder.Metadata.FindNavigation(nameof(CreatureTemplateParameter.Parameter));
			parameterNavigation.SetField(CreatureTemplateParameter.ParameterField);
			parameterNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
