using AutoMapper;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.Contracts.Exceptions.WebExceptions;
using Wastelands.Service.Domain.Contracts;
using Wastelands.Service.Domain.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Models.Dto;
using Wastelands.Service.Domain.Models.Filters;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.Infrastructure.Services
{
	public class CharacterService : ICharacterService
	{
		private readonly IMapper _mapper;
		private readonly ICharacterRepository _characterRepository;
		private readonly IUserContext _userContext;

		public CharacterService(ICharacterRepository repository, IMapper mapper, IUserContext userContext)
		{
			_characterRepository = repository;
			_mapper = mapper;
			_userContext = userContext;
		}

		public async Task<CharacterDto> GetCharacterByIdAsync(long id)
		{
			var character = await GetByIdAsync(id);
			return _mapper.Map<CharacterDto>(character);
		}

		public async Task<List<CharacterDto>> GetCharactersAsync(CharacterFilter filter)
		{
			var characters = await _characterRepository.GetListByFilterAsync(filter);

			var dtos = _mapper.Map<List<CharacterDto>>(characters);

			return dtos;
		}

		public async Task<CharacterDto> CreateCharacterAsync(CreateCharacterRequest request)
		{
			if(await _characterRepository.AnyAsync(x => x.Name == request.Name))
			{
				throw new BadRequestException(ErrorCode.InvalidArgument, ExceptionMessages.ValueMustBeUnique);
			}

			var entity = _mapper.Map<Character>(request);
			entity.UserId = _userContext.CurrentUserId;
			await _characterRepository.CreateAsync(entity);

			return _mapper.Map<CharacterDto>(entity);
		}

		public async Task<CharacterDto> UpdateCharacterAsync(UpdateCharacterRequest request)
		{
			if (await _characterRepository.AnyAsync(x => x.Name == request.Name))
			{
				throw new BadRequestException(ErrorCode.InvalidArgument, ExceptionMessages.ValueMustBeUnique);
			}

			var character = await GetByIdAsync(request.Id);
			_mapper.Map(request, character);

			await _characterRepository.UpdateAsync(character);

			return _mapper.Map<CharacterDto>(character);
		}

		public async Task DeleteCharacterAsync(long id)
		{
			var character = await GetByIdAsync(id);
			await _characterRepository.DeleteAsync(character);
		}

		private async Task<Character> GetByIdAsync(long id)
		{
			var character = await _characterRepository.GetByIdAsync(id);

			NotFoundException.ThrowIfNull(
			character,
			ErrorCode.CharacterNotFound,
			nameof(Character),
			nameof(Character.Id),
			id.ToString());

			return character;
		}
	}
}
