using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Пререквизит
	/// </summary>
	public class Prerequisite : EntityBase
	{
		/// <summary>
		/// Конструктор для EF
		/// </summary>
		protected Prerequisite()
		{
		}

		/// <summary>
		/// Поле для <see cref="_imgFile"/>
		/// </summary>
		public const string ImgFileField = nameof(_imgFile);

		private ImgFile _imgFile;

		/// <summary>
		/// Конструктор пререквезита
		/// </summary>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		public Prerequisite(
			ImgFile imgFile,
			string name,
			string description)
		{
			ImgFile = imgFile;
			Name = name;
			Description = description;
			ScriptPrerequisites = new List<ScriptPrerequisites>();
		}

		/// <summary>
		/// Айдишник графического файла
		/// </summary>
		public Guid? ImgFileId { get; protected set; }

		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }

		#region navigation properties

		/// <summary>
		/// Графический файл
		/// </summary>
		public ImgFile ImgFile
		{
			get => _imgFile;
			set
			{
				_imgFile = value;
				ImgFileId = _imgFile?.Id;
			}
		}

		/// <summary>
		/// Пререквизиты скриптов
		/// </summary>
		public List<ScriptPrerequisites> ScriptPrerequisites { get; set; }

		#endregion navigation properties
	}
}
