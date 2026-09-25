using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Drafts;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	/// <summary>
	/// Карта боя из шестигранников, которую мастер игры рисует заранее, чтобы потом использовать в бою.
	/// Сетка всегда прямоугольная и заполнена целиком: на каждую пару (Column, Row) в пределах
	/// Columns × Rows ровно один <see cref="BattleMapHex"/>.
	/// <para>
	/// Раскладка — "pointy-top odd-r": гексы стоят вершиной вверх, нечётные ряды (Row = 1, 3, ...)
	/// сдвинуты вправо на половину гекса. Фронтенд рисует и считает соседей/расстояния по той же схеме
	/// (witcher-frontend/src/features/battleMaps/hexGrid.ts) — при смене раскладки менять оба места.
	/// </para>
	/// </summary>
	public class BattleMap : Entity
	{
		public const int MinDimension = 1;
		public const int MaxDimension = 60;

		public long GameId { get; private set; }

		public string Name { get; private set; }

		public string? Description { get; private set; }

		public int Columns { get; private set; }

		public int Rows { get; private set; }

		public List<BattleMapHex> Hexes { get; private set; } = [];

		private BattleMap()
		{
		}

		/// <summary>Создаёт карту, целиком заполненную простым террейном стиля <paramref name="terrainStyle"/>.</summary>
		public BattleMap(long gameId, string name, string? description, int columns, int rows, HexTerrainStyle terrainStyle)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(gameId, nameof(gameId));
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
			ValidateDimensions(columns, rows);

			GameId = gameId;
			Name = name;
			Description = description;
			Columns = columns;
			Rows = rows;

			for (var row = 0; row < rows; row++)
			{
				for (var column = 0; column < columns; column++)
				{
					Hexes.Add(new BattleMapHex(column, row, HexTerrainType.Open, terrainStyle));
				}
			}
		}

		public void UpdateBattleMap(string name, string? description)
		{
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

			Name = name;
			Description = description;
		}

		/// <summary>
		/// Меняет размер карты, сохраняя уже нарисованное: гексы за новыми границами удаляются,
		/// появившиеся — заполняются простым террейном стиля <paramref name="fillStyle"/>.
		/// </summary>
		public void Resize(int columns, int rows, HexTerrainStyle fillStyle)
		{
			ValidateDimensions(columns, rows);

			Hexes.RemoveAll(h => h.Column >= columns || h.Row >= rows);

			for (var row = 0; row < rows; row++)
			{
				for (var column = 0; column < columns; column++)
				{
					if (row >= Rows || column >= Columns)
					{
						Hexes.Add(new BattleMapHex(column, row, HexTerrainType.Open, fillStyle));
					}
				}
			}

			Columns = columns;
			Rows = rows;
		}

		/// <summary>
		/// Перекрашивает набор гексов разом (одна "кисть" в редакторе — один вызов). Если хотя бы одна
		/// координата вне карты, не меняется ничего.
		/// </summary>
		public void PaintHexes(IReadOnlyCollection<BattleMapHexPaintDraft> paints)
		{
			var hexesByPosition = Hexes.ToDictionary(h => (h.Column, h.Row));
			var targets = new List<(BattleMapHex Hex, BattleMapHexPaintDraft Paint)>(paints.Count);

			foreach (var paint in paints)
			{
				var hex = hexesByPosition.GetValueOrDefault((paint.Column, paint.Row)) ?? GetHex(paint.Column, paint.Row);
				targets.Add((hex, paint));
			}

			foreach (var (hex, paint) in targets)
			{
				hex.Paint(paint.TerrainType, paint.TerrainStyle);
			}
		}

		public BattleMapHex? FindHex(int column, int row)
			=> Hexes.FirstOrDefault(h => h.Column == column && h.Row == row);

		/// <summary>Гекс по координатам; если координаты вне карты — исключение.</summary>
		public BattleMapHex GetHex(int column, int row)
		{
			return FindHex(column, row) ?? throw new InvalidArgumentException(
				ErrorCode.BattleMapHexOutOfBounds,
				$"Гекс ({column}, {row}) находится за пределами карты {Columns}×{Rows}.");
		}

		/// <summary>Ставит на гекс маркер с текстом (или меняет текст/видимость уже стоящего).</summary>
		public void SetMarker(int column, int row, string text, bool visibleToPlayers = false)
		{
			GetHex(column, row).SetMarker(text, visibleToPlayers);
		}

		public void RemoveMarker(int column, int row)
		{
			GetHex(column, row).RemoveMarker();
		}

		private static void ValidateDimensions(int columns, int rows)
		{
			InvalidArgumentException.ThrowIfNotInRange(columns, MinDimension, MaxDimension, nameof(columns));
			InvalidArgumentException.ThrowIfNotInRange(rows, MinDimension, MaxDimension, nameof(rows));
		}
	}
}
