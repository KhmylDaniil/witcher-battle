namespace Wastelands.Core.Contracts.Constants
{
	public static class ExceptionMessages
	{
		public const string MaxFieldLength = "Превышена длина поля {0}.";

		public const string FieldCantBeEmpty = "Не заполнено обязательное поле {0}.";

		public const string ValueMustBePositive = "Значение {0} должно быть больше нуля.";

		public const string ValueCantBeNegative = "Значение {0} не может быть меньше нуля.";

		public const string MinValueCantBeGreaterMaxValue = "Минимальное значение не может быть больше максимального.";

		public const string ValueMustBeBetween = "Значение должно быть в заданных пределах.";

		public const string ValueMustBeUnique = "Value {0} must be unique";

		public const string LoginNotFound = "Пользователь с таким логином не найден.";

		public const string PasswordIsIncorrect = "Пароль неверен.";

		public const string UserNotAuthorized = "User is not authorized";
	}
}
