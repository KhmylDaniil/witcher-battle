using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	public class PoisonEffectConfuguration : HierarchyConfiguration<PoisonEffect>
	{
		public override void ConfigureChild(EntityTypeBuilder<PoisonEffect> builder)
		{
			builder.ToTable("PoisonEffects", "Battles")
				.HasComment("Эффекты отравления");

			builder.Property(r => r.Toxicity)
			.HasColumnName("Toxicity")
			.HasComment("Тяжесть")
			.IsRequired();
		}
	}
}
