using FluentAssertions;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Domain.Drafts;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.UnitTest.Entities
{
	[TestClass]
	public class BattleMapTests
	{
		[TestMethod]
		public void Constructor_FillsWholeGridWithOpenTerrainOfGivenStyle()
		{
			var map = new BattleMap(gameId: 1, "Cave", null, columns: 4, rows: 3, HexTerrainStyle.Sand);

			map.Hexes.Should().HaveCount(12);
			map.Hexes.Select(h => (h.Column, h.Row)).Should().OnlyHaveUniqueItems();
			map.Hexes.Should().OnlyContain(h =>
				h.Column >= 0 && h.Column < 4 && h.Row >= 0 && h.Row < 3 &&
				h.TerrainType == HexTerrainType.Open && h.TerrainStyle == HexTerrainStyle.Sand);
		}

		[TestMethod]
		[DataRow(0, 5)]
		[DataRow(5, 0)]
		[DataRow(BattleMap.MaxDimension + 1, 5)]
		[DataRow(5, BattleMap.MaxDimension + 1)]
		public void Constructor_DimensionsOutOfRange_Throws(int columns, int rows)
		{
			var act = () => new BattleMap(1, "Map", null, columns, rows, HexTerrainStyle.Grass);

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void Resize_Grow_KeepsPaintedHexesAndFillsNewOnesWithFillStyle()
		{
			var map = new BattleMap(1, "Map", null, 2, 2, HexTerrainStyle.Grass);
			map.PaintHexes([new BattleMapHexPaintDraft(1, 1, HexTerrainType.Wall, HexTerrainStyle.AncientStreet)]);

			map.Resize(3, 4, HexTerrainStyle.Water);

			map.Columns.Should().Be(3);
			map.Rows.Should().Be(4);
			map.Hexes.Should().HaveCount(12);
			map.Hexes.Select(h => (h.Column, h.Row)).Should().OnlyHaveUniqueItems();
			map.FindHex(1, 1)!.TerrainType.Should().Be(HexTerrainType.Wall);
			map.FindHex(0, 0)!.TerrainStyle.Should().Be(HexTerrainStyle.Grass);
			map.FindHex(2, 0)!.TerrainStyle.Should().Be(HexTerrainStyle.Water);
			map.FindHex(0, 3)!.TerrainStyle.Should().Be(HexTerrainStyle.Water);
		}

		[TestMethod]
		public void Resize_Shrink_DropsHexesOutsideNewBounds()
		{
			var map = new BattleMap(1, "Map", null, 5, 5, HexTerrainStyle.Grass);

			map.Resize(2, 3, HexTerrainStyle.Grass);

			map.Hexes.Should().HaveCount(6);
			map.Hexes.Should().OnlyContain(h => h.Column < 2 && h.Row < 3);
		}

		[TestMethod]
		public void PaintHexes_ChangesTypeAndStyleOfTargetedHexesOnly()
		{
			var map = new BattleMap(1, "Map", null, 3, 3, HexTerrainStyle.Grass);

			map.PaintHexes(
			[
				new BattleMapHexPaintDraft(0, 0, HexTerrainType.Difficult, HexTerrainStyle.Sand),
				new BattleMapHexPaintDraft(2, 1, HexTerrainType.Void, HexTerrainStyle.Grass),
			]);

			map.FindHex(0, 0)!.TerrainType.Should().Be(HexTerrainType.Difficult);
			map.FindHex(0, 0)!.TerrainStyle.Should().Be(HexTerrainStyle.Sand);
			map.FindHex(2, 1)!.TerrainType.Should().Be(HexTerrainType.Void);
			map.Hexes.Count(h => h.TerrainType == HexTerrainType.Open).Should().Be(7);
		}

		[TestMethod]
		public void PaintHexes_AnyHexOutOfBounds_ThrowsAndChangesNothing()
		{
			var map = new BattleMap(1, "Map", null, 3, 3, HexTerrainStyle.Grass);

			var act = () => map.PaintHexes(
			[
				new BattleMapHexPaintDraft(0, 0, HexTerrainType.Wall, HexTerrainStyle.Grass),
				new BattleMapHexPaintDraft(3, 0, HexTerrainType.Wall, HexTerrainStyle.Grass),
			]);

			act.Should().Throw<InvalidArgumentException>();
			map.FindHex(0, 0)!.TerrainType.Should().Be(HexTerrainType.Open);
		}

		[TestMethod]
		[DataRow(HexTerrainType.Open, true, 1)]
		[DataRow(HexTerrainType.Difficult, true, BattleMapHex.DifficultTerrainMovementCost)]
		[DataRow(HexTerrainType.Impassable, false, null)]
		[DataRow(HexTerrainType.Wall, false, null)]
		[DataRow(HexTerrainType.Void, false, null)]
		public void Hex_MovementRulesFollowTerrainType(HexTerrainType terrainType, bool isPassable, int? movementCost)
		{
			var map = new BattleMap(1, "Map", null, 1, 1, HexTerrainStyle.Grass);
			map.PaintHexes([new BattleMapHexPaintDraft(0, 0, terrainType, HexTerrainStyle.Grass)]);

			var hex = map.FindHex(0, 0)!;

			hex.IsPassable.Should().Be(isPassable);
			hex.MovementCost.Should().Be(movementCost);
		}

		[TestMethod]
		public void SetMarker_StoresTrimmedText_RemoveMarker_ClearsIt()
		{
			var map = new BattleMap(1, "Map", null, 3, 3, HexTerrainStyle.Grass);

			map.SetMarker(1, 2, "  Сундук с ловушкой ");

			map.FindHex(1, 2)!.MarkerText.Should().Be("Сундук с ловушкой");

			map.RemoveMarker(1, 2);

			map.FindHex(1, 2)!.MarkerText.Should().BeNull();
		}

		[TestMethod]
		[DataRow("")]
		[DataRow("   ")]
		public void SetMarker_EmptyText_Throws(string text)
		{
			var map = new BattleMap(1, "Map", null, 3, 3, HexTerrainStyle.Grass);

			var act = () => map.SetMarker(0, 0, text);

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void SetMarker_TooLongOrOutOfBounds_Throws()
		{
			var map = new BattleMap(1, "Map", null, 3, 3, HexTerrainStyle.Grass);

			var tooLong = () => map.SetMarker(0, 0, new string('x', BattleMapHex.MaxMarkerTextLength + 1));
			var outOfBounds = () => map.SetMarker(3, 0, "text");

			tooLong.Should().Throw<InvalidArgumentException>();
			outOfBounds.Should().Throw<InvalidArgumentException>();
		}
	}
}
