using FluentValidation;
using Witcher.Core.Contracts.NotificationRequests;

namespace Witcher.Core.Validators.NotificationsRequestsValidators
{
	public sealed class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
	{
		public CreateNotificationCommandValidator()
		{
			RuleFor(x => x.Message).NotEmpty().WithMessage(BaseData.ExceptionMessages.FieldCantBeEmpty);

			RuleFor(x => x.Message).MaximumLength(100).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);
		}
	}
}
