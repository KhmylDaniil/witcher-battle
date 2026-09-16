using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class SetDamageTypeModifierRequest : BaseRequest
	{
		public long CreatureTemplateId { get; set; }

		public DamageType DamageType { get; set; }

		public DamageTypeModifier Modifier { get; set; }
	}
}
