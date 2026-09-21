using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Services
{
	/// <summary>
	/// Процесс атаки внутри уже идущего боя — выбор способности/цели, подтверждения сторон, расчёт
	/// попадания (<see cref="IBattleHitResolver"/>) и урона (математика — в
	/// <see cref="BattleCombatCalculator"/>), продолжение той же способности (multiattack) или
	/// завершение хода. Ростер/старт боя — в <see cref="BattleService"/>.
	/// </summary>
	public class BattleCombatService : IBattleCombatService
	{
		private readonly IBattleRepository _battleRepository;
		private readonly ICharacterRepository _characterRepository;
		private readonly IBattleParticipantAuthorizer _authorizer;
		private readonly IBattleCombatContextProvider _contextProvider;
		private readonly IBattleHitResolver _hitResolver;
		private readonly IBattleNotifier _battleNotifier;
		private readonly IBattleDtoMapper _dtoMapper;

		public BattleCombatService(
			IBattleRepository battleRepository,
			ICharacterRepository characterRepository,
			IBattleParticipantAuthorizer authorizer,
			IBattleCombatContextProvider contextProvider,
			IBattleHitResolver hitResolver,
			IBattleNotifier battleNotifier,
			IBattleDtoMapper dtoMapper)
		{
			_battleRepository = battleRepository;
			_characterRepository = characterRepository;
			_authorizer = authorizer;
			_contextProvider = contextProvider;
			_hitResolver = hitResolver;
			_battleNotifier = battleNotifier;
			_dtoMapper = dtoMapper;
		}

		public async Task<BattleDto> StartAttackAsync(StartAttackRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			BattleParticipants.EnsureInProgress(battle);

			var (attackerKind, attackerId) = BattleParticipants.GetActive(battle);
			await _authorizer.EnsureControllerAsync(battle, attackerKind, attackerId, ErrorCode.CurrentUserNotAttackController);

			BattleParticipants.EnsureExists(battle, request.DefenderKind, request.DefenderId);

			var attackerContext = await _contextProvider.GetContextAsync(battle, attackerKind, attackerId);
			var ability = attackerContext.Abilities.FirstOrDefault(a => a.Id == request.AbilityId);
			if (ability is null)
			{
				throw new InvalidArgumentException(ErrorCode.AbilityDoesNotBelongToAttacker, "У атакующего нет такой способности.");
			}

			var attack = new BattleAttack(
				battle.Id, attackerKind, attackerId, ability.Id, ability.AttacksPerTurn, request.DefenderKind, request.DefenderId);
			battle.StartAttack(attack);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> SetAttackerChoicesAsync(SetAttackerChoicesRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			if (request.TargetedCreaturePartId is { } partId)
			{
				var defenderContext = await _contextProvider.GetContextAsync(battle, attack.DefenderKind, attack.DefenderId);
				BattleParticipants.EnsureTargetedPartValid(defenderContext, attack.DefenderKind, partId);
			}

			if (request.TargetedHumanBodyPart is not null)
			{
				BattleParticipants.EnsureTargetedHumanBodyPartValid(attack.DefenderKind);
			}

			attack.SetTargetPart(request.TargetedCreaturePartId);
			attack.SetTargetHumanBodyPart(request.TargetedHumanBodyPart);
			attack.SetAttackRoll(request.AttackRoll);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> ConfirmAttackerAsync(long battleId)
		{
			var battle = await GetByIdAsync(battleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			attack.ConfirmAttacker();
			await _hitResolver.ResolveIfBothConfirmedAsync(battle, attack);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> SetDefenderChoiceAsync(SetDefenderChoiceRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.DefenderKind, attack.DefenderId, ErrorCode.CurrentUserNotDefenderController);

			var availableSkills = await _hitResolver.GetAvailableDefensiveSkillsAsync(battle, attack);
			if (!availableSkills.Contains(request.DefensiveSkill))
			{
				throw new InvalidArgumentException(ErrorCode.InvalidDefensiveSkillChoice, "Недопустимый защитный навык для этой способности.");
			}

			attack.SetDefenderChoice(request.DefensiveSkill, request.DefenseRoll);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> ConfirmDefenderAsync(long battleId)
		{
			var battle = await GetByIdAsync(battleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.DefenderKind, attack.DefenderId, ErrorCode.CurrentUserNotDefenderController);

			attack.ConfirmDefender();
			await _hitResolver.ResolveIfBothConfirmedAsync(battle, attack);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> SetDamageRollAsync(SetDamageRollRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			if (request.DamageRoll is { } roll)
			{
				var attackerContext = await _contextProvider.GetContextAsync(battle, attack.AttackerKind, attack.AttackerId);
				var ability = attackerContext.Abilities.First(a => a.Id == attack.AbilityId);
				BattleCombatCalculator.ValidateDamageRoll(roll, ability);
			}

			attack.SetDamageRoll(request.DamageRoll);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> ContinueDamageAsync(long battleId)
		{
			var battle = await GetByIdAsync(battleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			if (attack.Phase != BattleAttackPhase.AwaitingDamageRoll)
			{
				throw new InvalidArgumentException(ErrorCode.AttackNotInExpectedPhase, "Сейчас не ожидается расчёт урона.");
			}

			var attackerContext = await _contextProvider.GetContextAsync(battle, attack.AttackerKind, attack.AttackerId);
			var ability = attackerContext.Abilities.First(a => a.Id == attack.AbilityId);
			var defenderContext = await _contextProvider.GetContextAsync(battle, attack.DefenderKind, attack.DefenderId);

			var damage = BattleCombatCalculator.CalculateDamage(attackerContext, defenderContext, attack.DefenderKind, ability, attack);

			// Проверка состояний — только если удар нанёс хоть какой-то урон, независимо от того, кто защищается.
			var appliedConditions = damage.FinalDamage >= 1
				? BattleCombatCalculator.RollAppliedConditions(ability)
				: [];
			BattleParticipants.ApplyDamage(battle, attack, attack.DefenderKind, attack.DefenderId, damage, appliedConditions);

			// Износ конкретного экземпляра брони живёт на Character, а не на Battle — сохраняем отдельно.
			if (damage.WornArmorItemId is { } wornItemId)
			{
				defenderContext.Character!.Items.First(i => i.Id == wornItemId).WearArmor(attack.ResolvedHumanBodyPart!.Value);
				await _characterRepository.UpdateAsync(defenderContext.Character);
			}

			var attackerName = BattleParticipants.GetName(battle, attack.AttackerKind, attack.AttackerId);
			var defenderName = BattleParticipants.GetName(battle, attack.DefenderKind, attack.DefenderId);
			battle.AddLogEntry(BattleCombatLogFormatter.FormatHit(attackerName, ability.Name, defenderName, attack, damage, appliedConditions));

			attack.MarkDamageResolved();

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> NextSwingAsync(NextSwingRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			BattleParticipants.EnsureExists(battle, request.DefenderKind, request.DefenderId);
			attack.PrepareNextSwing(request.DefenderKind, request.DefenderId);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> EndActivationAsync(long battleId)
		{
			var battle = await GetByIdAsync(battleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			if (attack.Phase != BattleAttackPhase.SwingResolved)
			{
				throw new InvalidArgumentException(ErrorCode.AttackNotInExpectedPhase, "Текущий выпад ещё не разрешён.");
			}

			battle.ClearAttack();
			battle.AdvanceTurn();

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> SkipTurnAsync(long battleId)
		{
			var battle = await GetByIdAsync(battleId);
			BattleParticipants.EnsureInProgress(battle);

			if (battle.Attack is not null)
			{
				throw new InvalidArgumentException(ErrorCode.AttackAlreadyInProgress, "Нельзя пропустить ход во время незавершённой атаки.");
			}

			var (activeKind, activeId) = BattleParticipants.GetActive(battle);
			await _authorizer.EnsureControllerAsync(battle, activeKind, activeId, ErrorCode.NotYourTurn);

			battle.AdvanceTurn();

			return await SaveAndNotifyAsync(battle);
		}

		private async Task<Battle> GetByIdAsync(long id)
		{
			var battle = await _battleRepository.GetByIdAsync(id);
			NotFoundException.ThrowIfNull(battle, ErrorCode.BattleNotFound, nameof(Battle), nameof(Battle.Id), id.ToString());

			return battle;
		}

		private async Task<BattleDto> SaveAndNotifyAsync(Battle battle)
		{
			await _battleRepository.UpdateAsync(battle);
			await _battleNotifier.NotifyBattleUpdatedAsync(battle.Id);
			return await _dtoMapper.MapAsync(battle);
		}
	}
}
