using Witcher.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Witcher.Core.Abstractions
{
	/// <summary>
	/// Интерфейс сервиса изменения списков
	/// </summary>
	public interface IChangeListService
	{
		/// <summary>
		/// Сервис изменения списков текстовых файлов
		/// </summary>
		/// <param name="entity">Основная сущность</param>
		/// <param name="textFiles">Список текстовых файлов</param>
			public void ChangeTextFilesList(IEntityWithFiles entity,
			IEnumerable<TextFile> textFiles);

		/// <summary>
		/// Сервис изменения списков графических файлов
		/// </summary>
		/// <param name="entity">Основная сущность</param>
		/// <param name="imgFiles">Список графических файлов</param>
			public void ChangeImgFilesList(IEntityWithFiles entity,
			IEnumerable<ImgFile> imgFiles);
	}
}
