using FluentValidation;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.MVC.Validations
{
	public class AddOrUpdateCharacterSkillRequestValidator : AbstractValidator<AddOrUpdateCharacterSkillRequest>
	{
		public AddOrUpdateCharacterSkillRequestValidator()
		{
			RuleFor(x => x.Skill).IsInEnum();
			RuleFor(x => x.Value).GreaterThan(0).WithMessage(string.Format(ExceptionMessages.ValueMustBePositive, nameof(AddOrUpdateCharacterSkillRequest.Value)));
		}
	}
}
