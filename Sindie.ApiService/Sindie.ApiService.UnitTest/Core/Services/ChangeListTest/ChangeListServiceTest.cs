using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Entities;
using Witcher.Core.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System;
using Witcher.Core.Services.ChangeListService;

namespace Witcher.UnitTest.Core.Services.ChangeListTest
{
	/// <summary>
	/// служебный класс для реализации интерфейса
	/// </summary>
	internal class EntityWithFiles : EntityBase, IEntityWithFiles
	{
		/// <summary>
		/// Графические файлы
		/// </summary>
		public List<ImgFile> ImgFiles { get; set; }

		/// <summary>
		/// Текстовые файлы
		/// </summary>
		public List<TextFile> TextFiles { get; set; }
	}

	/// <summary>
	/// Тест для <see cref="ChangeListService">
	/// </summary>
	[TestClass]
	public class ChangeListServiceTest: UnitTestBase
	{
		private readonly ImgFile _imgFile;
		private readonly TextFile _textFile;
		private readonly ImgFile _imgFile2;
		private readonly TextFile _textFile2;
		private readonly IEntityWithFiles _entityWithFiles;
		
		/// <summary>
		/// Конструктор
		/// </summary>
		public ChangeListServiceTest(): base()
		{
			_imgFile = ImgFile.CreateForTest(Guid.NewGuid());
			_textFile = TextFile.CreateForTest(Guid.NewGuid());
			_imgFile2 = ImgFile.CreateForTest(Guid.NewGuid());
			_textFile2 = TextFile.CreateForTest(Guid.NewGuid());

			_entityWithFiles = new EntityWithFiles
			{
				ImgFiles = new List<ImgFile> { _imgFile },
				TextFiles = new List<TextFile> { _textFile },
				Id = Guid.NewGuid()
			};
			
		}

		/// <summary>
		/// Тест метода изменения списка текстовых файлов
		/// </summary>
		[TestMethod]
		public void ChangeTextFilesList_ShoildReturnVoid()
		{
			var textFiles = new List<TextFile> { _textFile2 };

			//Arrange пришлось обойти конфликт имен указанием полного имени класса
			var changeListService = new ChangeListService();

			//Act
			changeListService.ChangeTextFilesList(
				_entityWithFiles,
				textFiles);

			//Assert
			var addedFile = _entityWithFiles.TextFiles
				.FirstOrDefault(x => x.Id == _textFile2.Id);
			Assert.IsNotNull(addedFile);

			var deletedFile = _entityWithFiles.TextFiles
				.FirstOrDefault(x => x.Id == _textFile.Id);
			Assert.IsNull(deletedFile);
		}

		/// <summary>
		/// Тест метода изменения списка графических файлов
		/// </summary>
		[TestMethod]
		public void ChangeImgFilesList_ShoildReturnVoid()
		{
			var imgFiles = new List<ImgFile> { _imgFile2 };

			//Arrange пришлось обойти конфликт имен указанием полного имени класса
			var changeListService = new ChangeListService();

			//Act
			changeListService.ChangeImgFilesList(
				_entityWithFiles,
				imgFiles);

			//Assert
			var addedFile = _entityWithFiles.ImgFiles
				.FirstOrDefault(x => x.Id == _imgFile2.Id);
			Assert.IsNotNull(addedFile);

			var deletedFile = _entityWithFiles.ImgFiles
				.FirstOrDefault(x => x.Id == _imgFile.Id);
			Assert.IsNull(deletedFile);
		}
	}
}
