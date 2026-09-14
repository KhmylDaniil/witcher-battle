using System;
using System.Collections.Generic;
using System.Linq;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Contracts.RunBattleRequests;

namespace Witcher.MVC.Controllers.Api.Dto
{
	/// <summary>
	/// System.Text.Json (даже с IncludeFields = true) не умеет доставать именованные элементы ValueTuple
	/// (current/max — это компилятор-only метаданные, в рантайме это просто Item1/Item2) — поэтому
	/// для полей, которые реально показываются в UI (HP в списках существ), заводим явные DTO с нормальными именами.
	/// </summary>
	public sealed class MinMaxDto
	{
		public int Current { get; set; }
		public int Max { get; set; }

		public static MinMaxDto From((int current, int max) value) => new() { Current = value.current, Max = value.max };
	}

	public sealed class BattleCreatureDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string CreatureTemplateName { get; set; }
		public string Description { get; set; }
		public MinMaxDto Hp { get; set; }
		public string Effects { get; set; }
		public int Initiative { get; set; }
		public bool IsCharacter { get; set; }

		public static BattleCreatureDto From(GetBattleByIdResponseItem item) => new()
		{
			Id = item.Id,
			Name = item.Name,
			CreatureTemplateName = item.CreatureTemplateName,
			Description = item.Description,
			Hp = MinMaxDto.From(item.HP),
			Effects = item.Effects,
			Initiative = item.Initiative,
			IsCharacter = item.IsCharacter
		};
	}

	public sealed class BattleDetailsDto
	{
		public Guid BattleId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<BattleCreatureDto> Creatures { get; set; }

		public static BattleDetailsDto From(GetBattleByIdResponse response) => new()
		{
			BattleId = response.BattleId,
			Name = response.Name,
			Description = response.Description,
			Creatures = response.Creatures?.Select(BattleCreatureDto.From).ToList() ?? new List<BattleCreatureDto>()
		};
	}

	/// <summary>DTO для RunBattleResponse (RunBattleController.Run) — тот же список существ + текущий ход</summary>
	public sealed class RunBattleDto
	{
		public Guid BattleId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<BattleCreatureDto> Creatures { get; set; }
		public string BattleLog { get; set; }
		public Guid CreatureId { get; set; }
		public string CurrentCreatureName { get; set; }

		public static RunBattleDto From(RunBattleResponse response) => new()
		{
			BattleId = response.BattleId,
			Name = response.Name,
			Description = response.Description,
			Creatures = response.Creatures?.Select(BattleCreatureDto.From).ToList() ?? new List<BattleCreatureDto>(),
			BattleLog = response.BattleLog,
			CreatureId = response.CreatureId,
			CurrentCreatureName = response.CurrentCreatureName
		};
	}
}
