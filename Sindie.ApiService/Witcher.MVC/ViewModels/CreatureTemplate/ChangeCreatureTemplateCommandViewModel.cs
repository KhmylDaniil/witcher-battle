﻿using Witcher.Core.Contracts.CreatureTemplateRequests;

namespace Witcher.MVC.ViewModels.CreatureTemplate
{
	public class ChangeCreatureTemplateCommandViewModel : ChangeCreatureTemplateCommand
	{
		/// <summary>
		/// Данные шаблонов тела
		/// </summary>
		public Dictionary<Guid, string> BodyTemplatesDictionary { get; set; }

		/// <summary>
		/// Данные способностей
		/// </summary>
		public Dictionary<Guid, string> AbilitiesDictionary { get; set; }
	}
}
