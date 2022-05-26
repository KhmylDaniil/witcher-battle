using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для сущности сумка
	/// </summary>
	public class BagConfiguration : EntityBaseConfiguration<Bag>
	{
		/// <summary>
		/// Конфигурация для сущности сумка
		/// </summary>
		/// <param name="builder">строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Bag> builder)
		{
			builder.ToTable("Bags", "GameInstance")
				.HasComment("Сумки");

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasComment("Название сумки")
				.IsRequired();

			builder.Property(x => x.InstanceId)
				.HasColumnName("InstanceId")
				.HasComment("Айди экземпляра игры")
				.IsRequired();

			builder.Property(x => x.CharacterId)
				.HasColumnName("CharacterId")
				.HasComment("Айди персонажа");

			builder.Property(x => x.ImgFileId)
				.HasColumnName("ImgFileId")
				.HasComment("Айди графического файла");

			builder.Property(x => x.MaxBagSize)
				.HasColumnName("MaxBagSize")
				.HasComment("Максимальный размер сумки");

			builder.Property(r => r.Description)
				.HasColumnName("Description")
				.HasComment("Описание сумки");

			builder.HasOne(x => x.Instance)
				.WithMany(x => x.Bags)
				.HasForeignKey(x => x.InstanceId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ImgFile)
				.WithOne(x => x.Bag)
				.HasForeignKey<Bag>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Character)
				.WithOne()
				.HasForeignKey<Character>(x => x.BagId)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.BagItems)
				.WithOne(x => x.Bag)
				.HasForeignKey(x => x.BagId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.NotificationDeletedItems)
				.WithOne(x => x.Bag)
				.HasForeignKey(x => x.BagId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var instanceNavigation = builder.Metadata.FindNavigation(nameof(Bag.Instance));
			instanceNavigation.SetField(Bag.InstanceField);
			instanceNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var imgFileNavigation = builder.Metadata.FindNavigation(nameof(Bag.ImgFile));
			imgFileNavigation.SetField(Bag.ImgFileField);
			imgFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var characterNavigation = builder.Metadata.FindNavigation(nameof(Bag.Character));
			characterNavigation.SetField(Bag.CharacterField);
			characterNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}