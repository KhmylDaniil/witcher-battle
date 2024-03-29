﻿using Witcher.Core.Abstractions;
using System;

namespace Witcher.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Запрос получения битвы по айди
	/// </summary>
	public class GetBattleByIdQuery : IValidatableCommand<GetBattleByIdResponse>
	{
		/// <summary>
		/// Айди битвы
		/// </summary>
		public Guid Id { get; set; }
		
		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
