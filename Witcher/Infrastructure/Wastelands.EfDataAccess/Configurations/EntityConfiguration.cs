using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Wastelands.Core.EfDataAccess.Entities;

namespace Wastelands.EfDataAccess.Configurations
{
	public abstract class EntityConfiguration<TEntity> :
	IEntityTypeConfiguration<TEntity>
	where TEntity : Entity
	{
		public virtual void Configure(EntityTypeBuilder<TEntity> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id)
				.HasColumnName("Id")
				.HasColumnType("bigint")
				.IsRequired()
				.ValueGeneratedOnAdd();
		}
	}
}
