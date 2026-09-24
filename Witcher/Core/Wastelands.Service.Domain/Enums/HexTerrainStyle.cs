namespace Wastelands.Service.Domain.Enums
{
	/// <summary>
	/// Визуальный стиль террейна гекса — только отображение, на правила не влияет. Итоговая картинка
	/// гекса — сочетание <see cref="HexTerrainType"/> и стиля (например, сложный террейн в стиле песка —
	/// барханы, в стиле металла — нагромождение контейнеров). Для <see cref="HexTerrainType.Void"/> стиль
	/// хранится, но не отображается.
	/// </summary>
	public enum HexTerrainStyle
	{
		Grass = 0,
		Sand = 1,
		Water = 2,
		AncientStreet = 3,
		FuturisticMetal = 4,
	}
}
