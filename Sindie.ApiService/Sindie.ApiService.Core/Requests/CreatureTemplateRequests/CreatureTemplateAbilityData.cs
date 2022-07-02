using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.ChangeCreatureTemplate;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.CreateCreatureTemplate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sindie.ApiService.Core.Requests.CreatureTemplateRequests
{
	/// <summary>
	/// Данные для обновления способности
	/// </summary>
	public class AbilityData
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid? Id { get; set; }

		/// <summary>
		/// Наазвание способности
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание способности
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Параметр атаки
		/// </summary>
		public Parameter AttackParameter { get; set; }

		/// <summary>
		/// Количество кубов атаки
		/// </summary>
		public int AttackDiceQuantity { get; set; }

		/// <summary>
		/// Модификатор атаки
		/// </summary>
		public int DamageModifier { get; set; }

		/// <summary>
		/// Скорость атаки
		/// </summary>
		public int AttackSpeed { get; set; }

		/// <summary>
		/// Точность атаки
		/// </summary>
		public int Accuracy { get; set; }

		/// <summary>
		/// Применяемые состояния
		/// </summary>
		public List<(Guid? Id, Condition Condition, int ApplyChance)>  AppliedConditions { get; set; } 

		/// <summary>
		/// Создание данных для способности
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		/// <returns>Данные для способности</returns>
		internal static List<AbilityData> CreateAbilityData(CreateCreatureTemplateCommand request, Game game)
		{
			if (request?.Abilities == null)
				throw new ExceptionEntityNotIncluded<Ability>(nameof(Game));
			if (game?.Parameters == null)
				throw new ExceptionEntityNotIncluded<Parameter>(nameof(Game));
			if (game?.Conditions == null)
				throw new ExceptionEntityNotIncluded<Condition>(nameof(Game));

			var result = new List<AbilityData>();

			foreach (var requestItem in request.Abilities)
			{
				var resultItem = new AbilityData()
				{
					Id = null,
					Name = requestItem.Name,
					Description = requestItem.Description,
					AttackParameter = game.Parameters.FirstOrDefault(x => x.Id == requestItem.AttackParameterId),
					AttackDiceQuantity = requestItem.AttackDiceQuantity,
					DamageModifier = requestItem.DamageModifier,
					AttackSpeed = requestItem.AttackSpeed,
					Accuracy = requestItem.Accuracy,
					AppliedConditions = new List<(Guid? Id, Condition Condition, int ApplyChance)>()
				};
				foreach (var appliedCondition in requestItem.AppliedConditions)
					resultItem.AppliedConditions.Add((
						Guid.Empty,
						game.Conditions.FirstOrDefault(x => x.Id == appliedCondition.ConditionId),
						appliedCondition.ApplyChance));
				result.Add(resultItem);
			}
			return result;
		}

		/// <summary>
		/// Создание данных для способности
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		/// <returns>Данные для способности</returns>
		internal static List<AbilityData> CreateAbilityData(ChangeCreatureTemplateCommand request, Game game)
		{
			if (request?.Abilities == null)
				throw new ExceptionEntityNotIncluded<Ability>(nameof(Game));
			if (game?.Parameters == null)
				throw new ExceptionEntityNotIncluded<Parameter>(nameof(Game));
			if (game?.Conditions == null)
				throw new ExceptionEntityNotIncluded<Condition>(nameof(Game));

			var result = new List<AbilityData>();

			foreach (var requestItem in request.Abilities)
			{
				var resultItem = new AbilityData()
				{
					Id = requestItem.Id,
					Name = requestItem.Name,
					Description = requestItem.Description,
					AttackParameter = game.Parameters.FirstOrDefault(x => x.Id == requestItem.AttackParameterId),
					AttackDiceQuantity = requestItem.AttackDiceQuantity,
					DamageModifier = requestItem.DamageModifier,
					AttackSpeed = requestItem.AttackSpeed,
					Accuracy = requestItem.Accuracy,
					AppliedConditions = new List<(Guid? Id, Condition Condition, int ApplyChance)>()
				};
				foreach (var appliedCondition in requestItem.AppliedConditions)
					resultItem.AppliedConditions.Add((
						appliedCondition.Id,
						game.Conditions.FirstOrDefault(x => x.Id == appliedCondition.ConditionId),
						appliedCondition.ApplyChance));
				result.Add(resultItem);
			}
			return result;
		}

	}
}
