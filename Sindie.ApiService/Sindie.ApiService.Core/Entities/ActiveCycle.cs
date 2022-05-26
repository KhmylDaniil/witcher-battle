using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Цикл активности скрипта модификатора
	/// </summary>
	public class ActiveCycle : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_modifierScript"/>
		/// </summary>
		public const string ModifierScriptField = nameof(_modifierScript);

		private ModifierScript _modifierScript;
		private DateTime _activationEnd;

		/// <summary>
		/// Конструктор для EF
		/// </summary>
		protected ActiveCycle()
		{
		}

		/// <summary>
		/// Конструктор цикла активности скрипта модификатора
		/// </summary>
		/// <param name="activationBeginning">Начало цикла активности</param>
		/// <param name="activationEnd">Конец цикла активности</param>
		public ActiveCycle(
			DateTime activationBeginning,
			DateTime activationEnd)
		{
			ActivationBeginning = activationBeginning;
			ActivationEnd = activationEnd;
		}

		/// <summary>
		/// Начало цикла активности
		/// </summary>
		public DateTime ActivationBeginning { get; set; }

		/// <summary>
		/// Конец цикла активности
		/// </summary>
		public DateTime ActivationEnd
		{
			get => _activationEnd;
			set
			{
				if (value < ActivationBeginning)
					throw new ArgumentOutOfRangeException(nameof(ActivationEnd));
				_activationEnd = value;
			}
		}

		/// <summary>
		/// Айди скрипта модификатора
		/// </summary>
		public Guid ModifierScriptId { get; protected set; }

		#region navigation properties

		/// <summary>
		/// Скрипт модификатора
		/// </summary>
		public ModifierScript ModifierScript
		{
			get => _modifierScript;
			set
			{
				_modifierScript = value ?? throw new ApplicationException("Необходимо передать скрипт модификатора");
				ModifierScriptId = value.Id;
			}
		}

		#endregion navigation properties
	}
}
