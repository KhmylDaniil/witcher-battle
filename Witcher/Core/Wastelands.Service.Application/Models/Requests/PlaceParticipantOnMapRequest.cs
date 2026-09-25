using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class PlaceParticipantOnMapRequest : BaseRequest
	{
		public long BattleId { get; set; }

		public ParticipantKind Kind { get; set; }

		/// <summary>Id существа (Creature.Id) или персонажа (Character.Id).</summary>
		public long ParticipantId { get; set; }

		public int Column { get; set; }

		public int Row { get; set; }
	}
}
