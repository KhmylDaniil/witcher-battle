using Sindie.ApiService.Core.BaseData;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Тип части тела справочник
	/// </summary>
	public class BodyPartType: EntityBase
	{
		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected BodyPartType()
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
		public BodyPartType(
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

		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		#region navigation properties

		/// <summary>
		/// Части тела
		/// </summary>
		public List<BodyPart> BodyParts { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="name">Название</param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static BodyPartType CreateForTest(
				Guid? id = default,
				string name = default)
				=> new BodyPartType()
				{
					Id = id ?? BodyPartTypes.VoidId,
					Name = name ?? BodyPartTypes.VoidName,
					BodyParts = new List<BodyPart>()
				};
	}
}
