using Sindie.ApiService.Core.Entities;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Abstractions
{
	/// <summary>
	/// Интерфейс для сущности со списками графических и текстовых файлов
	/// </summary>
	public interface IEntityWithFiles //типа игра
	{
		/// <summary>
		/// Графические файлы
		/// </summary>
		public List<ImgFile> ImgFiles { get; }  

		/// <summary>
		/// Текстовые файлы
		/// </summary>
		public List<TextFile> TextFiles { get; }
	}
}
