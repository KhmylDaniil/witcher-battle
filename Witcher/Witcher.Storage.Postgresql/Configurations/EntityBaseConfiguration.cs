﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для базовых сущностей <see cref="TEntity"/>
	/// </summary>
	/// <typeparam name="TEntity">Базовые сущности</typeparam>
	public abstract class EntityBaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
		where TEntity : EntityBase
	{
		protected const string NowCommand = "now() at time zone 'utc'";

		/// <summary>
		/// Конфигурация для базовых сущностей
		/// </summary>
		/// <param name="builder">Строитель</param>
		public virtual void Configure(EntityTypeBuilder<TEntity> builder)
		{
			ConfigureId(builder);
			ConfigureBase(builder);
			ConfigureChild(builder);
		}

		/// <summary>
		/// Конфигурация сущности для наследников EntityBase
		/// </summary>
		/// <param name="builder">Строитель</param>
		public virtual void ConfigureBase(EntityTypeBuilder<TEntity> builder)
		{
			builder.Property(r => r.CreatedOn)
				.HasDefaultValueSql(NowCommand);
			builder.Property(r => r.ModifiedOn);
			builder.Property(r => r.CreatedByUserId);
			builder.Property(r => r.ModifiedByUserId);
		}

		/// <summary>
		/// Конфигурация сущности для наследников Prerequisite
		/// </summary>
		/// <param name="builder"></param>
		public abstract void ConfigureChild(EntityTypeBuilder<TEntity> builder);

		/// <summary>
		/// Коняигурация для айдишника
		/// </summary>
		/// <param name="builder">Строитель</param>
		public virtual void ConfigureId(EntityTypeBuilder<TEntity> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(r => r.Id)
				.HasDefaultValueSql("uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");
		}
	}
}
