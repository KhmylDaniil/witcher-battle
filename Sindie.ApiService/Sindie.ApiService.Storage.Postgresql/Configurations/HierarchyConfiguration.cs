using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	public abstract class HierarchyConfiguration<TEntity> : EntityBaseConfiguration<TEntity>
		where TEntity : EntityBase
	{
		/// <summary>
		/// Пустой метод для айдишника фиг знает зачем пустой
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureId(EntityTypeBuilder<TEntity> builder)
		{
		}
	}
}
