using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Witcher.Core.Entities;

namespace Witcher.Core.ExtensionMethods
{
	public static class LinqHelper
	{
		public static IQueryable<Game> GetCreaturesAndCharactersFormBattle(this IQueryable<Game> query, Guid battleId)
		{
			return query.Include(g => g.Battles.Where(b => b.Id == battleId))
				.ThenInclude(b => b.Creatures)
					.ThenInclude(c => c.CreatureSkills)
				.Include(g => g.Battles.Where(b => b.Id == battleId))
					.ThenInclude(b => b.Creatures)
						.ThenInclude(c => c.CreatureParts)
				.Include(g => g.Battles.Where(b => b.Id == battleId))
					.ThenInclude(b => b.Creatures)
						.ThenInclude(c => c.Abilities)
				.Include(g => g.Battles.Where(b => b.Id == battleId))
					.ThenInclude(b => b.Creatures)
						.ThenInclude(c => c.DamageTypeModifiers)
				.Include(g => g.Battles.Where(b => b.Id == battleId))
					.ThenInclude(b => b.Creatures)
						.ThenInclude(c => c.Effects)
				.Include(g => g.Characters.Where(ch => ch.BattleId == battleId))
					.ThenInclude(c => c.CreatureSkills)
				.Include(g => g.Characters.Where(ch => ch.BattleId == battleId))
					.ThenInclude(c => c.CreatureParts)
				.Include(g => g.Characters.Where(ch => ch.BattleId == battleId))
					.ThenInclude(c => c.Abilities)
				.Include(g => g.Characters.Where(ch => ch.BattleId == battleId))
					.ThenInclude(c => c.DamageTypeModifiers)
				.Include(g => g.Characters.Where(ch => ch.BattleId == battleId))
					.ThenInclude(c => c.Effects)
				.Include(g => g.Characters.Where(ch => ch.BattleId == battleId))
					.ThenInclude(c => c.EquippedWeapons)
					.ThenInclude(ew => ew.ItemTemplate);
		}
	}
}
