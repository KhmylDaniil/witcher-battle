using Witcher.Core.Abstractions;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Witcher.Core.Exceptions.SystemExceptions;
using Witcher.Core.Services.Authorization;

namespace Witcher.Core.Services.ChangeListService
{
	/// <summary>
	/// Сервис изменения списков графических и текстовых файлов
	/// </summary>
	public class ChangeListService: IChangeListService
	{
		/// <summary>
		/// Сервис изменения списков текстовых файлов
		/// </summary>
		/// <param name="entity">Основная сущность</param>
		/// <param name="textFiles">Список текстовых файлов</param>
		public void ChangeTextFilesList(IEntityWithFiles entity,
			IEnumerable<TextFile> textFiles)
		{
			if (entity.TextFiles is null)
				throw new ApplicationSystemNullException(nameof(ChangeListService), nameof(entity.TextFiles));

			if (textFiles != null || entity.TextFiles.Any())
			{
				var entitiesToDelete = entity.TextFiles.Where(x => !textFiles.
					Any(y => y.Id == x.Id)).ToList();
				var entitiesToAdd = textFiles.Where(x => !entity.TextFiles.
					Any(y => y.Id == x.Id)).ToList();

				if (entitiesToAdd != null)
					entity.TextFiles.AddRange(entitiesToAdd);

				if (entitiesToDelete != null)
					foreach (var textFile in entitiesToDelete)
						entity.TextFiles.Remove(textFile);
			}
			else
				entity.TextFiles.Clear();
		}

		/// <summary>
		/// Сервис изменения списков графических файлов
		/// </summary>
		/// <param name="entity">Основная сущность</param>
		/// <param name="imgFiles">Список графических файлов</param>
		public void ChangeImgFilesList(IEntityWithFiles entity,
			IEnumerable<ImgFile> imgFiles)
		{
			if (entity.ImgFiles is null)
				throw new ApplicationSystemNullException(nameof(ChangeListService), nameof(entity.ImgFiles));

			if (imgFiles != null || entity.ImgFiles.Any())
			{
				var entitiesToDelete = entity.ImgFiles.Where(x => !imgFiles.
					Any(y => y.Id == x.Id)).ToList();
				var entitiesToAdd = imgFiles.Where(x => !entity.ImgFiles.
					Any(y => y.Id == x.Id)).ToList();

				if (entitiesToAdd != null)
					entity.ImgFiles.AddRange(entitiesToAdd);

				if (entitiesToDelete != null)
					foreach (var imgFile in entitiesToDelete)
						entity.ImgFiles.Remove(imgFile);
			}
			else
				entity.ImgFiles.Clear();
		}
	}
}
