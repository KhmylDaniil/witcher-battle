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
	public class BleedEffectConfuguration : HierarchyConfiguration<BleedEffect>
	{
		public override void ConfigureChild(EntityTypeBuilder<BleedEffect> builder)
		{
			builder.ToTable("BleedEffects", "Battles")
				.HasComment("Эффекты кровотечения");
			
			builder.Property(r => r.Severity)
			.HasColumnName("Severity")
			.HasComment("Тяжесть")
			.IsRequired();
		}
	}
}
