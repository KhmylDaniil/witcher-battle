using System;
using MediatR;
using Witcher.Core.DAL.Entities;

namespace Witcher.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Команда создания боя
	/// </summary>
	public sealed class CreateBattleCommand : IRequest<Battle>
	{
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
