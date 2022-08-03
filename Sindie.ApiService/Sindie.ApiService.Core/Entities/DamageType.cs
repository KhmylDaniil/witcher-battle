using Sindie.ApiService.Core.BaseData;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Тип урона справочник
	/// </summary>
	public class DamageType: EntityBase
	{
		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected DamageType()
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
		public DamageType(
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
		/// Уязвимые шаблоны существ
		/// </summary>
		public List<CreatureTemplate> VulnerableCreatureTemplates { get; set; }

		/// <summary>
		/// Шаблоны существ с сопротивлением
		/// </summary>
		public List<CreatureTemplate> ResistantCreatureTemplates { get; set; }

		/// <summary>
		/// Уязвимые существа
		/// </summary>
		public List<Creature> VulnerableCreatures { get; set; }

		/// <summary>
		/// Существа с сопротивлением
		/// </summary>
		public List<Creature> ResistantCreatures { get; set; }

		/// <summary>
		/// Способности
		/// </summary>
		public List<Ability> Abilities { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="name">Название</param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static DamageType CreateForTest(
				Guid? id = default,
				string name = default)
				=> new ()
				{
					Id = id ?? DamageTypes.SlashingId,
					Name = name ?? DamageTypes.SlashingName,
					Abilities = new List<Ability>(),
					VulnerableCreatures = new List<Creature>(),
					ResistantCreatures = new List<Creature>(),
					VulnerableCreatureTemplates = new List<CreatureTemplate>(),
					ResistantCreatureTemplates = new List<CreatureTemplate>(),
				};
	}
}
