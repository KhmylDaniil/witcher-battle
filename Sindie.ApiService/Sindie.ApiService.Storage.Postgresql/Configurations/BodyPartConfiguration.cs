﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="BodyPart"/>
	/// </summary>
	public class BodyPartConfiguration : EntityBaseConfiguration<BodyPart>
	{
		/// <summary>
		/// Конфигурация для <see cref="BodyPart"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<BodyPart> builder)
		{
			builder.ToTable("BodyParts", "GameRules")
				.HasComment("Части тела");

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasComment("Название")
				.IsRequired();

			builder.Property(x => x.DamageModifier)
				.HasColumnName("DamageModifer")
				.HasComment("Модификатор урона")
				.IsRequired();

			builder.Property(x => x.HitPenalty)
				.HasColumnName("HitPenalty")
				.HasComment("Пенальти за прицеливание")
				.IsRequired();

			builder.Property(x => x.MinToHit)
				.HasColumnName("MinToHit")
				.HasComment("Минимальное значение попадания")
				.IsRequired();

			builder.Property(x => x.MaxToHit)
				.HasColumnName("MaxToHit")
				.HasComment("Максимальное значение попадания")
				.IsRequired();

			builder.Property(x => x.BodyPartTypeId)
				.HasColumnName("BodyPartTypeId")
				.HasComment("Айди типа части тела")
				.IsRequired();

			builder.HasOne(x => x.BodyPartType)
				.WithMany(x => x.BodyParts)
				.HasForeignKey(x => x.BodyPartTypeId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var bodyPartTypeeNavigation = builder.Metadata.FindNavigation(nameof(BodyPart.BodyPartType));
			bodyPartTypeeNavigation.SetField(BodyPart.BodyPartTypeField);
			bodyPartTypeeNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}