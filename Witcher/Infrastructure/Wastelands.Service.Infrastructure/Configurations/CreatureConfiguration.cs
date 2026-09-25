using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class CreatureConfiguration : EntityConfiguration<Creature>
	{
		public override void Configure(EntityTypeBuilder<Creature> builder)
		{
			base.Configure(builder);

			builder.ToTable("Creature");

			builder.Property(x => x.BattleId)
				.HasColumnName("BattleId")
				.IsRequired();

			builder.Property(x => x.CreatureTemplateId)
				.HasColumnName("CreatureTemplateId")
				.IsRequired();

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasColumnType("varchar(50)")
				.IsRequired();

			builder.Property(x => x.CreatureType)
				.HasColumnName("CreatureType")
				.IsRequired();

			builder.Property(x => x.MaxHP).HasColumnName("MaxHP").IsRequired();
			builder.Property(x => x.CurrentHP).HasColumnName("CurrentHP").IsRequired();
			builder.Property(x => x.MaxSta).HasColumnName("MaxSta").IsRequired();
			builder.Property(x => x.CurrentSta).HasColumnName("CurrentSta").IsRequired();
			builder.Property(x => x.Ref).HasColumnName("Ref").IsRequired();
			builder.Property(x => x.Recovery).HasColumnName("Recovery").HasComment("(Body+Will шаблона)/2, вычисляется на сервере").IsRequired();
			builder.Property(x => x.Stun).HasColumnName("Stun").HasComment("(Body+Will шаблона)/2, вычисляется на сервере").IsRequired();

			builder.Property(x => x.Initiative)
				.HasColumnName("Initiative")
				.IsRequired(false);

			builder.Property(x => x.MapColumn)
				.HasColumnName("MapColumn")
				.IsRequired(false);

			builder.Property(x => x.MapRow)
				.HasColumnName("MapRow")
				.IsRequired(false);

			// Страховка на уровне БД к доменному правилу "один участник на гекс" (Battle.PlaceParticipantOnMap)
			// в пределах этой таблицы; пересечение существо/персонаж проверяет только домен. NULL-позиции
			// (не выставлен) в Postgres уникальность не нарушают.
			builder.HasIndex(x => new { x.BattleId, x.MapColumn, x.MapRow })
				.IsUnique();

			builder.Property(x => x.AppliedConditions)
				.HasColumnType("jsonb")
				.HasColumnName("AppliedConditions")
				.HasComment("AppliedConditions")
				.IsRequired();

			builder.Property(x => x.ArmorReductionByPartId)
				.HasColumnType("jsonb")
				.HasColumnName("ArmorReductionByPartId")
				.HasComment("Износ брони по частям тела, накопленный этим существом в этом бою")
				.HasDefaultValueSql("'{}'")
				// HasDefaultValueSql иначе неявно включает конвенцию ValueGeneratedOnAdd — EF решит,
				// что колонка генерируется базой, и перестанет отправлять её в UPDATE после INSERT.
				.ValueGeneratedNever()
				.IsRequired()
				// Без явного ValueComparer EF сравнивает Dictionary по ссылке — мутация словаря на месте
				// (WearArmor) не отличалась бы от "не менялось", и колонка не попадала бы в UPDATE.
				.Metadata.SetValueComparer(new ValueComparer<Dictionary<long, int>>(
					(a, b) => (a ?? new Dictionary<long, int>()).SequenceEqual(b ?? new Dictionary<long, int>()),
					d => d.Aggregate(0, (hash, kv) => HashCode.Combine(hash, kv.Key, kv.Value)),
					d => new Dictionary<long, int>(d)));

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
		}
	}
}
