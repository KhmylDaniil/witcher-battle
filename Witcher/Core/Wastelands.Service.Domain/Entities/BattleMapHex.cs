using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	/// <summary>
	/// Один гекс карты боя. Адресуется offset-координатами (Column, Row) внутри своей карты — раскладку
	/// сетки см. в <see cref="BattleMap"/>. Отдельная строка на гекс, а не JSON-блоб на всю карту: к гексу
	/// в будущем будут привязываться туман войны и двери.
	/// <para>
	/// На гексе может лежать не больше одного объекта. Сейчас единственный вид объекта — текстовый
	/// маркер мастера (<see cref="MarkerText"/>); персонажи/существа хранят свою позицию сами (см.
	/// <see cref="Battle.PlaceParticipantOnMap"/>) — они не объекты карты, а участники конкретного боя.
	/// </para>
	/// </summary>
	public class BattleMapHex : Entity
	{
		/// <summary>Во сколько раз дороже обычного обходится шаг на гекс сложного террейна.</summary>
		public const int DifficultTerrainMovementCost = 2;

		public const int MaxMarkerTextLength = 500;

		public long BattleMapId { get; private set; }

		public int Column { get; private set; }

		public int Row { get; private set; }

		public HexTerrainType TerrainType { get; private set; }

		public HexTerrainStyle TerrainStyle { get; private set; }

		/// <summary>Текст маркера, который мастер поставил на гекс (показывается всплывающей подсказкой). Null — маркера нет.</summary>
		public string? MarkerText { get; private set; }

		/// <summary>
		/// Видят ли маркер игроки на карте идущего боя. По умолчанию нет: маркер — заметка мастера
		/// (ловушки, засады), открывать её игрокам мастер решает явно.
		/// </summary>
		public bool MarkerVisibleToPlayers { get; private set; }

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

		internal void SetMarker(string text, bool visibleToPlayers)
		{
			InvalidArgumentException.ThrowIfNullOrEmpty(text?.Trim(), nameof(text));
			if (text!.Length > MaxMarkerTextLength)
			{
				throw new InvalidArgumentException(ErrorCode.InvalidArgument, $"Текст маркера длиннее {MaxMarkerTextLength} символов.");
			}

			MarkerText = text.Trim();
			MarkerVisibleToPlayers = visibleToPlayers;
		}

		internal void RemoveMarker()
		{
			MarkerText = null;
			MarkerVisibleToPlayers = false;
		}
	}
}
