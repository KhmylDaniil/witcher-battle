using FluentValidation;

namespace Witcher.Core.Validators
{
	public class CustomAbstractValidator<T>: AbstractValidator<T>
	{
		protected readonly string MaxPasswordLength = "Превышена длина пароля.";

		protected readonly string FieldCantBeEmpty = "Не заполнено обязательное поле.";

		protected readonly string ValueMustBePositive = "Значение должно быть больше нуля.";
	}
}
