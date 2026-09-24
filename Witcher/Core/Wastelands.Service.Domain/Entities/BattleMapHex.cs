using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	/// <summary>
	/// Один гекс карты боя. Адресуется offset-координатами (Column, Row) внутри своей карты — раскладку
	/// сетки см. в <see cref="BattleMap"/>. Отдельная строка на гекс, а не JSON-блоб на всю карту: к гексу
	/// в будущем будут привязываться туман войны, двери, персонажи/существа и объекты.
	/// </summary>
	public class BattleMapHex : Entity
	{
		/// <summary>Во сколько раз дороже обычного обходится шаг на гекс сложного террейна.</summary>
		public const int DifficultTerrainMovementCost = 2;

		public long BattleMapId { get; private set; }

		public int Column { get; private set; }

		public int Row { get; private set; }

		public HexTerrainType TerrainType { get; private set; }

		public HexTerrainStyle TerrainStyle { get; private set; }

		/// <summary>Можно ли вообще зайти на гекс.</summary>
		public bool IsPassable => TerrainType is HexTerrainType.Open or HexTerrainType.Difficult;

		/// <summary>Стоимость шага на гекс в единицах движения; null — гекс непроходим.</summary>
		public int? MovementCost => TerrainType switch
		{
			HexTerrainType.Open => 1,
			HexTerrainType.Difficult => DifficultTerrainMovementCost,
			_ => null,
		};

		private BattleMapHex()
		{
		}

		// Без battleMapId: гекс создаётся как элемент BattleMap.Hexes, BattleMapId проставит EF Core при
		// SaveChanges по связи навигации (тот же приём, что и BodyTemplatePart).
		internal BattleMapHex(int column, int row, HexTerrainType terrainType, HexTerrainStyle terrainStyle)
		{
			Column = column;
			Row = row;
			TerrainType = terrainType;
			TerrainStyle = terrainStyle;
		}

		internal void Paint(HexTerrainType terrainType, HexTerrainStyle terrainStyle)
		{
			TerrainType = terrainType;
			TerrainStyle = terrainStyle;
		}
	}
}
