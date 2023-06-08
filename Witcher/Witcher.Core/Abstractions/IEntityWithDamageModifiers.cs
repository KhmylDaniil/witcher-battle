using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Witcher.Core.Entities;

namespace Witcher.Core.Abstractions
{
	public interface IEntityWithDamageModifiers
	{
		public Guid Id { get; }

		public List<EntityDamageTypeModifier> DamageTypeModifiers { get; }
	}
}
