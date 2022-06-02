using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;
using System;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="CreatureParameter"/>
	/// </summary>
	public class CreatureParameterConfiguration : EntityBaseConfiguration<CreatureParameter>
	{
		/// <summary>
		/// Конфигурация для <see cref="CreatureParameter"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<CreatureParameter> builder)
		{
			builder.ToTable("CreatureParameters", "GameInstance")
				.HasComment("Параметры существа");

			builder.Property(r => r.CreatureId)
				.HasColumnName("CreatureId")
				.HasComment("Айди существа")
				.IsRequired();

			builder.Property(r => r.ParameterId)
				.HasColumnName("ParameterId")
				.HasComment("Айди параметра")
				.IsRequired();

			builder.Property(r => r.ParameterValue)
				.HasColumnName("ParametrValue")
				.HasComment("Значение параметра")
				.IsRequired();

			builder.HasOne(x => x.Creature)
				.WithMany(x => x.CreatureParameters)
				.HasForeignKey(x => x.CreatureId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Parameter)
				.WithMany(x => x.CreatureParameters)
				.HasForeignKey(x => x.ParameterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var creatureNavigation = builder.Metadata.FindNavigation(nameof(CreatureParameter.Creature));
			creatureNavigation.SetField(CreatureParameter.CreatureField);
			creatureNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var parameterNavigation = builder.Metadata.FindNavigation(nameof(CreatureParameter.Parameter));
			parameterNavigation.SetField(CreatureParameter.ParameterField);
			parameterNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
