using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using System;

namespace Sindie.ApiService.Core.Services.Roll
{
	/// <summary>
	/// Серввис случайных бросков
	/// </summary>
	public class RollService : IRollService
	{
		private Random _random { get; set; }

		public RollService()
		{
			_random = new Random();
		}

		/// <summary>
		/// Бросок атаки
		/// </summary>
		/// <param name="attackBase">База атаки</param>
		/// <param name="defenseValue">Значение защиты</param>
		/// <param name="attackerFumble">Провал атаки</param>
		/// <returns>Успешность атаки</returns>
		public int RollAttack(int attackBase, int defenseValue, out int attackerFumble)
		{
			int attackRoll = Roll();

			attackerFumble = CheckFumble(attackRoll);
			attackBase += attackRoll;

			return attackBase - defenseValue < 0 ? 0 : attackBase - defenseValue;
		}

		/// <summary>
		/// Встречный бросок
		/// </summary>
		/// <param name="attackBase">База атаки</param>
		/// <param name="defenseBase">База защиты</param>
		/// <param name="attackerFumble">Провал атаки</param>
		/// <param name="defenderFumble">Провал защиты</param>
		/// <returns>Успешность атаки</returns>
		public int ContestRoll(int attackBase, int defenseBase, out int attackerFumble, out int defenderFumble)
		{
			int attackRoll = Roll();
			int defenseRoll = Roll();

			attackerFumble = CheckFumble(attackRoll);
			defenderFumble = CheckFumble(defenseRoll);

			attackBase += attackRoll;
			defenseBase += defenseRoll;

			return attackBase - defenseBase < 0 ? 0 : attackBase - defenseBase;
		}

		private int Roll()
		{
			var roll = _random.Next(1, DiceValue.Value);

			if (roll == DiceValue.Value)
				CritSuccess(ref roll);
			else if
				(roll == 1)
				CritMiss(ref roll);
			
			return roll;
		}

		private int CritSuccess(ref int input)
		{
			int roll;
			do
			{
				roll = _random.Next(1, 10);
				input += roll;
			}
			while (roll == 10);

			return input;
		}

		private int CritMiss(ref int input)
		{
			input = 0;
			CritSuccess(ref input);

			return -input;
		}

		private int CheckFumble(int input)
		{
			int fumble = 0;
			
			if (input <= -DiceValue.Value)
				fumble = DiceValue.Value;
			else if (input < DiceValue.Value / -2)
				fumble = -input;

			return fumble;
		}
	}
}

