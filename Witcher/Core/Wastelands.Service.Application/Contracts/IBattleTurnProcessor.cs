using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>
	/// Всё, что происходит в момент, когда чей-то ход начинается: Bleed/Poison/Fire/Sufflocation наносят
	/// урон, Staggered/Blinded спадают (см. BattleTurnProcessor). Всегда используется вместо голого
	/// Battle.AdvanceTurn() — единственное исключение, требующее отдельного метода, — самый первый
	/// активный участник только что начатого боя (MarkStarted сам инициативу не через AdvanceTurn выставляет).
	/// </summary>
	public interface IBattleTurnProcessor
	{
		/// <summary>Обработать начало хода самого первого активного участника — вызывать сразу после Battle.MarkStarted().</summary>
		Task OnBattleStartedAsync(Battle battle);

		/// <summary>Battle.AdvanceTurn() + обработка начала хода нового активного участника (с каскадом, если тот же тик его убивает).</summary>
		Task AdvanceTurnAsync(Battle battle);
	}
}
