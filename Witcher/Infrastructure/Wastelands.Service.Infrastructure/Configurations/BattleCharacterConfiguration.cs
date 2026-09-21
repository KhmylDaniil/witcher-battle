using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class BattleCharacterConfiguration : EntityConfiguration<BattleCharacter>
	{
		public override void Configure(EntityTypeBuilder<BattleCharacter> builder)
		{
			base.Configure(builder);

			builder.ToTable("BattleCharacter");

			builder.Property(x => x.BattleId)
				.HasColumnName("BattleId")
				.IsRequired();

			builder.Property(x => x.CharacterId)
				.HasColumnName("CharacterId")
				.IsRequired();

			builder.Property(x => x.MaxHP).HasColumnName("MaxHP").IsRequired();
			builder.Property(x => x.CurrentHP).HasColumnName("CurrentHP").IsRequired();
			builder.Property(x => x.MaxSta).HasColumnName("MaxSta").IsRequired();
			builder.Property(x => x.CurrentSta).HasColumnName("CurrentSta").IsRequired();

			builder.Property(x => x.Initiative)
				.HasColumnName("Initiative")
				.IsRequired(false);

			builder.Property(x => x.AppliedConditions)
				.HasColumnType("jsonb")
				.HasColumnName("AppliedConditions")
				.HasComment("AppliedConditions")
				.IsRequired();

			builder.Property(x => x.HasActedThisTurn)
				.HasColumnName("HasActedThisTurn")
				.HasComment("Основное действие текущего хода уже потрачено — открыто окно дополнительного действия")
				.IsRequired();

			builder.Property(x => x.CriticalWounds)
				.HasColumnType("jsonb")
				.HasColumnName("CriticalWounds")
				.HasComment("Критические ранения по слотам (часть тела + тип урона), см. CriticalWoundCatalog")
				.HasDefaultValueSql("'{}'")
				.ValueGeneratedNever()
				.IsRequired()
				.Metadata.SetValueComparer(new ValueComparer<Dictionary<string, Condition>>(
					(a, b) => (a ?? new Dictionary<string, Condition>()).SequenceEqual(b ?? new Dictionary<string, Condition>()),
					d => d.Aggregate(0, (hash, kv) => HashCode.Combine(hash, kv.Key, kv.Value)),
					d => new Dictionary<string, Condition>(d)));

			// Не cascade: удаление BattleCharacter не должно удалять Character, а Character удаляется
			// независимо (уже SetNull-обезопашен от игры и выпиливается из боёв вместе с самим Battle).
			builder.HasOne(x => x.Character)
				.WithMany()
				.HasForeignKey(x => x.CharacterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
