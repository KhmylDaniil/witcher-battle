using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.ChangeCreatureTemplate;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Requests.CreatureTemplateRequests.ChangeCreatureTemplate
{
	/// <summary>
	/// Команда изменения шаблона существа
	/// </summary>
	public class ChangeCreatureTemplateCommand: ChangeCreatureTemplateRequest
	{
		/// <summary>
		/// Конструктор для <see cref="ChangeCreatureTemplateCommand"/>
		/// </summary>
		/// <param name="id">Айди</param>
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
		public ChangeCreatureTemplateCommand(
			Guid id,
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
			List<ChangeCreatureTemplateRequestArmorList> armorList,
			List<Guid> abilities,
			List<ChangeCreatureTemplateRequestSkill> creatureTemplateSkills)
		{
			Id = id;
			GameId = gameId;
			ImgFileId = imgFileId;
			BodyTemplateId = bodyTemplateId;
			CreatureType = Enum.IsDefined(creatureType)
				? creatureType
				: throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateRequest>(nameof(creatureType));
			Name = string.IsNullOrWhiteSpace(name)
				? throw new RequestFieldNullException<ChangeCreatureTemplateRequest>(nameof(Name))
				: name;
			Description = description;
			HP = hp < 1 ? throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateRequest>(nameof(HP)) : hp;
			Sta = sta < 1 ? throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateRequest>(nameof(Sta)) : sta;
			Int = @int < 1 ? throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateRequest>(nameof(Int)) : @int;
			Ref = @ref < 1 ? throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateRequest>(nameof(Ref)) : @ref;
			Dex = dex < 1 ? throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateRequest>(nameof(Dex)) : dex;
			Body = body < 1 ? throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateRequest>(nameof(Body)) : body;
			Emp = emp < 1 ? throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateRequest>(nameof(Emp)) : emp;
			Cra = cra < 1 ? throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateRequest>(nameof(Cra)) : cra;
			Will = will < 1 ? throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateRequest>(nameof(Will)) : will;
			Speed = speed < 1 ? throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateRequest>(nameof(Speed)) : speed;
			Luck = luck < 1 ? throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateRequest>(nameof(Luck)) : luck;
			ArmorList = armorList ?? throw new RequestFieldNullException<ChangeCreatureTemplateRequest>(nameof(ArmorList));
			Abilities = abilities ?? throw new RequestFieldNullException<ChangeCreatureTemplateRequest>(nameof(Abilities));
			CreatureTemplateSkills = creatureTemplateSkills ?? throw new RequestFieldNullException<ChangeCreatureTemplateRequest>(nameof(CreatureTemplateSkills));
		}
	}
}
