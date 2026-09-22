using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Services
{
	/// <summary>См. IBattleTurnProcessor.</summary>
	public class BattleTurnProcessor : IBattleTurnProcessor
	{
		private readonly IBattleCombatContextProvider _contextProvider;
		private readonly ICharacterRepository _characterRepository;

		public BattleTurnProcessor(IBattleCombatContextProvider contextProvider, ICharacterRepository characterRepository)
		{
			_contextProvider = contextProvider;
			_characterRepository = characterRepository;
		}

		public Task OnBattleStartedAsync(Battle battle)
		{
			var (kind, id) = BattleParticipants.GetActive(battle);
			return ProcessStartOfTurnAsync(battle, kind, id);
		}

		public async Task AdvanceTurnAsync(Battle battle)
		{
			battle.AdvanceTurn();

			// Bleed/Poison/Fire/Sufflocation могут убить существо ровно в момент начала его хода, ещё до
			// того, как оно успеет сходить — тогда его нужно тут же убрать (Battle.RemoveCreature сам
			// передаёт ход дальше) и обработать начало хода уже для следующего участника, и так далее.
			while (true)
			{
				var (kind, id) = BattleParticipants.GetActive(battle);
				await ProcessStartOfTurnAsync(battle, kind, id);

				if (kind == ParticipantKind.Creature)
				{
					var creature = BattleParticipants.GetCreature(battle, id);
					if (creature.CurrentHP <= 0)
					{
						battle.AddLogEntry($"{creature.Name} погибает и выбывает из боя.");
						battle.RemoveCreature(creature);
						continue;
					}
				}

				break;
			}
		}

		private async Task ProcessStartOfTurnAsync(Battle battle, ParticipantKind kind, long id)
		{
			// Ошеломление/Ослепление действуют до начала следующего хода того же участника — снимаются
			// здесь безусловно, до применения периодического урона.
			if (BattleParticipants.HasCondition(battle, kind, id, Condition.Staggered))
			{
				BattleParticipants.RemoveCondition(battle, kind, id, Condition.Staggered);
				battle.AddLogEntry($"{BattleParticipants.GetName(battle, kind, id)}: Ошеломление спало.");
			}

			if (BattleParticipants.HasCondition(battle, kind, id, Condition.Blinded))
			{
				BattleParticipants.RemoveCondition(battle, kind, id, Condition.Blinded);
				battle.AddLogEntry($"{BattleParticipants.GetName(battle, kind, id)}: Ослепление спало.");
			}

			if (BattleParticipants.HasCondition(battle, kind, id, Condition.Bleed))
			{
				ApplyPeriodicDamageWithLog(battle, kind, id, 2, "кровотечения");
			}

			if (BattleParticipants.HasCondition(battle, kind, id, Condition.Poison))
			{
				ApplyPeriodicDamageWithLog(battle, kind, id, 3, "отравления");
			}

			if (BattleParticipants.HasCondition(battle, kind, id, Condition.Sufflocation))
			{
				ApplyPeriodicDamageWithLog(battle, kind, id, 3, "удушья");
			}

			if (BattleParticipants.HasCondition(battle, kind, id, Condition.Fire))
			{
				await ApplyFireDamageAsync(battle, kind, id);
			}
		}

		private static void ApplyPeriodicDamageWithLog(Battle battle, ParticipantKind kind, long id, int amount, string reasonGenitive)
		{
			var name = BattleParticipants.GetName(battle, kind, id);
			BattleParticipants.ApplyPeriodicDamage(battle, kind, id, amount);
			battle.AddLogEntry($"{name} получает {amount} урона от {reasonGenitive}.");
		}

		/// <summary>
		/// 5 урона в каждую часть тела (существо — по шаблону, персонаж — по всем 6 HumanBodyPart), броня
		/// поглощает, но изнашивается на 1 в каждой части всегда — независимо от того, поглотила ли она
		/// урон. Модификатор типа урона (Vulnerability/Resistance/Immunity к Fire) — как в обычном бою:
		/// у существа берётся из шаблона, у персонажа — из закрывающего эту часть предмета брони (если он
		/// есть); никакого модификатора части тела (голова/рука/...) не применяется — это не прицельный удар.
		/// </summary>
		private async Task ApplyFireDamageAsync(Battle battle, ParticipantKind kind, long id)
		{
			var context = await _contextProvider.GetContextAsync(battle, kind, id);
			var name = BattleParticipants.GetName(battle, kind, id);
			int totalDamage;

			if (kind == ParticipantKind.Creature)
			{
				var creature = context.Creature!;
				totalDamage = 0;
				foreach (var part in context.Template!.Parts)
				{
					var currentArmor = Math.Max(0, part.Armor - creature.GetArmorReduction(part.Id));
					var partDamage = Math.Max(0, 5 - currentArmor);
					totalDamage += (int)Math.Round(BattleCombatCalculator.ApplyDamageTypeModifier(partDamage, context.Template.DamageTypeModifiers, DamageType.Fire));
					creature.WearArmor(part.Id);
				}
			}
			else
			{
				var character = context.Character!;
				totalDamage = 0;
				var hasEquippedArmor = false;
				foreach (var humanPart in Enum.GetValues<HumanBodyPart>())
				{
					var armorItem = character.Items.FirstOrDefault(
						i => i.IsEquipped && i.ItemType == ItemType.Armor && i.ArmorParts.Any(p => p.Part == humanPart));
					var currentArmor = armorItem?.ArmorParts.First(p => p.Part == humanPart).CurrentDurability ?? 0;
					var partDamage = Math.Max(0, 5 - currentArmor);

					if (armorItem is not null)
					{
						hasEquippedArmor = true;
						partDamage = (int)Math.Round(BattleCombatCalculator.ApplyDamageTypeModifier(partDamage, armorItem.DamageTypeModifiers, DamageType.Fire));
						armorItem.WearArmor(humanPart);
					}

					totalDamage += partDamage;
				}

				// Item живёт на агрегате Character, а не Battle — износ брони нужно сохранить отдельно
				// (тот же приём, что и в BattleCombatService.ContinueDamageAsync).
				if (hasEquippedArmor)
				{
					await _characterRepository.UpdateAsync(character);
				}
			}

			BattleParticipants.ApplyPeriodicDamage(battle, kind, id, totalDamage);
			battle.AddLogEntry($"{name} получает {totalDamage} урона от огня (по всем частям тела), броня изношена на 1 в каждой части.");
		}
	}
}
