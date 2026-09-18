using AutoMapper;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Services
{
	public class BattleParticipantSheetService : IBattleParticipantSheetService
	{
		private readonly IBattleRepository _battleRepository;
		private readonly ICreatureTemplateRepository _creatureTemplateRepository;
		private readonly ICharacterRepository _characterRepository;
		private readonly IMapper _mapper;

		public BattleParticipantSheetService(
			IBattleRepository battleRepository,
			ICreatureTemplateRepository creatureTemplateRepository,
			ICharacterRepository characterRepository,
			IMapper mapper)
		{
			_battleRepository = battleRepository;
			_creatureTemplateRepository = creatureTemplateRepository;
			_characterRepository = characterRepository;
			_mapper = mapper;
		}

		public async Task<CreatureTemplateDto> GetCreatureSheetAsync(long battleId, long creatureId)
		{
			var battle = await GetBattleAsync(battleId);
			var creature = BattleParticipants.GetCreature(battle, creatureId);

			var template = await _creatureTemplateRepository.GetByIdUnscopedAsync(creature.CreatureTemplateId);
			NotFoundException.ThrowIfNull(
				template, ErrorCode.CreatureTemplateNotFound, nameof(CreatureTemplate), nameof(CreatureTemplate.Id), creature.CreatureTemplateId.ToString());

			return _mapper.Map<CreatureTemplateDto>(template);
		}

		public async Task<CharacterDto> GetCharacterSheetAsync(long battleId, long characterId)
		{
			var battle = await GetBattleAsync(battleId);
			BattleParticipants.GetBattleCharacter(battle, characterId);

			var character = await _characterRepository.GetByIdUnscopedAsync(characterId);
			NotFoundException.ThrowIfNull(character, ErrorCode.CharacterNotFound, nameof(Character), nameof(Character.Id), characterId.ToString());

			return _mapper.Map<CharacterDto>(character);
		}

		private async Task<Battle> GetBattleAsync(long battleId)
		{
			var battle = await _battleRepository.GetByIdAsync(battleId);
			NotFoundException.ThrowIfNull(battle, ErrorCode.BattleNotFound, nameof(Battle), nameof(Battle.Id), battleId.ToString());

			return battle;
		}
	}
}
