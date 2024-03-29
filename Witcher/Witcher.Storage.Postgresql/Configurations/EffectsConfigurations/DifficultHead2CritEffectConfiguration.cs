﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="DifficultHead2CritEffect"/>
	/// </summary>
	public class DifficultHead2CritEffectConfiguration : HierarchyConfiguration<DifficultHead2CritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DifficultHead2CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DifficultHead2CritEffect> builder)
		{
			builder.ToTable("DifficultHead2CritEffects", "Effects")
				.HasComment("Эффекты проломленного черепа");
		}
	}
}
