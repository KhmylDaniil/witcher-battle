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
	/// <summary>
	/// Конфигурация для <see cref="Condition"/>
	/// </summary>
	public class ConditionConfiguration: EntityBaseConfiguration<Condition>
	{
		/// <summary>
		/// Конфигурация для <see cref="Condition"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<Condition> builder)
		{
			builder.ToTable("Conditions", "GameRules").
				HasComment("Состояния");

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название состояния")
				.IsRequired();

			builder.HasMany(x => x.Creatures)
				.WithMany(x => x.Conditions);

			builder.HasMany(x => x.AppliedConditions)
				.WithOne(x => x.Condition)
				.HasForeignKey(x => x.ConditionId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
