using AutoMapper;

namespace Sindie.ApiService.Core.Infrastructure
{
	/// <summary>
	/// Профиль автомаппера
	/// </summary>
	public class MappingProfile : Profile
	{
		/// <summary>
		/// Экземпляры профиля Автомаппера
		/// </summary>
		public MappingProfile()
		{
			//GetGameById();
		}

		/*
		/// <summary>
		/// Карта для команды найти игру по айди
		/// </summary>
		private void GetGameById()
		{
			CreateMap<Game, GetGameByIdQueryResponse>();
			CreateMap<UserGame, GetGameByIdQueryResponseUserGame>()
				.ForMember(ug => ug.Id, opt => opt.MapFrom(s => s.Id))
				.ForMember(ug => ug.UserId, opt => opt.MapFrom(s => s.User.Id))
				.ForMember(ug => ug.Name, opt => opt.MapFrom(s => s.User.Name))
				.ForMember(ug => ug.Email, opt => opt.MapFrom(s => s.User.Email))
				.ForMember(ug => ug.Phone, opt => opt.MapFrom(s => s.User.Phone));
			CreateMap<Parameter, GetGameByIdQueryResponseParameter>();
			CreateMap<Slot, GetGameByIdQueryResponseSlot>();
			CreateMap<Item, GetGameByIdQueryResponseItem>();
			CreateMap<ParameterItem, GetGameByIdQueryResponseParameterItem>()
				.ForMember(i => i.Id, opt => opt.MapFrom(s => s.Id))
				.ForMember(i => i.PrameterId, opt => opt.MapFrom(s => s.Parameter.Id))
				.ForMember(i => i.Name, opt => opt.MapFrom(s => s.Parameter.Name))
				.ForMember(i => i.ItemValue, opt => opt.MapFrom(s => s.ItemValue))
				.ForMember(i => i.Description, opt => opt.MapFrom(s => s.Parameter.Description))
				.ForMember(i => i.MinValueParameter, opt => opt.MapFrom(s => s.Parameter.ParameterBounds.MinValueParameter))
				.ForMember(i => i.MaxValueParameter, opt => opt.MapFrom(s => s.Parameter.ParameterBounds.MaxValueParameter));
			CreateMap<Character, GetGameByIdQueryResponseCharacter>();
			CreateMap<UserCharacter, GetGameByIdQueryResponseUserCharacter>()
				.ForMember(uc => uc.Id, opt => opt.MapFrom(s => s.Id))
				.ForMember(ug => ug.UserId, opt => opt.MapFrom(s => s.User.Id))
				.ForMember(ug => ug.Name, opt => opt.MapFrom(s => s.User.Name))
				.ForMember(ug => ug.Email, opt => opt.MapFrom(s => s.User.Email))
				.ForMember(ug => ug.Phone, opt => opt.MapFrom(s => s.User.Phone));
			CreateMap<CharacterParameter, GetGameByIdQueryResponseCharacterParameter>()
				.ForMember(cp => cp.Id, opt => opt.MapFrom(s => s.Id))
				.ForMember(cp => cp.ParameterId, opt => opt.MapFrom(s => s.Parameter.Id))
				.ForMember(cp => cp.Name, opt => opt.MapFrom(s => s.Parameter.Name))
				.ForMember(cp => cp.Description, opt => opt.MapFrom(s => s.Parameter.Description))
				.ForMember(cp => cp.ParameterValue, opt => opt.MapFrom(s => s.ParameterValue))
				.ForMember(cp => cp.MinValueParameter, opt => opt.MapFrom(s => s.Parameter.ParameterBounds.MinValueParameter))
				.ForMember(cp => cp.MaxValueParameter, opt => opt.MapFrom(s => s.Parameter.ParameterBounds.MaxValueParameter));
			CreateMap<Body, GetGameByIdQueryResponseBody>().ForMember(uc => uc.Id, opt => opt.MapFrom(s => s.Id))
				.ForMember(b => b.Id, opt => opt.MapFrom(s => s.Id))
				.ForMember(b => b.SlotId, opt => opt.MapFrom(s => s.Slot.Id))
				.ForMember(b => b.Name, opt => opt.MapFrom(s => s.Slot.Name))
				.ForMember(b => b.Description, opt => opt.MapFrom(s => s.Slot.Description))
				.ForMember(b => b.MaxQuantityInSlot, opt => opt.MapFrom(s => s.MaxQuantityInSlot));
			CreateMap<BodyItem, GetGameByIdQueryResponseBodyItem>()
				.ForMember(bi => bi.Id, opt => opt.MapFrom(s => s.Id))
				.ForMember(bi => bi.ItemId, opt => opt.MapFrom(s => s.Item.Id))
				.ForMember(bi => bi.Name, opt => opt.MapFrom(s => s.Item.Name))
				.ForMember(bi => bi.Description, opt => opt.MapFrom(s => s.Item.Description));
				//.ForMember(b => b.QuantityItem, opt => opt.MapFrom(s => s.QuantityItem));
			CreateMap<Bag, GetGameByIdQueryResponseBag>();
			CreateMap<BagItem, GetGameByIdQueryResponseBagItem>()
				.ForMember(bi => bi.Id, opt => opt.MapFrom(s => s.Id))
				.ForMember(bi => bi.ItemId, opt => opt.MapFrom(s => s.Item.Id))
				.ForMember(bi => bi.Name, opt => opt.MapFrom(s => s.Item.Name))
				.ForMember(bi => bi.Description, opt => opt.MapFrom(s => s.Item.Description))
				.ForMember(b => b.QuantityItem, opt => opt.MapFrom(s => s.QuantityItem))
				.ForMember(b => b.Stack, opt => opt.MapFrom(s => s.Stack));
		}
		*/
	}
}
