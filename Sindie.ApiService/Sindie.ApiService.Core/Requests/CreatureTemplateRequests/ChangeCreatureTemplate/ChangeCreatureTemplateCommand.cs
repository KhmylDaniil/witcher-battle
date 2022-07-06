using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.ChangeCreatureTemplate;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;

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
		/// <param name="creatureTypeId">Айди типа существа</param>
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
		/// <param name="creatureTemplateParameters">Параметры шаблона существа</param>
		public ChangeCreatureTemplateCommand(
			Guid id,
			Guid gameId,
			Guid? imgFileId,
			Guid bodyTemplateId,
			Guid creatureTypeId,
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
			List<ChangeCreatureTemplateRequestParameter> creatureTemplateParameters)
		{
			Id = id;
			GameId = gameId;
			ImgFileId = imgFileId;
			BodyTemplateId = bodyTemplateId;
			CreatureTypeId = creatureTypeId;
			Name = string.IsNullOrWhiteSpace(name)
				? throw new ExceptionRequestFieldNull<ChangeCreatureTemplateRequest>(nameof(Name))
				: name;
			Description = description;
			HP = hp < 1 ? throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateRequest>(nameof(HP)) : hp;
			Sta = sta < 1 ? throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateRequest>(nameof(Sta)) : sta;
			Int = @int < 1 ? throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateRequest>(nameof(Int)) : @int;
			Ref = @ref < 1 ? throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateRequest>(nameof(Ref)) : @ref;
			Dex = dex < 1 ? throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateRequest>(nameof(Dex)) : dex;
			Body = body < 1 ? throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateRequest>(nameof(Body)) : body;
			Emp = emp < 1 ? throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateRequest>(nameof(Emp)) : emp;
			Cra = cra < 1 ? throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateRequest>(nameof(Cra)) : cra;
			Will = will < 1 ? throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateRequest>(nameof(Will)) : will;
			Speed = speed < 1 ? throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateRequest>(nameof(Speed)) : speed;
			Luck = luck < 1 ? throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateRequest>(nameof(Luck)) : luck;
			ArmorList = armorList == null 
				? throw new ExceptionRequestFieldNull<ChangeCreatureTemplateRequest>(nameof(ArmorList))
				: armorList;
			Abilities = abilities == null
				? throw new ExceptionRequestFieldNull<ChangeCreatureTemplateRequest>(nameof(Abilities))
				: abilities;
			CreatureTemplateParameters = creatureTemplateParameters == null
				? throw new ExceptionRequestFieldNull<ChangeCreatureTemplateRequest>(nameof(CreatureTemplateParameters))
				: creatureTemplateParameters;
		}
	}
}
