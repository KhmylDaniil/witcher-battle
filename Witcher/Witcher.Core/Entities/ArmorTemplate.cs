using System;
using System.Collections.Generic;
using System.Linq;
using Witcher.Core.Exceptions.EntityExceptions;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities
{
	public class ArmorTemplate : ItemTemplate
	{
		/// <summary>
		/// Поле для <see cref="_bodyTemplate"/>
		/// </summary>
		public const string BodyTemplateField = nameof(_bodyTemplate);

		private int _encumbranceValue;
		private int _armor;

		private BodyTemplate _bodyTemplate;

		protected ArmorTemplate() { }

		private ArmorTemplate(
			Game game,
			string name,
			string description,
			int weight,
			int price,
			BodyTemplate bodyTemplate,
			int armor,
			int encumbranceValue)
			: base (game, name, description, isStackable: false, weight, price)
		{
			BodyTemplate = bodyTemplate;
			Armor = armor;
			EncumbranceValue = encumbranceValue;
			BodyTemplateParts = new();
			ItemType = ItemType.Armor;
			DamageTypeModifiers = new();
		}

		public Guid BodyTemplateId { get; private set; }

		public int Armor
		{
			get => _armor;
			set
			{
				if (value < 0)
					throw new FieldOutOfRangeException<ArmorTemplate>(nameof(Armor));
				_armor = value;
			}
		}

		public int EncumbranceValue
		{
			get => _encumbranceValue;
			set
			{
				if (value< 0)
					throw new FieldOutOfRangeException<ArmorTemplate>(nameof(EncumbranceValue));
				_encumbranceValue = value;
			}
		}	

		public List<EntityDamageTypeModifier> DamageTypeModifiers { get; set; }

		#region navigation properties

		/// <summary>
		/// Шаблон тела
		/// </summary>
		public BodyTemplate BodyTemplate
		{
			get => _bodyTemplate;
			protected set
			{
				_bodyTemplate = value ?? throw new EntityNotIncludedException<BodyTemplatePart>(nameof(BodyTemplate));
				BodyTemplateId = value.Id;
			}
		}

		public List<BodyTemplatePart> BodyTemplateParts { get; set; }

		#endregion navigation properties

		public static ArmorTemplate CreateArmorTemplate(
			Game game,
			string name,
			string description,
			int weight,
			int price,
			BodyTemplate bodyTemplate,
			int armor,
			int encumbranceValue,
			List<BodyPartType> bodyPartTypes)
		{
			var result = new ArmorTemplate(
				game: game,
				name: name,
				description: description,
				weight: weight,
				price: price,
				bodyTemplate: bodyTemplate,
				armor: armor,
				encumbranceValue: encumbranceValue)
			{
				BodyTemplateParts = bodyTemplate.BodyTemplateParts.Where(x => bodyPartTypes.Contains(x.BodyPartType)).ToList()
			};

			return result;
		}
	}
}
