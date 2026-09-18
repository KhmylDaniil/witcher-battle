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

		// ability errors
		AbilityNotFound = 500,
		AbilityConditionNotFound = 501,
		AbilityConditionAlreadyExisted = 502,
		AbilityDefensiveSkillNotFound = 503,
		AbilityDefensiveSkillAlreadyExisted = 504,
		CreatureTemplateSkillAlreadyExisted = 505,
		AbilityManagedByEquippedItem = 506,

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

		// battle errors 600...
		BattleNotFound = 600,
		BattleAlreadyStarted = 601,
		BattleHasNoParticipants = 602,
		CreatureNotFoundInBattle = 603,
		BattleCharacterAlreadyExisted = 604,
		BattleCharacterNotFound = 605,
		CreatureTemplateBelongsToAnotherGame = 606,
		CharacterBelongsToAnotherGame = 607,
		NoActiveAttack = 608,
		AttackAlreadyInProgress = 609,
		NotYourTurn = 610,
		CurrentUserNotAttackController = 611,
		CurrentUserNotDefenderController = 612,
		AbilityDoesNotBelongToAttacker = 613,
		InvalidDefensiveSkillChoice = 614,
		AttackerAlreadyConfirmed = 615,
		DefenderAlreadyConfirmed = 616,
		NoAttacksRemaining = 617,
		AttackNotInExpectedPhase = 618,
		BattleNotInProgress = 619,
		InvalidDamageRoll = 620,

		// image errors 700...
		UnsupportedImageType = 700,
		ImageTooLarge = 701,

		// item/inventory errors 800...
		ItemTemplateNotFound = 800,
		ItemNotFound = 801,
		ItemTemplateBelongsToAnotherGame = 802,
		ItemTemplateConditionNotFound = 803,
		ItemTemplateConditionAlreadyExisted = 804,
		ItemTemplateNotWeapon = 805,
		ItemAlreadyEquipped = 806,
		ItemNotEquipped = 807,

		// Common error codes 1100...1199
		InvalidArgument = 1101,
		RequiredParameterCannotBeNull = 1103,
		CurrentUserNotAllowedToPerformThisAction = 1104,
	}
}
