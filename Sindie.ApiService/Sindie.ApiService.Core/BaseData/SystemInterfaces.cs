using System;

namespace Sindie.ApiService.Core.BaseData
{
	/// <summary>
	/// Интерфейсы справочник
	/// </summary>
	public static class SystemInterfaces
	{
		/// <summary>
		/// Гуид темной темы
		/// </summary>
		public static readonly Guid SystemDarkId = new Guid("8094e0d0-3137-4791-9053-9667cbe107d7");

		/// <summary>
		/// Название темной темы
		/// </summary>
		public static readonly string SystemDarkName = "SystemDarkTheme" ?? default;

		/// <summary>
		/// Гуид темной темы
		/// </summary>
		public static readonly Guid GameDarkId = new Guid("8094e0d0-3137-4791-9053-9667cbe107d6");

		/// <summary>
		/// Название темной темы
		/// </summary>
		public static readonly string GameDarkName = "GameDarkTheme" ?? default;

		/// <summary>
		/// Гуид темной темы
		/// </summary>
		public static readonly Guid CharacterDarkId = new Guid("8094e0d0-3137-4791-9053-9667cbe107d5");

		/// <summary>
		/// Название темной темы
		/// </summary>
		public static readonly string CharacterDarkName = "CharacterDarkTheme" ?? default;

		/// <summary>
		/// Гуид светлой темы
		/// </summary>
		public static readonly Guid SystemLightId = new Guid("8094e0d0-3137-4791-9053-9667cbe107d8");

		/// <summary>
		/// Название светлой темы
		/// </summary>
		public static readonly string SystemLightName = " SystemLightTheme" ?? default;

		/// <summary>
		/// Гуид светлой темы
		/// </summary>
		public static readonly Guid GameLightId = new Guid("8094e0d0-3137-4791-9053-9667cbe107d9");

		/// <summary>
		/// Название светлой темы
		/// </summary>
		public static readonly string GameLightName = "GameLightTheme" ?? default;

		/// <summary>
		/// Гуид светлой темы
		/// </summary>
		public static readonly Guid CharacterLightId = new Guid("8094e0d0-3137-4791-9053-9667cbe107d0");

		/// <summary>
		/// Название светлой темы
		/// </summary>
		public static readonly string CharacterLightName = "CharacterLightTheme" ?? default;
	}
}
