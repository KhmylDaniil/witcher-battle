using System.Collections.Generic;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Abstractions;
using System.Linq;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Exceptions.SystemExceptions;
using System;

namespace Witcher.Core.Entities
{
	public class WeaponTemplate : ItemTemplate, IAttackFormula
	{
		private int _attackDiceQuantity;
		private int _durability;
		private int? _range;

		protected WeaponTemplate() { }

		private WeaponTemplate(
			Game game,
			string name,
			string description,
			int weight,
			int price,
			Skill attackSkill,
			DamageType damageType,
			int attackDiceQuantity,
			int damageModifier,
			int accuracy,
			int durability,
			int? range)
			: base (game, name, description, isStackable: false, price, weight)
		{
			AttackSkill = attackSkill;
			DamageType = damageType;
			AttackDiceQuantity = attackDiceQuantity;
			DamageModifier = damageModifier;
			Accuracy = accuracy;
			Durability = durability;
			Range = range;
			AppliedConditions = new();
			DefensiveSkills = new();
		}

		/// <summary>
		/// Навык атаки
		/// </summary>
		public Skill AttackSkill { get; set; }

		/// <summary>
		/// Тип урона
		/// </summary>
		public DamageType DamageType { get; set; }

		/// <summary>
		/// Количество кубов атаки
		/// </summary>
		public int AttackDiceQuantity
		{
			get => _attackDiceQuantity;
			set
			{
				if (value < 0)
					throw new FieldOutOfRangeException<WeaponTemplate>(nameof(AttackDiceQuantity));
				_attackDiceQuantity = value;
			}
		}

		/// <summary>
		/// Модификатор атаки
		/// </summary>
		public int DamageModifier { get; set; }

		/// <summary>
		/// Точность атаки
		/// </summary>
		public int Accuracy { get; set; }

		/// <summary>
		/// Прочность
		/// </summary>
		public int Durability
		{
			get => _durability;
			set
			{
				if (value < 0)
					throw new FieldOutOfRangeException<WeaponTemplate>(nameof(Durability));
				_durability = value;
			}
		}

		/// <summary>
		/// Дальность, если больше базовой
		/// </summary>
		public int? Range
		{
			get => _range;
			set
			{
				if (value is not null && value < 2)
					throw new FieldOutOfRangeException<WeaponTemplate>(nameof(Range));
				_range = value;
			}
		}


		#region navigation properties

		/// <summary>
		/// Накладываемые состояния
		/// </summary>
		public List<AppliedCondition> AppliedConditions { get; set; }

		/// <summary>
		/// Защитные навыки
		/// </summary>
		public List<DefensiveSkill> DefensiveSkills { get; set; }

		#endregion navigation properties

		public static WeaponTemplate CreateWeaponTemplate(
			Game game,
			string name,
			string description,
			int weight,
			int price,
			Skill attackSkill,
			DamageType damageType,
			int attackDiceQuantity,
			int damageModifier,
			int accuracy,
			int durability,
			int? range,
			IEnumerable<UpdateAttackFormulaCommandItemAppledCondition> appliedConditions)
		{
			var newWeaponTemplate = new WeaponTemplate(
				game: game,
				name: name,
				description: description,
				weight: weight,
				price: price,
				attackSkill: attackSkill,
				damageType: damageType,
				attackDiceQuantity: attackDiceQuantity,
				damageModifier: damageModifier,
				accuracy: accuracy,
				durability: durability,
				range: range);

			newWeaponTemplate.UpdateDefensiveSkills(Drafts.AbilityDrafts.DefensiveSkillsDrafts.BaseDefensiveSkills);
			newWeaponTemplate.UpdateAplliedConditions(appliedConditions);

			return newWeaponTemplate;
		}

		public void ChangeWeaponTemplate(
			string name,
			string description,
			int weight,
			int price,
			Skill attackSkill,
			DamageType damageType,
			int attackDiceQuantity,
			int damageModifier,
			int accuracy,
			int durability,
			int? range,
			IEnumerable<UpdateAttackFormulaCommandItemAppledCondition> appliedConditions)
		{
			ChangeItemTemplate(name, description, isStackable: false, price, weight);
			AttackSkill = attackSkill;
			DamageType = damageType;
			AttackDiceQuantity = attackDiceQuantity;
			DamageModifier = damageModifier;
			Accuracy = accuracy;
			Durability = durability;
			Range = range;
			UpdateAplliedConditions(appliedConditions);
		}

		private void UpdateAplliedConditions(IEnumerable<UpdateAttackFormulaCommandItemAppledCondition> data)
		{
			if (data == null)
				return;

			if (AppliedConditions == null)
				throw new ApplicationSystemNullException<WeaponTemplate>(nameof(AppliedConditions));

			var entitiesToDelete = AppliedConditions.Where(x => !data
				.Any(y => y.Id == x.Id)).ToList();

			if (entitiesToDelete.Any())
				foreach (var entity in entitiesToDelete)
					AppliedConditions.Remove(entity);

			if (!data.Any())
				return;

			foreach (var dataItem in data)
			{
				var appliedCondition = AppliedConditions.FirstOrDefault(x => x.Id == dataItem.Id);
				if (appliedCondition == null)
					AppliedConditions.Add(new AppliedCondition(dataItem.Condition, dataItem.ApplyChance));
				else
					appliedCondition.ChangeAppliedCondition(
						condition: dataItem.Condition,
						applyChance: dataItem.ApplyChance);
			}
		}

		private void UpdateDefensiveSkills(List<Skill> data)
			=> DefensiveSkills = data is null
			? throw new ApplicationSystemNullException<WeaponTemplate>(nameof(data))
			: data.Select(x => new DefensiveSkill(x)).ToList();

		[Obsolete("Только для тестов")]
		public static WeaponTemplate CreateForTest(
			Guid? id = default,
			Game game = default,
			string name = default,
			string description = default,
			int price = default,
			int weight = default,
			int attackDiceQuantity = default,
			int damageModifier = default,
			int accuracy = default,
			Skill attackSkill = Skill.Melee,
			DamageType damageType = DamageType.Slashing,
			int durability = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		{
			WeaponTemplate result = new()
			{
				Id = id ?? Guid.NewGuid(),
				Game = game,
				Name = name ?? "name",
				Description = description,
				Price = price,
				Weight = weight,
				IsStackable = false,
				AttackDiceQuantity = attackDiceQuantity,
				DamageModifier = damageModifier,
				Accuracy = accuracy,
				AttackSkill = attackSkill,
				DefensiveSkills = new List<DefensiveSkill>(),
				DamageType = damageType,
				CreatedOn = createdOn,
				ModifiedOn = modifiedOn,
				CreatedByUserId = createdByUserId,
				AppliedConditions = new List<AppliedCondition>()
			};
			result.UpdateDefensiveSkills(Drafts.AbilityDrafts.DefensiveSkillsDrafts.BaseDefensiveSkills);
			return result;
		}
	}
}
