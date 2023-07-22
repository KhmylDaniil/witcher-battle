using System;
using MediatR;

namespace Witcher.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Запрос изменения боя
	/// </summary>
	public sealed class ChangeBattleCommand : IRequest
	{
		/// <summary>
		/// Айди боя
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Айди графического файла
		/// </summary>
		public Guid? ImgFileId { get; set; }

		/// <summary>
		/// Название боя
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание боя
		/// </summary>
		public string Description { get; set; }
	}
}
