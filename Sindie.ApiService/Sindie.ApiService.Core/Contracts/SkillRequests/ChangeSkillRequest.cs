using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.SkillRequests
{
	/// <summary>
	/// Запрос на изменение навыка
	/// </summary>
	public class ChangeSkillRequest: IRequest
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Название навыка
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание навыка
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Название корреспондиирующей характеристики
		/// </summary>
		public string StatName { get; set; }

		/// <summary>
		/// Минимальное значение навыка
		/// </summary>
		public int MinValueSkill { get; set; }

		/// <summary>
		/// Максимальное значение навыка
		/// </summary>
		public int MaxValueSkill { get; set; }
	}
}
