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
	/// Конфигурация для <see cref="Creature"/>
	/// </summary>
	public class CreatureConfiguration : EntityBaseConfiguration<Creature>
	{
		/// <summary>
		/// Конфигурация для <see cref="Creature"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<Creature> builder)
		{
			builder.ToTable("Creatures", "GameRules").
				HasComment("Существа");

			builder.Property(r => r.InstanceId)
				.HasColumnName("InstanceId")
				.HasComment("Айди Экземпляра")
				.IsRequired();

			builder.Property(r => r.ImgFileId)
				.HasColumnName("ImgFileId")
				.HasComment("Айди графического файла");

			builder.Property(r => r.CreatureBodyId)
				.HasColumnName("CreatureBodyId")
				.HasComment("Айди тела существа")
				.IsRequired();

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название существа")
				.IsRequired();

			builder.Property(r => r.Type)
				.HasColumnName("Type")
				.HasComment("Тип шаблона существа")
				.IsRequired();

			builder.Property(r => r.Description)
				.HasColumnName("Description")
				.HasComment("Описание шаблона");

			builder.HasOne(x => x.Instance)
				.WithMany(x => x.Creatures)
				.HasForeignKey(x => x.InstanceId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ImgFile)
				.WithOne(x => x.Creature)
				.HasForeignKey<Creature>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.CreatureBody)
				.WithOne(x => x.Creature)
				.HasForeignKey<CreatureBody>(x => x.CreatureId)
				.HasPrincipalKey<Creature>(x => x.CreatureBodyId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CreatureParameters)
				.WithOne(x => x.Creature)
				.HasForeignKey(x => x.CreatureId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var instanceNavigation = builder.Metadata.FindNavigation(nameof(Creature.Instance));
			instanceNavigation.SetField(Creature.InstanceField);
			instanceNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var imgFileNavigation = builder.Metadata.FindNavigation(nameof(Creature.ImgFile));
			imgFileNavigation.SetField(Creature.ImgFileField);
			imgFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var creatureBodyNavigation = builder.Metadata.FindNavigation(nameof(Creature.CreatureBody));
			creatureBodyNavigation.SetField(Creature.CreatureBodyField);
			creatureBodyNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
