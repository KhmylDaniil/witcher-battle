using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Entities
{
	public class BleedEffect : Effect
	{
		public int Severity { get; set; }

		protected BleedEffect()
		{
		}

		public BleedEffect(Creature creature, Condition condition, int severity) : base(creature, condition)
		{
			Severity = severity;
		}

		public static BleedEffect Create(Creature creature, Condition condition)
			=> new BleedEffect(creature, condition, 5);
	}
}
