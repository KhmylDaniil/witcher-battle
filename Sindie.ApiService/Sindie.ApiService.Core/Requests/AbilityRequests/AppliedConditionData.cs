using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using Sindie.ApiService.Core.Requests.AbilityRequests.ChangeAbility;
using Sindie.ApiService.Core.Requests.AbilityRequests.CreateAbility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.AbilityRequests
{
	/// <summary>
	/// Данные для обновления накладываемых состояний
	/// </summary>
	public class AppliedConditionData
	{
		/// <summary>
		/// Айди накладываемого состояния
		/// </summary>
		public Guid? AppliedConditionId { get; set; }

		/// <summary>
		/// Состояние
		/// </summary>
		public Condition Condition { get; set; }

		/// <summary>
		/// Шанс наложения
		/// </summary>
		public int ApplyChance { get; set; }

		/// <summary>
		/// Создание данных для обновления накладываемых состояний
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="conditions">Состояния</param>
		/// <returns>Данные для обновления накладываемых состояний</returns>
		internal static List<AppliedConditionData> CreateAbilityData(CreateAbilityCommand request, List<Condition> conditions)
		{
			if (request?.AppliedConditions == null)
				throw new ExceptionRequestFieldNull<CreateAbilityCommand>(nameof(request.AppliedConditions));
			
			if (conditions == null)
				throw new ExceptionEntityNotFound<Condition>();

			var result = new List<AppliedConditionData>();

			foreach (var requestItem in request.AppliedConditions)
				result.Add(new AppliedConditionData()
				{
					AppliedConditionId = Guid.Empty,
					Condition = conditions.FirstOrDefault(x => x.Id == requestItem.ConditionId),
					ApplyChance = requestItem.ApplyChance
				});

			return result;
		}

		/// <summary>
		/// Создание данных для обновления накладываемых состояний
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="conditions">Состояния</param>
		/// <returns>Данные для обновления накладываемых состояний</returns>
		internal static List<AppliedConditionData> CreateAbilityData(ChangeAbilityCommand request, List<Condition> conditions)
		{
			if (request?.AppliedConditions == null)
				throw new ExceptionRequestFieldNull<CreateAbilityCommand>(nameof(request.AppliedConditions));

			if (conditions == null)
				throw new ExceptionEntityNotFound<Condition>();

			var result = new List<AppliedConditionData>();

			foreach (var requestItem in request.AppliedConditions)
				result.Add(new AppliedConditionData()
				{
					AppliedConditionId = requestItem.Id,
					Condition = conditions.FirstOrDefault(x => x.Id == requestItem.ConditionId),
					ApplyChance = requestItem.ApplyChance
				});

			return result;
		}
	}
}
