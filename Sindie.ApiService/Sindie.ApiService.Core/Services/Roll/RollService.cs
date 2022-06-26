using Sindie.ApiService.Core.Abstractions;
using System;


namespace Sindie.ApiService.Core.Services.Roll
{
	/// <summary>
	/// Серввис случайных бросков
	/// </summary>
	public class RollService : IRollService
	{
		public Random Random { get; set; }

		public RollService()
		{
			Random = new Random();
		}

		/// <summary>
		/// Бросок атаки
		/// </summary>
		/// <param name="attackValue">База атаки</param>
		/// <param name="defenseValue">База защиты</param>
		/// <returns></returns>
		public int RollAttack(int attackValue, int defenseValue)
		{
			var roll = Random.Next(1, 10);
			if (roll == 10)
			{
				attackValue += roll;
				do
				{
					roll = Random.Next(1, 10);
					attackValue += roll;
				}
				while (roll == 10);
			}
			else if (roll == 1)
			{
				var critMiss = Random.Next(-1, -10);
				if (critMiss < -5)
					return critMiss;
				else
				{
					attackValue += critMiss;
					do
					{
						roll = Random.Next(1, 10);
						attackValue -= roll;
					}
					while (roll == 10);
				}
			}
			else
				attackValue += roll;

			return attackValue - defenseValue < 0 ? 0 : attackValue - defenseValue;
		}
	}
}

