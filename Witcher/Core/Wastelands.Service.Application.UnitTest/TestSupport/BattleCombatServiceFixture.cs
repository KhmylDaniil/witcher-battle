using Moq;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Application.Models;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Services;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.UnitTest.TestSupport
{
	/// <summary>
	/// Мок-обвязка BattleCombatService — сервис зависит только от интерфейсов (см. его конструктор),
	/// поэтому не требует ни EF, ни БД. По умолчанию все зависимости настроены на "счастливый путь"
	/// (авторизация проходит, сохранение/уведомление/маппинг — no-op), тесты переопределяют точечно.
	/// </summary>
	internal sealed class BattleCombatServiceFixture
	{
		public Mock<IBattleRepository> BattleRepository { get; } = new();

		public Mock<ICharacterRepository> CharacterRepository { get; } = new();

		public Mock<IBattleParticipantAuthorizer> Authorizer { get; } = new();

		public Mock<IBattleCombatContextProvider> ContextProvider { get; } = new();

		public Mock<IBattleHitResolver> HitResolver { get; } = new();

		public Mock<IBattleFumbleResolver> FumbleResolver { get; } = new();

		public Mock<IBattleNotifier> Notifier { get; } = new();

		public Mock<IBattleDtoMapper> DtoMapper { get; } = new();

		public Mock<IBattleTurnProcessor> TurnProcessor { get; } = new();

		public BattleCombatServiceFixture()
		{
			Authorizer
				.Setup(a => a.EnsureControllerAsync(It.IsAny<Battle>(), It.IsAny<ParticipantKind>(), It.IsAny<long>(), It.IsAny<Core.Contracts.Enums.ErrorCode>()))
				.Returns(Task.CompletedTask);
			DtoMapper.Setup(m => m.MapAsync(It.IsAny<Battle>())).ReturnsAsync(new BattleDto());
			Notifier.Setup(n => n.NotifyBattleUpdatedAsync(It.IsAny<long>())).Returns(Task.CompletedTask);
			BattleRepository.Setup(r => r.UpdateAsync(It.IsAny<Battle>())).Returns(Task.CompletedTask);
			CharacterRepository.Setup(r => r.UpdateAsync(It.IsAny<Character>())).Returns(Task.CompletedTask);
			TurnProcessor.Setup(t => t.AdvanceTurnAsync(It.IsAny<Battle>())).Returns(Task.CompletedTask);
			TurnProcessor.Setup(t => t.ProcessCurrentTurnAsync(It.IsAny<Battle>())).Returns(Task.CompletedTask);
			TurnProcessor.Setup(t => t.OnBattleStartedAsync(It.IsAny<Battle>())).Returns(Task.CompletedTask);
			HitResolver.Setup(h => h.ResolveIfBothConfirmedAsync(It.IsAny<Battle>(), It.IsAny<BattleAttack>())).Returns(Task.CompletedTask);
			FumbleResolver.Setup(f => f.FinalizeSwingAsync(It.IsAny<Battle>(), It.IsAny<BattleAttack>())).ReturnsAsync(false);
		}

		/// <summary>battle.Id всегда 0 вне EF (см. EntityIdSetter) — репозиторий мокается по battleId явно, а не по battle.Id.</summary>
		public void SetBattle(long battleId, Battle battle)
			=> BattleRepository.Setup(r => r.GetByIdAsync(battleId)).ReturnsAsync(battle);

		public void SetContext(ParticipantKind kind, long participantId, ParticipantCombatContext context)
			=> ContextProvider.Setup(p => p.GetContextAsync(It.IsAny<Battle>(), kind, participantId)).ReturnsAsync(context);

		public BattleCombatService BuildService()
			=> new(
				BattleRepository.Object,
				CharacterRepository.Object,
				Authorizer.Object,
				ContextProvider.Object,
				HitResolver.Object,
				FumbleResolver.Object,
				Notifier.Object,
				DtoMapper.Object,
				TurnProcessor.Object);
	}
}
