using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.CreateCreatureTemplate;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Requests.CreatureTemplateRequests.CreateCreatureTemplate
{
	/// <summary>
	/// Команда создания шаблона существа
	/// </summary>
	public class CreateCreatureTemplateCommand : CreateCreatureTemplateRequest
	{
		/// <summary>
		/// Конструктор для <see cref="CreateCreatureTemplateCommand"/>
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		/// <param name="imgFileId">Айди графического файла</param>
		/// <param name="bodyTemplateId">Айди шаблона тела</param>
		/// <param name="creatureType">Тип существа</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="hp">Хиты</param>
		/// <param name="sta">Стамина</param>
		/// <param name="int">Интеллект</param>
		/// <param name="ref">Рефлексы</param>
		/// <param name="dex">Ловкость</param>
		/// <param name="body">Телосложение</param>
		/// <param name="emp">Эмпатия</param>
		/// <param name="cra">Крафт</param>
		/// <param name="will">Воля</param>
		/// <param name="speed">Скорость</param>
		/// <param name="luck">Удача</param>
		/// <param name="armorList">Броня</param>
		/// <param name="abilities">Способности</param>
		/// <param name="creatureTemplateSkills">Навыки шаблона существа</param>
		public CreateCreatureTemplateCommand(
			Guid gameId,
			Guid? imgFileId,
			Guid bodyTemplateId,
			CreatureType creatureType,
			string name,
			string description,
			int hp,
			int sta,
			int @int,
			int @ref,
			int dex,
			int body,
			int emp,
			int cra,
			int will,
			int speed,
			int luck,
			List<CreateCreatureTemplateRequestArmorList> armorList,
			List<Guid> abilities,
			List<CreateCreatureTemplateRequestSkill> creatureTemplateSkills)
		{
			GameId = gameId;
			ImgFileId = imgFileId;
			BodyTemplateId = bodyTemplateId;
			CreatureType = Enum.IsDefined(creatureType)
				? creatureType
				: throw new RequestFieldIncorrectDataException<CreateCreatureTemplateRequest>(nameof(creatureType));
			Name = string.IsNullOrWhiteSpace(name)
				? throw new RequestFieldNullException<CreateCreatureTemplateRequest>(nameof(Name))
				: name;
			Description = description;
			HP = hp < 1 ? throw new RequestFieldIncorrectDataException<CreateCreatureTemplateRequest>(nameof(HP)) : hp;
			Sta = sta < 1 ? throw new RequestFieldIncorrectDataException<CreateCreatureTemplateRequest>(nameof(Sta)) : sta;
			Int = @int < 1 ? throw new RequestFieldIncorrectDataException<CreateCreatureTemplateRequest>(nameof(Int)) : @int;
			Ref = @ref < 1 ? throw new RequestFieldIncorrectDataException<CreateCreatureTemplateRequest>(nameof(Ref)) : @ref;
			Dex = dex < 1 ? throw new RequestFieldIncorrectDataException<CreateCreatureTemplateRequest>(nameof(Dex)) : dex;
			Body = body < 1 ? throw new RequestFieldIncorrectDataException<CreateCreatureTemplateRequest>(nameof(Body)) : body;
			Emp = emp < 1 ? throw new RequestFieldIncorrectDataException<CreateCreatureTemplateRequest>(nameof(Emp)) : emp;
			Cra = cra < 1 ? throw new RequestFieldIncorrectDataException<CreateCreatureTemplateRequest>(nameof(Cra)) : cra;
			Will = will < 1 ? throw new RequestFieldIncorrectDataException<CreateCreatureTemplateRequest>(nameof(Will)) : will;
			Speed = speed < 1 ? throw new RequestFieldIncorrectDataException<CreateCreatureTemplateRequest>(nameof(Speed)) : speed;
			Luck = luck < 1 ? throw new RequestFieldIncorrectDataException<CreateCreatureTemplateRequest>(nameof(Luck)) : luck;
			ArmorList = armorList ?? throw new RequestFieldNullException<CreateCreatureTemplateRequest>(nameof(ArmorList));
			Abilities = abilities ?? throw new RequestFieldNullException<CreateCreatureTemplateRequest>(nameof(Abilities));
			CreatureTemplateSkills = creatureTemplateSkills ?? throw new RequestFieldNullException<CreateCreatureTemplateRequest>(nameof(CreatureTemplateSkills));
		}
	}
}
