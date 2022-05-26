using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Пререквизиты скрипта
	/// </summary>
	public class ScriptPrerequisites : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_prerequisite"/>
		/// </summary>
		public const string PrerequisiteField = nameof(_prerequisite);

		/// <summary>
		/// Поле для <see cref="_script"/>
		/// </summary>
		public const string ScriptField = nameof(_script);

		private Prerequisite _prerequisite;
		private Script _script;

		/// <summary>
		/// Конструктор для EF
		/// </summary>
		protected ScriptPrerequisites()
		{
		}

		/// <summary>
		/// Конструктор для пререквизитов скрипта
		/// </summary>
		/// <param name="prerequisite">Пререквизит</param>
		/// <param name="script">Скрипт</param>
		/// <param name="isValid">Валидность</param>
		/// <param name="validationText">Валидационный текст</param>
		public ScriptPrerequisites(
			Prerequisite prerequisite,
			Script script,
			bool isValid,
			string validationText = default)
		{
			Prerequisite = prerequisite;
			Script = script;
			IsValid = isValid;
			ValidationText = validationText;
		}

		/// <summary>
		/// Айдишник скрипта
		/// </summary>
		public Guid ScriptId { get; protected set; }

		/// <summary>
		/// Айдишник пререквизита
		/// </summary>
		public Guid? PrerequisiteId { get; protected set; }

		/// <summary>
		/// Валидность
		/// </summary>
		public bool IsValid { get; set; }

		/// <summary>
		/// Валидационный текст
		/// </summary>
		public string ValidationText { get; set; }

		#region navigation properties

		/// <summary>
		/// Пререквизит
		/// </summary>
		public Prerequisite Prerequisite
		{
			get => _prerequisite;
			protected set
			{
				_prerequisite = value;
				PrerequisiteId = value?.Id;
			}
		}

		/// <summary>
		/// Скрипт
		/// </summary>
		public Script Script
		{
			get => _script;
			protected set
			{
				_script = value ?? throw new ApplicationException("Необходимо передать скрипт");
				ScriptId = value.Id;
			}
		}

		#endregion navigation properties
	}
}
