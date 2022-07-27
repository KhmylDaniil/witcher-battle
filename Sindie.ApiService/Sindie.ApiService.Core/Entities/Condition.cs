using Sindie.ApiService.Core.Abstractions;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Состояние справочник
	/// </summary>
	public class Condition : EntityBase
	{
		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected Condition()
		{
		}

		/// <summary>
		/// Конструктор только для конфигурации
		/// </summary>
		/// <param name="id">Ид</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Айди создавшего пользователя</param>
		/// <param name="modifiedByUserId">Айди изменившего пользователя</param>
		/// <param name="roleCreatedUser">Роль создавшего пользователя</param>
		/// <param name="roleModifiedUser">Роль изменившего пользователя</param>
		/// <param name="name">Название роли</param>
		public Condition(
			string name,
			Guid id,
			DateTime createdOn,
			DateTime modifiedOn,
			Guid createdByUserId,
			Guid modifiedByUserId,
			string roleCreatedUser = "Default",
			string roleModifiedUser = "Default")
			: base(
				  id,
				  createdOn,
				  modifiedOn,
				  createdByUserId,
				  modifiedByUserId,
				  roleCreatedUser,
				  roleModifiedUser)
		{
			Name = name;
		}

		#region navigation properties

		/// <summary>
		/// Применяемые условия
		/// </summary>
		public List<AppliedCondition> AppliedConditions { get; set; }

		/// <summary>
		/// Эффекты
		/// </summary>
		public List<Effect> Effects { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="name">Название</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns>Состояние</returns>
		[Obsolete("Только для тестов")]
		public static Condition CreateForTest(
			Guid? id = default,
			string name = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new Condition()
		{
			Id = id ?? Guid.NewGuid(),
			Name = name ?? BaseData.Conditions.BleedName,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId,
			Effects = new List<Effect>(),
			AppliedConditions = new List<AppliedCondition>()
		};
	}
}
