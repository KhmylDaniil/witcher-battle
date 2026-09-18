using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class SetItemTemplateDamageTypeModifierRequest : BaseRequest
	{
		public long ItemTemplateId { get; set; }

		public DamageType DamageType { get; set; }

		public DamageTypeModifier Modifier { get; set; }
	}
}
