using Witcher.Core.BaseData;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions;
using System;
using System.Linq;
using System.Text;
using Witcher.Core.Entities.Effects;

namespace Witcher.Core.Logic
{
	public static class RunBattle
	{
		static public void FormInitiativeOrder(Battle battle)
		{
			if (!battle.Creatures.Any(x => x.InitiativeInBattle == 0))
				return;

			int count = 1;
			foreach(var creature in battle.Creatures.OrderByDescending(x => CalculateInitiative(x)).ThenByDescending(x => x.Ref))
				creature.InitiativeInBattle = count++;
		}

		static int CalculateInitiative(Creature creature)
		{
			Random random = new();
			
			return creature.Ref + random.Next(1, DiceValue.Value);
		}

		/// <summary>
		/// Обработка начала хода существа
		/// </summary>
		/// <param name="battle">Битва</param>
		/// <param name="currentCreature">Существо</param>
		/// <returns></returns>
		static public void TurnBeginning(Battle battle, out Creature currentCreature)
		{
			if (!battle.Creatures.Any())
				throw new LogicBaseException("Нет существ для осуществления хода");

			StringBuilder message = new();

			if (battle.NextInitiative > battle.Creatures.Count)
			{
				battle.NextInitiative = 1;

				foreach (var creature in battle.Creatures)
					creature.Turn = new();

				message.AppendLine("Начался новый раунд.");
			}

			currentCreature = battle.Creatures.OrderBy(x => x.InitiativeInBattle)
				.First(x => x.InitiativeInBattle >= battle.NextInitiative);

			if (currentCreature.Turn.TurnState == Enums.TurnState.TurnNotBeginned)
			{
				foreach (var effect in currentCreature.Effects)
				{
					effect.Run(currentCreature, ref message);
					effect.AutoEnd(currentCreature, ref message);
				}

				if (RemoveDeadBodies(battle))
				{
					battle.NextInitiative++;
					message.AppendLine("Существо погибло. Ход переходит к следующему существу.");
					TurnBeginning(battle, out currentCreature);
				}
				else
					currentCreature.Turn.TurnState = Enums.TurnState.ReadyForAction;
			}

			battle.BattleLog += message.ToString();
		}

		static public void RemoveNonCharacterCreaturesAndUntieCharactersFromBattle(this Battle battle)
		{
			battle.Creatures?.ForEach(x => x.Battle = null);
		}

		/// <summary>
		/// Удаление мертвых существ
		/// </summary>
		/// <param name="battle"></param>
		internal static bool RemoveDeadBodies(Battle battle)
		{
			return battle.Creatures.RemoveAll(x => x is not Character
			&& (x.HP <= 0 || x.Effects.Any(x => x is DeadEffect))) > 0;
		}
	}
}
