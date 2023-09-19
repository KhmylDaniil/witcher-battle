using FluentValidation;
using Witcher.Core.Contracts.UserGameRequests;

namespace Witcher.Core.Validators.UserGameRequestsValidators
{
	public sealed class CreateUserGameCommandValidator : AbstractValidator<CreateUserGameCommand>
	{
		public CreateUserGameCommandValidator()
		{
			RuleFor(x => x.RoleId).NotEqual(BaseData.GameRoles.MainMasterRoleId).WithMessage("MainMaster role can`t be assigned.");
		}
	}
}
