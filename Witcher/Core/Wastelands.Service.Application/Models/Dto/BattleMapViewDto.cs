using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Dto
{
	/// <summary>Всё, что нужно окну "Карта боя": сама карта и участники боя с их позициями и аватарками.</summary>
	public class BattleMapViewDto
	{
		public long BattleId { get; set; }

		public string BattleName { get; set; }

		public BattleStatus Status { get; set; }

		public int? CurrentInitiative { get; set; }

		/// <summary>Подключённая карта; null — к бою карта не подключена.</summary>
		public BattleMapDto? Map { get; set; }

		public List<BattleMapParticipantDto> Participants { get; set; } = [];
	}

	public class BattleMapParticipantDto
	{
		public ParticipantKind Kind { get; set; }

		/// <summary>Id существа (Creature.Id) или персонажа (Character.Id) — как в BattleAttack.</summary>
		public long Id { get; set; }

		public string Name { get; set; }

		/// <summary>Аватарка (шаблона существа или персонажа); null — картинки нет, фронт рисует карточку с началом имени.</summary>
		public string? ImageUrl { get; set; }

		public int CurrentHP { get; set; }

		public int MaxHP { get; set; }

		public int? Initiative { get; set; }

		/// <summary>Позиция на карте; null — участник не выставлен (или стоит за пределами карты после её уменьшения).</summary>
		public int? Column { get; set; }

		public int? Row { get; set; }
	}
}
