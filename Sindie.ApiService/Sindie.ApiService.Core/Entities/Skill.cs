﻿using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System;
using System.Collections.Generic;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Навык справочник
	/// </summary>
	public sealed class Skill : EntityBase
	{
		private Stats _statName;

		/// <summary>
		/// Пустой конструктор для EF
		/// </summary>
		protected Skill()
		{
		}

		/// <summary>
		/// Конструктор навыка
		/// </summary>
		/// <param name="name">Имя</param>
		/// <param name="statName">Название корреспондирующей характеристики</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Айди создавшего пользователя</param>
		/// <param name="modifiedByUserId">Айди изменившего пользователя</param>
		/// <param name="roleCreatedUser">Роль создавшего пользователя</param>
		/// <param name="roleModifiedUser">Роль изменившего пользователя</param>
		public Skill(
			string name,
			Stats stats,
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
			StatName = stats;
			CreatureSkills = new List<CreatureSkill>();
			CreatureTemplateSkills = new List<CreatureTemplateSkill>();
			AbilitiesForAttack = new List<Ability>();
			AbilitiesForDefense = new List<Ability>();

		}
		/// <summary>
		/// Название навыка
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Название корреспондирующей характеристики
		/// </summary>
		public Stats StatName
		{
			get => _statName;
			set => _statName = Enum.IsDefined(typeof(Stats), value) ? value : throw new ExceptionFieldOutOfRange<Skill>(nameof(StatName));
		}

		#region navigation properties
		/// <summary>
		/// Навыки шаблона существа
		/// </summary>
		public List<CreatureTemplateSkill> CreatureTemplateSkills { get; set; }

		/// <summary>
		/// Навыки существа
		/// </summary>
		public List<CreatureSkill> CreatureSkills { get; set; }

		/// <summary>
		/// Способности, в которых используется для атаки
		/// </summary>
		public List<Ability> AbilitiesForAttack { get; set; }

		/// <summary>
		/// Способности, в которых используется для защиты
		/// </summary>
		public List<Ability> AbilitiesForDefense { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="name">Название</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <param name="statName">Название корреспондирующей характеристики</param>
		/// <returns>Навык</returns>
		[Obsolete("Только для тестов")]
		public static Skill CreateForTest(
			Guid? id = default,
			string name = default,
			Stats statName = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new ()
		{
			Id = id ?? Guid.NewGuid(),
			Name = name ?? "Skill",
			StatName = statName == default ? Stats.Ref : statName,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId,
			AbilitiesForAttack = new List<Ability>(),
			AbilitiesForDefense = new List<Ability>(),
			CreatureSkills = new List<CreatureSkill>(),
			CreatureTemplateSkills = new List<CreatureTemplateSkill>()
		};
	}
}