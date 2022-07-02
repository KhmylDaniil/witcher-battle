using Sindie.ApiService.Core.BaseData;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Тип существа справочник
	/// </summary>
	public class CreatureType: EntityBase
	{
		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected CreatureType()
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
		public CreatureType(
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
		/// Шаблоны существ
		/// </summary>
		public List<CreatureTemplate> CreatureTemplates { get; set; }

		/// <summary>
		/// Существа
		/// </summary>
		public List<Creature> Creatures { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="name">Название</param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static CreatureType CreateForTest(
				Guid? id = default,
				string name = default)
				=> new CreatureType()
				{
					Id = id ?? CreatureTypes.HumanId,
					Name = name ?? CreatureTypes.HumanName,
					CreatureTemplates = new List<CreatureTemplate>(),
					Creatures = new List<Creature>()
				};
	}
}

