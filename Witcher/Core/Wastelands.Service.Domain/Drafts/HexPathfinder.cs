using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Domain.Drafts
{
	/// <summary>
	/// Кратчайшие (по стоимости движения) пути по карте боя — используется и для подсветки гексов,
	/// на которые участник может дойти (см. Battle.GetReachableHexes), и для расчёта фактической
	/// стоимости переноса на конкретный гекс при перемещении (см. Battle.MoveParticipant): второе —
	/// частный случай первого, стоимость до цели просто ищется в общей карте достижимости.
	/// <para>
	/// Соседство гексов считается той же формулой offset→cube, что и на фронтенде
	/// (witcher-frontend/src/features/battleMaps/hexGrid.ts, раскладка "pointy-top odd-r") — при смене
	/// раскладки менять оба места.
	/// </para>
	/// </summary>
	public static class HexPathfinder
	{
		public readonly record struct HexPosition(int Column, int Row);

		private static readonly (int Dq, int Dr)[] CubeDirections =
		[
			(1, 0), (1, -1), (0, -1), (-1, 0), (-1, 1), (0, 1),
		];

		private static (int Q, int R) ToCube(int column, int row)
		{
			var q = column - (row - (row & 1)) / 2;
			return (q, row);
		}

		private static HexPosition FromCube(int q, int r) => new(q + (r - (r & 1)) / 2, r);

		private static IEnumerable<HexPosition> Neighbors(HexPosition hex)
		{
			var (q, r) = ToCube(hex.Column, hex.Row);
			foreach (var (dq, dr) in CubeDirections)
			{
				yield return FromCube(q + dq, r + dr);
			}
		}

		/// <summary>
		/// Дейкстра от start в пределах budget очков движения. Гексы из occupied (кроме самого start)
		/// считаются непроходимыми — на них уже стоят другие участники боя; непроходимый по террейну
		/// гекс (BattleMapHex.MovementCost == null) тоже не входит в результат. Возвращает минимальную
		/// стоимость достижения каждого гекса в пределах budget, включая сам start (стоимость 0).
		/// </summary>
		public static Dictionary<HexPosition, int> ComputeReachable(
			BattleMap battleMap, HexPosition start, int budget, IReadOnlySet<HexPosition> occupied)
		{
			var costs = new Dictionary<HexPosition, int> { [start] = 0 };
			if (budget <= 0)
			{
				return costs;
			}

			var queue = new PriorityQueue<HexPosition, int>();
			queue.Enqueue(start, 0);

			while (queue.TryDequeue(out var current, out var currentCost))
			{
				if (currentCost > costs.GetValueOrDefault(current, int.MaxValue))
				{
					continue;
				}

				foreach (var neighbor in Neighbors(current))
				{
					if (neighbor != start && occupied.Contains(neighbor))
					{
						continue;
					}

					var hex = battleMap.FindHex(neighbor.Column, neighbor.Row);
					if (hex?.MovementCost is not { } stepCost)
					{
						continue;
					}

					var newCost = currentCost + stepCost;
					if (newCost > budget)
					{
						continue;
					}

					if (costs.TryGetValue(neighbor, out var existingCost) && existingCost <= newCost)
					{
						continue;
					}

					costs[neighbor] = newCost;
					queue.Enqueue(neighbor, newCost);
				}
			}

			return costs;
		}
	}
}
