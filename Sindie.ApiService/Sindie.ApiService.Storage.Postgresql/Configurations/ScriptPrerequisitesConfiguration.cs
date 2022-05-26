using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для Скрипта пререквизита
	/// </summary>
	public class ScriptPrerequisitesConfiguration : EntityBaseConfiguration<ScriptPrerequisites>
	{
		/// <summary>
		/// Конфигурация для Скрипта пререквизита
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<ScriptPrerequisites> builder)
		{
			builder.ToTable("ScriptPrerequisites", "GameRules").
				HasComment("Пререквизиты скрипта");

			builder.Property(r => r.ScriptId)
				.HasColumnName("ScriptId")
				.HasComment("Айди скрипта")
				.IsRequired();

			builder.Property(r => r.PrerequisiteId)
				.HasColumnName("PrerequisiteId")
				.HasComment("Айди пререквизита");

			builder.Property(r => r.IsValid)
				.HasColumnName("IsValid")
				.HasComment("Валидность скрипта")
				.IsRequired();

			builder.Property(r => r.ValidationText)
				.HasColumnName("ValidationText")
				.HasComment("Валидационный текст");

			builder.HasOne(x => x.Prerequisite)
				.WithMany(x => x.ScriptPrerequisites)
				.HasForeignKey(x => x.PrerequisiteId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Script)
				.WithMany(x => x.ScriptPrerequisites)
				.HasForeignKey(x => x.ScriptId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var gameNavigationPrerequisite = builder.Metadata.FindNavigation(nameof(ScriptPrerequisites.Prerequisite));
			gameNavigationPrerequisite.SetField(ScriptPrerequisites.PrerequisiteField);
			gameNavigationPrerequisite.SetPropertyAccessMode(PropertyAccessMode.Field);

			var gameNavigationScript = builder.Metadata.FindNavigation(nameof(ScriptPrerequisites.Script));
			gameNavigationScript.SetField(ScriptPrerequisites.ScriptField);
			gameNavigationScript.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
