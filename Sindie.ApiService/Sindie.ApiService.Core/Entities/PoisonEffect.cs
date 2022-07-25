using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Entities
{
	public class PoisonEffect : Effect
	{
		public string Toxicity { get; set; }

		protected PoisonEffect()
		{
		}
		
		public PoisonEffect(Creature creature, Condition condition, string toxicity) : base (creature, condition)
		{
			Toxicity = toxicity;
		}

		public static PoisonEffect Create(Creature creature, Condition condition)
			=> new PoisonEffect(creature, condition, "Severe");
	}
}
