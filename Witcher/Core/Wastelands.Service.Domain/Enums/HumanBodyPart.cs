namespace Wastelands.Service.Domain.Enums
{
	/// <summary>
	/// Фиксированный набор частей тела персонажа-человека — в отличие от существ, у персонажей нет
	/// редактируемого BodyTemplate (брони достаточно "стандартного человека"), поэтому геометрия
	/// удара захардкожена в HumanBodyPartCatalog и используется одинаково для всех персонажей.
	/// </summary>
	public enum HumanBodyPart
	{
		Head = 0,
		Torso = 1,
		RightArm = 2,
		LeftArm = 3,
		RightLeg = 4,
		LeftLeg = 5,
	}
}
