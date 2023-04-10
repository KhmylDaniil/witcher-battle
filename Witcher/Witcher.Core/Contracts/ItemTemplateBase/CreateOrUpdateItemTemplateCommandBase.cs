using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Contracts.ItemTemplateBase
{
	public class CreateOrUpdateItemTemplateCommandBase
	{
		/// <summary>
		/// Наазвание
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Цена
		/// </summary>
		public int Price { get; set; }

		/// <summary>
		/// Вес
		/// </summary>
		public int Weight { get; set; }

		public virtual void Validate()
		{
			if (string.IsNullOrEmpty(Name))
				throw new RequestFieldNullException<CreateOrUpdateItemTemplateCommandBase>(nameof(Name));

			if (Price < 1)
				throw new RequestFieldIncorrectDataException<CreateOrUpdateItemTemplateCommandBase>(nameof(Price), "Значение должно быть больше нуля");

			if (Weight < 1)
				throw new RequestFieldIncorrectDataException<CreateOrUpdateItemTemplateCommandBase>(nameof(Weight), "Значение должно быть больше нуля");
		}
	}
}
