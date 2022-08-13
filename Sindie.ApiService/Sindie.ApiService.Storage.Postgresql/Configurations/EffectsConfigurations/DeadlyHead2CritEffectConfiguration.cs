﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="DeadlyHead2CritEffect"/>
	/// </summary>
	public class DeadlyHead2CritEffectConfiguration : HierarchyConfiguration<DeadlyHead2CritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DeadlyHead2CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DeadlyHead2CritEffect> builder)
		{
			builder.ToTable("DeadlyHead2CritEffects", "Effects")
				.HasComment("Эффекты отсечения головы");
		}
	}
}