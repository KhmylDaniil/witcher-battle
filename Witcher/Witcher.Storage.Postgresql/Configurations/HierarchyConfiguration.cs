using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.MySql.Configurations
{
	public abstract class HierarchyConfiguration<TEntity> : EntityBaseConfiguration<TEntity>
		where TEntity : EntityBase
	{
		/// <summary>
		/// Пустой метод для айдишника
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureId(EntityTypeBuilder<TEntity> builder)
		{
		}
	}
}
