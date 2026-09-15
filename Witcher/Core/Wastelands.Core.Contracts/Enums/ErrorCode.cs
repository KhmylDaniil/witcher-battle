namespace Wastelands.Core.Contracts.Enums
{
	public enum ErrorCode
	{
		// auth errors
		LoginNotFound = 1,
		InvalidPassword = 2,
		UserNotAuthorized = 3,

		// general error codes
		PropertyIsLessOrEqualToZero = 100,
		PropertyIsNullOrLessThanZero = 101,

		// character errors
		CharacterNotFound = 200,
		CharacterSkillAlreadyExisted = 201,

		// game errors
		GameNotFound = 300,
		GameJoinRequestNotFound = 301,
		GameJoinRequestAlreadyPending = 302,
		UserAlreadyGameMember = 303,
		UserNotGameMember = 304,
		GameJoinRequestNotPending = 305,

		// body/creature template errors
		BodyTemplateNotFound = 400,
		CreatureTemplateNotFound = 401,
		BodyTemplateBelongsToAnotherGame = 402,
		CreatureTemplatePartNotFound = 403,
		BodyTemplatePartNotFound = 404,
		BodyTemplatePartRangeOverlap = 405,

		// Common error codes 1100...1199
		RequestValidationError = 1100,
		InvalidArgument = 1101,
		UnexpectedError = 1102,
		RequiredParameterCannotBeNull = 1103,
		CurrentUserNotAllowedToPerformThisAction = 1104,
	}
}
