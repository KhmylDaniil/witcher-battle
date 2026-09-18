using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Application.Models;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Services
{
	public class BattleCombatContextProvider : IBattleCombatContextProvider
	{
		private readonly ICreatureTemplateRepository _creatureTemplateRepository;
		private readonly ICharacterRepository _characterRepository;

		public BattleCombatContextProvider(ICreatureTemplateRepository creatureTemplateRepository, ICharacterRepository characterRepository)
		{
			_creatureTemplateRepository = creatureTemplateRepository;
			_characterRepository = characterRepository;
		}

		public async Task<ParticipantCombatContext> GetContextAsync(Battle battle, ParticipantKind kind, long participantId)
		{
			if (kind == ParticipantKind.Creature)
			{
				var creature = BattleParticipants.GetCreature(battle, participantId);
				var template = await _creatureTemplateRepository.GetByIdUnscopedAsync(creature.CreatureTemplateId);
				NotFoundException.ThrowIfNull(
					template, ErrorCode.CreatureTemplateNotFound, nameof(CreatureTemplate), nameof(CreatureTemplate.Id), creature.CreatureTemplateId.ToString());

				return new ParticipantCombatContext
				{
					Abilities = template.Abilities,
					GetSkillValue = skill => SkillHelpers.GetCreatureTemplateSkillValue(template, skill),
					Template = template,
					Creature = creature,
				};
			}

			var battleCharacter = BattleParticipants.GetBattleCharacter(battle, participantId);
			var character = await _characterRepository.GetByIdUnscopedAsync(battleCharacter.CharacterId);
			NotFoundException.ThrowIfNull(character, ErrorCode.CharacterNotFound, nameof(Character), nameof(Character.Id), battleCharacter.CharacterId.ToString());

			return new ParticipantCombatContext
			{
				Abilities = character.Abilities,
				GetSkillValue = skill => SkillHelpers.GetCharacterSkillValue(character, skill),
			};
		}
	}
}
